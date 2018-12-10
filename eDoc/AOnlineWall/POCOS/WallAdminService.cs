using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;

namespace AOnlineWall.POCOS
{
    public class WallAdminService
    {
        /// <summary>
        /// search user by email.   user is exists
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Boolean AOnlineDocIsAdmin(string email)
        {
            string sql = string.Format("select top 1 count(id) from WallAdmin where Email='{0}' and [Type]='AOnlineDoc' ", email);

            return (int)SQLHelper.ExecuteScalar(sql) > 0;
        }

        //AOnlineHomePage 页面权限
        public Boolean AOnlineHomePageIsAdmin(string email)
        {
            string sql = string.Format("select top 1 count(id) from WallAdmin where Email='{0}' and [Type]='AOnlineHomePage' ", email);

            return (int)SQLHelper.ExecuteScalar(sql) > 0;
        }
    }
}
