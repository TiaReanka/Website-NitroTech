using MigraDoc.DocumentObjectModel.Tables;
using NitroTechWebsite.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;

namespace NitroTechWebsite
{
    public partial class Statements : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomerIDs();
            }
        }

        protected void btnGenerateStatement_Click(object sender, EventArgs e)
        {
            string customerId = customerID.SelectedValue?.Trim();

            if (string.IsNullOrEmpty(customerId))
            {
                
                Response.Write("<script>alert('Please select a customer ID');</script>");
                return;
            }
            
            //Get customer details

            try
            {
                
                DataTable dtCustomer = ExecuteDataTable(
                    "SELECT * FROM tblCustomer WHERE customerID=@id",
                    new SqlParameter("@id", customerId));

                if (dtCustomer.Rows.Count == 0)
                {
                    Response.Write("<script>alert('Customer does not exist');</script>");
                    return;
                }

                //Assign variables with details

                DataRow row = dtCustomer.Rows[0];
                string clientName = row["customerName"]?.ToString() ?? "";
                decimal total = row["customerOwe"] != DBNull.Value ? Convert.ToDecimal(row["customerOwe"]) : 0m;

                //Generate statement number
                string statementNumber = GenerateStatementNumber(customerId, clientName);

                // Calculate total invoices from past month
                decimal totalInvoices = ExecuteScalar<decimal>(
                    "SELECT ISNULL(SUM(invoiceAmountDue), 0) " +
                    "FROM tblInvoice " +
                    "WHERE customerID=@id AND invoiceDate >= DATEADD(MONTH, -1, GETDATE())",
                    new SqlParameter("@id", customerId));

                // Calculate total payments from past month
                decimal totalPayments = ExecuteScalar<decimal>(
                    "SELECT ISNULL(SUM(paidAmount), 0) " +
                    "FROM tblPayment " +
                    "WHERE customerID=@id AND dateOfPayment >= DATEADD(MONTH, -1, GETDATE())",
                    new SqlParameter("@id", customerId));

                // Calculate statement amount: Previous balance + New invoices - Recent payments
                decimal statementAmount = total + totalInvoices - totalPayments;



                //INSERT STATEMENT INTO DATABASE
                int rows = ExecuteNonQuery(
                    "INSERT INTO tblStatement (statementNumber, statementDate, statementAmountDue, customerID) " +
                    "VALUES (@num, @date, @amt, @cid)",
                    new SqlParameter("@num", statementNumber),
                    new SqlParameter("@date", DateTime.Now),
                    new SqlParameter("@amt", statementAmount),
                    new SqlParameter("@cid", customerId));


                // After ExecuteNonQuery insert
                string script = $"window.open('DownloadStatement.aspx?snum={statementNumber}&cid={customerId}', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "openStatementPDF", script, true);

                ResetPage();


            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error creating statement: {ex.Message}');</script>");
            }



        }

        private void LoadCustomerIDs()
        {
            
            DataTable dtCustomers = ExecuteDataTable("SELECT customerID FROM tblCustomer");

            // Bind to DropDownList
            customerID.DataSource = dtCustomers;
            customerID.DataTextField = "customerID";   // What the user sees
            customerID.DataValueField = "customerID";  // The value used in code
            customerID.DataBind();

            
            customerID.Items.Insert(0, new ListItem("-- Select Customer ID --", ""));
        }

        //Helper Methods
        private T ExecuteScalar<T>(string query, params SqlParameter[] parameters)
        {
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value) return default(T);
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        private DataTable ExecuteDataTable(string query, params SqlParameter[] parameters)
        {
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                using (var da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        private int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (var conn = DatabaseHelper.OpenConnection())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    using (var cmd = new SqlCommand(query, conn, transaction))
                    {
                        if (parameters != null) cmd.Parameters.AddRange(parameters);
                        int result = cmd.ExecuteNonQuery();
                        transaction.Commit(); // Explicitly commit
                        return result;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        //Generate Statement Number
        private string GenerateStatementNumber(string ID, string name)
        {
            int count = ExecuteScalar<int>("SELECT COUNT(*) FROM tblStatement");

            char middleChar = 'X';
            if (!string.IsNullOrEmpty(name))
            {
                int middleIndex = name.Length / 2;
                middleChar = (name[middleIndex] == ' ' && middleIndex > 0) ? name[middleIndex - 1] : name[middleIndex];
            }

            char lastChar = !string.IsNullOrEmpty(name) ? name[name.Length - 1] : 'Y';
            string firstFourID = ID.Length >= 4 ? ID.Substring(0, 4) : ID.PadRight(4, '0');
            string midID = ID.Length >= 12 ? ID.Substring(7, 5) : (ID.Length > 7 ? ID.Substring(7).PadRight(5, '0') : "00000");

            return $"S{count + 1} - {middleChar}{lastChar}{firstFourID}{midID}";
        }

        private void ResetPage()
        {
            // Reset dropdown to default
            customerID.SelectedIndex = 0;
      
        }
    }
}
