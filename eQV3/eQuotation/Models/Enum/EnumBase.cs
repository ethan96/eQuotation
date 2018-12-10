using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eQuotation.Models.Enum
{
    public class EnumBase<TEnum> where TEnum : struct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual TEnum ID { get; set; }
        [Required]
        public virtual int Category { get; set; }
        [Required]
        public virtual int Code { get; set; }

        public virtual string Value { get; set; }

    }
}