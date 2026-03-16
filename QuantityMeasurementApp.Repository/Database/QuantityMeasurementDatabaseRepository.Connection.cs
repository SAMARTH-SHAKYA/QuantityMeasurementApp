using System;
using Microsoft.Data.SqlClient;

namespace QuantityMeasurementApp.Repository
{
    public partial class QuantityMeasurementDatabaseRepository
    {
        private SqlConnection OpenConnection()
        {
            try
            {
                var conn = new SqlConnection(_connectionString);
                conn.Open();
                return conn;
            }
            catch (SqlException ex)
            {
                throw new DatabaseException(
                    $"Unable to open SQL Server connection: {ex.Message}", ex);
            }
        }

        private void VerifyConnection()
        {
            try
            {
                using var conn = OpenConnection();
                // Connection opened and closed successfully — table assumed to exist
                // (schema.sql must be run in SSMS first).
            }
            catch (DatabaseException)
            {
                throw; // propagate as-is
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                    $"Database verification failed: {ex.Message}", ex);
            }
        }
    }
}

