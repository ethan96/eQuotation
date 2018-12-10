using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AOnlineWall.Entity;
using System.Data.SqlClient;


namespace AOnlineWall.POCOS
{
    public class WallFileService
    {
        /// <summary>
        /// 获取所有的 父级 文件
        /// </summary>
        /// <returns></returns>
        public List<WallFile> GetAllWallFile()
        {
            List<WallFile> wallFileList = new List<WallFile>();

            string sql = string.Format("select wf.* from WallFile wf " +
                                                "left join FileCategoryMapping fcm ON fcm.Fid=wf.Id left JOIN WallCategory wc ON wc.Id = fcm.Cid order by wf.id desc");
            using (SqlDataReader dr = SQLHelper.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    WallFile wallFile = new WallFile();
                    wallFile.Id = int.Parse(dr["Id"].ToString());
                    wallFile.Name = dr["Name"].ToString();
                    wallFile.FileName = dr["FileName"].ToString();
                    wallFile.FileSize = double.Parse(dr["FileSize"].ToString());
                    wallFile.Type = dr["Type"].ToString();
                    wallFile.UpdateDate = DateTime.Parse(dr["UpdateDate"].ToString());
                    wallFile.Owner = dr["Owner"].ToString();
                    
                    wallFileList.Add(wallFile);
                }
                dr.Close();
            }
            return wallFileList;
        }

        public List<WallFile> GetWallFileByCategory(int categoryId)
        {
            List<WallFile> wallFileList = new List<WallFile>();

            string sql = string.Format("select wf.* from WallFile wf " +
                                                "inner join FileCategoryMapping fcm ON fcm.Fid=wf.Id inner JOIN WallCategory wc ON wc.Id = fcm.Cid where wc.Id = " + categoryId + " order by wf.id desc");
            using (SqlDataReader dr = SQLHelper.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    WallFile wallFile = new WallFile();
                    wallFile.Id = int.Parse(dr["Id"].ToString());
                    wallFile.Name = dr["Name"].ToString();
                    wallFile.FileName = dr["FileName"].ToString();
                    wallFile.FileSize = double.Parse(dr["FileSize"].ToString());
                    wallFile.Type = dr["Type"].ToString();
                    wallFile.UpdateDate = DateTime.Parse(dr["UpdateDate"].ToString());
                    wallFile.Owner = dr["Owner"].ToString();

                    wallFileList.Add(wallFile);
                }
                dr.Close();
            }
            return wallFileList;
        }

        public int Save(WallFile wf)
        {
            string sql = string.Format("DECLARE @fid int;INSERT WallFile(Name,Filename,FileSize,Type,UpdateDate,Owner)" +
                            "VALUES (@Name,@FileName,@FileSize,@Type,getdate(),@Owner);select @fid = SCOPE_IDENTITY();INSERT FileCategoryMapping VALUES(@fid,@Category)");


            SqlParameter[] cmdParams = new SqlParameter[6];
            cmdParams[0] = new SqlParameter("@Name", wf.Name);
            cmdParams[1] = new SqlParameter("@FileName", wf.FileName);
            cmdParams[2] = new SqlParameter("@FileSize", wf.FileSize);
            cmdParams[3] = new SqlParameter("@Type", wf.Type);
            cmdParams[4] = new SqlParameter("@Owner", wf.Owner);
            cmdParams[5] = new SqlParameter("@Category", wf.CategoryId);

            return SQLHelper.ExecuteNonQuery(sql, cmdParams);
        }

        /// <summary>
        /// 删除 WallFile
        /// </summary>
        /// <param name="removeIdList"></param>
        /// <returns></returns>
        public int DeleteWallFile(int fid)
        {
            string sql = string.Format("DELETE FROM FileCategoryMapping WHERE Fid={0};DELETE FROM WallFile where Id={0};", fid);
            return SQLHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 删除 WallFile List  没有删除文件
        /// </summary>
        /// <param name="removeIdList"></param>
        /// <returns></returns>
        public int DeleteWallFileList(List<int> removeIdList)
        {
            //删除文件 还么有做.
            string sql = string.Empty;
            int successCount = 0;
            if (removeIdList.Count > 0)
            {
                int submitCount = 0;
                for (int i = 0; i < removeIdList.Count; i++)
                {
                    sql += string.Format("DELETE FROM FileCategoryMapping WHERE Fid={0};DELETE FROM WallFile where Id={0};", removeIdList[i]);

                    submitCount++;
                    if (submitCount == 5 || i == removeIdList.Count - 1)
                    {
                        successCount += SQLHelper.ExecuteNonQuery(sql);
                        sql = "";
                        submitCount = 0;
                    }
                }
            }
            return successCount;
        }

        /// <summary>
        /// 获取 Wall File
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public WallFile GetWallFileById(int fid)
        {
            string sql = string.Format("select * from WallFile where Id = {0}", fid);

            WallFile wallFile = null;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(sql))
            {
                if (dr.Read())
                {
                    wallFile = new WallFile();
                    wallFile.Id = int.Parse(dr["Id"].ToString());
                    wallFile.Name = dr["Name"].ToString();
                    wallFile.FileName = dr["FileName"].ToString();
                    wallFile.FileSize = double.Parse(dr["FileSize"].ToString());
                    wallFile.Type = dr["Type"].ToString();
                    wallFile.UpdateDate = DateTime.Parse(dr["UpdateDate"].ToString());
                    wallFile.Owner = dr["Owner"].ToString();
                }
                dr.Close();
            }
            return wallFile;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="wf"></param>
        /// <returns></returns>
        public int Update(WallFile wf)
        {
            string sql = string.Format("UPDATE WallFile set Name=@Name,UpdateDate=getDate(),Owner=@Owner where Id=@Id");


            SqlParameter[] cmdParams = new SqlParameter[3];
            cmdParams[0] = new SqlParameter("@Name", wf.Name);
            cmdParams[1] = new SqlParameter("@Owner", wf.Owner);
            cmdParams[2] = new SqlParameter("@Id", wf.Id);

            return SQLHelper.ExecuteNonQuery(sql, cmdParams);
        }

        //删除 WallFile List
        public bool DeleteWallFileList(List<int> removeIdList, string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    foreach (int fid in removeIdList)
                    {
                        WallFile wallFile = GetWallFileById(fid);
                        if (wallFile != null)
                        {
                            if (wallFile.Type.Equals("File"))
                            {
                                AOnlineWall.Business.Common com = new AOnlineWall.Business.Common();
                                com.DeleteFile(filePath, wallFile.FileName);
                            }

                            DeleteWallFile(wallFile.Id);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
