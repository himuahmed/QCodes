using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.DbObjects
{
    public class GlobalMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string user { get; set; }
        [Required]
        public string userId { get; set; }
        [Required]
        public string message { get; set; }
        [Required]
        public string date { get; set; }
    }
}
