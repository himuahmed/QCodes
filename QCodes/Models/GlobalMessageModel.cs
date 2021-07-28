using QCodes.DbObjects;
using System;


namespace QCodes.Models
{
    public class GlobalMessageModel
    {
        public Guid Id { get; set; }
        public string user { get; set; }
        public string userId { get; set; }
        public string message { get; set; }
        public string date { get; set; }
    }
}
