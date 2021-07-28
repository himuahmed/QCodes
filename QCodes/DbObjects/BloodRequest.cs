using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.DbObjects
{
    public class BloodRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string userId { get; set; }
        [Required]
        public string BloodGroup { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime createdAt { get; set; }


        public string PersonId { get; set; }
        public Person Person { get; set; }
    }
}
