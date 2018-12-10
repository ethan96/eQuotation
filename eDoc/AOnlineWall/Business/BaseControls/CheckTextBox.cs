using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: WebResource("AOnlineWall.Business.BaseControls.CheckTextBox.js", "application/x-javascript", PerformSubstitution = true)]
namespace AOnlineWall.Business.BaseControls
{
    public class CheckTextBox : System.Web.UI.WebControls.TextBox   
    {
        public CheckTextBox()
        {
            this.Attributes.Add("typeX", "textAearBoxCSS");
        }

        public override string Text
        {
            get
            {
                return base.Text.Trim();
            }
            set
            {
                base.Text = value;
            }
        }

        /// <summary>
        /// 去除特殊字符
        /// </summary>
        public string textCode
        {
            get
            {
                return System.Text.RegularExpressions.Regex.Replace(Text, @"[\""®™&%$#*!@^~～！@#￥%……&×]|(<.*?>)", "");
            }
        }

        #region 控件
        private Label c_Label = new Label();
        private Label m_Label = new Label();
        #endregion

        #region 控件自定义属性
        //自定义最大长度
        public Int32 MaxLengthX
        {
            get { return ViewState["OMTextMaxLength"] == null ? 500 : (Int32)ViewState["OMTextMaxLength"]; }
            set { ViewState["OMTextMaxLength"] = value; }
        }
        //自定义最小长度
        public Int32 MixLengthX
        {
            get { return ViewState["OMTextMixLength"] == null ? 0 : (Int32)ViewState["OMTextMixLength"]; }
            set { ViewState["OMTextMixLength"] = value; }
        }
        //样式
        public string CssName
        {
            get { return ViewState["TextCssName"] == null ? "colorRed" : (string)ViewState["TextCssName"]; }
            set { ViewState["TextCssName"] = value; }
        }
        public Boolean MessageIsLine { get; set; }
        #endregion

        #region EnsureChildControls
        protected override void EnsureChildControls()
        {
            //this.c_Label.CssClass = this.CssName;
            if (MixLengthX == 0)
                this.c_Label.Text = " MaxLength: " + this.MaxLengthX.ToString().Trim() + " byte";
            else
                this.c_Label.Text = " MinLength: " + this.MixLengthX.ToString() + "  /  MaxLength: " + this.MaxLengthX.ToString().Trim() + " byte";
            this.c_Label.EnableViewState = true;
            //将子控件添加到此自定义控件中
            this.Controls.Add(c_Label);
            this.m_Label.CssClass = this.CssName;
            this.m_Label.EnableViewState = true;
            //将子控件添加到此自定义控件中
            this.Controls.Add(m_Label);

        }
        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            this.Page.ClientScript.RegisterClientScriptResource(this.GetType(), "AOnlineWall.Business.BaseControls.CheckTextBox.js");
            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter output)
        {
            this.Attributes.Add("MaxLengthX", MaxLengthX.ToString());
            base.Render(output);
            output.Write("&nbsp;");
            if (MessageIsLine)
                output.Write("<br>");
            output.Write("(");
            this.m_Label.ID = base.ID + "_Message";
            this.m_Label.RenderControl(output);
            //output.Write("&nbsp;&nbsp;");
            this.c_Label.ID = "ml" + base.ID;
            this.c_Label.RenderControl(output);
            output.Write(")");
        }
    }
}
