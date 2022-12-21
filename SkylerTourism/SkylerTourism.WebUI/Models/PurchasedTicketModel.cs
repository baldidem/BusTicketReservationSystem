using SkylerTourism.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkylerTourism.WebUI.Models
{
    public class PurchasedTicketModel
    {
        public int PassengerId { get; set; }
        public string PassengerName { get; set; }
        public string PassengerSurname { get; set; }
        public string EMail { get; set; }
        public int TripId { get; set; }
        public int DepartureCityId { get; set; }
        public City DepartureCity { get; set; }
        public int ArrivalCityId { get; set; }
        public City ArrivalCity { get; set; }
        public DateTime TripDate { get; set; }
        public DateTime TripTime { get; set; }
        public decimal Price { get; set; }
        public int SeatNumber { get; set; }
        public List<City> Cities { get; set; }
    }
}
