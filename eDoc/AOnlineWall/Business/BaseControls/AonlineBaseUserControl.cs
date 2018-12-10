using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace AOnlineWall.Business.BaseControls
{
    public class AonlineBaseUserControl : UserControl
    {
        #region Property
        private Dictionary<string,string> _messageBoxs;
        public Dictionary<string,string> MessageBoxs
        {
            get {
                if (_messageBoxs == null)
                {
                    if (this.Page is AonlineBasePage)
                        _messageBoxs = ((AonlineBasePage)this.Page).MessageBoxs;
                }
                return _messageBoxs;
            }
        }

        public void ShowMessage(string key, string value)
        {
            if (MessageBoxs != null)
            {
                if (MessageBoxs.ContainsKey(key))
                    MessageBoxs[key] = MessageBoxs[key] + "<br>" + value;
                else
                    MessageBoxs.Add(key, value);
            }
        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
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
