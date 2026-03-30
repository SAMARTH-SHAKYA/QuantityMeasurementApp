using QuantityMeasurementApp.Entity;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.API.Interface
{
    public interface IBusinessLogicClient
    {
        [Post("/api/measurement/convert")]
        Task<QuantityDTO> ConvertAsync([Body] object request);

        [Post("/api/measurement/compare")]
        Task<object> CompareAsync([Body] object request);

        [Post("/api/measurement/add")]
        Task<QuantityDTO> AddAsync([Body] object request);

        [Post("/api/measurement/subtract")]
        Task<QuantityDTO> SubtractAsync([Body] object request);

        [Post("/api/measurement/multiply")]
        Task<QuantityDTO> MultiplyAsync([Body] object request);

        [Post("/api/measurement/divide")]
        Task<QuantityDTO> DivideAsync([Body] object request);

        [Get("/api/measurement/history")]
        Task<List<QuantityMeasurementEntity>> GetHistoryAsync();

        [Post("/api/auth/register")]
        Task<QuantityMeasurementApp.Entity.DTO.AuthResponseDTO> RegisterAsync([Body] QuantityMeasurementApp.Entity.DTO.RegisterDTO dto);

        [Post("/api/auth/login")]
        Task<QuantityMeasurementApp.Entity.DTO.AuthResponseDTO> LoginAsync([Body] QuantityMeasurementApp.Entity.DTO.LoginDTO dto);
    }
}
