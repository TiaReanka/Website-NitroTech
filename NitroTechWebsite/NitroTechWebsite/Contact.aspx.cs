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


            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com"); //dont change

                mail.From = new MailAddress("zanitrotech@gmail.com");
                mail.To.Add("zanitrotech@gmail.com");
                mail.Subject = "Feedback Email from NitroTech Systems";
                mail.Body = feedback;

                smtpServer.Port = 587;//gmail port , dont change
                smtpServer.Credentials = new NetworkCredential("zanitrotech@gmail.com", "tbke jmvx ttyg lyob");//dont change
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "success",
                        "alert('✔ Thank you for your feedback! Your message has been sent.');", true);
                txtFeedback.Text = string.Empty; // Clear the textbox after successful submission
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
