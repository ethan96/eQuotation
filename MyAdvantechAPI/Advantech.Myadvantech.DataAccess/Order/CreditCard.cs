using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class CreditCard
    {

        //S_CreditCardRow.Cc_Name = .HOLDER : S_CreditCardRow.Cc_Number = .NUMBER : S_CreditCardRow.Cc_Type = .TYPE
        //S_CreditCardRow.Cc_Valid_T = .EXPIRED : S_CreditCardRow.Cc_Verif_Value = .VERIFICATION_VALUE


        public String Holder
        {
            get;
            set;
        }

        public String Number
        {
            get;
            set;
        }

        public String Type
        {
            get;
            set;
        }

        public String Expired
        {
            get;
            set;
        }

        public String VerificationValue
        {
            get;
            set;
        }


    }
}
