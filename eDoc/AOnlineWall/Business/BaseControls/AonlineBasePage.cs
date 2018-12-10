using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace AOnlineWall.Business.BaseControls
{
    public class AonlineBasePage : System.Web.UI.Page
    {
        #region Property
        public Dictionary<string, string> MessageBoxs = new Dictionary<string, string>();

        public void ShowMessage(string key, string value)
        {
            if (MessageBoxs.ContainsKey(key))
                MessageBoxs[key] = MessageBoxs[key] + "<br>" + value;
            else
                MessageBoxs.Add(key, value);
        }
        #endregion
        

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (MessageBoxs.Any())
            {
                if (MessageBoxs.Count > 1)
                {
                    string message = string.Join("<br/>", MessageBoxs.Values);
                    dialogMessage("Message", message);
                }
                else
                {
                    var message = MessageBoxs.LastOrDefault();
                    dialogMessage(message.Key, message.Value);
                }
            }
            base.Render(writer);
        }

        /// <summary>
        /// 弹出提示消息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        protected virtual void dialogMessage(string title, string message)
        {
            Literal ltMessage = new Literal();
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='dialog-modal' title='" + title + "'>");
            sb.Append("<p>" + message + "</p>");
            sb.Append("</div>");
            sb.Append("<script type='text/javascript'>");
            sb.Append("$(function() {");
            sb.Append("$( '#dialog-modal' ).dialog({ width:465, height: 200, modal: true });");
            sb.Append("});");
            sb.Append("</script>");
            ltMessage.Text = sb.ToString();
            this.Form.Controls.AddAt(this.Form.Controls.Count, ltMessage);
        }

        /// <summary>
        /// 从Mail中获取名称
        /// </summary>
        /// <param name="emailName"></param>
        /// <returns></returns>
        public string replaceEmailName(string emailName)
        {
            if (!string.IsNullOrEmpty(emailName) && emailName.Contains("@"))
                return emailName.Substring(0, emailName.IndexOf("@"));
            else
                return emailName;
        }

        /// <summary>
        /// Format email 格式
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        protected string splitEmail(string email)
        {
            if (email.Contains("@"))
            {
                string[] mls = email.Split('@');
                return mls[0] + "<br />@" + mls[1];
            }
            else
                return email;
        }
    }
}
