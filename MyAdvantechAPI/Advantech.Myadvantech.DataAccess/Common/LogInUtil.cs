using Advantech.Myadvantech.DataAccess.MemberShip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    /// <summary>
    /// Login class for all kind of method
    /// </summary>
    public class LogInUtil
    {
        /// <summary>
        /// Use SSO membership to authenticate ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="password"></param>
        /// <param name="siteID"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static string CheckSSOLogIn(string ID, string password, string siteID, string IP)
        {
            MembershipWebservice sso = new MembershipWebservice();
            sso.Timeout = -1;

            try
            {
                return sso.login(ID, password, siteID, IP);
            }
            catch (Exception ex)
            {
                //Save login error
                return string.Empty;
            }
            finally
            {
                if (sso != null)
                    sso.Dispose();
            }
        }

        public static string CheckEZLogIn(string ID, string password)
        {
            //TO DO - Use Yuwan's function to check
            return string.Empty;
        }
    }
}
