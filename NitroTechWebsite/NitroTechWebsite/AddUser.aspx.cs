using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class AddUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    //    protected void btnAddUser_Click(object sender, EventArgs e)
    //    {
    //        string username = txtUsername.Text.Trim();
    //        string password = txtPassword.Text.Trim();
    //        string confirmPassword = txtConfirmPassword.Text.Trim();
    //        string role = ddlRoles.SelectedValue;
    //        string securityQuestion = txtSecurityQuestion.Text.Trim();
    //        string securityAnswer = txtSecurityAnswer.Text.Trim();

    //        if (isValidChange(username, password, confirmPassword, role))
    //        {
    //            try
    //            {
    //                string hashedPassword = HashText(password);
    //                string hashedSQAnswer = HashText(securityAnswer);

    //                // Insert into DB using DatabaseHelper
    //                using (var conn = DatabaseHelper.OpenConnection())
    //                using (var cmd = new SqlCommand(@"
    //                            INSERT INTO tblUsers (Username, userPassword, userRole, userActiveStatus, SecurityQuestion, SecurityAnswer)
    //                            VALUES (@Username, @Password, @Role, 1, @SecurityQuestion, @SecurityAnswer)", conn))
    //                {
    //                    cmd.Parameters.AddWithValue("@Username", username);
    //                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
    //                    cmd.Parameters.AddWithValue("@Role", role);
    //                    cmd.Parameters.AddWithValue("@SecurityQuestion", securityQuestion);
    //                    cmd.Parameters.AddWithValue("@SecurityAnswer", hashedSQAnswer);

    //                    cmd.ExecuteNonQuery();
    //                }

    //                lblMessage.Text = "User registered successfully!";
    //                LogUser(username);
    //                SyncLogs();

    //                ClearFields();
    //            }
    //            catch (Exception ex)
    //            {
    //                lblMessage.Text = "Error adding user: " + ex.Message;
    //            }
    //        }
    //    }

    //    private string HashText(string input)
    //    {
    //        using (SHA256 sha256 = SHA256.Create())
    //        {
    //            byte[] bytes = Encoding.UTF8.GetBytes(input);
    //            byte[] hashBytes = sha256.ComputeHash(bytes);
    //            return Convert.ToBase64String(hashBytes);
    //        }
    //    }

    //    private static bool isValidPassword(string newPassword, string confirmPassword)
    //    {
    //        if (string.IsNullOrEmpty(newPassword)) return false;
    //        if (newPassword.Length < 8) return false;
    //        if (!Regex.IsMatch(newPassword, @"\d")) return false;
    //        if (!Regex.IsMatch(newPassword, @"[\W_]")) return false;
    //        if (!Regex.IsMatch(newPassword, @"[A-Z]")) return false;
    //        if (Regex.IsMatch(newPassword, @"\s")) return false;
    //        if (newPassword != confirmPassword) return false;
    //        return true;
    //    }

    //    private bool isValidChange(string username, string newPassword, string confirmPassword, string role)
    //    {
    //        if (!isNullCheck())
    //        {
    //            lblMessage.Text = "Please fill in all fields.";
    //            return false;
    //        }

    //        // Check if username exists
    //        using (var conn = DatabaseHelper.OpenConnection())
    //        using (var cmd = new SqlCommand("SELECT COUNT(*) FROM tblUsers WHERE Username=@Username", conn))
    //        {
    //            cmd.Parameters.AddWithValue("@Username", username);
    //            int count = (int)cmd.ExecuteScalar();
    //            if (count > 0)
    //            {
    //                lblMessage.Text = "That username is already taken.";
    //                return false;
    //            }
    //        }

    //        if (!isValidPassword(newPassword, confirmPassword))
    //        {
    //            lblMessage.Text = "Invalid password. Must be at least 8 chars, contain a digit, special char, uppercase, and match confirm password.";
    //            return false;
    //        }

    //        return true;
    //    }

    //    private bool isNullCheck()
    //    {
    //        return !(string.IsNullOrEmpty(txtUsername.Text) ||
    //                 string.IsNullOrEmpty(txtPassword.Text) ||
    //                 string.IsNullOrEmpty(txtSecurityQuestion.Text) ||
    //                 string.IsNullOrEmpty(txtSecurityAnswer.Text) ||
    //                 string.IsNullOrEmpty(txtConfirmPassword.Text) ||
    //                 ddlRoles.SelectedIndex == -1);
    //    }

    //    private void ClearFields()
    //    {
    //        txtUsername.Text = "";
    //        txtPassword.Text = "";
    //        txtConfirmPassword.Text = "";
    //        txtSecurityAnswer.Text = "";
    //        txtSecurityQuestion.Text = "";
    //        ddlRoles.SelectedIndex = -1;
    //    }

    //    private void SyncLogs()
    //    {
    //        string folderPath = Server.MapPath("~/Logs");
    //        string logFilePath = Path.Combine(folderPath, "administration_log.json");

    //        try
    //        {
    //            var pendingLogs = LogQueue.FlushQueue();
    //            if (pendingLogs.Count == 0) return;

    //            if (!Directory.Exists(folderPath))
    //            {
    //                Directory.CreateDirectory(folderPath);
    //            }

    //            List<UserLogEntry> existingLogs = new List<UserLogEntry>();
    //            if (File.Exists(logFilePath))
    //            {
    //                string existingJson = File.ReadAllText(logFilePath);
    //                existingLogs = JsonConvert.DeserializeObject<List<UserLogEntry>>(existingJson)
    //                               ?? new List<UserLogEntry>();
    //            }

    //            existingLogs.AddRange(pendingLogs);
    //            File.WriteAllText(logFilePath, JsonConvert.SerializeObject(existingLogs, Formatting.Indented));
    //        }
    //        catch (Exception ex)
    //        {
    //            lblMessage.Text = "Log sync failed: " + ex.Message;
    //        }
    //    }

    //    public class UserLogEntry
    //    {
    //        public DateTime Timestamp { get; set; }
    //        public string Username { get; set; }
    //        public string Action { get; set; }
    //    }

    //    private void LogUser(string username)
    //    {
    //        LogQueue.EnqueueAddUser(new LogQueue.UserAddEntry
    //        {
    //            AdminUsername = Session["Name"]?.ToString() ?? "Unknown",
    //            NewUser = username,
    //            AddTime = DateTime.Now
    //        });
    //    }
    }
}