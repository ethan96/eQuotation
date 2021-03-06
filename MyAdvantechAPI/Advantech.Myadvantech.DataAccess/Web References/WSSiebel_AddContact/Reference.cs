﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Advantech.Myadvantech.DataAccess.WSSiebel_AddContact {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ADVWebService-AddContact", Namespace="http://siebel.com/CustomUI")]
    public partial class ADVWebServiceAddContact : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback AddContactOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ADVWebServiceAddContact() {
            this.Url = global::Advantech.Myadvantech.DataAccess.Properties.Settings.Default.Advantech_Myadvantech_DataAccess_UpdOppty_ADVWebService_UpdOppty;
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
        public event AddContactCompletedEventHandler AddContactCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("document/http://siebel.com/CustomUI:AddContact", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("AddContact_Output", Namespace="http://siebel.com/CustomUI")]
        public AddContact_Output AddContact([System.Xml.Serialization.XmlElementAttribute(Namespace="http://siebel.com/CustomUI")] AddContact_Input AddContact_Input) {
            object[] results = this.Invoke("AddContact", new object[] {
                        AddContact_Input});
            return ((AddContact_Output)(results[0]));
        }
        
        /// <remarks/>
        public void AddContactAsync(AddContact_Input AddContact_Input) {
            this.AddContactAsync(AddContact_Input, null);
        }
        
        /// <remarks/>
        public void AddContactAsync(AddContact_Input AddContact_Input, object userState) {
            if ((this.AddContactOperationCompleted == null)) {
                this.AddContactOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddContactOperationCompleted);
            }
            this.InvokeAsync("AddContact", new object[] {
                        AddContact_Input}, this.AddContactOperationCompleted, userState);
        }
        
        private void OnAddContactOperationCompleted(object arg) {
            if ((this.AddContactCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddContactCompleted(this, new AddContactCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://siebel.com/CustomUI")]
    public partial class AddContact_Input {
        
        private CON cONField;
        
        private string sOURCEField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
        public CON CON {
            get {
                return this.cONField;
            }
            set {
                this.cONField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
    public partial class CON {
        
        private string iS_ACTIVEField;
        
        private string cELLULARField;
        
        private string cMTField;
        
        private string eMAILField;
        
        private string fAX_PHONEField;
        
        private string fST_NAMEField;
        
        private string dATA_SRCField;
        
        private string iNTEGRATION_IDField;
        
        private string jOB_FUNCTIONField;
        
        private string jOB_TITLEField;
        
        private string lST_NAMEField;
        
        private string mR_MSField;
        
        private string mIDDLE_NAMEField;
        
        private string rOW_IDField;
        
        private string iS_NAVER_CALLField;
        
        private string iS_NEVER_EMAILField;
        
        private string iS_NAVER_MAILField;
        
        private string uPLOAD_FROMField;
        
        private string uSER_TYPEField;
        
        private string wORK_PHONEField;
        
        private ORG[] oRGField;
        
        private POSITION[] pOSITIONField;
        
        private BAA[] bAAField;
        
        private LIST_IP[] lIST_IPField;
        
        private LIST_IN[] lIST_INField;
        
        private LIST_MP[] lIST_MPField;
        
        private ACC[] aCCField;
        
        private TAG[] tAGField;
        
        private SELECTTOPIC[] sELECTTOPICField;
        
        private SIC[] sICField;
        
        /// <remarks/>
        public string IS_ACTIVE {
            get {
                return this.iS_ACTIVEField;
            }
            set {
                this.iS_ACTIVEField = value;
            }
        }
        
        /// <remarks/>
        public string CELLULAR {
            get {
                return this.cELLULARField;
            }
            set {
                this.cELLULARField = value;
            }
        }
        
        /// <remarks/>
        public string CMT {
            get {
                return this.cMTField;
            }
            set {
                this.cMTField = value;
            }
        }
        
        /// <remarks/>
        public string EMAIL {
            get {
                return this.eMAILField;
            }
            set {
                this.eMAILField = value;
            }
        }
        
        /// <remarks/>
        public string FAX_PHONE {
            get {
                return this.fAX_PHONEField;
            }
            set {
                this.fAX_PHONEField = value;
            }
        }
        
        /// <remarks/>
        public string FST_NAME {
            get {
                return this.fST_NAMEField;
            }
            set {
                this.fST_NAMEField = value;
            }
        }
        
        /// <remarks/>
        public string DATA_SRC {
            get {
                return this.dATA_SRCField;
            }
            set {
                this.dATA_SRCField = value;
            }
        }
        
        /// <remarks/>
        public string INTEGRATION_ID {
            get {
                return this.iNTEGRATION_IDField;
            }
            set {
                this.iNTEGRATION_IDField = value;
            }
        }
        
        /// <remarks/>
        public string JOB_FUNCTION {
            get {
                return this.jOB_FUNCTIONField;
            }
            set {
                this.jOB_FUNCTIONField = value;
            }
        }
        
        /// <remarks/>
        public string JOB_TITLE {
            get {
                return this.jOB_TITLEField;
            }
            set {
                this.jOB_TITLEField = value;
            }
        }
        
        /// <remarks/>
        public string LST_NAME {
            get {
                return this.lST_NAMEField;
            }
            set {
                this.lST_NAMEField = value;
            }
        }
        
        /// <remarks/>
        public string MR_MS {
            get {
                return this.mR_MSField;
            }
            set {
                this.mR_MSField = value;
            }
        }
        
        /// <remarks/>
        public string MIDDLE_NAME {
            get {
                return this.mIDDLE_NAMEField;
            }
            set {
                this.mIDDLE_NAMEField = value;
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
        public string IS_NAVER_CALL {
            get {
                return this.iS_NAVER_CALLField;
            }
            set {
                this.iS_NAVER_CALLField = value;
            }
        }
        
        /// <remarks/>
        public string IS_NEVER_EMAIL {
            get {
                return this.iS_NEVER_EMAILField;
            }
            set {
                this.iS_NEVER_EMAILField = value;
            }
        }
        
        /// <remarks/>
        public string IS_NAVER_MAIL {
            get {
                return this.iS_NAVER_MAILField;
            }
            set {
                this.iS_NAVER_MAILField = value;
            }
        }
        
        /// <remarks/>
        public string UPLOAD_FROM {
            get {
                return this.uPLOAD_FROMField;
            }
            set {
                this.uPLOAD_FROMField = value;
            }
        }
        
        /// <remarks/>
        public string USER_TYPE {
            get {
                return this.uSER_TYPEField;
            }
            set {
                this.uSER_TYPEField = value;
            }
        }
        
        /// <remarks/>
        public string WORK_PHONE {
            get {
                return this.wORK_PHONEField;
            }
            set {
                this.wORK_PHONEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ORG")]
        public ORG[] ORG {
            get {
                return this.oRGField;
            }
            set {
                this.oRGField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("POSITION")]
        public POSITION[] POSITION {
            get {
                return this.pOSITIONField;
            }
            set {
                this.pOSITIONField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BAA")]
        public BAA[] BAA {
            get {
                return this.bAAField;
            }
            set {
                this.bAAField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LIST_IP")]
        public LIST_IP[] LIST_IP {
            get {
                return this.lIST_IPField;
            }
            set {
                this.lIST_IPField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LIST_IN")]
        public LIST_IN[] LIST_IN {
            get {
                return this.lIST_INField;
            }
            set {
                this.lIST_INField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LIST_MP")]
        public LIST_MP[] LIST_MP {
            get {
                return this.lIST_MPField;
            }
            set {
                this.lIST_MPField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ACC")]
        public ACC[] ACC {
            get {
                return this.aCCField;
            }
            set {
                this.aCCField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TAG")]
        public TAG[] TAG {
            get {
                return this.tAGField;
            }
            set {
                this.tAGField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SELECTTOPIC")]
        public SELECTTOPIC[] SELECTTOPIC {
            get {
                return this.sELECTTOPICField;
            }
            set {
                this.sELECTTOPICField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SIC")]
        public SIC[] SIC {
            get {
                return this.sICField;
            }
            set {
                this.sICField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
    public partial class ORG {
        
        private string nAMEField;
        
        private string iS_PRIMARYField;
        
        /// <remarks/>
        public string NAME {
            get {
                return this.nAMEField;
            }
            set {
                this.nAMEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IS_PRIMARY {
            get {
                return this.iS_PRIMARYField;
            }
            set {
                this.iS_PRIMARYField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
    public partial class SIC {
        
        private string nAMEField;
        
        private string cODEField;
        
        private string iS_PRIMARYField;
        
        /// <remarks/>
        public string NAME {
            get {
                return this.nAMEField;
            }
            set {
                this.nAMEField = value;
            }
        }
        
        /// <remarks/>
        public string CODE {
            get {
                return this.cODEField;
            }
            set {
                this.cODEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IS_PRIMARY {
            get {
                return this.iS_PRIMARYField;
            }
            set {
                this.iS_PRIMARYField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
    public partial class SELECTTOPIC {
        
        private string tAGField;
        
        /// <remarks/>
        public string TAG {
            get {
                return this.tAGField;
            }
            set {
                this.tAGField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
    public partial class TAG {
        
        private string nAMEField;
        
        private string cOMMENTField;
        
        /// <remarks/>
        public string NAME {
            get {
                return this.nAMEField;
            }
            set {
                this.nAMEField = value;
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
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
    public partial class ACC {
        
        private string aCC_ROW_IDField;
        
        /// <remarks/>
        public string ACC_ROW_ID {
            get {
                return this.aCC_ROW_IDField;
            }
            set {
                this.aCC_ROW_IDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
    public partial class LIST_MP {
        
        private string nAMEField;
        
        /// <remarks/>
        public string NAME {
            get {
                return this.nAMEField;
            }
            set {
                this.nAMEField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
    public partial class LIST_IN {
        
        private string nAMEField;
        
        /// <remarks/>
        public string NAME {
            get {
                return this.nAMEField;
            }
            set {
                this.nAMEField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
    public partial class LIST_IP {
        
        private string nAMEField;
        
        /// <remarks/>
        public string NAME {
            get {
                return this.nAMEField;
            }
            set {
                this.nAMEField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
    public partial class BAA {
        
        private string nAMEField;
        
        /// <remarks/>
        public string NAME {
            get {
                return this.nAMEField;
            }
            set {
                this.nAMEField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.siebel.com/xml/ADVContactIO")]
    public partial class POSITION {
        
        private string nAMEField;
        
        private string iS_PRIMARYField;
        
        /// <remarks/>
        public string NAME {
            get {
                return this.nAMEField;
            }
            set {
                this.nAMEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IS_PRIMARY {
            get {
                return this.iS_PRIMARYField;
            }
            set {
                this.iS_PRIMARYField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2117.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://siebel.com/CustomUI")]
    public partial class AddContact_Output {
        
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    public delegate void AddContactCompletedEventHandler(object sender, AddContactCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddContactCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AddContactCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public AddContact_Output Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((AddContact_Output)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591