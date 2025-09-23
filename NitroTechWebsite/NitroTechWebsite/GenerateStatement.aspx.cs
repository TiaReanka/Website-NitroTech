using NitroTechWebsite.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace NitroTechWebsite
{
    public partial class Statements : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerateStatement_Click(object sender, EventArgs e)
        {
            string customerId = customerID.SelectedValue?.Trim();

            if (string.IsNullOrEmpty(customerId))
            {
                
                Response.Write("<script>alert('Please select a customer ID');</script>");
                return;
            }
            
            //Get customer details in a single query

            try
            {
                
                DataTable dtCustomer = ExecuteDataTable(
                    "SELECT CustomerName, CustomerAddress, CustomerEmail, CustomerPhone, AmountOwed " +
                    "FROM tblCustomer WHERE CustomerID=@id",
                    new SqlParameter("@id", customerId));

                if (dtCustomer.Rows.Count == 0)
                {
                   
                    Response.Write("<script>alert('Customer does not exist');</script>");
                    return;
                }

                DataRow row = dtCustomer.Rows[0];
                string clientName = row["CustomerName"]?.ToString() ?? "";
                string clientAddress = row["CustomerAddress"]?.ToString() ?? "";
                string clientEmail = row["CustomerEmail"]?.ToString() ?? "";
                string clientPhone = row["CustomerPhone"]?.ToString() ?? "";
                decimal total = row["AmountOwed"] != DBNull.Value ? Convert.ToDecimal(row["AmountOwed"]) : 0m;

                //Generate statement number
                string statementNumber = GenerateStatementNumber(customerId, clientName);

                
                //Get invoices & payments from past month
                DataTable invoices = ExecuteDataTable(
                    "SELECT invoiceNumber, invoiceAmountDue, invoiceDate " +
                    "FROM tblInvoices WHERE CustomerID=@id AND invoiceDate >= DATEADD(MONTH, -1, GETDATE())",
                    new SqlParameter("@id", customerId));

                
                DataTable payments = ExecuteDataTable(
                    "SELECT paymentID, paidAmount, dateOfPayment " +
                    "FROM tblPayments WHERE CustomerID=@id AND dateOfPayment >= DATEADD(MONTH, -1, GETDATE())",
                    new SqlParameter("@id", customerId));


                //Combine transactions
                var combined = invoices.AsEnumerable()
                    .Select(r => new { ID = r["invoiceNumber"].ToString(), Amount = Convert.ToDecimal(r["invoiceAmountDue"]), Date = Convert.ToDateTime(r["invoiceDate"]), Type = "I" })
                    .Concat(payments.AsEnumerable()
                        .Select(r => new { ID = r["paymentID"].ToString(), Amount = Convert.ToDecimal(r["paidAmount"]), Date = Convert.ToDateTime(r["dateOfPayment"]), Type = "P" }))
                    .OrderBy(t => t.Date)
                    .ToList();

                string[,] transactions = new string[combined.Count, 3];
                decimal transactionSum = 0;

                for (int i = 0; i < combined.Count; i++)
                {
                    var t = combined[i];
                    string dtype = t.Type == "P" ? "Payment" : "Invoice - " + t.ID;
                    string formattedAmount = t.Amount.ToString("0.00").Replace('.', ',');

                    transactions[i, 0] = dtype;
                    transactions[i, 1] = formattedAmount;
                    transactions[i, 2] = t.Date.ToString("yyyy/MM/dd");

                    transactionSum += (t.Type == "I") ? t.Amount : -t.Amount;
                }

                string initial = (total - transactionSum).ToString("0.00");

                // Insert statement
                ExecuteNonQuery(
                    "INSERT INTO tblStatements (statementNumber, statementDate, statementAmount, customerID) " +
                    "VALUES (@num, @date, @amt, @cid)",
                    new SqlParameter("@num", statementNumber),
                    new SqlParameter("@date", DateTime.Now),
                    new SqlParameter("@amt", total),
                    new SqlParameter("@cid", customerId));

                //Generate PDF and stream to browser
                //var statementService = new StatementService();
                //var document = statementService.GetStatement(clientName, clientAddress, clientEmail, clientPhone, statementNumber, transactions, initial);

                //Response.Clear();
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-Disposition", $"attachment; filename={statementNumber}.pdf");
                //document.Save(Response.OutputStream);
                //Response.Flush();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error creating statement');</script>");
            }
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

        private void ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
        }

        //Generate Statement Number
        private string GenerateStatementNumber(string ID, string name)
        {
            int count = ExecuteScalar<int>("SELECT COUNT(*) FROM tblStatements");

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
    }
}
