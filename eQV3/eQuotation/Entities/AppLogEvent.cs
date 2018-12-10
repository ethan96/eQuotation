using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eQuotation.Entities
{
    public class AppLogEvent
    {
        /// <summary>
        /// Stores the Id of the log event as a GUID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Uid { get; set; }

        /// <summary>
        /// The date of the log event
        /// </summary>
        public DateTime LogDate { get; set; }

        /// <summary>
        /// The name of the log provider
        /// Example values are NLog, Log4Net, Elmah, Health Monitoring
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Information about where the error occurred
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The machine where the error occured
        /// </summary>
        public string Machine { get; set; }

        /// <summary>
        /// Error code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Title of error 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The level of the message logged
        /// Valid values are : Debug, Info, Warning, Error, Fatal
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// The message that was logged
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// If the message was from an error this value will contain details of the stack trace.
        /// Otherwise it will be empty
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// If the message was from an error this value will contain details of the HTTP Server variables and Cookies.
        /// Otherwise it will be empty
        /// Parameter/Query string passed along request
        /// </summary>
        public string AllXml { get; set; }

        /// <summary> 
        /// Username who logon
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// It contains route data of httpcontext
        /// </summary>
        public string RequestUrl { get; set; }

        public double Duration { get; set; }
    }
}