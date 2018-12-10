using eQuotation.DataAccess;
using eQuotation.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;

namespace eQuotation.Utility
{
    public class LogEventManager : IDisposable
    {
        private bool disposed = false;

        private IUnitOfWork UnitWork { get; set; }

        public RequestInfo Request { get; set; }

        public Exception Exception { get; set; }

        public double Duration { get; set; }

        //private LogEvent Event { get; set; }

        public LogEventManager()
        {
            this.UnitWork = new UnitOfWork();
            //this.Event = new LogEvent();
            this.Request = null;
            this.Exception = null;
        }

        public LogEventManager(RequestInfo request)
            : this()
        {
            this.Request = request;
        }

        public LogEventManager(RequestInfo request, Exception exception)
            : this(request)
        {
            this.Exception = exception;
        }

        public void Info(string message, string source = null, double duration = 0)
        {
            var logEvent = new AppLogEvent();

            logEvent.Level = LogLevel.Info;
            logEvent.LogDate = DateTime.Now;
            logEvent.ProviderName = "Advantech.iFS";
            logEvent.User = HttpContext.Current.User.Identity.Name;
            logEvent.Message = message;
            logEvent.Code = 808;
            logEvent.Source = source;
            logEvent.Duration = duration;

            if (this.Request != null)
            {
                logEvent.User = this.Request.User;
                logEvent.Machine = this.Request.Machine;
                logEvent.RequestUrl = this.Request.Url;
                logEvent.AllXml = this.Request.Params;
            }

            this.UnitWork.AppLogEvent.Insert(logEvent);
            Save();
        }
        public void Debug() { }

        public void Error()
        {
            var logEvent = new AppLogEvent();

            logEvent.Level = LogLevel.Error;
            logEvent.LogDate = DateTime.Now;
            logEvent.ProviderName = "Advantech.iFS";

            if (this.Request != null)
            {
                logEvent.User = this.Request.User;
                logEvent.Machine = this.Request.Machine;
                logEvent.RequestUrl = this.Request.Url;
                logEvent.AllXml = this.Request.Params;
            }

            if (this.Exception != null)
            {
                //get error code
                if (logEvent.Code != 0)
                    logEvent.Code = this.Exception.HResult;
                else
                    logEvent.Code = this.Exception.HResult;

                //read error messave
                if (!string.IsNullOrEmpty(this.Exception.Message))
                    logEvent.Message = this.Exception.Message;
                else
                {
                    if (!string.IsNullOrEmpty(this.Exception.Message))
                        logEvent.Message = this.Exception.Message;

                    if (!string.IsNullOrEmpty(this.Exception.InnerException.Message))
                        logEvent.Message = string.Format("{0} ### {1}", logEvent.Message,
                            this.Exception.InnerException.Message);
                }

                //read stack trace
                logEvent.StackTrace = this.Exception.StackTrace;
                logEvent.Source = this.Exception.Source;
            }


            //insert into database
            this.UnitWork.AppLogEvent.Insert(logEvent);
            Save();

        }

        public void Warning(string message, string source = null)
        {
            var logEvent = new AppLogEvent();

            logEvent.Level = LogLevel.Warning;
            logEvent.LogDate = DateTime.Now;
            logEvent.ProviderName = "Advantech.Europe";
            logEvent.User = HttpContext.Current.User.Identity.Name;
            logEvent.Message = message;
            logEvent.Code = 708;
            logEvent.Source = source;

            if (this.Request != null)
            {
                logEvent.User = this.Request.User;
                logEvent.Machine = this.Request.Machine;
                logEvent.RequestUrl = this.Request.Url;
                logEvent.AllXml = this.Request.Params;
            }

            this.UnitWork.AppLogEvent.Insert(logEvent);
            Save();
        }

        public void Fatal() { }

        private void Save()
        {
            this.UnitWork.Save();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.UnitWork.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}