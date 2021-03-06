﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.17929.
// 
#pragma warning disable 1591

namespace eStoreServices.com.advantech.buy {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="shippingrateSoap", Namespace="http://tempuri.org/")]
    public partial class shippingrate : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getShippingRateOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public shippingrate() {
            this.Url = global::eStoreServices.Properties.Settings.Default.eStoreServices_com_advantech_buy_shippingrate;
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
        public event getShippingRateCompletedEventHandler getShippingRateCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/getShippingRate", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Response getShippingRate(Order order) {
            object[] results = this.Invoke("getShippingRate", new object[] {
                        order});
            return ((Response)(results[0]));
        }
        
        /// <remarks/>
        public void getShippingRateAsync(Order order) {
            this.getShippingRateAsync(order, null);
        }
        
        /// <remarks/>
        public void getShippingRateAsync(Order order, object userState) {
            if ((this.getShippingRateOperationCompleted == null)) {
                this.getShippingRateOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetShippingRateOperationCompleted);
            }
            this.InvokeAsync("getShippingRate", new object[] {
                        order}, this.getShippingRateOperationCompleted, userState);
        }
        
        private void OngetShippingRateOperationCompleted(object arg) {
            if ((this.getShippingRateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getShippingRateCompleted(this, new getShippingRateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class Order {
        
        private string storeIdField;
        
        private Address shiptoField;
        
        private Address billtoField;
        
        private Item[] itemsField;
        
        private ConfigSystem[] systemsField;
        
        /// <remarks/>
        public string StoreId {
            get {
                return this.storeIdField;
            }
            set {
                this.storeIdField = value;
            }
        }
        
        /// <remarks/>
        public Address Shipto {
            get {
                return this.shiptoField;
            }
            set {
                this.shiptoField = value;
            }
        }
        
        /// <remarks/>
        public Address Billto {
            get {
                return this.billtoField;
            }
            set {
                this.billtoField = value;
            }
        }
        
        /// <remarks/>
        public Item[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
        
        /// <remarks/>
        public ConfigSystem[] Systems {
            get {
                return this.systemsField;
            }
            set {
                this.systemsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class Address {
        
        private string countrycodeField;
        
        private string stateCodeField;
        
        private string zipcodeField;
        
        /// <remarks/>
        public string Countrycode {
            get {
                return this.countrycodeField;
            }
            set {
                this.countrycodeField = value;
            }
        }
        
        /// <remarks/>
        public string StateCode {
            get {
                return this.stateCodeField;
            }
            set {
                this.stateCodeField = value;
            }
        }
        
        /// <remarks/>
        public string Zipcode {
            get {
                return this.zipcodeField;
            }
            set {
                this.zipcodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class Box {
        
        private decimal widthField;
        
        private decimal lengthField;
        
        private decimal heightField;
        
        private decimal weightField;
        
        private Item[] detailsField;
        
        /// <remarks/>
        public decimal Width {
            get {
                return this.widthField;
            }
            set {
                this.widthField = value;
            }
        }
        
        /// <remarks/>
        public decimal Length {
            get {
                return this.lengthField;
            }
            set {
                this.lengthField = value;
            }
        }
        
        /// <remarks/>
        public decimal Height {
            get {
                return this.heightField;
            }
            set {
                this.heightField = value;
            }
        }
        
        /// <remarks/>
        public decimal Weight {
            get {
                return this.weightField;
            }
            set {
                this.weightField = value;
            }
        }
        
        /// <remarks/>
        public Item[] Details {
            get {
                return this.detailsField;
            }
            set {
                this.detailsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConfigSystem))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class Item {
        
        private string productIDField;
        
        private int qtyField;
        
        /// <remarks/>
        public string ProductID {
            get {
                return this.productIDField;
            }
            set {
                this.productIDField = value;
            }
        }
        
        /// <remarks/>
        public int Qty {
            get {
                return this.qtyField;
            }
            set {
                this.qtyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class ConfigSystem : Item {
        
        private Item[] detailsField;
        
        /// <remarks/>
        public Item[] Details {
            get {
                return this.detailsField;
            }
            set {
                this.detailsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class ShippingRate {
        
        private string nmaeField;
        
        private float rateField;
        
        /// <remarks/>
        public string Nmae {
            get {
                return this.nmaeField;
            }
            set {
                this.nmaeField = value;
            }
        }
        
        /// <remarks/>
        public float Rate {
            get {
                return this.rateField;
            }
            set {
                this.rateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class Response {
        
        private string statusField;
        
        private string messageField;
        
        private ShippingRate[] shippingRatesField;
        
        private Box[] boxexField;
        
        /// <remarks/>
        public string Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        public ShippingRate[] ShippingRates {
            get {
                return this.shippingRatesField;
            }
            set {
                this.shippingRatesField = value;
            }
        }
        
        /// <remarks/>
        public Box[] Boxex {
            get {
                return this.boxexField;
            }
            set {
                this.boxexField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void getShippingRateCompletedEventHandler(object sender, getShippingRateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getShippingRateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getShippingRateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Response Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Response)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591