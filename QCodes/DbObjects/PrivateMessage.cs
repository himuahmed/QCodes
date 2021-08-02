using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.DbObjects
{
    public class PrivateMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Receiver { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool isDelivered { get; set; }
    }
}
