using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using QCodes.DbObjects;

namespace QCodes.Models
{
    public class QTagModel
    {

        [Key]
        public string TagId { get; set; }
        [Required]
        public Person Person { get; set; }
        public string UrlName { get; set; }
        public string Url { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
