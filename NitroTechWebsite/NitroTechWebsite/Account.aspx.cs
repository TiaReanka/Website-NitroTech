using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class Account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                return;  // do nothing on initial load

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter both username and password.');", true);
                return;
            }

            // Validate against tblUsers

            string hashedPassword = HashPassword(password);

            if (LoginUtility.ValidateUser(username, hashedPassword, Session))
            {
                // Role is already stored in Session["Role"]
                string role = Session["Role"].ToString();

                // Redirect everyone to a common landing page (e.g. Menu)
                // Tab visibility will be controlled in Site.Master based on role
                Response.Redirect("Default.aspx");
                return;
            }
            else
            {
                // Invalid login attempt
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Attempt Invalid.');", true);
                return;
            }
        }



        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
    
}