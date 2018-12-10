using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;

namespace Advantech.Myadvantech.DataAccess
{
    public static class CBOMV2_CatalogEditorDAL
    {
        public static String InitializeTree(String orgid)
        {
            List<EasyUITreeNode> TreeNodes = new List<EasyUITreeNode>();
            List<CBOM_CATALOG_RECORD> CBOMCatalogRecords = GetCBOMCatalogTreeByRootId(orgid);
            List<CBOM_CATALOG_RECORD> RootRecord = (from q in CBOMCatalogRecords where q.LEVEL == 1 select q).ToList();

            if (RootRecord.Count == 1)
            {
                EasyUITreeNode RootTreeNode = new EasyUITreeNode(RootRecord.First().ID, RootRecord.First().ID, RootRecord.First().CATALOG_NAME, "", RootRecord.First().HIE_ID, "", 0, 0, 1, 0, 0, 0, 0);
                RootTreeNode.csstype = NodeCssType.Tree_Node_Root;
                CBOMCatalogRecordsToEasyUITreeNode(CBOMCatalogRecords, RootTreeNode);
                TreeNodes.Add(RootTreeNode);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(TreeNodes);
        }

        public static List<CBOM_CATALOG_RECORD> GetCBOMCatalogTreeByRootId(string OrgId)
        {
            String str = " DECLARE @ID  hierarchyid " +
                         " SELECT @ID  = HIE_ID " +
                         " FROM CBOM_CATALOG_V2 WHERE ID = '" + OrgId + "_Root'" +
                         " SELECT IsNull(cast(HIE_ID.GetAncestor(1) as nvarchar(100)),'') as PAR_HIE_ID, " +
                         " HIE_ID.GetLevel() AS [LEVEL], ID AS [ID], ID AS [VIRTUAL_ID], " +
                         " HIE_ID.ToString() AS [HIE_ID], CATALOG_NAME , CATALOG_TYPE, " +
                         " CATALOG_DESC, SEQ_NO, ORG " +
                         " FROM CBOM_CATALOG_V2 WHERE HIE_ID.IsDescendantOf(@ID) = 1 " +
                         " ORDER BY HIE_ID.GetLevel() ";

            DataTable dtCatalogTree = SqlProvider.dbGetDataTable("CBOMV2", str);
            List<CBOM_CATALOG_RECORD> CBOMCatalogRecords = dtCatalogTree.DataTableToList<CBOM_CATALOG_RECORD>();
            return CBOMCatalogRecords;
        }

        public static void CBOMCatalogRecordsToEasyUITreeNode(List<CBOM_CATALOG_RECORD> CBOMCatalogRecords, EasyUITreeNode CurrentNode)
        {
            String CurrentNodeHieId = CurrentNode.hieid;
            List<CBOM_CATALOG_RECORD> SubRecord = (from q in CBOMCatalogRecords where q.PAR_HIE_ID == CurrentNodeHieId orderby q.SEQ_NO select q).ToList();

            if (SubRecord.Count == 0)
                return;

            foreach (CBOM_CATALOG_RECORD SubRecord_loopVariable in SubRecord)
            {
                EasyUITreeNode SubTreeNode = new EasyUITreeNode(SubRecord_loopVariable.ID, SubRecord_loopVariable.VIRTUAL_ID, SubRecord_loopVariable.CATALOG_NAME, CurrentNode.id, SubRecord_loopVariable.HIE_ID, SubRecord_loopVariable.CATALOG_DESC, (int)SubRecord_loopVariable.CATALOG_TYPE, SubRecord_loopVariable.SEQ_NO, SubRecord_loopVariable.QTY, 0, 0, 0, 0);

                switch (SubRecord_loopVariable.CATALOG_TYPE)
                {
                    case CategoryTypes.Category:
                        SubTreeNode.csstype = NodeCssType.Tree_Node_Category;
                        break;
                    case CategoryTypes.Component:
                        SubTreeNode.csstype = NodeCssType.Tree_Node_Component;
                        break;
                    case CategoryTypes.Root:
                        SubTreeNode.csstype = NodeCssType.Tree_Node_Root;
                        break;
                    default:
                        break;
                }

                CurrentNode.children.Add(SubTreeNode);
                CBOMCatalogRecordsToEasyUITreeNode(CBOMCatalogRecords, SubTreeNode);
            }
        }

        public static int GetSeqNo(String _parentguid)
        {
            String str = "DECLARE @Child hierarchyid " +
                            " SELECT @Child = HIE_ID FROM CBOM_CATALOG_V2 " +
                            " WHERE ID = '" + _parentguid + "'" +
                            " SELECT ISNULL(MAX(SEQ_NO),0) AS [SEQ_NO] " +
                            " FROM CBOM_CATALOG_V2 " +
                            " WHERE HIE_ID.GetAncestor(1) = @Child ";

            return Convert.ToInt32(SqlProvider.dbExecuteScalar("CBOMV2", str)) + 1;
        }

        public static Boolean HasRepeatBrother(String _parentguid, String _categoryid)
        {
            String str = "DECLARE @Child hierarchyid " +
                         " SELECT @Child = HIE_ID FROM CBOM_CATALOG_V2 " +
                         " WHERE ID = '" + _parentguid + "' " +
                         " SELECT COUNT(*) FROM CBOM_CATALOG_V2 " +
                         " WHERE HIE_ID.GetAncestor(1) = @Child " +
                         " AND CATALOG_NAME = N'" + _categoryid + "'";
            int count = Convert.ToInt32(SqlProvider.dbExecuteScalar("CBOMV2", str));
            return (count > 0) ? true : false;
        }


        #region Method

        public static string AddNew(string ParentGUID, string CategoryID, string CategoryNote, string OrgID, CategoryTypes CategoryType, string Creator, string CategoryGUID)
        {
            UpdateDBResult res = new UpdateDBResult();

            try
            {
                string guid = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 30);
                int seq = 0;

                if (!HasRepeatBrother(ParentGUID, CategoryID))
                {
                    seq = GetSeqNo(ParentGUID);
                    Tuple<bool, string> result = CreateNewCatalog(ParentGUID, guid, OrgID, CategoryID, CategoryNote, (int)CategoryType, seq, "", Creator, DateTime.Now, CategoryGUID);

                    //// if create new node to database failed, return false
                    if (!result.Item1)
                    {
                        res.IsUpdated = false;
                        res.ServerMessage = "Error occurs while creating data to database.";
                        return Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    }
                }
                else
                {
                    res.IsUpdated = false;
                    res.ServerMessage = "Same name is already existed in same level.";
                    return Newtonsoft.Json.JsonConvert.SerializeObject(res);
                }
            }
            catch (Exception ex)
            {
                res.IsUpdated = false;
                res.ServerMessage = ex.Message;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        public static string UpdateSelectedNode(string GUID, string CategoryID, string Desc)
        {
            UpdateDBResult res = new UpdateDBResult();

            try
            {
                String str = "update CBOM_CATALOG_V2 set CATALOG_NAME = N'" + CategoryID + "', CATALOG_DESC = N'" + Desc + "' where ID = N'" + GUID + "'";
                SqlProvider.dbExecuteNoQuery("CBOMV2", str);
                res.IsUpdated = true;
            }
            catch (Exception ex)
            {
                res.IsUpdated = false;
                res.ServerMessage = ex.Message;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        public static string DeleteNode(string GUID, string NodeType)
        {
            UpdateDBResult res = new UpdateDBResult();
            try
            {
                CategoryTypes type = CategoryTypes.Root;
                if (Enum.TryParse<CategoryTypes>(NodeType, out type) == true)
                {
                    switch (type)
                    {
                        case CategoryTypes.Category:
                        case CategoryTypes.Component:
                            SqlProvider.dbExecuteNoQuery("CBOMV2", "delete from CBOM_CATALOG_V2 where ID = '" + GUID + "'");
                            break;
                        case CategoryTypes.Root:
                        default:
                            break;
                    }
                }
                res.IsUpdated = true;
            }
            catch (Exception ex)
            {
                res.IsUpdated = false;
                res.ServerMessage = ex.Message;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        public static string DropTreeNode(string parentid, string currentid, string currentseq, string targetid, string targetseq, string point)
        {
            UpdateDBResult res = new UpdateDBResult();
            int FinalSeq = 0;

            try
            {
                if (Convert.ToInt32(targetseq) < Convert.ToInt32(currentseq))
                    FinalSeq = point.Equals("top", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(targetseq) : Convert.ToInt32(targetseq) + 1;
                else if (Convert.ToInt32(targetseq) > Convert.ToInt32(currentseq))
                    FinalSeq = point.Equals("top", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(targetseq) - 1 : Convert.ToInt32(targetseq);
                else
                {
                    res.IsUpdated = false;
                    res.ServerMessage = "Invalid operation, please try again.";
                    return Newtonsoft.Json.JsonConvert.SerializeObject(res);
                }

                // final seq = itself, no need to do anything, return false.
                if (FinalSeq == Convert.ToInt32(currentseq))
                {
                    res.IsUpdated = false;
                    res.ServerMessage = "Invalid operation - moving to current sequence.";
                    return Newtonsoft.Json.JsonConvert.SerializeObject(res);
                }

                String str = " DECLARE @ID hierarchyid " +
                             " SELECT @ID  = HIE_ID " +
                             " FROM CBOM_CATALOG_V2 WHERE ID = '" + parentid + "' " +
                             " update CBOM_CATALOG_V2 SET SEQ_NO = SEQ_NO " + (FinalSeq > Convert.ToInt32(currentseq) ? " -1 " : " +1 ") +
                             " WHERE HIE_ID.GetAncestor(1) = @ID " +
                             " AND SEQ_NO >= '" + (FinalSeq > Convert.ToInt32(currentseq) ? Convert.ToInt32(currentseq) : FinalSeq) + "'" +
                             " AND SEQ_NO <= '" + (FinalSeq > Convert.ToInt32(currentseq) ? FinalSeq : Convert.ToInt32(currentseq)) + "' ";
                SqlProvider.dbExecuteNoQuery("CBOMV2", str);
                SqlProvider.dbExecuteNoQuery("CBOMV2", String.Format(" update  CBOM_CATALOG_V2 SET SEQ_NO = " + FinalSeq + " where ID = '" + currentid + "' "));

                res.IsUpdated = true;
            }
            catch (Exception ex)
            {
                res.IsUpdated = false;
                res.ServerMessage = ex.Message;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        public static string ReOrderByAlphabetical(string GUID)
        {
            UpdateDBResult res = new UpdateDBResult();
            String select_str = String.Empty;

            try
            {
                select_str = " DECLARE @Child hierarchyid " +
                             " SELECT @Child = HIE_ID FROM CBOM_CATALOG_V2 " +
                             " WHERE ID = '" + GUID + "' " +
                             " SELECT * FROM CBOM_CATALOG_V2 " +
                             " WHERE HIE_ID.GetAncestor(1) = @Child " +
                             " order by CATALOG_NAME ";
                DataTable dt = SqlProvider.dbGetDataTable("CBOMV2", select_str);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int seq = 0;
                    String str1 = String.Empty;
                    List<String> str2 = new List<string>();

                    foreach (DataRow d in dt.Rows)
                    {
                        str1 += " WHEN '" + d["CATALOG_NAME"].ToString() + "' THEN '" + seq.ToString() + "' ";
                        str2.Add("'" + d["CATALOG_NAME"].ToString() + "'");
                        seq++;
                    }

                    String update_str = " UPDATE CBOM_CATALOG_V2 " +
                             " SET SEQ_NO = CASE CATALOG_NAME " +
                             str1 +
                             " ELSE SEQ_NO " +
                             " END " +
                             " WHERE CATALOG_NAME IN " + "(" + String.Join(", ", str2.ToArray()) + ")" + "; ";
                    SqlProvider.dbExecuteNoQuery("CBOMV2", update_str);

                    res.IsUpdated = true;
                }
                else
                {
                    res.IsUpdated = false;
                    res.ServerMessage = "Children nodes not found.";
                }
            }
            catch (Exception ex)
            {
                res.IsUpdated = false;
                res.ServerMessage = ex.Message;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        public static string ReOrderBySeq(string GUID)
        {
            UpdateDBResult res = new UpdateDBResult();
            String select_str = String.Empty;

            try
            {
                select_str = " DECLARE @Child hierarchyid " +
                             " SELECT @Child = HIE_ID FROM CBOM_CATALOG_V2 " +
                             " WHERE ID = '" + GUID + "' " +
                             " SELECT * FROM CBOM_CATALOG_V2 " +
                             " WHERE HIE_ID.GetAncestor(1) = @Child " +
                             " ORDER BY SEQ_NO ";
                DataTable dt = SqlProvider.dbGetDataTable("CBOMV2", select_str);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int seq = 0;
                    String str1 = String.Empty;
                    List<String> str2 = new List<string>();

                    foreach (DataRow d in dt.Rows)
                    {
                        str1 += " WHEN '" + d["CATALOG_NAME"].ToString() + "' THEN '" + seq.ToString() + "' ";
                        str2.Add("'" + d["CATALOG_NAME"].ToString() + "'");
                        seq++;
                    }

                    String update_str = " UPDATE CBOM_CATALOG_V2 " +
                             " SET SEQ_NO = CASE CATALOG_NAME " +
                             str1 +
                             " ELSE SEQ_NO " +
                             " END " +
                             " WHERE CATALOG_NAME IN " + "(" + String.Join(", ", str2.ToArray()) + ")" + "; ";
                    SqlProvider.dbExecuteNoQuery("CBOMV2", update_str);

                    res.IsUpdated = true;
                }
                else
                {
                    res.IsUpdated = false;
                    res.ServerMessage = "Children nodes not found.";
                }
            }
            catch (Exception ex)
            {
                res.IsUpdated = false;
                res.ServerMessage = ex.Message;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        #endregion


        public static Tuple<bool, string> CreateNewCatalog(String _parentid, String _guid, String _org, String _catalogname,
     String _catalogdesc, int _catalogtype, int _seq, String _ext, String _creator, DateTime _createtime, String _categoryguid)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CBOMV2"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Insert_CBOM_Catalog_V2", conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@parentID", System.Data.SqlDbType.NVarChar, 30);
            cmd.Parameters["@parentID"].Value = _parentid;

            cmd.Parameters.Add("@guid", System.Data.SqlDbType.NVarChar, 30);
            cmd.Parameters["@guid"].Value = _guid;

            cmd.Parameters.Add("@org", System.Data.SqlDbType.NVarChar, 10);
            cmd.Parameters["@org"].Value = _org;

            cmd.Parameters.Add("@catalogname", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@catalogname"].Value = _catalogname;

            cmd.Parameters.Add("@catalogdesc", System.Data.SqlDbType.NVarChar, 300);
            cmd.Parameters["@catalogdesc"].Value = _catalogdesc;

            cmd.Parameters.Add("@catalogtype", System.Data.SqlDbType.Int);
            cmd.Parameters["@catalogtype"].Value = _catalogtype;

            cmd.Parameters.Add("@seq", System.Data.SqlDbType.Int);
            cmd.Parameters["@seq"].Value = _seq;

            cmd.Parameters.Add("@ext", System.Data.SqlDbType.NVarChar, 200);
            cmd.Parameters["@ext"].Value = _ext;

            cmd.Parameters.Add("@creator", System.Data.SqlDbType.NVarChar, 50);
            cmd.Parameters["@creator"].Value = _creator;

            cmd.Parameters.Add("@createtime", System.Data.SqlDbType.DateTime);
            cmd.Parameters["@createtime"].Value = _createtime;

            cmd.Parameters.Add("@categoryguid", System.Data.SqlDbType.NVarChar, 50);
            cmd.Parameters["@categoryguid"].Value = _categoryguid;


            SqlParameter returnData = cmd.Parameters.Add("@OutputID", SqlDbType.NVarChar, 200);
            returnData.Direction = ParameterDirection.Output;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                cmd.Dispose();
                conn.Dispose();
            }
            return new Tuple<bool, string>(true, returnData.Value.ToString());
        }

    }
}
