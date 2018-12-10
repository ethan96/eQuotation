using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Advantech.Myadvantech.DataAccess
{
    public class Category
    {

        public virtual string Category_ID
        {
            get;
            internal set;
        }

        public virtual string Catalog_ID
        {
            get;
            internal set;
        }

        public virtual string Category_Name
        {
            get;
            internal set;
        }

        public virtual string Parent_Category_ID
        {
            get;
            internal set;
        }

        public virtual Category Parent_Category
        {
            get;
            internal set;
        }

        public virtual string Category_Description
        {
            get;
            internal set;
        }

        public virtual string Display_Name
        {
            get;
            internal set;
        }
        public virtual string Category_Extended_Description
        {
            get;
            internal set;
        }

        public virtual int Sequence
        {
            get;
            internal set;
        }

        public virtual string Keywords
        {
            get;
            internal set;
        }

        public virtual bool ActiveStatus
        {
            get;
            internal set;
        }

        internal void LoadCategoryInformation()
        {
            if (string.IsNullOrEmpty(this.Category_ID)) { return; }
            IDbConnection cnn = DatabaceFactory.GetPISConnection();

            IDbCommand cmd = null;
            IDbDataParameter dp = null;
            DbDataAdapter da = null;

            cmd = DatabaceFactory.CreateCommand(this.GetCategotrySQL(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "category_id";
            dp.Value = this.Category_ID;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataSet DsModel = new DataSet();
            da.Fill(DsModel);

            this.WriteToAttributes(DsModel.Tables[0]);

        }

        private string GetCategotrySQL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Clear();
            sql.Append(" Select c.CATEGORY_ID,c.DISPLAY_NAME,c.PARENT_CATEGORY_ID,c.CATEGORY_DESC ");
            sql.Append(" ,c.EXTENDED_DESC,c.SEQ_NO,c.KEYWORDS,c.ACTIVE_FLG ");
            sql.Append(" From PIS.dbo.Category c ");
            sql.Append(" Where c.CATEGORY_ID=@category_id ");
            return sql.ToString();
        }

        private void WriteToAttributes(DataTable dt)
        {
            DataRow _row = dt.Rows[0];
            //this.Display_Name = (string)_row["CATEGORY_ID"];
            this.Display_Name = (string)_row["DISPLAY_NAME"];
            this.Parent_Category_ID = (string)_row["PARENT_CATEGORY_ID"];
            this.Category_Description = (string)_row["CATEGORY_DESC"];
            this.Category_Extended_Description = (string)_row["EXTENDED_DESC"];
            this.Sequence = (int)_row["SEQ_NO"];
            this.ActiveStatus = (bool)_row["ACTIVE_FLG"];
        }

    }
}
