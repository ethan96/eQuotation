using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowlAPI.Utility
{
    public static class MailHelper
    {
        public static void SendMail(string emailAddress, string subject, string mailbody)
        {
            var oMail = new System.Net.Mail.MailMessage();
            oMail.From = new System.Net.Mail.MailAddress("myadvantech@advantech.com");
            var toEmail = emailAddress.Split(';');
            foreach (var email in toEmail)
            {
                if (!string.IsNullOrWhiteSpace(email))
                {
                    oMail.To.Add(email);
                }
            }
            //oMail.CC.Add("myadvantech@advantech.com");


            oMail.Subject = subject;
            oMail.IsBodyHtml = true;
            oMail.Body = "<div>" + mailbody + "</div>";



            string smtp = ConfigurationManager.AppSettings["SMTPServer"];
            var oSmtp = new System.Net.Mail.SmtpClient(smtp);
            try
            {
                oSmtp.Send(oMail);
            }
            catch
            {
                try
                {
                    smtp = ConfigurationManager.AppSettings["SMTPServerBAK"];
                    oSmtp = new System.Net.Mail.SmtpClient(smtp);
                    oSmtp.Send(oMail);
                }
                catch { }
            }

        }
    }
}
