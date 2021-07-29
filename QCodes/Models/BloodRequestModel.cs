using QCodes.DbObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.Models
{
    public class BloodRequestModel
    {
        public Guid Id { get; set; }
        public Person Person { get; set; }
        public string userId { get; set; }
        public string BloodGroup { get; set; }
        public string Description { get; set; }
        public DateTime createdAt { get; set; }

        public string PersonId { get; set; }
    }
}
