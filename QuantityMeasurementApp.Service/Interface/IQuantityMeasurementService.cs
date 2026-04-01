using QuantityMeasurementApp.Entity;

namespace QuantityMeasurementApp.Service
{
    // Interface for performing quantity operations
    public interface IQuantityMeasurementService
    {
        QuantityDTO Compare(QuantityDTO q1, QuantityDTO q2, int? userId = null);
        QuantityDTO Convert(QuantityDTO source, string targetUnitName, int? userId = null);
        QuantityDTO Add(QuantityDTO q1, QuantityDTO q2, string targetUnitName, int? userId = null);
        QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2, string targetUnitName, int? userId = null);
        QuantityDTO Divide(QuantityDTO q1, QuantityDTO q2, int? userId = null);
        QuantityDTO Multiply(QuantityDTO q1, QuantityDTO q2, string targetUnitName, int? userId = null);
    }
}

