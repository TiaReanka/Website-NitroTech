using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
                ShowProfileIcon(); 
            }
            if (Session["UserId"] != null)
            {
                var link = liForgotPassword.FindControl("forgotPasswordLink") as HtmlAnchor;
                if (link != null)
                {
                    link.InnerText = "Reset Password";
                }
                else
                {
                    link.InnerText = "Forgot Password";
                }

                string role = Session["Role"]?.ToString() ?? "";

                if (role.Equals("Director", StringComparison.OrdinalIgnoreCase) ||
                    role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                {
                    // 1️⃣ Query for low stock parts using DatabaseHelper
                    string sql = @"
                    SELECT partName 
                    FROM dbo.tblParts
                    WHERE partReOrderLevel >= partQuantity;";

                    DataTable lowStockParts = new DataTable();

                    using (var conn = DatabaseHelper.OpenConnection())
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(lowStockParts);
                    }

                    // 2️⃣ If there are low stock parts, show a browser alert
                    if (lowStockParts.Rows.Count > 0)
                    {
                        var partNames = lowStockParts.AsEnumerable()
                                                     .Select(row => row.Field<string>("partName"))
                                                     .ToList();

                        string message = "The following parts are low in stock:\\n- " + string.Join("\\n- ", partNames);

                        string script = $@"
                        <script type='text/javascript'>
                            setTimeout(function() {{
                                alert('{message}');
                            }}, 800);
                        </script>";

                       // ClientScript.RegisterStartupScript(this.GetType(), "LowStockAlert", script);
                    }
                }

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
            liPowerBI.Visible = false;

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
                    liPowerBI.Visible = true;
                    break;
                case "manager":
                    liInvoices.Visible = true;
                    liStatements.Visible = true;
                    liQuotations.Visible = true;
                    liCustomers.Visible = true;
                    liPayments.Visible = true;
                    liParts.Visible = true;
                    liPowerBI.Visible = true;
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

