using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace NitroTechWebsite
{
    public class VehicleTransfer
    {
        //Execute a scalar query
        public object ExecuteScalar(string query, SqlConnection conn, SqlTransaction tran = null, params SqlParameter[] parameters)
        {
            using (var cmd = new SqlCommand(query, conn, tran))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        //Execute a non-query (INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string query, SqlConnection conn, SqlTransaction tran = null, params SqlParameter[] parameters)
        {
            using (var cmd = new SqlCommand(query, conn, tran))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        //Get a DataTable from a query
        public DataTable GetDataTable(string query, params SqlParameter[] parameters)
        {
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                using (var adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        //Transfer a vehicle safely
        public TransferResult TransferVehicle(string vin, string oldCustomerId, string newCustomerId)
        {
            using (var connection = DatabaseHelper.OpenConnection())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Verify vehicle ownership
                    var currentCustomerId = ExecuteScalar(
                        "SELECT customerID FROM tblVehicle WHERE VIN = @VIN",
                        connection, transaction,
                        new SqlParameter("@VIN", vin)
                    )?.ToString();

                    if (currentCustomerId == null)
                        return new TransferResult { Success = false, Message = "Vehicle not found" };

                    if (currentCustomerId != oldCustomerId)
                        return new TransferResult { Success = false, Message = "Vehicle does not belong to this customer" };

                    //Verify new customer exists
                    var customerExists = Convert.ToInt32(ExecuteScalar(
                        "SELECT COUNT(*) FROM tblCustomer WHERE customerID = @CustomerID",
                        connection, transaction,
                        new SqlParameter("@CustomerID", newCustomerId)
                    )) > 0;

                    if (!customerExists)
                        return new TransferResult { Success = false, Message = "New customer not found" };

                    //  Update ownership
                    ExecuteNonQuery(
                        "UPDATE tblVehicle SET customerID = @NewCustomerID WHERE VIN = @VIN",
                        connection, transaction,
                        new SqlParameter("@NewCustomerID", newCustomerId),
                        new SqlParameter("@VIN", vin)
                    );

                    transaction.Commit();
                    return new TransferResult { Success = true, Message = "Vehicle transferred successfully" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new TransferResult { Success = false, Message = $"Transfer failed: {ex.Message}" };
                }
            }
        }

        //Get all vehicles for a customer
        public DataTable GetVehiclesByCustomer(string customerId)
        {
            return GetDataTable(
                "SELECT VIN, vehicleMake, vehicleModel, vehicleYear FROM tblVehicle WHERE customerID = @CustomerID",
                new SqlParameter("@CustomerID", customerId)
            );
        }

        // Get all customers
        public DataTable GetAllCustomers()
        {
            return GetDataTable("SELECT customerID, customerName FROM tblCustomer ORDER BY customerName");
        }
    }

    public class TransferResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}