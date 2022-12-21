using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using SkylerTourism.Business.Abstract;
using SkylerTourism.Core;
using SkylerTourism.Entity;
using SkylerTourism.WebUI.Models;
using System.Net.Sockets;
using Options = Iyzipay.Options;

namespace SkylerTourism.WebUI.Controllers
{

    public class HomeController : Controller
    {
        private ICityService _cityService;
        private ITripService _tripService;
        private ITicketService _ticketService;
        private IPassengerService _passengerService;


        public HomeController(ICityService cityService, ITripService tripService, ITicketService ticketService, IPassengerService passengerService)
        {
            _cityService = cityService;
            _tripService = tripService;
            _ticketService = ticketService;
            _passengerService = passengerService;
        }
        public IActionResult Index()
        {
            var result = _cityService.GetAll();
            SearchTripModel tripModel = new SearchTripModel()
            {
                Cities =  result
            };
            return View(tripModel);
        }
        private SearchTripModel Fill(SearchTripModel tripModel)
        {
            var result = _cityService.GetAll();

            return new SearchTripModel()
            {
                ArrivalCityId = tripModel.ArrivalCityId,
                DepartureCityId = tripModel.DepartureCityId,
                //TripDate = tripModel.TripDate,
                Cities = result
            };
           
        }
        public IActionResult TripList(SearchTripModel tripModel)
        {
            //Departure ve arrival girilmisken eski tarih secebiliyorum. DUZELT BUNU!
            SearchTripModel trip2 = new SearchTripModel();
            DateTime itsNow = DateTime.Now;
            int compareDate = DateTime.Compare(tripModel.TripDate, itsNow);
            if (compareDate < 0)
            {
                trip2 = Fill(tripModel);
                if (tripModel.TripDate==new DateTime(1,1,1))
                {
                    ViewBag.TripDateErrorMessage = "Please select a date!";
                }
                else
                {
                    ViewBag.TripDateErrorMessage = "You can not select the past time!";
                }

            }

            if (ModelState.IsValid && compareDate >= 0)
            {

                var trips = _tripService.GetAvailableTrips(tripModel.DepartureCityId, tripModel.ArrivalCityId, tripModel.TripDate);

                List<TripListModel> tripListModel = trips.Select(t => new TripListModel()
                {
                    TripId = t.TripId,
                    Price = t.Price,
                    TripTime = t.TripTime,
                    TripDate = t.TripDate,
                    DepartureCity = t.DepartureCity,
                    ArrivalCity = t.ArrivalCity
                }).ToList();


                return View(tripListModel);
            }

            //Burdan itibaren hata kontrolleri olacak.
            //1) departure, arrival cityler ve tarih secilmis olacak.
            //2) tarih gecmisten secilemesin.

            if (tripModel.DepartureCityId == 0 || tripModel.ArrivalCityId == 0 )
            {
                var result = _cityService.GetAll();

                SearchTripModel trip = new SearchTripModel()
                {
                    ArrivalCityId = tripModel.ArrivalCityId,
                    DepartureCityId = tripModel.DepartureCityId,
                    TripDate=tripModel.TripDate,
                    Cities = result
                };

                ViewBag.DepartureCityErrorMessage = tripModel.DepartureCityId == 0 ? "Please select a departure city!" : "";
                ViewBag.ArrivalCityErrorMessage = tripModel.ArrivalCityId == 0 ?  "Please select a arrival city!" : "";


                return View("Index",trip);
            }
            if(tripModel.DepartureCityId == tripModel.ArrivalCityId)
            {
                ViewBag.SameCityErrorMessage = "You can not select same cities!";
            }

            return View("Index", trip2);

        }
        public IActionResult BuyTicket(int id)
        {
            var tripId = id;
            var result = _tripService.GetSeatCapacity(tripId);
            var busResult = _tripService.GetBusId(tripId);

            List<int> seats = new List<int>();
            for (int i = 1; i <= result; i++)
            {
                seats.Add(i);
            }

            var result2 = _ticketService.GetSelectedSeats(id);
            //Assagidakini yeni ekliyorum. Bitisine de isaret koyucam.
            ViewBag.Seats = seats;  
            ViewBag.SelectedSeats = result2;

            foreach (var seat in result2)
            {
                seats.Remove(seat);
            }
            if (seats.Count()==0)
            {
                ViewBag.FullBusErrorMessage = "This bus is full";
            }
            //ViewBag.AvailableSeats = seats;

            PaymentTicketModel ticket = new PaymentTicketModel()
            {
                SeatCapacity = result,
                TripId = tripId,
                BusId = busResult
            };
            return View(ticket);
        }



