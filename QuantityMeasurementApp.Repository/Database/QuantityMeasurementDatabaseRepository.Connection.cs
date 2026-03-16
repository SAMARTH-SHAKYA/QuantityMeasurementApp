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
                using var cmd = new SqlCommand(
                    "SELECT OBJECT_ID(N'dbo.QuantityMeasurements', N'U')",
                    conn);

                object? obj = cmd.ExecuteScalar();
                if (obj == null || obj == DBNull.Value)
                {
                    throw new DatabaseException(
                        "Connected to SQL Server, but table dbo.QuantityMeasurements was not found. " +
                        "Ensure you are using the correct database and have run the schema script.");
                }
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

