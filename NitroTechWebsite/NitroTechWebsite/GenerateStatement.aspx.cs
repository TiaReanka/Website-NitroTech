using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class Statements : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnGenerateStatement_Click(object sender, EventArgs e)
        {
            string customerId = customerID.SelectedValue;

            if (string.IsNullOrEmpty(customerId))
            {
                Response.Write("<script>alert('Please select a customer ID.');</script>");
                return;
            }

            // TODO: Fetch statement data from DB 
            Response.Write($"<script>alert('Generating statement for {customerId}...');</script>");
        }
    }
}