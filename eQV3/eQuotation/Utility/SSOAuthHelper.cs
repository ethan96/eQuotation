using eQuotation.DataAccess;
using eQuotation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Utility
{
    public class SSOAuthHelper
    {
        private static MemberShipSSO.MembershipWebservice sso = new MemberShipSSO.MembershipWebservice();
        public static AppUser MappingSSOProfileToAppuser(MemberShipSSO.SSOUSER profile, string email)
        {


                var user = new AppUser();
                user.Email = email;
                user.EmailConfirmed = true;

                if (string.IsNullOrWhiteSpace(profile.first_name) && string.IsNullOrWhiteSpace(profile.last_name))
                {
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        var _name = email.Split('@')[0];
                        if (_name != null)
                        {
                            if (_name.Split('.').Count() > 1)
                            {
                                user.FirstName = _name.Split('.')[0];
                                user.LastName = _name.Split('.')[1];
                                user.UserName = _name.Split('.')[0] + _name.Split('.')[1];
                            }
                            else
                            {
                                user.FirstName = _name.Split('.')[0];
                                user.LastName = _name.Split('.')[0];
                                user.UserName = _name.Split('.')[0];
                            }
                        }
                        else
                        {
                            user.UserName = profile.first_name + profile.last_name;
                            user.FirstName = profile.first_name;
                            user.LastName = profile.last_name;
                        }
                    }
                    else
                    {
                        user.UserName = profile.first_name + profile.last_name;
                        user.FirstName = profile.first_name;
                        user.LastName = profile.last_name;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(profile.first_name) && !string.IsNullOrWhiteSpace(profile.last_name))
                    {
                        user.UserName = profile.first_name + profile.last_name;
                        user.FirstName = profile.first_name;
                        user.LastName = profile.last_name;
                    }
                    else if (!string.IsNullOrWhiteSpace(profile.first_name))
                    {
                        user.UserName = profile.first_name;
                        user.FirstName = profile.first_name;
                        user.LastName = profile.first_name;
                    }
                    else if (!string.IsNullOrWhiteSpace(profile.last_name))
                    {
                        user.UserName = profile.last_name;
                        user.FirstName = profile.last_name;
                        user.LastName = profile.last_name;
                    }
                }

                user.Company = profile.company_name;
                user.Department = profile.department;

                return user;
        }

        public static bool IsEmployee(string email)
        {
            Object o = DBUtil.dbExecuteScalar("EZ", String.Format("SELECT count(email_addr) FROM [Employee_New].[dbo].[EZ_PROFILE] where email_addr='{0}'", email));
            if (Util.IsNumeric(o) && Convert.ToInt32(o) > 0)
            {
                return true;
            }
            return false;
        }

        public static MemberShipSSO.SSOUSER GetAdvantechMemberProfile(string email)
        {
            MemberShipSSO.SSOUSER profile = new MemberShipSSO.SSOUSER();
            return  sso.getProfile(email, "MY");
        }

        public static string GetSSOloginTicket(string email, string password)
        {
            String _IP = Util.GetClientIP();
            String loginTicket = sso.login(email, password, "MY", _IP);
            return loginTicket;
        }
    }
}