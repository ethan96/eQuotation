﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Advantech.Myadvantech.Business")]
namespace Advantech.Myadvantech.DataAccess
{
    public class Product : Part
    {
        public virtual int LineNumber
        {
            get;
            set;
        }

        public int HigherLevel
        {
            get;
            set;
        }

        public string DeliveryGroup
        {
            get;
            set;
        }

        public string CustomDescription
        {
            get;
            set;
        }

        public string ShipPoint
        {
            get;
            set;
        }

        public string StorageLoc
        {
            get;
            set;
        }



        public DateTime? RequireDate
        {
            get;
            set;
        }


        [DefaultValue(0)]
        public virtual int ParentLineNumber
        {
            get;
            set;
        }

        [DefaultValue(1)]
        public virtual int Quantity
        {
            get;
            set;
        }


        public Decimal SellingPrice
        {
            get;
            set;
        }
        public Order  order
        {
            get;
            set;
        }
        [DefaultValue(LineItemType.LooseItem)]
        public LineItemType LineItemType
        {
            get;
            set;
        }

        private String _ERPID;

        public Product()
        {

        }

        public Product(String ERPID, Int16 LineNumber, String PartNo)
        {
            this._ERPID = ERPID;
            this.LineNumber = LineNumber;
            this.PartNumber = PartNo;
                 
            this.Load();

        }

        public Product(String ERPID, Int16 LineNumber, String PartNo, Decimal UnitPrice)
        {


        }


        public Product(String ERPID, Int16 LineNumber, String PartNo, Int16 Quantity, Decimal UnitPrice)
        {


        }

        protected void Load()
        {
         //   this.LoadPartDetail();
         //   this.LoadPrice(_ERPID);
        }

       





    }
}
