using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Entity
{
    public class Bus
    {
        public int BusId { get; set; }
        public int SeatCapacity { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Trip> Trips { get; set; }

    }
}
