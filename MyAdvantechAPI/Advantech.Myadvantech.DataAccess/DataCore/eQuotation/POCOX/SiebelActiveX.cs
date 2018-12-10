using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class SiebelActive
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SiebelActive()
        {
        
        }

        /// <summary>
        /// 建構子，給創建或更新 Opportunity使用
        /// </summary>
        /// <param name="eQuoteID"></param>
        /// <param name="optyName"></param>
        /// <param name="optyStage"></param>
        /// <param name="createdBy"></param>
        /// <param name="optyOwnerEmail"></param>
        public SiebelActive(string eQuoteID, string optyName, string optyStage, string createdBy, string optyOwnerEmail = "")
        {
            this.QuoteID = eQuoteID;
            this.OptyName = optyName;
            this.OptyStage = optyStage;
            this.OptyOwnerEmail = optyOwnerEmail;
            this.CreateBy = createdBy;
            this.LastUpdatedBy = createdBy;

            this.OrderID = string.Empty;
            this.OptyID = string.Empty;
            this.FailedLog = string.Empty;
            this.ProcessID = string.Empty;
        }

        /// <summary>
        /// 建構子，給創建Quote使用
        /// </summary>
        /// <param name="eQuoteID"></param>
        /// <param name="createdBy"></param>
        public SiebelActive(string eQuoteID, string createdBy)
        {
            this.QuoteID = eQuoteID;
            this.CreateBy = createdBy;
            this.LastUpdatedBy = createdBy;

            this.OrderID = string.Empty;
            this.OptyID = string.Empty;
            this.OptyName = string.Empty;
            this.OptyStage = string.Empty;
            this.OptyOwnerEmail = string.Empty;
            this.FailedLog = string.Empty;
            this.ProcessID = string.Empty;
        }

        #endregion
        
        #region Expansion property

        private QuotationMaster master;
        private List<QuotationDetail> detail;
        private optyQuote siebelOpty;
        private quoteSiebelQuote siebelQuote;
        
        #endregion

        #region Expansion property function
        public QuotationMaster QuotationMaster
        {
            get
            {
                if (master == null)
                    master = eQuotationContext.Current.QuotationMaster.Find(this.QuoteID);
                return master;
            }
        }

        public List<QuotationDetail> QuotationDetail
        {
            get
            {
                if (detail == null)
                    detail = eQuotationContext.Current.QuotationDetail.Where(p => p.quoteId == this.QuoteID).ToList();
                return detail;
            }
        }

        public optyQuote SiebelOpty
        {
            get
            {
                if (siebelOpty == null)
                    siebelOpty = eQuotationContext.Current.optyQuote.Find(this.QuoteID);
                return siebelOpty;
            }
        }

        public quoteSiebelQuote SiebelQuote
        {
            get
            {
                if (siebelQuote == null)
                    siebelQuote = eQuotationContext.Current.quoteSiebelQuote.Where(p => p.quoteId == this.QuoteID).FirstOrDefault();
                return siebelQuote;
            }
        }
        #endregion
        
        #region DB functions - only can be used in Businees layer

        private DbContext CurrentContext
        {
            get
            {
                return eQuotationContext.Current;
            }
        }

        internal void Update()
        {
         //   CurrentContext.Entry(this).State = EntityState.Modified;
            CurrentContext.SaveChanges();
        }

        internal void Add()
        {
            ((eQEntities)CurrentContext).SiebelActive.Add(this);
           // CurrentContext.Entry(this).State = EntityState.Added;
            CurrentContext.SaveChanges();
        }

        internal void UpdateActiveProcessing(SiebelActiveUpdatedUser user)
        {
            string sql = string.Format("update SiebelActive set Status='{0}',LastUpdatedBy='{1}',LastUpdatedDate=getdate() where id={2}", SiebelActiveStatus.Processing.ToString(), user.ToString(), this.ID);
            CurrentContext.Database.ExecuteSqlCommand(sql);
        }


        internal void UpdateActiveSuccess(SiebelActiveUpdatedUser user,string wsparas)
        {
            //this.Status = SiebelActiveStatus.Success.ToString();
            //this.LastUpdatedBy = user.ToString();
            //this.LastUpdatedDate = DateTime.Now;
            //this.FailedLog = string.Empty; //清空錯誤欄位
            //this.Update();
            string sql = string.Format("update SiebelActive set Status='{0}',LastUpdatedBy='{1}',LastUpdatedDate=getdate(),FailedLog='{2}',WSParameters='{3}' where id={4}", SiebelActiveStatus.Success.ToString(), user.ToString(), string.Empty, wsparas.Replace("'","''"), this.ID);
            CurrentContext.Database.ExecuteSqlCommand(sql);
        }

        internal void UpdateActiveFailed(SiebelActiveUpdatedUser user, string failedLog, string wsparas)
        {
            //this.Status = SiebelActiveStatus.Failed.ToString();
            //this.LastUpdatedBy = user.ToString();
            //this.LastUpdatedDate = DateTime.Now;
            //this.FailedLog = failedLog;
            //this.Update();
            string sql = string.Format("update SiebelActive set Status='{0}',LastUpdatedBy='{1}',LastUpdatedDate=getdate(),FailedLog='{2}',WSParameters='{3}' where id={4}", SiebelActiveStatus.Failed.ToString(), user.ToString(), failedLog.Replace("'", "''"), wsparas.Replace("'", "''"), this.ID);
            CurrentContext.Database.ExecuteSqlCommand(sql);
        }

        internal void UpdateOptyIDtoOptyQuote(string quoteid, string optyid)
        {
            //this.Status = SiebelActiveStatus.Success.ToString();
            //this.LastUpdatedBy = user.ToString();
            //this.LastUpdatedDate = DateTime.Now;
            //this.FailedLog = string.Empty; //清空錯誤欄位
            //this.Update();
            string sql = string.Format("update optyquote set optyid='{0}' where quoteid='{1}'", optyid, quoteid);
            CurrentContext.Database.ExecuteSqlCommand(sql);

        }


        #endregion
        
    }
}
