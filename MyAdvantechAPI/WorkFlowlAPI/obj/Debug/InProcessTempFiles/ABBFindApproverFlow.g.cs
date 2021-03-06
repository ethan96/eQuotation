//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------


namespace WorkFlowlAPI.ABBGPApproval {
    
    
    [System.Runtime.InteropServices.ComVisible(false)]
    public partial class ABBFindApproverFlow : System.Activities.Activity, System.ComponentModel.ISupportInitialize {
        
        private bool _contentLoaded;
        
        private System.Activities.InArgument<string> _QuoteId;
        
        private System.Activities.OutArgument<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>> _ApprovalList;
        
        private System.Activities.InArgument<string> _Url;
        
        private System.Activities.InArgument<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>> _QuotationDetails;
        
        private System.Activities.InArgument<string> _QuoteToERPID;
        
        private System.Activities.InArgument<System.DateTime> _ExpiredDate;
        
        private System.Activities.InArgument<System.DateTime> _QuoteDate;
        
        private System.Activities.OutArgument<WorkFlowlAPI.FindApproverResult> _FindApproverResult;
        
        private System.Activities.InArgument<string> _SalesCode;
        
        private System.Activities.InArgument<string> _SalesEmail;
        
partial void BeforeInitializeComponent(ref bool isInitialized);

partial void AfterInitializeComponent();

        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "15.0.0.0")]
        public ABBFindApproverFlow() {
            this.InitializeComponent();
        }
        
        public System.Activities.InArgument<string> QuoteId {
            get {
                return this._QuoteId;
            }
            set {
                this._QuoteId = value;
            }
        }
        
        public System.Activities.OutArgument<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>> ApprovalList {
            get {
                return this._ApprovalList;
            }
            set {
                this._ApprovalList = value;
            }
        }
        
        public System.Activities.InArgument<string> Url {
            get {
                return this._Url;
            }
            set {
                this._Url = value;
            }
        }
        
        public System.Activities.InArgument<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>> QuotationDetails {
            get {
                return this._QuotationDetails;
            }
            set {
                this._QuotationDetails = value;
            }
        }
        
        public System.Activities.InArgument<string> QuoteToERPID {
            get {
                return this._QuoteToERPID;
            }
            set {
                this._QuoteToERPID = value;
            }
        }
        
        public System.Activities.InArgument<System.DateTime> ExpiredDate {
            get {
                return this._ExpiredDate;
            }
            set {
                this._ExpiredDate = value;
            }
        }
        
        public System.Activities.InArgument<System.DateTime> QuoteDate {
            get {
                return this._QuoteDate;
            }
            set {
                this._QuoteDate = value;
            }
        }
        
        public System.Activities.OutArgument<WorkFlowlAPI.FindApproverResult> FindApproverResult {
            get {
                return this._FindApproverResult;
            }
            set {
                this._FindApproverResult = value;
            }
        }
        
        public System.Activities.InArgument<string> SalesCode {
            get {
                return this._SalesCode;
            }
            set {
                this._SalesCode = value;
            }
        }
        
        public System.Activities.InArgument<string> SalesEmail {
            get {
                return this._SalesEmail;
            }
            set {
                this._SalesEmail = value;
            }
        }
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "15.0.0.0")]
        public void InitializeComponent() {
            if ((this._contentLoaded == true)) {
                return;
            }
            this._contentLoaded = true;
            bool isInitialized = false;
            this.BeforeInitializeComponent(ref isInitialized);
            if ((isInitialized == true)) {
                this.AfterInitializeComponent();
                return;
            }
            string resourceName = this.FindResource();
            System.IO.Stream initializeXaml = typeof(ABBFindApproverFlow).Assembly.GetManifestResourceStream(resourceName);
            System.Xml.XmlReader xmlReader = null;
            System.Xaml.XamlReader reader = null;
            System.Xaml.XamlObjectWriter objectWriter = null;
            try {
                System.Xaml.XamlSchemaContext schemaContext = XamlStaticHelperNamespace._XamlStaticHelper.SchemaContext;
                xmlReader = System.Xml.XmlReader.Create(initializeXaml);
                System.Xaml.XamlXmlReaderSettings readerSettings = new System.Xaml.XamlXmlReaderSettings();
                readerSettings.LocalAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                readerSettings.AllowProtectedMembersOnRoot = true;
                reader = new System.Xaml.XamlXmlReader(xmlReader, schemaContext, readerSettings);
                System.Xaml.XamlObjectWriterSettings writerSettings = new System.Xaml.XamlObjectWriterSettings();
                writerSettings.RootObjectInstance = this;
                writerSettings.AccessLevel = System.Xaml.Permissions.XamlAccessLevel.PrivateAccessTo(typeof(ABBFindApproverFlow));
                objectWriter = new System.Xaml.XamlObjectWriter(schemaContext, writerSettings);
                System.Xaml.XamlServices.Transform(reader, objectWriter);
            }
            finally {
                if ((xmlReader != null)) {
                    ((System.IDisposable)(xmlReader)).Dispose();
                }
                if ((reader != null)) {
                    ((System.IDisposable)(reader)).Dispose();
                }
                if ((objectWriter != null)) {
                    ((System.IDisposable)(objectWriter)).Dispose();
                }
            }
            this.AfterInitializeComponent();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "15.0.0.0")]
        private string FindResource() {
            string[] resources = typeof(ABBFindApproverFlow).Assembly.GetManifestResourceNames();
            for (int i = 0; (i < resources.Length); i = (i + 1)) {
                string resource = resources[i];
                if ((resource.Contains(".ABBFindApproverFlow.g.xaml") || resource.Equals("ABBFindApproverFlow.g.xaml"))) {
                    return resource;
                }
            }
            throw new System.InvalidOperationException("Resource not found.");
        }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033")]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "15.0.0.0")]
        void System.ComponentModel.ISupportInitialize.BeginInit() {
        }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033")]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "15.0.0.0")]
        void System.ComponentModel.ISupportInitialize.EndInit() {
            this.InitializeComponent();
        }
    }
}
