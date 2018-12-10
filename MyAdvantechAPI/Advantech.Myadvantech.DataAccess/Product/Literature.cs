using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class Literature
    {
        public virtual string LITERATURE_ID
        {
            get;
            set;
        }

        public virtual string HTTP_URL
        {
            get;
            set;
        }


        public virtual PISLiteratureType LIT_TYPE
        {
            get;
            set;
        }

        public virtual string LIT_NAME
        {
            get;
            set;
        }

        public virtual string FILE_NAME
        {
            get;
            set;
        }

        public virtual string LAST_UPDATED_BY
        {
            get;
            set;
        }

        public virtual string FILE_EXT
        {
            get;
            set;
        }

        public virtual LanguageCode Language
        {
            get;
            set;
        }

        public Literature()
        {
        }

        public Literature(string literatureid)
        {
            // TODO: Complete member initialization
            this.LITERATURE_ID = literatureid;
        }
    }
}
