using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Core
{
    public class Message
    {
        public string Title { get; set; } 
        public string MessageBody { get; set; }
        public string AlertType { get; set; }

        public static string CreateMessage(string title, string messageBody, string alertType)
        {
            var message = new Message()
            {
                Title = title,
                MessageBody = messageBody,
                AlertType = alertType
            };
            return JsonConvert.SerializeObject(message);
        }
    }

}
