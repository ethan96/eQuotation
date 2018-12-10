using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class ProductSpec
    {


        public virtual int SpecCategoryID
        {
            get;
            set;
        }

        public virtual string SpecCategoryName
        {
            get;
            set;
        }

        public virtual int SpecItemID
        {
            get;
            set;
        }

        public virtual string SpecItemName
        {
            get;
            set;
        }
        public virtual int SpecItemValueID
        {
            get;
            set;
        }

        public virtual string SpecItemValueName
        {
            get;
            set;
        }

        public virtual int Sequence
        {
            get;
            set;
        }

        public virtual bool IsFilterOption
        {
            get;
            set;
        }

    }
}
