using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class Model_Lang
    {
        public virtual string Model_Name
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }

        public virtual string Introduction
        {
            get;
            set;
        }
        public virtual LanguageCode Language
        {
            get;
            set;
        }

    }
}
