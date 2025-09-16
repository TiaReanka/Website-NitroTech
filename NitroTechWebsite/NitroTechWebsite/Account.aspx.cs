using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Timers;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class Account : System.Web.UI.Page
    {

        private static System.Timers.Timer syncTimer;
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

                // NEW: Store the username in session so it appears in profile dropdown
                Session["UserName"] = username;

                // Redirect everyone to a common landing page (e.g. Menu)
                // Tab visibility will be controlled in Site.Master based on role
                LogQueue.EnqueueLogin(new LoginLogEntry
                {
                    Username = username,
                    Role = role,
                    LoginTime = DateTime.Now,
                    IpAddress = Request.UserHostAddress
                });
                StartLogSync();

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

        private void StartLogSync()
        {
            if (syncTimer != null)
                return; // already running

            syncTimer = new System.Timers.Timer(30000); // every 30 seconds
            syncTimer.Elapsed += (s, e) => SyncLogs();
            syncTimer.AutoReset = true;
            syncTimer.Enabled = true;
        }

        private void SyncLogs()
        {
            string folderPath = HttpContext.Current.Server.MapPath("~/App_Data/Logs");

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 🔹 Process each log queue
                WriteLogs(LogQueue.FlushLoginQueue(), Path.Combine(folderPath, "login_log.json"));
                WriteLogs(LogQueue.FlushLogoffQueue(), Path.Combine(folderPath, "logoff_log.json"));
                WriteLogs(LogQueue.FlushAddUserQueue(), Path.Combine(folderPath, "adduser_log.json"));
                WriteLogs(LogQueue.FlushDeleteUserQueue(), Path.Combine(folderPath, "deleteuser_log.json"));
                WriteLogs(LogQueue.FlushResetPasswordQueue(), Path.Combine(folderPath, "resetpassword_log.json"));
            }
            catch (Exception ex)
            {
                // Log error to Event Viewer or a fallback file
                File.AppendAllText(Path.Combine(folderPath, "error_log.txt"),
                    DateTime.Now + " - Log sync failed: " + ex.Message + Environment.NewLine);
            }
        }
        private void WriteLogs<T>(List<T> newLogs, string filePath)
        {
            if (newLogs.Count == 0)
                return;

            List<T> existingLogs = new List<T>();
            if (File.Exists(filePath))
            {
                string existingJson = File.ReadAllText(filePath);
                existingLogs = JsonConvert.DeserializeObject<List<T>>(existingJson) ?? new List<T>();
            }

            existingLogs.AddRange(newLogs);
            File.WriteAllText(filePath, JsonConvert.SerializeObject(existingLogs, Formatting.Indented));
        }

        public static void LogUserLogoff(HttpSessionStateBase session)
        {
            if (session["UserName"] == null || session["LoginTime"] == null)
                return;

            string username = session["UserName"].ToString();
            DateTime loginTime = (DateTime)session["LoginTime"];
            DateTime logoffTime = DateTime.Now;
            TimeSpan duration = logoffTime - loginTime;

            LogQueue.EnqueueLogoff(new UserLogoffEntry
            {
                Username = username,
                LogoffTime = logoffTime,
                SessionDuration = duration
            });

            session.Clear();
            session.Abandon();
        }
    }
}
