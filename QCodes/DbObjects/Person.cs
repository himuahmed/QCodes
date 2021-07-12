﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.DbObjects
{
    public class Person
    {
        [Key]
        public string PersonId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string FullName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string BloodGroup { get; set; }
        public string Division { get; set; }
        public string ContactNo { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Union { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
