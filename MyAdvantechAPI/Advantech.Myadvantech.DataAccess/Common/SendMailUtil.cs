using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Sgml;

namespace Advantech.Myadvantech.DataAccess.Common
{
    public class SendMailUtil
    {
        public static void SendSystemMessagebyMail(string MailSubject, string MailBody, string MailTo, Boolean IsBodyHtml)
        {
            System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient("172.20.0.76");

            MailMessage newEmail = new MailMessage();
            newEmail.From = new MailAddress("myadvantech@advantech.com");
            newEmail.To.Add(new MailAddress("myadvantech@advantech.com"));
            newEmail.Subject = MailSubject;
            newEmail.Body = MailBody;
            newEmail.IsBodyHtml = IsBodyHtml;
            newEmail.Priority = MailPriority.Normal;
            smtpClient1.Send(newEmail);

        }

        public static void SendMail(String _MailTo, String _MailSubject, String _MailBody)
        {
            System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
            smtpClient1.Send("MyAdvantech@advantech.com", _MailTo, _MailSubject, _MailBody + "\r\nTime: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        }

        public static void SendMail(string MailFrom, string MailTo, string MailCC, string MailBCC, string MailSubject, string MailBody, Boolean IsBodyHtml,  Byte[] AttachedFile = null, String AttachedName = "")
        {
            System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient("172.20.0.76");

            MailMessage newEmail = new MailMessage();
            newEmail.From = new MailAddress(MailFrom);

            List<String> toEmail = MailTo.Trim().Split(';').ToList();
            if (toEmail != null && toEmail.Count > 0)
            {
                foreach (String s in toEmail)
                {
                    newEmail.To.Add(s);
                }
            }
            List<String> CCEmail = MailCC.Trim().Split(';').ToList();
            if (CCEmail != null && CCEmail.Count > 0)
            {
                foreach (String s in CCEmail)
                {
                    newEmail.CC.Add(s);
                }
            }

            List<String> BCCEmail = MailBCC.Trim().Split(';').ToList();
            if (BCCEmail != null && BCCEmail.Count > 0)
            {
                foreach (String s in BCCEmail)
                {
                    newEmail.Bcc.Add(s);
                }
            }


            newEmail.Subject = MailSubject;

            SgmlReader mysgmlReader = new SgmlReader();
            System.Xml.XmlDocument XMLDOC = new System.Xml.XmlDocument();
            mysgmlReader.DocType = "HTML";
            mysgmlReader.WhitespaceHandling = System.Xml.WhitespaceHandling.All;
            mysgmlReader.CaseFolding = CaseFolding.ToLower;
            mysgmlReader.InputStream = new System.IO.StringReader(MailBody);
            XMLDOC.PreserveWhitespace = true;
            XMLDOC.XmlResolver = null;
            XMLDOC.Load(mysgmlReader);

            newEmail.Body = XMLDOC.OuterXml;
            newEmail.IsBodyHtml = IsBodyHtml;
            newEmail.Priority = MailPriority.Normal;

            if (AttachedFile != null)
            {
                Attachment attachment = new Attachment(new System.IO.MemoryStream(AttachedFile), AttachedName);
                newEmail.Attachments.Add(attachment);
            }

            smtpClient1.Send(newEmail);
        }
    }
}
