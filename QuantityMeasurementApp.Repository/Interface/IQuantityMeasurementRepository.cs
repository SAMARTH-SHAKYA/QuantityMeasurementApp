using System.Collections.Generic;
using QuantityMeasurementApp.Entity;

namespace QuantityMeasurementApp.Repository
{
    // Interface Segregation Principle: specific interface for repository operations
    public interface IQuantityMeasurementRepository
    {
        // Save a new measurement entity to the repository
        void SaveMeasurement(QuantityMeasurementEntity measurement);

        // Retrieve all stored measurements
        IEnumerable<QuantityMeasurementEntity> GetAllMeasurements();

        // Retrieve measurements filtered by operation type (e.g. "Addition", "Conversion")
        IEnumerable<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType);

        // Retrieve measurements filtered by measurement type (e.g. "Length", "Weight")
        IEnumerable<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType);

        // Return the total count of stored measurements
        int GetTotalCount();

        // Delete all measurements (useful for testing / reset)
        void DeleteAll();
    }
}

