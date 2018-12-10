using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AOnlineWall.Entity;
using System.Data.SqlClient;

namespace AOnlineWall.POCOS
{
    public class WallCategoryService
    {
        /// <summary>
        /// 获取所有category 或者 根据父类获取category
        /// </summary>
        /// <param name="parentCategoryId"></param>
        /// <returns></returns>
        public List<WallCategory> GetWallCategory(int parentCategoryId = 0,bool isRoot = false)
        {
            List<WallCategory> wcList = new List<WallCategory>();
            string sql = string.Empty;
            if (parentCategoryId != 0)
                sql = string.Format("select * from WallCategory where ParentCategoryId = {0}  order by id desc",parentCategoryId);
            else
                sql = string.Format("select * from WallCategory {0} order by id desc",isRoot ? "where ParentCategoryId is null" : "");
            using (SqlDataReader dr = SQLHelper.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    WallCategory wcFile = new WallCategory();
                    wcFile.Id = int.Parse(dr["Id"].ToString());
                    wcFile.CategoryName = dr["CategoryName"].ToString();
                    wcFile.ParentCategoryId = dr["ParentCategoryId"] is DBNull ? 0 : int.Parse(dr["ParentCategoryId"].ToString());
                    wcFile.UpdateDate = DateTime.Parse(dr["UpdateDate"].ToString());
                    wcFile.Owner = dr["Owner"].ToString();
                    wcList.Add(wcFile);
                }
                dr.Close();
            }
            return wcList;
        }

        /// <summary>
        /// 添加Wall Category
        /// </summary>
        /// <param name="wc"></param>
        /// <returns></returns>
        public int Save(WallCategory wc)
        {
            string sql = string.Format("insert WallCategory values(@CategoryName,@ParentCategoryId,getDate(),@Owner);select id = SCOPE_IDENTITY();");
            
            SqlParameter[] cmdParams = new SqlParameter[3];
            cmdParams[0] = new SqlParameter("@CategoryName", wc.CategoryName);
            object parentCategory;
            if (wc.ParentCategoryId == 0)
                parentCategory = DBNull.Value;
            else
                parentCategory = wc.ParentCategoryId;
            cmdParams[1] = new SqlParameter("@ParentCategoryId", parentCategory);
            cmdParams[2] = new SqlParameter("@Owner", wc.Owner);

            return int.Parse(SQLHelper.ExecuteScalar(sql, cmdParams).ToString());
        }

        public WallCategory GetWallCategoryById(int categoryId)
        {
            string sql = string.Format("select * from WallCategory where Id = {0}", categoryId);

            WallCategory wcFile = null;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(sql))
            {
                if (dr.Read())
                {
                    wcFile = new WallCategory();
                    wcFile.Id = int.Parse(dr["Id"].ToString());
                    wcFile.CategoryName = dr["CategoryName"].ToString();
                    wcFile.ParentCategoryId = dr["ParentCategoryId"] is DBNull ? 0 : int.Parse(dr["ParentCategoryId"].ToString());
                    wcFile.UpdateDate = DateTime.Parse(dr["UpdateDate"].ToString());
                    wcFile.Owner = dr["Owner"].ToString();
                }
                dr.Close();
            }
            return wcFile;
        }
        /// <summary>
        /// 更新category
        /// </summary>
        /// <param name="wc"></param>
        /// <returns></returns>
        public int UpdateWallCategory(WallCategory wc)
        {
            string sql = string.Format("update WallCategory set CategoryName=@CategoryName,UpdateDate=getDate(),Owner=@Owner where Id=@Id");

            SqlParameter[] cmdParams = new SqlParameter[3];
            cmdParams[0] = new SqlParameter("@CategoryName", wc.CategoryName);
            cmdParams[1] = new SqlParameter("@Owner", wc.Owner);
            cmdParams[2] = new SqlParameter("@Id", wc.Id);

            return SQLHelper.ExecuteNonQuery(sql, cmdParams);
        }

    }
}
