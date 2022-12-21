using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Entity
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int PassengerId { get; set; }
        public int TripId { get; set; }
        public int BusId { get; set; }
        public int SeatNumber { get; set; }
        public Passenger Passenger { get; set; }
        public Trip Trip { get; set; }
        public Bus Bus { get; set; }
    }
}
