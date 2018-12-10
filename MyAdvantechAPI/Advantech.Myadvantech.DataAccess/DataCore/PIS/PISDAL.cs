using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Advantech.Myadvantech.DataAccess.Entities;
using System.Linq;

namespace Advantech.Myadvantech.DataAccess
{
    public class PISDAL
    {

        //private string ConnStr_PIS  = "Data Source=ACLSTNR12;database=PIS;User Id =PISApp;Password =pisuser;Application Name=PISLibrary";
        //private string ConnStr_PISBackend = "Data Source=ACLSTNR12;database=PISBackend;User Id =PISApp;Password =pisuser;Application Name=PISLibrary";
        private string ConnStr_PIS = "Data Source=ACLSQL4;database=PIS;User Id =PISApp;Password =pisuser;Application Name=PISLibrary";
        private string ConnStr_PISBackend = "Data Source=ACLSQL4;database=PISBackend;User Id =PISApp;Password =pisuser;Application Name=PISLibrary";
        private Boolean IsUsePISBackend = false;
        private SqlConnection sqlConn = null;


    //      Public Sub SetConnectionString(ByVal PISConnectionstr As String)
    //    Me.ConnStr_PIS = PISConnectionstr
    //    IsUsePISBackend = False
    //End Sub

    //Public Sub ConnectToPISBackend(ByVal IsUseBackendDB As Boolean)
    //    Me.IsUsePISBackend = IsUseBackendDB
    //End Sub



        internal void OpenConnection()
        {
            if (this.IsUsePISBackend)
            {
                this.sqlConn = new SqlConnection(ConnStr_PISBackend);
            }
            else
            {
                this.sqlConn = new SqlConnection(ConnStr_PIS);
            }

            if (this.sqlConn.State != ConnectionState.Open) this.sqlConn.Open();
        }

        internal void CloseConnection()
        {
            try {
                if (this.sqlConn != null)
                {
                    this.sqlConn.Close();
                }
            }
            catch { 
            }
        }
        internal DataTable ExccuteDataTable(SqlCommand mSQLCommand, ref SqlConnection conn)
        {

             

                      
                if(string.IsNullOrEmpty(mSQLCommand.CommandText)) return null;

                if (conn.State != ConnectionState.Open) conn.Open();

                mSQLCommand.Connection = conn;

                SqlDataReader _SqlDataReader = mSQLCommand.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(_SqlDataReader);

                return dt;

            }

        //    internal DataTable GetModelListByPart(String partNo)
        //    {

        //    StringBuilder sql = new StringBuilder();
        //    sql.Append(" Select model_name  ");
        //    sql.Append(" From model  ");
        //    SqlCommand sqlCmd = new SqlCommand(sql.ToString(),this.sqlConn);
        //        //sqlCmd.Parameters.AddWithValue
        //    DataTable _dt = null;

        //    using (SqlDataReader dr = sqlCmd.ExecuteReader())
        //    {
        //        _dt = new DataTable();
        //        _dt.Load(dr);
        //    }

        //    return _dt;
        //}

        public static DataTable getMainCategoryList(string ModelName)
        {
            var _sql = new StringBuilder(); var sql = new StringBuilder();
            var DT = new DataTable();
            _sql.AppendLine("SELECT model_name,Category_id,isnull(MainCategory,'') as MainCategory FROM [Category_Model] ");
            _sql.AppendLine("where model_name='" + ModelName + "'");
            DataTable _dt = SqlProvider.dbGetDataTable("PIS", _sql.ToString());
            DT.TableName = "Main Categories List";
            DT.Columns.Add("Path"); DT.Columns.Add("CategoryID"); DT.Columns.Add("Main_Category");

            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow _row in _dt.Rows)
                {
                    string _Path = ""; DataTable dtPath = null;
                    string _Category = _row[1].ToString();
                    string _CategoryID = "";
                    Boolean _MainCategory;
                    if (_row[2].ToString() == "Y")
                    {
                        _MainCategory = true;
                    }
                    else
                    {
                        _MainCategory = false;
                    }

                    for (var i = 0; i <= 6; i++)
                    {
                        if (i == 0)
                        {
                            _CategoryID = _Category;
                        }

                        sql.Clear();
                        sql.AppendLine(" SELECT isnull([PARENT_CATEGORY_ID],'') as PARENT_CATEGORY_ID,isnull([DISPLAY_NAME],'') as DISPLAY_NAME ");
                        sql.AppendLine(" FROM [CATEGORY] With(nolock) ");
                        sql.AppendLine(" where CATEGORY_ID='" + _CategoryID + "' ");

                        dtPath = SqlProvider.dbGetDataTable("PIS", sql.ToString());

                        if (dtPath.Rows.Count != 0)
                        {
                            if (dtPath.Rows[0][0].ToString() == "root")
                            {
                                _Path = dtPath.Rows[0][1].ToString() + " / " + _Path; //抓取Display name
                                break; //已經到root後，抓取完成就離開
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(_Path))
                                {
                                    _Path = dtPath.Rows[0][1].ToString();//抓取Display name
                                }
                                else
                                {
                                    _Path = dtPath.Rows[0][1].ToString() + " / " + _Path; //抓取Display name
                                }
                                _CategoryID = dtPath.Rows[0][0].ToString(); //抓取Parent categoryID
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(_Path))
                    {
                        DataRow newRow_DT = DT.NewRow();
                        newRow_DT["Path"] = _Path;
                        newRow_DT["CategoryID"] = _Category;
                        newRow_DT["Main_Category"] = _MainCategory;

                        DT.Rows.Add(newRow_DT);
                    }
                }

            }
            return DT;
        }