        public Payment PayWithIyzico(PaymentTicketModel paymentTicketModel)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-dYnHCjrjsBujYx70FfUSHpKADG4pOMX4";
            options.SecretKey = "sandbox-lLi8shIfR9j4cESgGMu0Gpjcvh6e7UdH";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";


            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = paymentTicketModel.Price.ToString();
            request.PaidPrice = paymentTicketModel.Price.ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = paymentTicketModel.CardHolderName;
            paymentCard.CardNumber = paymentTicketModel.CardNumber;
            paymentCard.ExpireMonth = paymentTicketModel.ExpireMonth;
            paymentCard.ExpireYear = paymentTicketModel.ExpireYear;
            paymentCard.Cvc = paymentTicketModel.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = paymentTicketModel.FirstName;
            buyer.Surname = paymentTicketModel.LastName;
            buyer.GsmNumber = paymentTicketModel.Phone;
            buyer.Email = paymentTicketModel.Email;
            buyer.IdentityNumber = paymentTicketModel.IdentityNumber;
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = "BI101";
            firstBasketItem.Name = "Binocular";
            firstBasketItem.Category1 = "Collectibles";
            firstBasketItem.Category2 = "Accessories";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = paymentTicketModel.Price.ToString();
            basketItems.Add(firstBasketItem);
            request.BasketItems = basketItems;


            Payment payment = Payment.Create(request, options);
            return payment;
        }

        [HttpPost]
        public IActionResult BuyTicket(PaymentTicketModel paymentTicketModel)
        {

            if (ModelState.IsValid && paymentTicketModel.SeatNumber != 0 )
            {
                paymentTicketModel.Price = _tripService.GetPrice(paymentTicketModel.TripId);

                Payment payment = PayWithIyzico(paymentTicketModel);

                if (payment.Status == "success")
                {

                    Passenger passenger = new Passenger()
                    {
                        PassengerName = paymentTicketModel.FirstName,
                        PassengerSurname = paymentTicketModel.LastName,
                        IdentityNumber = paymentTicketModel.IdentityNumber,
                        EMail = paymentTicketModel.Email
                    };
                    _passengerService.Create(passenger);

                    Ticket ticket = new Ticket()
                    {
                        PassengerId = passenger.PassengerId,
                        TripId = paymentTicketModel.TripId,
                        BusId = paymentTicketModel.BusId,
                        SeatNumber = paymentTicketModel.SeatNumber
                    };
                    _ticketService.Create(ticket);

                    return RedirectToAction("PurchasedTicket", ticket);
                }
  
            }


            var tripId = paymentTicketModel.TripId;
            var result = _tripService.GetSeatCapacity(tripId);
            var busResult = _tripService.GetBusId(tripId);

            List<int> seats = new List<int>();
            for (int i = 1; i < result; i++)
            {
                seats.Add(i);
            }

            var result2 = _ticketService.GetSelectedSeats(tripId);
            
            ViewBag.Seats = seats;
            ViewBag.SelectedSeats = result2;

            foreach (var seat in result2)
            {
                seats.Remove(seat);
            }
            //ViewBag.AvailableSeats = seats;
            TempData["Message"] = Message.CreateMessage("Warning!", "Write your information correctly", "danger");

            return View(paymentTicketModel);        

            }

        public IActionResult PurchasedTicket (Ticket ticket)
        {
            Passenger passenger = _passengerService.GetById(ticket.PassengerId);
            Trip trip = _tripService.GetById(ticket.TripId);
            ViewBag.Cities = _cityService.GetAll();

            PurchasedTicketModel purchasedTicketModel = new PurchasedTicketModel()
            {
                PassengerName = passenger.PassengerName,
                PassengerSurname = passenger.PassengerSurname,
                EMail = passenger.EMail,
                ArrivalCity = trip.ArrivalCity,
                DepartureCity = trip.DepartureCity,
                TripDate = trip.TripDate,
                TripTime = trip.TripTime,
                Price = trip.Price,
                SeatNumber = ticket.SeatNumber
            };

            TempData["Message"] = Message.CreateMessage("Notification!", "Your registration has been successfully created.", "success");

            return View(purchasedTicketModel);    
        }

    }
    
}
