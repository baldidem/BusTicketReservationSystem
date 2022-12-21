using SkylerTourism.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkylerTourism.WebUI.Models
{
    public class PaymentTicketModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string CardHolderName { get; set; }
        public string IdentityNumber { get; set; }
        public string CardNumber { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string Cvc { get; set; }
        public decimal Price { get; set; }

        public Ticket Ticket { get; set; }
        public int TripId { get; set; }
        public int PassengerId { get; set; }       
        public int SeatCapacity { get; set; }

        public int SeatNumber { get; set; }
        public int BusId { get; set; }
        public int TicketId { get; set; }

    }
}
