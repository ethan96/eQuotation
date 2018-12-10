using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eQuotation.Entities
{
    public class AppError
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public int Code { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string UserName { get; set; }

        public DateTime Timestamp { get; set; }

        public string Client { get; set; }

        public string ParamInfo { get; set; }

        public string StackTrace { get; set; }
    }
}