//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Advantech.Myadvantech.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class SA_FKNBK
    {
        public int ID { get; set; }
        public int HeadID { get; set; }
        public string Bankl { get; set; }
        public string Bankn { get; set; }
        public string Banks { get; set; }
        public string Bkont { get; set; }
        public string Bkref { get; set; }
        public string Bvtyp { get; set; }
        public string Ebpp_Accname { get; set; }
        public string Ebpp_Bvstatus { get; set; }
        public string Kobis { get; set; }
        public string Koinh { get; set; }
        public string Kovon { get; set; }
        public string Kunnr { get; set; }
        public string Kz { get; set; }
        public string Mandt { get; set; }
        public string Xezer { get; set; }
    
        public virtual SA_APPLICATION2COMPANY SA_APPLICATION2COMPANY { get; set; }
    }
}
