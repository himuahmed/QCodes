using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.Models
{
    public class PrivateMessageModel
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public string Receiver { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
