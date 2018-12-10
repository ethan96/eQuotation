using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.Business
{
    public class BasePage : System.Web.UI.Page
    {
        private bool FIsVerifyRender = true;
        public bool IsVerifyRender
        {
            get { return FIsVerifyRender; }
            set { FIsVerifyRender = value; }
        }
        public override void VerifyRenderingInServerForm(System.Web.UI.Control Control)
        {
            if (this.IsVerifyRender)
            {
                base.VerifyRenderingInServerForm(Control);
            }
        }
        public override bool EnableEventValidation
        {
            get
            {
                if (this.IsVerifyRender)
                {
                    return base.EnableEventValidation;
                }
                else
                {
                    return false;
                }
            }
            set { base.EnableEventValidation = value; }
        }
    }
}
