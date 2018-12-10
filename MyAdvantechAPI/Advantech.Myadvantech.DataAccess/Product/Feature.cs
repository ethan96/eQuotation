using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{

    public class Feature
    {

        public virtual int FEATURE_ID
        {
            get;
            set;
        }

        public virtual long FEATURE_SEQ
        {
            get;
            set;
        }

        public virtual string FEATURE_DESC
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
