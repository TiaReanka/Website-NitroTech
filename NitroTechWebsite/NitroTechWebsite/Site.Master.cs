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
                ShowProfileIcon(); // show profile icon if logged in
                                   // In Page_Load
                

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
                    liReviewStatement.Visible = false; 
                    liReviewQuotation.Visible = false; 
                    break;
                case "clerk":
                    liQuotations.Visible = true;
                    liCustomers.Visible = true;
                    liReviewQuotation.Visible = false; 
                    break;
                default:
                    break; // no access to restricted items
            }
        }

        // NEW: Show profile icon if user is logged in
        private void ShowProfileIcon()
        {
            if (Session["UserId"] != null)
            {
                profileIcon.Visible = true;
                lblProfileName.InnerText = $"{Session["UserName"]} ({Session["Role"]})";
            }
            else
            {
                profileIcon.Visible = false;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Account.LogUserLogoff(new HttpSessionStateWrapper(Session));
            // Clear session
            Session.Clear();
            Session.Abandon();

            // Optional: clear authentication cookie if using Forms Authentication
            if (Request.Cookies[".ASPXAUTH"] != null)
            {
                var cookie = new HttpCookie(".ASPXAUTH");
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }

            // Redirect to home or login page
            Response.Redirect("~/Default.aspx");
        }
    }
}

