using QCodes.DbObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.Models
{
    public class ChatPersonList
    {
        public PrivateMessage LastMessage {get;set;}
        public Person PersonImChattingWith {get;set;}
    }
}
