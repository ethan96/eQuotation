using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using eQuotation.Utility;
using System.Net.Mail;
using System.Configuration;

namespace eQuotation.SupplierApproval
{

    public sealed class SendMail : NativeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> ReceiverEmail { get; set; }
        public InArgument<string> ReceiverName { get; set; }
        public InArgument<string> Content { get; set; }

        public InArgument<string> FlowStatus { get; set; }

        public InArgument<string> BookmarkName { get; set; }
        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(NativeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            //MailHelper.SendMail(context.GetValue(this.Name), context.GetValue(this.Email), context.GetValue(this.Content));

            string strContent = "";
            Email email = new Email();

            strContent = "<p>親愛的 " + context.GetValue(this.ReceiverName) + " </p>";

            if (context.GetValue(this.FlowStatus) == "邀請中")
            {
                email.Subject = string.Format("誠摯的邀請您試用我們最新的供應商平台");
                strContent += "<p>誠摯的邀請您試用我們最新的供應商平台</p>";
                strContent += "請按此處 <a href='http://localhost:62584/#/Vendor/VendorProfile'>註冊</a>";
                strContent += "<br>" + context.GetValue(this.Content);
            }
            else if (context.GetValue(this.FlowStatus) == "等待簽核")
            {
                email.Subject = string.Format("SRM有新廠商等待您簽核中");
                strContent += "<p>您邀請的廠商已經填完相關資料</p>";
                strContent += "請按此處 <a href='http://localhost:62584/#/Vendor/VendorProfile'>進行審核</a>";
                strContent += "<br>" + context.GetValue(this.Content);
            }        
            
            email.MailFrom = "SRM@advantech.com";
            email.MailToAddress = context.GetValue(this.ReceiverEmail);
            email.MailBody += strContent;
            email.SendEmail();

            //2. Create bookmark

            if (BookmarkName.Get(context) != null && BookmarkName.Get(context) !="")
                context.CreateBookmark(BookmarkName.Get(context), new BookmarkCallback(this.OnBookmarkCallback));

        }


        // NativeActivity derived activities that do asynchronous operations by calling   
        // one of the CreateBookmark overloads defined on System.Activities.NativeActivityContext   
        // must override the CanInduceIdle property and return true.  
        protected override bool CanInduceIdle
        {
            get { return true; }
        }


        public void OnBookmarkCallback(NativeActivityContext context, Bookmark bookmark, object obj)
        {

            //this.Name2.Set(context, (string)obj);
            // When the Bookmark is resumed, assign its value to
            // the Result argument.
            //Result.Set(context, (string)obj);
        }

    }



    public class Email
    {
        //private string _smtpMasterHost = ConfigurationManager.AppSettings.Get("MasterSMTP");
        //private string _smtpSlaveHost = ConfigurationManager.AppSettings.Get("SlaveSMTP");

        //private string _mailToAddress = ConfigurationManager.AppSettings.Get("MailTo");
        private string _cc = "";
        public string MailToAddress { get; set; }

        public string CC
        {
            get { return _cc; }
            set { this._cc = value; }
        }

        //private string _mailFrom = ConfigurationManager.AppSettings.Get("MailFrom");
        public string MailFrom { get; set; }

        private string _subject = string.Empty;
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        private string _mailBody = string.Empty;
        public string MailBody
        {
            get { return _mailBody; }
            set { _mailBody = value; }
        }

        public void SendEmail()
        {
            MailMessage mail = new MailMessage();
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;

            mail.Priority = MailPriority.High;
            mail.Subject = _subject;

            MailAddress from = new MailAddress(MailFrom);
            mail.From = from;

            string[] mailTo = MailToAddress.Split(',');
            foreach (string m in mailTo)
            {
                MailAddress addr = new MailAddress(m);
                mail.To.Add(addr);
            }

            string[] cc = _cc.Split(',');
            foreach (string c in cc)
            {
                if (!string.IsNullOrEmpty(c))
                {
                    MailAddress ccAddr = new MailAddress(c);
                    mail.CC.Add(ccAddr);
                }
            }

            mail.IsBodyHtml = true;
            mail.Body = _mailBody;

            SmtpClient masterSMTP = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
            try
            {
                masterSMTP.Send(mail);
            }
            catch (SmtpFailedRecipientsException smtpEx)
            {
                for (int i = 0; i < smtpEx.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = smtpEx.InnerExceptions[i].StatusCode;
                    // If mail server is busy or server is unavailable, mailer will resend mail in 5 minutes.
                    if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                    {
                        System.Threading.Thread.Sleep(100000);
                        masterSMTP.Send(mail);
                    }

                }

            }
            catch
            {
                SmtpClient slaveSMTP = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                try
                {
                    slaveSMTP.Send(mail);
                }
                catch (SmtpFailedRecipientsException smtpEx)
                {
                    for (int i = 0; i < smtpEx.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = smtpEx.InnerExceptions[i].StatusCode;
                        // If mail server is busy or server is unavailable, mailer will resend mail in 5 minutes.
                        if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                        {
                            System.Threading.Thread.Sleep(100000);
                            slaveSMTP.Send(mail);
                        }

                    }

                }
            }
        }
    }
}
