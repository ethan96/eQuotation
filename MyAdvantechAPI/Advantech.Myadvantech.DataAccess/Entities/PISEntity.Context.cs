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
    
    public partial class PISEntities : DbContext
    {
        public PISEntities()
            : base("name=PISEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CATEGORY> CATEGORY { get; set; }
        public virtual DbSet<CATEGORY_LANG> CATEGORY_LANG { get; set; }
        public virtual DbSet<Category_Model> Category_Model { get; set; }
        public virtual DbSet<Model> Model { get; set; }
        public virtual DbSet<MODEL_LANG> MODEL_LANG { get; set; }
        public virtual DbSet<model_product> model_product { get; set; }
        public virtual DbSet<model_displayarea> model_displayarea { get; set; }
        public virtual DbSet<MODEL_FEATURE> MODEL_FEATURE { get; set; }
        public virtual DbSet<Model_lit> Model_lit { get; set; }
        public virtual DbSet<LITERATURE> LITERATURE { get; set; }
    }
}
