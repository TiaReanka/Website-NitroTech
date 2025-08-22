using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class Invoices : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnConfirmQuotation_Click(object sender, EventArgs e)
        {
            string quotationNum = quotationNumber.SelectedValue;

            if (string.IsNullOrEmpty(quotationNum))
            {
                Response.Write("<script>alert('Please select a quotation number.');</script>");
                return;
            }

            Response.Write($"<script>alert('Quotation {quotationNum} confirmed.');</script>");
        }

        protected void btnGenerateInvoice_Click(object sender, EventArgs e)
        {
            string quotationNum = quotationNumber.SelectedValue;

            if (string.IsNullOrEmpty(quotationNum))
            {
                Response.Write("<script>alert('Please select a quotation number before generating invoice.');</script>"); 
                return;
            }

            Response.Write($"<script>alert('Generating invoice for {quotationNum}...');</script>");
        }
    }
}