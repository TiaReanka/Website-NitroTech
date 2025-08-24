using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetMenuVisibility();
            }
        }
        private void SetMenuVisibility()
        {
            bool loggedIn = Session["UserId"] != null;
            string role = loggedIn ? Session["Role"]?.ToString() ?? "" : "";

            // Hide login when logged in, show logout only when logged in
            liLoginOption.Visible = !loggedIn;
            liLogout.Visible = loggedIn;

            // Default: hide restricted items
            
            liInvoices.Visible = false;
            liStatements.Visible = false;
            liQuotations.Visible = false;
            liCustomers.Visible = false;
            liPayments.Visible = false;
            liParts.Visible = false;
            liAddUser.Visible = false;
            liArchiveUser.Visible = false;


            // Role-specific access
            switch (role.ToLower())
            {
                case "director":
                    liInvoices.Visible = true;
                    liStatements.Visible = true;
                    liQuotations.Visible = true;
                    liCustomers.Visible = true;
                    liPayments.Visible = true;
                    liParts.Visible = true;
                    liAddUser.Visible = true;
                    liArchiveUser.Visible = true;
                    break;
                case "manager":
                    liInvoices.Visible = true;
                    liStatements.Visible = true;
                    liQuotations.Visible = true;
                    liCustomers.Visible = true;
                    liPayments.Visible = true;
                    liParts.Visible = true;
                    break;
                case "admin":
                    liInvoices.Visible = true;
                    liStatements.Visible = true;
                    liQuotations.Visible = true;
                    liCustomers.Visible = true;
                    liPayments.Visible = true;
                    liReviewStatement.Visible = false; // admin cannot review statements
                    liReviewQuotation.Visible = false; // admin cannot review quotations
                    break;
                case "clerk":
                    // minimal access
                    liQuotations.Visible = true;
                    liCustomers.Visible = true;
                    liReviewQuotation.Visible = false; // clerk cannot review quotations
                    break;
            }
        }
    }
}