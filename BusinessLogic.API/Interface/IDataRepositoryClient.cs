using QuantityMeasurementApp.Entity;
using QuantityMeasurementApp.Entity.Entities;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.API.Interface
{
    public interface IDataRepositoryClient
    {
        [Get("/api/data/users/{username}")]
        Task<UserEntity> GetUserAsync(string username);

        [Post("/api/data/users")]
        Task CreateUserAsync([Body] UserEntity user);

        [Get("/api/data/measurements")]
        Task<List<QuantityMeasurementEntity>> GetAllMeasurementsAsync();

        [Post("/api/data/measurements")]
        Task SaveMeasurementAsync([Body] QuantityMeasurementEntity measurement);
    }
}
