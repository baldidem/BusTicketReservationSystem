using SkylerTourism.Entity;
using System.ComponentModel.DataAnnotations;

namespace SkylerTourism.WebUI.Models
{
    public class BuyTicketModel
    {
        public int TripId { get; set; }
        public int PassengerId { get; set; }
        [Required]
        public string PassengerName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string PassengerSurname { get; set; }
        [Required(ErrorMessage = "It must be 11 digit")]
        [MaxLength(11)]
        [MinLength(11)]
        public string IdentityNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }
        public int SeatCapacity { get; set; }
        [Required]
        public int SeatNumber { get; set; }
        public int BusId { get; set; }
        public int TicketId { get; set; }
    }
}
