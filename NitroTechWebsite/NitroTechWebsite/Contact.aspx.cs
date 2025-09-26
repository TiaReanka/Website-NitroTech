using System;
using System.Net;
using System.Net.Mail;
using System.Web.UI;

namespace NitroTechWebsite
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmitFeedback_Click(object sender, EventArgs e)
        {
            string feedback = txtFeedback.Text.Trim();

            if (string.IsNullOrEmpty(feedback))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "emptyFeedback",
                    "alert('Please enter your feedback before submitting.');", true);
                return;
            }

            try
            {
                // --- Outlook account details ---
                string fromEmail = "autoengineeringfeedback@outlook.com"; // sender Outlook email
                string fromPassword = "Engineering4"; // or App Password
                string toEmail = "tiarnaidoo2003@gmail.com";  // recipient

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(toEmail);
                    mail.Subject = "New Feedback Submitted";
                    mail.Body = $"A new feedback has been submitted:\n\n{feedback}";

                    using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        smtp.Send(mail);
                    }
                }

                txtFeedback.Text = ""; // clear box
                ScriptManager.RegisterStartupScript(this, this.GetType(), "success",
                    "alert('✔ Thank you for your feedback! Your message has been sent.');", true);
            }
            catch (SmtpException smtpEx)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                    $"alert('❌ SMTP Error sending feedback: {smtpEx.Message}');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                    $"alert('❌ General Error sending feedback: {ex.Message}');", true);
            }
        }
    }
}
