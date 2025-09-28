using NitroTechWebsite.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Util;

namespace NitroTechWebsite
{
    public partial class DownloadStatement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string statementNumber = Request.QueryString["snum"];
            string customerId = Request.QueryString["cid"];

            if (string.IsNullOrEmpty(statementNumber) || string.IsNullOrEmpty(customerId))
            {
                Response.Write("Invalid request – missing statement number or customer ID.");
                return;
            }

            GenerateStatement(statementNumber, customerId);
        }

        private void makeStatement(string clientName, string clientAddress, string clientEmail, string clientPhone,
                                   string statementNumber, string[,] tabled, string initial)
        {
            var statementService = new StatementService();
            var document = statementService.GetStatement(clientName, clientAddress, clientEmail, clientPhone,
                                                         statementNumber, tabled, initial);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition",
                "attachment;filename=" + statementNumber + ".pdf");

            using (var ms = new MemoryStream())
            {
                document.Save(ms);
                ms.WriteTo(Response.OutputStream);
            }

            Response.Flush();
            Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void GenerateStatement(string statementNumber, string customerId)
        {
            try
            {
                string clientName = "";
                string clientAddress = "";
                string clientEmail = "";
                string clientPhone = "";
                decimal clientOwe = 0.00M;

                // Get client info + initial balance
                using (var conn = DatabaseHelper.OpenConnection())
                using (var cmd = new SqlCommand(@"
                    SELECT c.customerName, c.customerAddress, c.customerEmailAddress, c.customerContactNumber, c.customerOwe, s.statementAmountDue
                    FROM tblStatement s
                    INNER JOIN tblCustomer c ON s.customerID = c.customerID
                    WHERE s.statementNumber = @SNum", conn))
                {
                    cmd.Parameters.AddWithValue("@SNum", statementNumber);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            clientName = reader["customerName"].ToString();
                            clientAddress = reader["customerAddress"].ToString();
                            clientEmail = reader["customerEmailAddress"].ToString();
                            clientPhone = reader["customerContactNumber"].ToString();
                            clientOwe = Convert.ToDecimal(reader["customerOwe"]);
                        }
                    }
                }


                var invoices = TransactionHelper.GetInvoicesLastMonth(customerId);
                var payments = TransactionHelper.GetPaymentsLastMonth(customerId);

                // Combine if you want a single list
                var combined = invoices.AsEnumerable()
                    .Select(r => new
                    {
                        ID = r["invoiceNumber"].ToString(),
                        Amount = (decimal)r["invoiceAmountDue"],
                        Date = (DateTime)r["invoiceDate"],
                        Type = "I"
                    })
                    .Concat(payments.AsEnumerable().Select(r => new
                    {
                        ID = r["paymentID"].ToString(),
                        Amount = (decimal)r["paidAmount"],
                        Date = (DateTime)r["dateOfPayment"],
                        Type = "P"
                    }))
                    .OrderByDescending(t => t.Date)
                    .ToList();

                combined = combined.OrderBy(t => t.Date).ToList();

                // Build 2D array
                string[,] transactions = new string[combined.Count, 3];
                decimal transactionSum = 0;

                for (int i = 0; i < combined.Count; i++)
                {
                    string dtype;
                    var t = combined[i];
                    string formattedAmount = t.Amount.ToString("0.00").Replace('.', ',');
                    if (t.Type == "P")
                    {
                        dtype = "Payment "; // Payments are negative
                    }
                    else
                    {
                        dtype = "Invoice - " + t.ID; // Invoices are positive
                    }
                    transactions[i, 0] = dtype;
                    transactions[i, 1] = formattedAmount;
                    transactions[i, 2] = t.Date.ToString("yyyy/MM/dd");

                    if (t.Type == "I")
                    {
                        transactionSum += t.Amount; // Add invoice amounts to balance
                    }
                    else
                    {
                        transactionSum -= t.Amount; // Subtract payment amounts from balance
                    }
                }

                // Reverse calculation for initial balance
                string initial = (clientOwe - transactionSum).ToString();

               

                

                // Generate PDF
                makeStatement(clientName, clientAddress, clientEmail, clientPhone, statementNumber, transactions, initial);
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    $"alert('Error generating statement PDF: {ex.Message}');", true);
            }
        }
    }
}
