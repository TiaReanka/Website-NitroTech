using MigraDoc.DocumentObjectModel.Tables;
using NitroTechWebsite.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;
using System.Globalization;


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
                
                Response.Write("<script>alert('⚠ Please select a customer ID');</script>");
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
                    Response.Write("<script>alert('⚠ Customer does not exist');</script>");
                    return;
                }
               
                //Assign variables with details

                DataRow row = dtCustomer.Rows[0];
                string clientName = row["customerName"]?.ToString() ?? "";

                decimal currentBalance = row["customerOwe"] != DBNull.Value
                    ? Convert.ToDecimal(row["customerOwe"])
                    : 0m;


                //Generate statement number
                string statementNumber = GenerateStatementNumber(customerId, clientName);

                // Get invoices from last 30 days
                DataTable invoices = ExecuteDataTable(
                    "SELECT invoiceNumber, invoiceAmountDue, invoiceDate " +
                    "FROM tblInvoice " +
                    "WHERE customerID=@id AND invoiceDate >= DATEADD(MONTH, -1, GETDATE())",
                    new SqlParameter("@id", customerId));

                // Get payments from last 30 days
                DataTable payments = ExecuteDataTable(
                    "SELECT paymentID, paidAmount, dateOfPayment " +
                    "FROM tblPayment " +
                    "WHERE customerID=@id AND dateOfPayment >= DATEADD(MONTH, -1, GETDATE())",
                    new SqlParameter("@id", customerId));

                // 🔹 Combine into a single transaction list (I = Invoice, P = Payment)
                List<(string ID, decimal Amount, DateTime Date, string Type)> combined =
                    new List<(string ID, decimal Amount, DateTime Date, string Type)>();

                foreach (DataRow r in invoices.Rows)
                {
                    combined.Add((
                        ID: r["invoiceNumber"].ToString(),
                        Amount: Convert.ToDecimal(r["invoiceAmountDue"]),
                        Date: Convert.ToDateTime(r["invoiceDate"]),
                        Type: "I"
                    ));
                }

                foreach (DataRow r in payments.Rows)
                {
                    combined.Add((
                        ID: r["paymentID"].ToString(),
                        Amount: Convert.ToDecimal(r["paidAmount"]),
                        Date: Convert.ToDateTime(r["dateOfPayment"]),
                        Type: "P"
                    ));
                }

                // 🔹 Sort by date ASC
                combined = combined.OrderBy(t => t.Date).ToList();

                // 🔹 Build string[,] for MigraDoc PDF
                string[,] transactions = new string[combined.Count, 3];
                decimal transactionSum = 0;

                for (int i = 0; i < combined.Count; i++)
                {
                    var t = combined[i];

                    string typeDesc =
                        t.Type == "P" ? "Payment" : "Invoice - " + t.ID;

                    string formattedAmount = t.Amount.ToString("N2", CultureInfo.GetCultureInfo("en-ZA"));



                    transactions[i, 0] = typeDesc;
                    transactions[i, 1] = formattedAmount;
                    transactions[i, 2] = t.Date.ToString("yyyy/MM/dd");

                    if (t.Type == "I") transactionSum += t.Amount;
                    else transactionSum -= t.Amount;
                }

                // 🔹 Reverse calculate initial balance
                string initialBalance = (currentBalance - transactionSum).ToString("0.00");



                //INSERT STATEMENT INTO DATABASE
                int rows = ExecuteNonQuery(
                    "INSERT INTO tblStatement (statementNumber, statementDate, statementAmountDue, customerID) " +
                    "VALUES (@num, @date, @amt, @cid)",
                    new SqlParameter("@num", statementNumber),
                    new SqlParameter("@date", DateTime.Now),
                    new SqlParameter("@amt", currentBalance),
                    new SqlParameter("@cid", customerId));


                // After ExecuteNonQuery insert
                string script = $"window.open('DownloadStatement.aspx?snum={statementNumber}&cid={customerId}', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "openStatementPDF", script, true);

                ResetPage();


            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('❌ Error: {ex.Message.Replace("'", "")}');</script>");
                Response.Write($"<pre>{ex}</pre>");
            }



        }

        private void LoadCustomerIDs()
        {
            // Get both ID and Name
            DataTable dtCustomers = ExecuteDataTable(
                "SELECT customerID, customerName FROM tblCustomer ORDER BY customerName"
            );

            // Create a combined display column
            dtCustomers.Columns.Add("DisplayText", typeof(string));

            foreach (DataRow row in dtCustomers.Rows)
            {
                row["DisplayText"] = row["customerName"] + " - " + row["customerID"] + "";
            }

            // Bind to DropDownList
            customerID.DataSource = dtCustomers;
            customerID.DataTextField = "DisplayText";   // what user sees
            customerID.DataValueField = "customerID";   // value used in code
            customerID.DataBind();

            // Insert default item
            customerID.Items.Insert(0, new ListItem("-- Select Customer --", ""));
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
                if (typeof(T) == typeof(decimal))
                {
                    return (T)(object)Convert.ToDecimal(
                        result.ToString(),
                        System.Globalization.CultureInfo.InvariantCulture
                    );
                }

                return (T)Convert.ChangeType(result, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
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
