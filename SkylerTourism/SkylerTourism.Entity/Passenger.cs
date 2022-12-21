using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Entity
{
    public class Passenger
    {
        public int PassengerId { get; set; }
        public string PassengerName { get; set; }
        public string PassengerSurname { get; set; }
        public string IdentityNumber { get; set; }
        public string EMail { get; set; }
        public List<Ticket> Tickets { get; set; }

    }
}
