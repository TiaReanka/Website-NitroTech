using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class NewPassword : System.Web.UI.Page
    {
        bool isLoggedIn = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                bool isLoggedIn = Session["UserId"] != null;

                if (isLoggedIn)
                {
                    // User logged in → show readonly username and both options
                    username.Value = Session["Name"].ToString();
                    username.Attributes["readonly"] = "readonly";
                    loadSQButton.Visible = false;
                    oldPasswordRadio.Disabled = false;
                    oldPasswordRadio.Checked = false;
                    securityQuestionRadio.Checked = true;
                }
                else
                {
                    // User not logged in → must use security question
                    username.Value = "";
                    username.Attributes.Remove("readonly");
                    loadSQButton.Visible = true;
                    oldPasswordRadio.Disabled = true;         
                    oldPasswordRadio.Checked = false;
                    securityQuestionRadio.Checked = true;
                    oldPassword.Attributes["disabled"] = "disabled";
                    btnChangePassword.Attributes["disabled"] = "disabled";   // Disable
                }
            }
        }

        protected void LoadSecurityQuestion_Click(object sender, EventArgs e)
        {
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand("SELECT userSecurityQuestion FROM tblUsers WHERE Username=@u", conn))
            {
                cmd.Parameters.AddWithValue("@u", username.Value.Trim());
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    securityQuestionLabel.Text = "Please answer your security question: " + result.ToString();
                    btnChangePassword.Attributes.Remove("disabled"); // Enable
                }
                else
                {
                    ShowMessage("User not found.");
                }
            }
        }

        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            string uName = username.Value.Trim();
            string newPass = newPassword.Value;
            string confirmPass = confirmPassword.Value;
            string inputAnswer = securityAnswer.Value;

            if (string.IsNullOrEmpty(uName) || string.IsNullOrEmpty(newPass) ||
                string.IsNullOrEmpty(confirmPass) || string.IsNullOrEmpty(inputAnswer))
            {
                ShowMessage("Please fill in all fields.");
                return;
            }

            if (newPass != confirmPass)
            {
                ShowMessage("Passwords do not match.");
                return;
            }

            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand("SELECT userPassword, userSQAnswer, userActiveStatus FROM tblUsers WHERE Username=@u", conn))
            {
                cmd.Parameters.AddWithValue("@u", uName);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        ShowMessage("User not found.");
                        return;
                    }

                    if (!(bool)reader["userActiveStatus"])
                    {
                        ShowMessage("Account is inactive.");
                        return;
                    }

                    string dbPassword = reader["userPassword"].ToString();
                    string dbAnswer = reader["userSQAnswer"].ToString();

                    if (oldPasswordRadio.Checked == true)
                    {
                        if (HashPassword(inputAnswer) != dbPassword)
                        {
                            ShowMessage("Old password is incorrect.");
                            return;
                        }
                    }
                    else if (securityQuestionRadio.Checked == true)
                    {
                        if (!string.Equals(inputAnswer, dbAnswer, StringComparison.OrdinalIgnoreCase))
                        {
                            ShowMessage("Security answer incorrect.");
                            return;
                        }
                    }
                }
            }

            // Validate new password
            if (!IsValidPassword(newPass))
            {
                ShowMessage("Password must be at least 8 characters, include uppercase, digit, and special character.");
                return;
            }

            // Update password
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand("UPDATE tblUsers SET userPassword=@p WHERE Username=@u", conn))
            {
                cmd.Parameters.AddWithValue("@u", uName);
                cmd.Parameters.AddWithValue("@p", HashPassword(newPass));
                cmd.ExecuteNonQuery();
            }

            Session.Clear();     
            Session.Abandon();

            // Show success + redirect
            ShowMessageAndRedirect("Password updated successfully!", "Account.aspx");
        }

        private void ShowMessage(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{message}');", true);
        }

        private void ShowMessageAndRedirect(string message, string redirectUrl)
        {
            string script = $"alert('{message}'); window.location='{redirectUrl}';";
            ClientScript.RegisterStartupScript(this.GetType(), "alertRedirect", script, true);
        }

        private string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 &&
                   System.Text.RegularExpressions.Regex.IsMatch(password, @"\d") &&
                   System.Text.RegularExpressions.Regex.IsMatch(password, @"[\W_]") &&
                   System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]") &&
                   !password.Contains(" ");
        }

        protected void OptionChanged(object sender, EventArgs e)
        {
            if (oldPasswordRadio.Checked)
            {
                oldPassword.Attributes.Remove("disabled");
                securityAnswer.Disabled = true;
                securityQuestionRadio.Checked = false;
                btnChangePassword.Attributes.Remove("disabled"); // Enable

            }
            else if (securityQuestionRadio.Checked)
            {
                oldPassword.Attributes["disabled"] = "disabled";
                btnChangePassword.Attributes["disabled"] = "disabled";   // Disable
                securityAnswer.Disabled = false;
                oldPasswordRadio.Checked = false;

            }
        }
    }
}