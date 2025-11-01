using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class ArchiveUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginUtility.EnsureLoggedIn(this, "Director");
        }

        protected void btnArchive_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string currentUser = Session["Name"]?.ToString();

            if (string.IsNullOrEmpty(username))
            {
                lblMessage.Text = "⚠ Please enter a username.";
                return;
            }

            if (!string.IsNullOrEmpty(currentUser) &&
                username.Equals(currentUser, StringComparison.OrdinalIgnoreCase))
            {
                lblMessage.Text = "⚠ You cannot archive your own account.";
                return;
            }

            try
            {
                using (var conn = DatabaseHelper.OpenConnection())
                {
                    // Check if user exists
                    using (var checkCmd = new SqlCommand(
                        "SELECT userActiveStatus FROM tblUsers WHERE Username=@Username", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", username);

                        var result = checkCmd.ExecuteScalar();
                        if (result == null)
                        {
                            lblMessage.Text = "⚠ User does not exist.";
                            return;
                        }

                        if (Convert.ToInt32(result) == 0)
                        {
                            lblMessage.Text = "⚠ User is already deactivated.";
                            return;
                        }
                    }

                    // Deactivate the user
                    using (var updateCmd = new SqlCommand(
                        "UPDATE tblUsers SET userActiveStatus=0 WHERE Username=@Username", conn))
                    {
                        updateCmd.Parameters.AddWithValue("@Username", username);
                        updateCmd.ExecuteNonQuery();
                    }
                }

                //  Log the deletion
                LogQueue.EnqueueDeleteUser(new UserDeleteEntry
                {
                    AdminUsername = Session["Name"]?.ToString() ?? "Unknown",
                    DeletedUser = username,
                    DeleteTime = DateTime.Now
                });

                SyncDeleteLogs();

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "User archived successfully.";
                txtUsername.Text = "";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Error archiving user: " + ex.Message;
            }
        }

        private void SyncDeleteLogs()
        {
            string folderPath = Server.MapPath("~/App_Data/Logs");
            string logFilePath = Path.Combine(folderPath, "administration_log.json");

            try
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var pendingLogs = LogQueue.FlushDeleteUserQueue();
                if (pendingLogs.Count == 0) return;

                List<UserDeleteEntry> existingLogs = new List<UserDeleteEntry>();
                if (File.Exists(logFilePath))
                {
                    string existingJson = File.ReadAllText(logFilePath);
                    existingLogs = JsonConvert.DeserializeObject<List<UserDeleteEntry>>(existingJson)
                                   ?? new List<UserDeleteEntry>();
                }

                existingLogs.AddRange(pendingLogs);
                File.WriteAllText(logFilePath, JsonConvert.SerializeObject(existingLogs, Formatting.Indented));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Log sync failed: " + ex.Message;
            }
        }
    }
}