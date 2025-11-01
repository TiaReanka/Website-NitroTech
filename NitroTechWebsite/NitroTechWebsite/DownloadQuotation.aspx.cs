using NitroTechWebsite.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace NitroTechWebsite
{
    public partial class DownloadQuotation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string quotationNumber = Request.QueryString["qnum"];
            if (string.IsNullOrEmpty(quotationNumber))
            {
                Response.Write("⚠ Invalid quotation number.");
                return;
            }

            GenerateQuotation(quotationNumber);
        }

        private void makeQuotation(string[,] products, string clientName, string clientAddress, string clientEmail, string clientPhone, string vehicleVIN, string vehicleName, string quotationNumber, string labourFee, string total)
        {
            var quotationService = new QuotationService();
            var document = quotationService.GetQuotation(products, clientName, clientAddress, clientEmail, clientPhone, vehicleVIN, vehicleName, quotationNumber, labourFee, total);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition",
                "attachment;filename=" + quotationNumber + ".pdf");

            using (var ms = new MemoryStream())
            {
                document.Save(ms);
                ms.WriteTo(Response.OutputStream);
            }

            Response.Flush();
            Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();


        }

        private void GenerateQuotation(string quotationNumber)
        {
            try
            {
                // Fetch faults for the quotation
                var faults = new DataTable();
                using (var conn = DatabaseHelper.OpenConnection())
                using (var cmd = new SqlCommand(
                    "SELECT f.fault, f.amountProductUsed, p.partID, p.partName, p.partUnitPrice, f.faultTotal " +
                    "FROM tblFaults f " +
                    "INNER JOIN tblParts p ON f.productID = p.partID " +
                    "INNER JOIN tblQuotation q ON f.quotationNumber = q.quotationNumber " +
                    "WHERE q.quotationNumber = @QNum", conn))
                {
                    cmd.Parameters.AddWithValue("@QNum", quotationNumber);
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(faults);
                    }
                }

                int rowCount = faults.Rows.Count;
                string[,] products = new string[rowCount, 6];
                decimal totalProductCost = 0;

                for (int i = 0; i < rowCount; i++)
                {
                    var row = faults.Rows[i];
                    int itemNo = i + 1;
                    string faultDesc = row["fault"].ToString();
                    string partName = row["partName"].ToString();
                    string amountUsed = row["amountProductUsed"].ToString();
                    string productBaseCost = "R" + Convert.ToDecimal(row["partUnitPrice"]).ToString("0.00");
                    string totalAmount = "R" + Convert.ToDecimal(row["faultTotal"]).ToString("0.00");

                    totalProductCost += Convert.ToDecimal(row["faultTotal"]);

                    products[i, 0] = itemNo.ToString();
                    products[i, 1] = faultDesc;
                    products[i, 2] = partName;
                    products[i, 3] = amountUsed;
                    products[i, 4] = productBaseCost;
                    products[i, 5] = totalAmount;
                }

                decimal labourFee = totalProductCost * 0.2m; // 20% labour

                string clientName = "";
                string clientAddress = "";
                string clientEmail = "";
                string clientPhone = "";
                string vehicleVIN = "";
                string vehicleName = "";

                using (var conn = DatabaseHelper.OpenConnection())
                using (var cmd = new SqlCommand(@"
    SELECT 
        c.customerName,
        c.customerAddress,
        c.customerEmailAddress,
        c.customerContactNumber,
        v.VIN,
        v.vehicleMake,
        v.vehicleModel,
        v.vehicleYear
    FROM tblQuotation q
    INNER JOIN tblCustomer c ON q.customerID = c.customerID
    INNER JOIN tblVehicle v ON q.vehicleVIN = v.VIN
    WHERE q.quotationNumber = @QNum", conn))
                {
                    cmd.Parameters.AddWithValue("@QNum", quotationNumber);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            clientName = reader["customerName"].ToString();
                            clientAddress = reader["customerAddress"].ToString();
                            clientEmail = reader["customerEmailAddress"].ToString();
                            clientPhone = reader["customerContactNumber"].ToString();

                            vehicleVIN = reader["VIN"].ToString();
                            vehicleName = $"{reader["vehicleMake"]} {reader["vehicleModel"]} ({reader["vehicleYear"]})";
                        }
                    }
                }


                string labourFeeStr = labourFee.ToString("0.00");
                string totalStr = totalProductCost.ToString("0.00");

                // Generate PDF and send to browser
                makeQuotation(products, clientName, clientAddress, clientEmail, clientPhone, vehicleVIN, vehicleName, quotationNumber, labourFeeStr, totalStr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    $"alert('❌ Error generating quotation PDF: {ex.Message}');", true);
            }
        }
    }
}
