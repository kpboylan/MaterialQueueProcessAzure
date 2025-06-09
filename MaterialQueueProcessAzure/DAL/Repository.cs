using MaterialQueueProcessAzure.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialQueueProcessAzure.DAL
{
    public class Repository : IRepository
    {
        public void AddMaterial(Material material, ILogger<Worker> _logger, string _connString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connString))
                {
                    using (SqlCommand command = new SqlCommand("dbo.InsertMaterial", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@MaterialName", material.MaterialName);
                        command.Parameters.AddWithValue("@MaterialType", material.MaterialType);
                        command.Parameters.AddWithValue("@Description", material.Description);
                        command.Parameters.AddWithValue("@CurrentStock", material.CurrentStock);
                        command.Parameters.AddWithValue("@UOMID", material.UOMId);
                        command.Parameters.AddWithValue("@Active", material.Active);

                        connection.Open();

                        _logger.LogInformation($"✅ SQL Server connected successfully.");

                        command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"❌ SQL Server connection failed: {ex.Message}");
            }
        }
    }
}
