﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Advantech.Myadvantech.DataAccess.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MyAdvantechGlobalEntities : DbContext
    {
        public MyAdvantechGlobalEntities()
            : base("name=MyAdvantechGlobalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ORDER_DETAIL> ORDER_DETAIL { get; set; }
        public virtual DbSet<order_Master_ExtensionV2> order_Master_ExtensionV2 { get; set; }
        public virtual DbSet<ORDER_MASTER> ORDER_MASTER { get; set; }
        public virtual DbSet<ORDER_MASTER_EXTENSION> ORDER_MASTER_EXTENSION { get; set; }
        public virtual DbSet<TIMEZONE> TIMEZONE { get; set; }
        public virtual DbSet<PRODUCT_DEPENDENCY> PRODUCT_DEPENDENCY { get; set; }
        public virtual DbSet<Cart2OrderMaping> Cart2OrderMaping { get; set; }
        public virtual DbSet<CARTMASTERV2> CARTMASTERV2 { get; set; }
        public virtual DbSet<SIEBEL_CONTACT> SIEBEL_CONTACT { get; set; }
        public virtual DbSet<SIEBEL_CONTACT_PRIVILEGE> SIEBEL_CONTACT_PRIVILEGE { get; set; }
        public virtual DbSet<SIEBEL_CONTACT_PRIVILEGE_TEMP> SIEBEL_CONTACT_PRIVILEGE_TEMP { get; set; }
        public virtual DbSet<SAP_PRODUCT> SAP_PRODUCT { get; set; }
        public virtual DbSet<SAP_PRODUCT_ORG> SAP_PRODUCT_ORG { get; set; }
        public virtual DbSet<SIEBEL_ACCOUNT> SIEBEL_ACCOUNT { get; set; }
        public virtual DbSet<cart_DETAIL_V2> cart_DETAIL_V2 { get; set; }
        public virtual DbSet<CheckPointOrder2Cart> CheckPointOrder2Cart { get; set; }
        public virtual DbSet<SAP_PRODUCT_ABC> SAP_PRODUCT_ABC { get; set; }
        public virtual DbSet<SAP_EMPLOYEE> SAP_EMPLOYEE { get; set; }
        public virtual DbSet<SAP_DIMCOMPANY> SAP_DIMCOMPANY { get; set; }
        public virtual DbSet<SAP_COMPANY_PARTNERS> SAP_COMPANY_PARTNERS { get; set; }
        public virtual DbSet<PRODUCT_COMPATIBILITY> PRODUCT_COMPATIBILITY { get; set; }
        public virtual DbSet<SAP_PRODUCT_STATUS> SAP_PRODUCT_STATUS { get; set; }
        public virtual DbSet<SAP_PRODUCT_STATUS_ORDERABLE> SAP_PRODUCT_STATUS_ORDERABLE { get; set; }
        public virtual DbSet<OrderForwarderService> OrderForwarderServices { get; set; }
        public virtual DbSet<ORDER_PARTNERS> ORDER_PARTNERS { get; set; }
        public virtual DbSet<BB_ESTORE_ORDER> BB_ESTORE_ORDER { get; set; }
        public virtual DbSet<FreightOption> FreightOptions { get; set; }
        public virtual DbSet<BB_CREDITCARD_ORDER> BB_CREDITCARD_ORDER { get; set; }
        public virtual DbSet<ConfigurationHub_ConfiguredResult> ConfigurationHub_ConfiguredResult { get; set; }
    }
}
