﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18444.
// 
#pragma warning disable 1591

namespace Advantech.Myadvantech.DataAccess.WSSiebel_AddAction {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ADVWebServoce-AddAction", Namespace="http://siebel.com/CustomUI")]
    public partial class ADVWebServoceAddAction : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback AddActionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ADVWebServoceAddAction() {
            this.Url = global::Advantech.Myadvantech.DataAccess.Properties.Settings.Default.Advantech_Myadvantech_DataAccess_WSSiebel_AddAction_ADVWebServoce_AddAction;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event AddActionCompletedEventHandler AddActionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("document/http://siebel.com/CustomUI:AddAction", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("AddAction_Output", Namespace="http://siebel.com/CustomUI")]
        public AddAction_Output AddAction([System.Xml.Serialization.XmlElementAttribute(Namespace="http://siebel.com/CustomUI")] AddAction_Input AddAction_Input) {
            object[] results = this.Invoke("AddAction", new object[] {
                        AddAction_Input});
            return ((AddAction_Output)(results[0]));
        }
        
        /// <remarks/>
        public void AddActionAsync(AddAction_Input AddAction_Input) {
            this.AddActionAsync(AddAction_Input, null);
        }
        
        /// <remarks/>
        public void AddActionAsync(AddAction_Input AddAction_Input, object userState) {
            if ((this.AddActionOperationCompleted == null)) {
                this.AddActionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddActionOperationCompleted);
            }
            this.InvokeAsync("AddAction", new object[] {
                        AddAction_Input}, this.AddActionOperationCompleted, userState);
        }
        
        private void OnAddActionOperationCompleted(object arg) {
            if ((this.AddActionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddActionCompleted(this, new AddActionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://siebel.com/CustomUI")]
    public partial class AddAction_Input {
        
        private ACT aCTField;
        
        private string sOURCEField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="http://www.siebel.com/xml/ADVActivityIO")]
        public ACT ACT {
            get {
                return this.aCTField;
            }
            set {
                this.aCTField = value;
            }
        }
        
        /// <remarks/>
        public string SOURCE {
            get {
                return this.sOURCEField;
            }
            set {
                this.sOURCEField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVActivityIO")]
    public partial class ACT {
        
        private string sR_ROW_IDField;
        
        private string aLARMField;
        
        private string sRC_ROW_IDField;
        
        private string cOMMENTField;
        
        private string cONTACT_EMAILField;
        
        private string dESPField;
        
        private string dONE_FLAGField;
        
        private string lEAD_ROW_IDField;
        
        private string oPPORTUNITY_IDField;
        
        private string oRGField;
        
        private string oWNER_EMAILField;
        
        private string pLANNED_STARTField;
        
        private string cON_ROW_IDField;
        
        private string sALES_LEADS_FLAGField;
        
        private string sTATUSField;
        
        private string aCT_TYPEField;
        
        /// <remarks/>
        public string SR_ROW_ID {
            get {
                return this.sR_ROW_IDField;
            }
            set {
                this.sR_ROW_IDField = value;
            }
        }
        
        /// <remarks/>
        public string ALARM {
            get {
                return this.aLARMField;
            }
            set {
                this.aLARMField = value;
            }
        }
        
        /// <remarks/>
        public string SRC_ROW_ID {
            get {
                return this.sRC_ROW_IDField;
            }
            set {
                this.sRC_ROW_IDField = value;
            }
        }
        
        /// <remarks/>
        public string COMMENT {
            get {
                return this.cOMMENTField;
            }
            set {
                this.cOMMENTField = value;
            }
        }
        
        /// <remarks/>
        public string CONTACT_EMAIL {
            get {
                return this.cONTACT_EMAILField;
            }
            set {
                this.cONTACT_EMAILField = value;
            }
        }
        
        /// <remarks/>
        public string DESP {
            get {
                return this.dESPField;
            }
            set {
                this.dESPField = value;
            }
        }
        
        /// <remarks/>
        public string DONE_FLAG {
            get {
                return this.dONE_FLAGField;
            }
            set {
                this.dONE_FLAGField = value;
            }
        }
        
        /// <remarks/>
        public string LEAD_ROW_ID {
            get {
                return this.lEAD_ROW_IDField;
            }
            set {
                this.lEAD_ROW_IDField = value;
            }
        }
        
        /// <remarks/>
        public string OPPORTUNITY_ID {
            get {
                return this.oPPORTUNITY_IDField;
            }
            set {
                this.oPPORTUNITY_IDField = value;
            }
        }
        
        /// <remarks/>
        public string ORG {
            get {
                return this.oRGField;
            }
            set {
                this.oRGField = value;
            }
        }
        
        /// <remarks/>
        public string OWNER_EMAIL {
            get {
                return this.oWNER_EMAILField;
            }
            set {
                this.oWNER_EMAILField = value;
            }
        }
        
        /// <remarks/>
        public string PLANNED_START {
            get {
                return this.pLANNED_STARTField;
            }
            set {
                this.pLANNED_STARTField = value;
            }
        }
        
        /// <remarks/>
        public string CON_ROW_ID {
            get {
                return this.cON_ROW_IDField;
            }
            set {
                this.cON_ROW_IDField = value;
            }
        }
        
        /// <remarks/>
        public string SALES_LEADS_FLAG {
            get {
                return this.sALES_LEADS_FLAGField;
            }
            set {
                this.sALES_LEADS_FLAGField = value;
            }
        }
        
        /// <remarks/>
        public string STATUS {
            get {
                return this.sTATUSField;
            }
            set {
                this.sTATUSField = value;
            }
        }
        
        /// <remarks/>
        public string ACT_TYPE {
            get {
                return this.aCT_TYPEField;
            }
            set {
                this.aCT_TYPEField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://siebel.com/CustomUI")]
    public partial class AddAction_Output {
        
        private string error_spcCodeField;
        
        private string error_spcMessageField;
        
        private string rOW_IDField;
        
        private string sTATUSField;
        
        /// <remarks/>
        public string Error_spcCode {
            get {
                return this.error_spcCodeField;
            }
            set {
                this.error_spcCodeField = value;
            }
        }
        
        /// <remarks/>
        public string Error_spcMessage {
            get {
                return this.error_spcMessageField;
            }
            set {
                this.error_spcMessageField = value;
            }
        }
        
        /// <remarks/>
        public string ROW_ID {
            get {
                return this.rOW_IDField;
            }
            set {
                this.rOW_IDField = value;
            }
        }
        
        /// <remarks/>
        public string STATUS {
            get {
                return this.sTATUSField;
            }
            set {
                this.sTATUSField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void AddActionCompletedEventHandler(object sender, AddActionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddActionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AddActionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public AddAction_Output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((AddAction_Output)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591