        public static DataTable getPartNo(string ModelName, string partno)
        {
            var _sql = new StringBuilder();
            if (!string.IsNullOrEmpty(partno))
            {
                _sql.AppendLine("SELECT PART_NO,isnull(PRODUCT_DESC,'') as [DESC_PLM],isnull(PRODUCT_DESC,'') as [DESC_COM] ");
                _sql.AppendLine(",'inactive' as ACTIVE,isnull(STATUS,'') AS SAP_STATUS ");
                _sql.AppendLine(" FROM [PIS].[dbo].[PRODUCT_LOGISTICS_NEW] ");
                _sql.AppendLine(" where part_no='" + partno + "' and ORG_ID='TW01' ");
            }
            else
            {
                _sql.AppendLine("select a.PART_NO,isnull(c.PRODUCT_DESC,'') as [DESC_PLM],a.desc_com as [DESC_COM],c.STATUS as SAP_STATUS, ");
                _sql.AppendLine(" CASE when a.status is null Then 'active' when a.status = '' Then 'active' else rtrim(a.status) end as ACTIVE ");
                _sql.AppendLine("from model_product a ");
                _sql.AppendLine("left join PIS.dbo.PRODUCT_LOGISTICS_NEW c on c.part_no=a.part_no  and c.ORG_ID='TW01' ");
                _sql.AppendLine(" where a.model_name='" + ModelName + "' and a.relation = 'product' and a.status <> 'deleted' ");
                _sql.AppendLine("order by a.seq_num ");
            }

            DataTable _dt = SqlProvider.dbGetDataTable("PIS", _sql.ToString());

            return _dt;
        }

        public static string getMarketPlace(string model_name)
        {
            string request = "";
            var _sql = new StringBuilder();
            if (!string.IsNullOrEmpty(model_name))
            {
                _sql.AppendLine("select isnull(Category_id,'') as Category_id from Category_Model ");
                _sql.AppendLine("where Category_id in('7cb22a45-ffb1-4bf6-928e-4e743bf87c15','5e4cbeae-f003-41d1-b9cf-1dc0baefa4ec','fed9a697-c31c-4fee-b112-74ad00b2815f','3a7cf93c-0bd4-4fa3-93b1-1345682be707') ");
                _sql.AppendLine(string.Format("and model_name='{0}' ",model_name));
                
            }
            DataTable _dt = SqlProvider.dbGetDataTable("PIS", _sql.ToString());

            if(_dt.Rows.Count > 0)
            {
                request = _dt.Rows[0][0].ToString();
            }
            
            return request;
        }

