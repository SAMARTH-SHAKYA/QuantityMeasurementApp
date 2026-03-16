using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using QuantityMeasurementApp.Entity;

namespace QuantityMeasurementApp.Repository
{
    // UC16: ADO.NET-based repository that persists quantity measurements to
    // SQL Server via parameterised queries (SQL-injection safe).
    // Implements the full IQuantityMeasurementRepository contract.
    public partial class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly string _connectionString;

        // ─── Constructor ──────────────────────────────────────────────────

        // Creates the repository and verifies the database is reachable.
        // connectionString: full SQL Server connection string, e.g. from appsettings.json.
        public QuantityMeasurementDatabaseRepository(string connectionString)
        {
            _connectionString = connectionString;
            VerifyConnection();
            Console.WriteLine("[DatabaseRepository] Connected to SQL Server.");
        }

        // ─── Private Helpers ──────────────────────────────────────────────

        // ─── IQuantityMeasurementRepository Implementation ────────────────

        /// <inheritdoc/>
        public void SaveMeasurement(QuantityMeasurementEntity measurement)
        {
            if (measurement == null) throw new ArgumentNullException(nameof(measurement));

            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqlCommand(SqlStatements.Insert, conn);

                cmd.Parameters.AddWithValue("@FirstOperand",    measurement.FirstOperand);
                cmd.Parameters.AddWithValue("@SecondOperand",   measurement.SecondOperand);
                cmd.Parameters.AddWithValue("@OperationType",   measurement.OperationType);
                cmd.Parameters.AddWithValue("@MeasurementType", measurement.MeasurementType);
                cmd.Parameters.AddWithValue("@FinalResult",     measurement.FinalResult);
                cmd.Parameters.AddWithValue("@HasError",        measurement.HasError);
                cmd.Parameters.AddWithValue("@ErrorMessage",    measurement.ErrorMessage);
                cmd.Parameters.AddWithValue("@RecordedAt",      measurement.Timestamp);

                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DatabaseException($"Error saving measurement: {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            var results = new List<QuantityMeasurementEntity>();

            try
            {
                using var conn   = OpenConnection();
                using var cmd    = new SqlCommand(SqlStatements.SelectAll, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                    results.Add(QuantityMeasurementRowMapper.MapRow(reader));
            }
            catch (SqlException ex)
            {
                throw new DatabaseException($"Error retrieving all measurements: {ex.Message}", ex);
            }

            return results;
        }

        /// <inheritdoc/>
        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
        {
            if (string.IsNullOrWhiteSpace(operationType))
                throw new ArgumentException("operationType must not be null or empty.", nameof(operationType));

            var results = new List<QuantityMeasurementEntity>();

            try
            {
                using var conn   = OpenConnection();
                using var cmd    = new SqlCommand(SqlStatements.SelectByOperation, conn);
                cmd.Parameters.AddWithValue("@OperationType", operationType);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    results.Add(QuantityMeasurementRowMapper.MapRow(reader));
            }
            catch (SqlException ex)
            {
                throw new DatabaseException(
                    $"Error retrieving measurements by operation: {ex.Message}", ex);
            }

            return results;
        }

        /// <inheritdoc/>
        public List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType)
        {
            if (string.IsNullOrWhiteSpace(measurementType))
                throw new ArgumentException("measurementType must not be null or empty.", nameof(measurementType));

            var results = new List<QuantityMeasurementEntity>();

            try
            {
                using var conn   = OpenConnection();
                using var cmd    = new SqlCommand(SqlStatements.SelectByMeasurementType, conn);
                cmd.Parameters.AddWithValue("@MeasurementType", measurementType);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    results.Add(QuantityMeasurementRowMapper.MapRow(reader));
            }
            catch (SqlException ex)
            {
                throw new DatabaseException(
                    $"Error retrieving measurements by type: {ex.Message}", ex);
            }

            return results;
        }

        /// <inheritdoc/>
        public int GetTotalCount()
        {
            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqlCommand(SqlStatements.Count, conn);
                return (int)cmd.ExecuteScalar()!;
            }
            catch (SqlException ex)
            {
                throw new DatabaseException($"Error getting measurement count: {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public void DeleteAll()
        {
            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqlCommand(SqlStatements.DeleteAll, conn);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine($"[DatabaseRepository] Deleted {rows} measurement(s) from the database.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseException($"Error deleting all measurements: {ex.Message}", ex);
            }
        }
    }
}
