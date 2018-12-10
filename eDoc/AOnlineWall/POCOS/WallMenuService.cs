using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AOnlineWall.Entity;
using System.Data.SqlClient;

namespace AOnlineWall.POCOS
{
    public class WallMenuService
    {
        /// <summary>
        /// 获取所有menu 或者 根据父类获取menu
        /// </summary>
        /// <param name="parentMenuId"></param>
        /// <param name="isRoot"></param>
        /// <returns></returns>
        public List<WallMenu> GetWallMenu(int parentMenuId = 0, bool isRoot = false , bool onlyPublish = false)
        {
            List<WallMenu> wmList = new List<WallMenu>();
            string sql = string.Empty;
            if (onlyPublish)
                sql = string.Format("select * from WallMenu where PublishStatus = 1 order by id");
            else
            {
                if (parentMenuId != 0)
                    sql = string.Format("select * from WallMenu where ParentMenuId = {0}  order by id ", parentMenuId);
                else
                    sql = string.Format("select * from WallMenu {0} order by id ", isRoot ? "where ParentMenuId is null" : "");
            }
            using (SqlDataReader dr = SQLHelper.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    WallMenu wm = new WallMenu();
                    wm.Id = int.Parse(dr["Id"].ToString());
                    wm.ParentMenuId = dr["ParentMenuId"] is DBNull ? 0 : int.Parse(dr["ParentMenuId"].ToString());
                    wm.MenuName = dr["MenuName"].ToString();
                    wm.PublishStatus = Boolean.Parse(dr["PublishStatus"].ToString());
                    wm.Url = dr["Url"].ToString();
                    wm.UpdateDate = DateTime.Parse(dr["UpdateDate"].ToString());
                    wm.Owner = dr["Owner"].ToString();
                    wmList.Add(wm);
                }
                dr.Close();
            }
            return wmList;
        }


        /// <summary>
        /// 获取 Wall Menu
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public WallMenu GetWallMenuById(int mid)
        {
            string sql = string.Format("select * from WallMenu where Id = {0}", mid);

            WallMenu wm = null;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(sql))
            {
                if (dr.Read())
                {
                    wm = new WallMenu();
                    wm.Id = int.Parse(dr["Id"].ToString());
                    wm.ParentMenuId = dr["ParentMenuId"] is DBNull ? 0 : int.Parse(dr["ParentMenuId"].ToString());
                    wm.MenuName = dr["MenuName"].ToString();
                    wm.PublishStatus = Boolean.Parse(dr["PublishStatus"].ToString());
                    wm.Url = dr["Url"].ToString();
                    wm.UpdateDate = DateTime.Parse(dr["UpdateDate"].ToString());
                    wm.Owner = dr["Owner"].ToString();
                }
                dr.Close();
            }
            return wm;
        }
        //更新
        public int Update(WallMenu wm)
        {
            string sql = string.Format("UPDATE WallMenu set MenuName=@MenuName,PublishStatus=@PublishStatus,Url=@Url,UpdateDate=getDate(),Owner=@Owner where Id=@Id");


            SqlParameter[] cmdParams = new SqlParameter[5];
            cmdParams[0] = new SqlParameter("@MenuName", wm.MenuName);
            cmdParams[1] = new SqlParameter("@PublishStatus", wm.PublishStatus.ToString());
            cmdParams[2] = new SqlParameter("@Url", wm.Url);
            cmdParams[3] = new SqlParameter("@Owner", wm.Owner);
            cmdParams[4] = new SqlParameter("@Id", wm.Id);

            return SQLHelper.ExecuteNonQuery(sql, cmdParams);
        }
        //保存menu
        public int Save(WallMenu wm)
        {
            string sql = string.Format("INSERT WallMenu(ParentMenuId,MenuName,PublishStatus,Url,UpdateDate,Owner)" +
                            "VALUES (@ParentMenuId,@MenuName,@PublishStatus,@Url,getdate(),@Owner);select id = SCOPE_IDENTITY();");


            SqlParameter[] cmdParams = new SqlParameter[5];
            object parentCategory;
            if (wm.ParentMenuId == 0)
                parentCategory = DBNull.Value;
            else
                parentCategory = wm.ParentMenuId;
            cmdParams[0] = new SqlParameter("@ParentMenuId", parentCategory);
            cmdParams[1] = new SqlParameter("@MenuName", wm.MenuName);
            cmdParams[2] = new SqlParameter("@PublishStatus", wm.PublishStatus);
            cmdParams[3] = new SqlParameter("@Url", wm.Url);
            cmdParams[4] = new SqlParameter("@Owner", wm.Owner);

            return int.Parse(SQLHelper.ExecuteScalar(sql, cmdParams).ToString());
        }
        //删除Menu
        public int DeleteWallMenu(int wid)
        {
            string sql = string.Format("DELETE FROM WallMenu WHERE id={0};", wid);
            return SQLHelper.ExecuteNonQuery(sql);
        }
        //查询是否有 子menu
        public int getChildMenuCountById(int wid)
        {
            string sql = string.Format("select count(id) from WallMenu WHERE ParentMenuId={0};", wid);
            return int.Parse(SQLHelper.ExecuteScalar(sql).ToString());
        }
    }
}
