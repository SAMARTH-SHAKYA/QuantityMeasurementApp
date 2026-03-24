using System.Threading.Tasks;
using QuantityMeasurementApp.Entity.Models;

namespace QuantityMeasurementApp.Service.Interface
{
    public interface IAuthService
    {
        Task<User?> RegisterUserAsync(string email, string password, string phoneNumber);
        Task<string?> LoginAsync(string email, string password);
    }
}
