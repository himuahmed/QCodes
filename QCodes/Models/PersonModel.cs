using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.Models
{
    public class PersonModel
    {
        public string PersonId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Village { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
