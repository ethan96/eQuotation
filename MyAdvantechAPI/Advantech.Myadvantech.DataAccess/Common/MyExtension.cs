using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.Myadvantech.DataAccess
{
    public static class MyExtension
    {
        public static string ToMyAdvantechPart(this string partno)
        {
            return SAPDAL.RemovePrecedingZeros(partno);
        }
        public static bool IsNumberPart(this string partno)
        {
            char[] pChar = partno.ToCharArray();
            int j;
            for (int i = 0; i <= pChar.Length - 1; i++)
            {
                if (!int.TryParse(pChar[i].ToString(), out j))
                {
                    return false;
                }
            }
            return true;
        }

        //public static string GetPISLanguageCode(LanguageCode langcode)
        //{
        //    switch (langcode)
        //    {
        //        case LanguageCode.en_us:
        //            return "ENU";
        //        case LanguageCode.zh_tw:
        //            return "CHT";
        //        case LanguageCode.zh_cn:
        //            return "CHS";
        //        case LanguageCode.ja:
        //            return "JP";
        //        case LanguageCode.ko:
        //            return "KOR";
        //        default:
        //            return "ENU";
        //    }
        //}

        public static LanguageCode GetLanguageCodeByPISLanguageID(String PISLangID)
        {
            switch (PISLangID)
            {
                case "ENU":
                    return LanguageCode.en_us;
                case "CHT":
                    return LanguageCode.zh_tw;
                case "CHS":
                    return LanguageCode.zh_cn;
                case "JP":
                    return LanguageCode.ja;
                case "KOR":
                    return LanguageCode.ko;
                case "GER":
                    return LanguageCode.de;
                case "RUS":
                    return LanguageCode.ru;
                default:
                    return LanguageCode.en_us;
            }
        }

        public static LanguageCode GetLanguageCodeByLanguageID(String PISLangID)
        {
            LanguageCode LangCode = (LanguageCode)Enum.Parse(typeof(LanguageCode),PISLangID,true);
            return LangCode;
        }

        public static string GetCurrencySignByCurrency(string _Currency)
        {
            if (string.IsNullOrEmpty(_Currency)) return "";
            switch (_Currency.ToUpper())
            {
                case "TWD":
                    return "TWD";
                case "NT":
                    return "NT";
                case "US":
                case "USD":
                    return "$";
                case "EUR":
                    return "€";
                case "CNY":
                case "RMB":
                    return "¥";
                case "YEN":
                case "JPY":
                    return "J.¥";
                case "GBP":
                    return "£";
                case "AUD":
                    return "AUD";
                case "SGD":
                    return "S$";
                default:
                    return "$";
            }
        }

    }
}
