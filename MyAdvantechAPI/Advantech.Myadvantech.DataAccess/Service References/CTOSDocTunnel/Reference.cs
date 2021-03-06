﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Advantech.Myadvantech.DataAccess.CTOSDocTunnel {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CTOSDocItem", Namespace="http://schemas.datacontract.org/2004/07/Advantech.Mes.Ctos.Services.CTOS")]
    [System.SerializableAttribute()]
    public partial class CTOSDocItem : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustomerPartNOField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DocumentIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DocumentNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FAENameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StandardPartNOField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CustomerPartNO {
            get {
                return this.CustomerPartNOField;
            }
            set {
                if ((object.ReferenceEquals(this.CustomerPartNOField, value) != true)) {
                    this.CustomerPartNOField = value;
                    this.RaisePropertyChanged("CustomerPartNO");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DocumentID {
            get {
                return this.DocumentIDField;
            }
            set {
                if ((object.ReferenceEquals(this.DocumentIDField, value) != true)) {
                    this.DocumentIDField = value;
                    this.RaisePropertyChanged("DocumentID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DocumentName {
            get {
                return this.DocumentNameField;
            }
            set {
                if ((object.ReferenceEquals(this.DocumentNameField, value) != true)) {
                    this.DocumentNameField = value;
                    this.RaisePropertyChanged("DocumentName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FAEName {
            get {
                return this.FAENameField;
            }
            set {
                if ((object.ReferenceEquals(this.FAENameField, value) != true)) {
                    this.FAENameField = value;
                    this.RaisePropertyChanged("FAEName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StandardPartNO {
            get {
                return this.StandardPartNOField;
            }
            set {
                if ((object.ReferenceEquals(this.StandardPartNOField, value) != true)) {
                    this.StandardPartNOField = value;
                    this.RaisePropertyChanged("StandardPartNO");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CTOSDocTunnel.ICTOSDocTunnel")]
    public interface ICTOSDocTunnel {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICTOSDocTunnel/GetFAEDocumentList", ReplyAction="http://tempuri.org/ICTOSDocTunnel/GetFAEDocumentListResponse")]
        Advantech.Myadvantech.DataAccess.CTOSDocTunnel.CTOSDocItem[] GetFAEDocumentList(string partNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICTOSDocTunnel/GetCustomerDocumentList", ReplyAction="http://tempuri.org/ICTOSDocTunnel/GetCustomerDocumentListResponse")]
        Advantech.Myadvantech.DataAccess.CTOSDocTunnel.CTOSDocItem[] GetCustomerDocumentList(string customerID, string standardPartNO);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICTOSDocTunnelChannel : Advantech.Myadvantech.DataAccess.CTOSDocTunnel.ICTOSDocTunnel, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CTOSDocTunnelClient : System.ServiceModel.ClientBase<Advantech.Myadvantech.DataAccess.CTOSDocTunnel.ICTOSDocTunnel>, Advantech.Myadvantech.DataAccess.CTOSDocTunnel.ICTOSDocTunnel {
        
        public CTOSDocTunnelClient() {
        }
        
        public CTOSDocTunnelClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CTOSDocTunnelClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CTOSDocTunnelClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CTOSDocTunnelClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Advantech.Myadvantech.DataAccess.CTOSDocTunnel.CTOSDocItem[] GetFAEDocumentList(string partNo) {
            return base.Channel.GetFAEDocumentList(partNo);
        }
        
        public Advantech.Myadvantech.DataAccess.CTOSDocTunnel.CTOSDocItem[] GetCustomerDocumentList(string customerID, string standardPartNO) {
            return base.Channel.GetCustomerDocumentList(customerID, standardPartNO);
        }
    }
}