        public static string setMarketPlace(string model_id,string model_name,string category_id)
        {
            string request = "";
            var _sql = new StringBuilder();
            if (!string.IsNullOrEmpty(model_name) && !string.IsNullOrEmpty(category_id))
            {
                try
                {
                    _sql.Clear();
                    _sql.AppendLine(string.Format("DELETE FROM Category_Model where model_name='{0}' ", model_name));
                    _sql.AppendLine(" and Category_id in('7cb22a45-ffb1-4bf6-928e-4e743bf87c15','5e4cbeae-f003-41d1-b9cf-1dc0baefa4ec','fed9a697-c31c-4fee-b112-74ad00b2815f','3a7cf93c-0bd4-4fa3-93b1-1345682be707')");
                    var _request1 = SqlProvider.dbExecuteNoQuery("PIS", _sql.ToString());
                    var dt_seq = SqlProvider.dbGetDataTable("PIS", string.Format("select isnull(max(SEQ),'') as seq from Category_Model where Category_id='{0}'", category_id));
                    var dt_ID = SqlProvider.dbGetDataTable("PIS","select isnull(max(ID),'') as ID from Category_Model");
                    int ID = string.IsNullOrEmpty(dt_ID.Rows[0][0].ToString()) ? 0 +1 : Convert.ToInt32(dt_ID.Rows[0][0]) + 1;
                    int seq = string.IsNullOrEmpty(dt_seq.Rows[0][0].ToString()) ? 0 + 1 : Convert.ToInt32(dt_seq.Rows[0][0]) + 1;
                    _sql.Clear();
                    _sql.AppendLine(string.Format("INSERT INTO Category_Model VALUES ({0},'{1}','{2}','{3}'", ID, model_id, model_name, category_id));
                    _sql.AppendLine(string.Format(",{0},GETDATE(),'JJ.Lin@advantech.com.tw','')", seq));
                    var _request = SqlProvider.dbExecuteNoQuery("PIS", _sql.ToString());
                }
                catch(Exception e)
                {
                    request = "setMarketPlace failed : " + e.ToString();
                }
            }else
            {
                request = "setMarketPlace is failed : model name: " + model_name + " & category id: " + category_id;
            }
           
            return request;
        }

        //public static DataTable getFeature(string ModelName, string Language)
        //{
        //    if (string.IsNullOrEmpty(Language)) Language = "ENU";
        //    var _sql = new StringBuilder(); var sql = new StringBuilder();
        //    _sql.AppendLine("SELECT * FROM model_feature WHERE model_name='" + ModelName + "'");
        //    _sql.AppendLine(" AND LANG_ID='" + Language + "'");

        //    DataTable _dt = SqlProvider.dbGetDataTable("PIS", _sql.ToString());

        //    return _dt;
        //}

        //public static List<display_area> getDisplayArea(string area_id)
        //{
        //    return PISContext.Current.display_area.ToList();
        //}

        public static List<model_displayarea> getModelArea(string model_name)
        {
            return PISContext.Current.model_displayarea.Where(d => d.model_name == model_name).ToList();
        }

        public static int AddLiterature(LITERATURE lit)
        {
            PISContext.Current.LITERATURE.Add(lit);
            return PISContext.Current.SaveChanges();
        }

        public static int DelLiterature(string LiteratureID)
        {
            var lit = from l in PISContext.Current.LITERATURE
                       where l.LITERATURE_ID == LiteratureID
                       select l;
            PISContext.Current.LITERATURE.RemoveRange(lit);
            return PISContext.Current.SaveChanges();
        }

        public static int AddModelLit(Model_lit ml)
        {
            PISContext.Current.Model_lit.Add(ml);
            return PISContext.Current.SaveChanges();
        }

        public static int DelModelLit(string ModelName, string LiteratureID)
        {
            var request = 0;
            if (string.IsNullOrEmpty(ModelName) && string.IsNullOrEmpty(LiteratureID)) return 0;
            var model_lit = from ml in PISContext.Current.Model_lit
                      where (string.IsNullOrEmpty(LiteratureID) ? true : ml.literature_id == LiteratureID)
                      && (string.IsNullOrEmpty(ModelName) ? true : ml.model_name == ModelName)
                      select ml;
            PISContext.Current.Model_lit.RemoveRange(model_lit);
            request = PISContext.Current.SaveChanges();
            return request;
        }

        public static List<LITERATURE> getLiterature(string ModelName,string litType,string lang)
        {
            List<string> _litType = new List<string>();
            if(litType == "picture")
            {
                _litType.Add("Product-Photo(Main)");
                _litType.Add("Product-Photo(B)");
                _litType.Add("Product-Photo(3D)");
                _litType.Add("Product-Photo(board)");
                _litType.Add("Product-Photo(Main)-Thumbnail");
                _litType.Add("Product-Photo(Ori)");
            }else
            {
                _litType.Add("Product-Datasheet");
            }

           var lit = (from m in PISContext.Current.Model_lit
                        join l in PISContext.Current.LITERATURE on m.literature_id equals l.LITERATURE_ID
                        where m.model_name == ModelName 
                        && (litType== "" ? true : _litType.Contains(l.LIT_TYPE))
                        && (lang == "" ? true : l.LANG == lang)
                        select l).ToList();

            return lit;
        }
    }
}
