using eQuotation.Entities;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Models.Material
{
    public class ProductManager : ViewModelBase<Product>
    {
        public Product Header { get; set; }
        public ProductManager()
        {
            Init();
        }
        public override void Init()
        {
            Header = new Product();
        }
        public override void SetValue()
        {
            // create new record if not existed
            if (!HasEntity(this.Header.ID))
            {
                Entity.ID = this.UnitWork.Product.NewID(p => p.ID);
            }
            Entity.Name = Header.Name;
            Entity.ExtDesc = Header.ExtDesc;

            this.ID = Entity.ID;
            this.Header.ID = Entity.ID;
        }
        public override void GetValue(Product data)
        {
            this.Header = data;
        }
    }
}