using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Entity
{
    public class Trip
    {
        public int TripId { get; set; }
        public int DepartureCityId { get; set; }
        [ForeignKey("DepartureCityId")]
        public City DepartureCity { get; set; }
        public int ArrivalCityId { get; set; }
        [ForeignKey("ArrivalCityId")]
        public City ArrivalCity { get; set; }
        public DateTime TripDate { get; set; }      
        public  DateTime TripTime { get; set; }
        public decimal Price { get; set; }
        public int BusId { get; set; }
        public Bus Bus { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
