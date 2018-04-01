using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz.SMTP;
using Windows.ApplicationModel.Email;

namespace Hausautomation.Model
{
    public class SendMail
    {
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string NCUsername { get; set; }
        public string NCPassword { get; set; }
        public bool Authentification { get; set; }
        public bool SSL { get; set; }

        public SendMail()
        {
            if (MainPage.settingsPage != null)
            {
                SMTPServer = MainPage.settingsPage.sm.SMTPServer;
                SMTPPort = MainPage.settingsPage.sm.SMTPPort;
                NCUsername = MainPage.settingsPage.sm.NCUsername;
                NCPassword = MainPage.settingsPage.sm.NCPassword;
                Authentification = MainPage.settingsPage.sm.Authentification;
                SSL = MainPage.settingsPage.sm.SSL;
            }
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
                using (SmtpClient client = new SmtpClient(SMTPServer, SMTPPort, SSL, Username, Password))
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
                    emailMessage.Sender.Address = "noreply@gmc.at";
                    emailMessage.Sender.Name = "Hausautomation";
                    await client.SendMailAsync(emailMessage);
                    Debug.WriteLine("Message sent. " + subject + " " + DateTime.Now.ToString());
                    if (MainPage.settingsPage != null)
                        await MainPage.settingsPage.UpdateTitle("Mail sent. " + subject);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendEmail " + ex.Message.ToString());
            }
        }
    }
}
