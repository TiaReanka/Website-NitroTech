using Org.BouncyCastle.Asn1.Cmp;
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


            string fromAddress = "zanitrotech@gmail.com";
            string toEmail = "tiarnaidoo2003@gmail.com";  // recipient
            string fromPassword = "JuiceEminem#"; // Use the 16-character app password that is obtained from your Google Security settings
            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toEmail)
            {
                Subject = "Test Message from Google Mail Server",
                Body = $"A new feedback has been submitted:\n\n{feedback}",
                IsBodyHtml = true // Set to true if sending HTML content
            })
                try
                {
                    smtpClient.Send(message);
                    txtFeedback.Text = ""; // clear box
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success",
                        "alert('✔ Thank you for your feedback! Your message has been sent.');", true);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
    $"alert('❌ General Error sending feedback: {ex.Message}');", true);
                }
        }

        protected void txtFeedback_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
