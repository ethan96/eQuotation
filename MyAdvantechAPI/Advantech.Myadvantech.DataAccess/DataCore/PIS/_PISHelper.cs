using Advantech.Myadvantech.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class _PISHelper
    {
        protected PISEntities context;
        public _PISHelper()
        {
            //context = new eQEntities();
            context = PISContext.Current;
        }
    }
}
