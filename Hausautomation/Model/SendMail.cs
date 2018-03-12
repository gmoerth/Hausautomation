using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;

namespace Hausautomation.Model
{
    class SendMail
    {
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
        public string NCUsername { get; set; }
        public string NCPassword { get; set; }
        public bool Authentification { get; set; }
        public bool SSL { get; set; }

        private static MainPage MainWindow;
        public void SetMainWindow(MainPage value)
        {
            MainWindow = value;
        }

        public async void SendEmail(string mailto, string subject, string body)
        {
            try
            {
                string Username = null, Password = null;
                if (Authentification)
                {
                    Username = NCUsername;
                    Password = NCPassword;
                }
                // SMTPPort = 25 ohne SSL || 465 mit SSL || 587 ohne SSL
                using (@SmtpClient client = new SmtpClient(SMTPHost, SMTPPort, SSL, Username, Password))
                {
                    EmailMessage emailMessage = new EmailMessage();
                    if (mailto.Contains(',') == true)
                    {
                        // Mehrerer Empfänger per CC
                        string[] mailtoar;
                        mailtoar = mailto.Split(',');
                        emailMessage.To.Add(new EmailRecipient(mailtoar[0]));
                        for (int i = 1; i < mailtoar.Length; i++)
                            emailMessage.CC.Add(new EmailRecipient(mailtoar[i]));
                    }
                    else
                    {
                        // 1 Empfänger
                        emailMessage.To.Add(new EmailRecipient(mailto));
                    }
                    emailMessage.Subject = subject;
                    emailMessage.Body = body;
                    emailMessage.Sender.Address = "noreply@fwm.at";
                    emailMessage.Sender.Name = "Fritz_WLAN_Monitoring";
                    await client.SendMailAsync(emailMessage);
                    Debug.WriteLine("Message sent. " + subject + " " + DateTime.Now.ToString());
                    if (MainWindow != null)
                        await MainWindow.UpdateTitle("Mail sent. " + subject);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendEmail " + ex.Message.ToString());
            }
        }
    }
}
