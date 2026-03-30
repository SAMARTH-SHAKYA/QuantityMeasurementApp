using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementApp.Entity.DTO;
using QuantityMeasurementApp.Entity.Entities;

using QuantityMeasurementApp.Service.Interface;
using QuantityMeasurementApp.Service.Security;

namespace QuantityMeasurementApp.Service
{
    public class AuthService : IAuthService
    {
        private readonly BusinessLogic.API.Interface.IDataRepositoryClient _dataClient;
        private readonly IConfiguration _configuration;

        public AuthService(BusinessLogic.API.Interface.IDataRepositoryClient dataClient, IConfiguration configuration)
        {
            _dataClient = dataClient;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            try 
            {
                var existingUser = await _dataClient.GetUserAsync(dto.Username);
                if (existingUser != null)
                {
                    throw new Exception("User already exists with this username.");
                }
            } 
            catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // User doesn't exist, which is what we want for registration!
            }

            var user = new UserEntity
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashingHelper.HashPassword(dto.Password)
            };

            await _dataClient.CreateUserAsync(user);

            return new AuthResponseDTO
            {
                Username = user.Username,
                Message = "Registration successful"
            };
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            UserEntity user = null;
            try
            {
                user = await _dataClient.GetUserAsync(dto.Username);
            }
            catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Invalid username or password.");
            }
            
            if (user == null || !HashingHelper.VerifyPassword(user.PasswordHash, dto.Password))
            {
                throw new Exception("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDTO
            {
                Token = token,
                Username = user.Username,
                Message = "Login successful"
            };
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "super_secret_key_that_is_long_enough_for_hmac_sha256");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"] ?? "60")),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
