using System.Collections.Generic;
using QuantityMeasurementApp.Entity;

namespace QuantityMeasurementApp.Repository
{
    // Interface Segregation Principle: specific interface for repository operations
    public interface IQuantityMeasurementRepository
    {
        // Save a new measurement entity to the repository
        void SaveMeasurement(QuantityMeasurementEntity measurement);

        // Retrieve stored measurements for a user
        List<QuantityMeasurementEntity> GetMeasurementsForUser(int? userId);

        // Return the total count of stored measurements
        int GetTotalCount();
    }
}

