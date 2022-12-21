using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SkylerTourism.Business.Abstract;
using SkylerTourism.Core;
using SkylerTourism.Entity;
using SkylerTourism.WebUI.Identity;
using SkylerTourism.WebUI.Models;
using System;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace SkylerTourism.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICityService _cityService;
        private readonly ITripService _tripService;
        private readonly IBusService _busService;
        private readonly IPassengerService _passengerService;
        private readonly ITicketService _ticketService;
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly SignInManager<MyIdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ICityService cityService,ITripService tripService, IBusService busService, IPassengerService passengerService, ITicketService ticketService, UserManager<MyIdentityUser> userManager, SignInManager<MyIdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _cityService = cityService;
            _tripService = tripService;
            _busService = busService;
            _passengerService = passengerService;
            _ticketService = ticketService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        #region Role
        public IActionResult RoleList()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        public IActionResult RoleCreate()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel roleModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = roleModel.Name
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    TempData["Message"]= Message.CreateMessage("Information", "Role is added", "success");
                    return RedirectToAction("RoleList");
                }
            }

            return View(roleModel);
        }
        public async Task<IActionResult> RoleEdit(string id)
        {
            var users = await _userManager.Users.ToListAsync();
            var role = await _roleManager.FindByIdAsync(id);
            var members = new List<MyIdentityUser>();
            var nonMembers = new List<MyIdentityUser>();
            foreach (var user in users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            RoleDetails roleDetails = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };
            return View(roleDetails);
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel roleEditModel)
        {
            if (ModelState.IsValid)
            {
                //Seçili role eklenecek userlar
                foreach (var userId in roleEditModel.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, roleEditModel.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                //Seçili rolden çıkarılacak userlar
                foreach (var userId in roleEditModel.IdsToRemove ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, roleEditModel.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

            }
            return Redirect($"/Admin/RoleEdit/{roleEditModel.RoleId}");
        }
        public async Task<IActionResult> RoleDelete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(); 
            }
            foreach (var user in await _userManager.Users.ToListAsync())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    TempData["Message"] = Message.CreateMessage("FAIL!", "There are users in this role, you need to delete users first.", "danger");
                    return RedirectToAction("RoleList");
                }
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Message"] = Message.CreateMessage("Success!", "you deleted this role.", "success");
            }
            return RedirectToAction("RoleList");
        }

        #endregion
        #region User
        public async Task<IActionResult> UserList()
        {
            var result = await _userManager.Users.ToListAsync();
            return View(result);
        }
        public async Task<IActionResult> UserCreate()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.Roles = roles;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserCreate(UserModel userModel, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                MyIdentityUser user = new MyIdentityUser()
                {
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    UserName = userModel.UserName,
                    Email = userModel.Email
                };
                var result = await _userManager.CreateAsync(user, "Qwe123.");
                if (result.Succeeded)
                {
                    selectedRoles = selectedRoles ?? new string[] { };
                    await _userManager.AddToRolesAsync(user, selectedRoles);
                    TempData["Message"] = Message.CreateMessage("Congratulations!", "User is created successfully!", "success");
                    return RedirectToAction("UserList");
                }
            }
            ViewBag.SelectedRoles = selectedRoles;
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(userModel);
        }
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) { return RedirectToAction("UserList"); }
            var userModel = new UserModel()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                SelectedRoles = await _userManager.GetRolesAsync(user)
            };
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userModel.UserId);
                if (user != null)
                {
                    user.FirstName = userModel.FirstName;
                    user.LastName = userModel.LastName;
                    user.UserName = userModel.UserName;
                    user.Email = userModel.Email;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        userModel.SelectedRoles = userModel.SelectedRoles ?? new string[] { }; //nullse boş bir dizi yarat dedik.
                        await _userManager.AddToRolesAsync(user, userModel.SelectedRoles.Except(userRoles).ToArray<string>()); //modelden yeni gelenleri ekliyor eskiden var olanlara dokunmadan. aynı user-role eşleşmesi tekrar yazılmasın diye.
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(userModel.SelectedRoles).ToArray<string>()); // edit yaparken çıkarılan rolleri veri tabanından da çıkartmak için yapıyoruz.arraya çeviriyoruz çünkü bize ienumerable tipinde döndürüyor.
                        TempData["Message"] = Message.CreateMessage("Congratulations", "Update is done successfully.", "success");
                        return RedirectToAction("UserList");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    return View(userModel);
                }
                TempData["Message"] = Message.CreateMessage("Fail", "There is no user like that!", "danger");
            }
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(userModel);
        }

        public async Task<IActionResult> UserDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = Message.CreateMessage("Hey!", "you deleted this user.", "danger");
            }
            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> PasswordEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) { return RedirectToAction("UserList"); }
            PasswordModel passwordModel = new PasswordModel() { UserId = user.Id };
            return View(passwordModel);
        }

        [HttpPost]
        public async Task<IActionResult> PasswordEdit(PasswordModel passwordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(passwordModel.UserId);
                if (user == null)
                {
                    return NotFound();
                }
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, passwordModel.Password);
                var result = await _userManager.UpdateAsync(user);
                return RedirectToAction("UserList");
            }
            return View(passwordModel);
        }
        #endregion
        public IActionResult AllTrips()
        {
            List<Trip> trips = _tripService.GetAll();
            ViewBag.Cities = _cityService.GetAll();
            return View(trips);
        }

        public IActionResult TripCreate()
        {
            var result = _cityService.GetAll();
            var result2 = _busService.GetAll();
            TripCreateModel tripCreateModel = new TripCreateModel()
            {
                Cities = result,
                Buses = result2
            };
            return View(tripCreateModel);
        }

        [HttpPost]
        public IActionResult TripCreate(TripCreateModel tripCreateModel)
        {

            DateTime itsNow = DateTime.Now;
            int compareDate = DateTime.Compare(tripCreateModel.TripDate, itsNow);
            if (compareDate < 0)
            {
                if (tripCreateModel.TripDate == new DateTime(1, 1, 1))
                {
                    ViewBag.TripDateErrorMessage = "Please select a date!";
                }
                else
                {
                    ViewBag.TripDateErrorMessage = "You can not select the past time!";
                }

            }


            if (ModelState.IsValid && compareDate >= 0 && tripCreateModel.DepartureCityId != tripCreateModel.ArrivalCityId)
            {

                Trip trip = new Trip()
                {
                    DepartureCityId = tripCreateModel.DepartureCityId,
                    ArrivalCityId = tripCreateModel.ArrivalCityId,
                    TripDate = tripCreateModel.TripDate,
                    TripTime = tripCreateModel.TripTime,
                    Price = tripCreateModel.Price,
                    BusId = tripCreateModel.BusId
                };
                //trip kaydeden methodu yazip burda cagir.
                _tripService.Create(trip);
                TempData["Message"] = Message.CreateMessage("Notification!", "Trip is created.", "success");
                return RedirectToAction("AllTrips", "Admin");
            }


            if (tripCreateModel.DepartureCityId == 0 || tripCreateModel.ArrivalCityId == 0 || tripCreateModel.BusId == 0 || tripCreateModel.Price == 0 || tripCreateModel.TripTime == new DateTime ( 1,1,1))
            {
                var result2 = _cityService.GetAll();
                var result3 = _busService.GetAll();

                TripCreateModel trip = new TripCreateModel()
                {
                    DepartureCityId = tripCreateModel.DepartureCityId,
                    ArrivalCityId = tripCreateModel.ArrivalCityId,
                    TripDate = tripCreateModel.TripDate,
                    TripTime = tripCreateModel.TripTime,
                    Price = tripCreateModel.Price,
                    BusId = tripCreateModel.BusId,
                    Buses = result3,
                    Cities = result2
                };

                ViewBag.DepartureCityErrorMessage = tripCreateModel.DepartureCityId == 0 ? "Please select a departure city!" : "";
                ViewBag.ArrivalCityErrorMessage = tripCreateModel.ArrivalCityId == 0 ? "Please select a arrival city!" : "";
                ViewBag.BusErrorMessage = tripCreateModel.BusId == 0 ? "Please write a Bus Id" : "";
                ViewBag.PriceErrorMessage = tripCreateModel.Price == 0 ? "Please write the Price" : "";
                ViewBag.TimeErrorMessage = tripCreateModel.Price == 0 ? "Please select the Time" : "";


                return View("TripCreate", trip);
            }
            if (tripCreateModel.DepartureCityId == tripCreateModel.ArrivalCityId)
            {
                var result2 = _cityService.GetAll();
                var result3 = _busService.GetAll();
                TripCreateModel trip = new TripCreateModel()
                {
                    TripDate = tripCreateModel.TripDate,
                    TripTime = tripCreateModel.TripTime,
                    Price = tripCreateModel.Price,
                    BusId = tripCreateModel.BusId,
                    Buses = result3,
                    Cities = result2
                };
                ViewBag.SameCityErrorMessage = "You can not select departure city and arrival city same!";
                return View(trip);
            }



            return View(tripCreateModel);
        }
        public IActionResult TripEdit(int id )
        {
            List<City> cities = _cityService.GetAll();
            List<Bus> buses = _busService.GetAll();
            Trip trip = _tripService.GetById(id);
            TripCreateModel tripModel = new TripCreateModel()
            {
                TripId = id,
                TripTime = trip.TripTime,
                TripDate = trip.TripDate,
                Price = trip.Price,
                DepartureCity = trip.DepartureCity,
                ArrivalCity = trip.ArrivalCity,
                DepartureCityId = trip.DepartureCityId,
                ArrivalCityId = trip.ArrivalCityId,
                BusId = trip.BusId,
                Cities = cities,
                Buses =  buses
            };
            return View(tripModel);
        }

        [HttpPost]
        public IActionResult TripEdit(TripCreateModel tripCreateModel)
        {
            if (ModelState.IsValid)
            {
                var trip = _tripService.GetById(tripCreateModel.TripId);

                    trip.TripId = tripCreateModel.TripId;
                    trip.DepartureCityId = tripCreateModel.DepartureCityId;
                    trip.ArrivalCityId = tripCreateModel.ArrivalCityId;
                    trip.TripDate = tripCreateModel.TripDate;
                    trip.TripTime = tripCreateModel.TripTime;
                    trip.Price = tripCreateModel.Price;
                    trip.BusId = tripCreateModel.BusId;
                    trip.DepartureCity = tripCreateModel.DepartureCity;
                trip.ArrivalCity = tripCreateModel.ArrivalCity;
                _tripService.Update(trip);
                TempData["Message"] = Message.CreateMessage("Notification!", "Trip is updated", "warning");
                return RedirectToAction("AllTrips", "Admin");
            }
                tripCreateModel.Cities = _cityService.GetAll();
                tripCreateModel.Buses = _busService.GetAll();

            //return View(tripCreateModel);
            return RedirectToAction("AllTrips");
            //Burda arrival ve daprture citidler gelmiyor??????
        }

        public IActionResult TripDelete(int id)
        {
            Trip trip = _tripService.GetById(id);
            if (trip == null)
            {
                return NotFound();
            }
            _tripService.Delete(trip);
            TempData["Message"] = Message.CreateMessage("Warning!", "Trip is deleted.", "danger");
            return RedirectToAction("AllTrips");
        }
        public IActionResult CityList()
        {
            List<City> cities = _cityService.GetAll();
            return View(cities);
        }
        [HttpPost]
        public IActionResult CityCreate(string CityName)
        {
            if (CityName != null)
            {
                City city = new City()
                {
                    CityName = CityName
                };
                _cityService.Create(city);
                TempData["Message"] = Message.CreateMessage("Notification!", "City is created.", "success");

            }
            TempData["EmptyCityError"] = "Please write a city name!";


            return RedirectToAction("CityList");
        }
        public IActionResult CityDelete(int id)
        {
            City city = _cityService.GetById(id);
            if (city == null)
            {
                return NotFound();
            }
            _cityService.Delete(city);
            TempData["Message"] = Message.CreateMessage("Warning!", "City is deleted.", "success");
            return RedirectToAction("CityList");
        }

        public IActionResult CityEdit(int id)
        {
            var city = _cityService.GetById(id);

            return View(city);
        }
        [HttpPost]
        public IActionResult CityEdit(City city)
        {

                var result = _cityService.GetById(city.CityId);
                result.CityName = city.CityName;
               _cityService.Update(result);
            TempData["Message"] = Message.CreateMessage("Notification!", "City is updated.", "warning");
            return RedirectToAction("CityList");

            
        }
        public IActionResult BusList()
        {
            var busses = _busService.GetAll();
            return View(busses);
        }
        [HttpPost]
        public IActionResult BusCreate(int seatCapacity)
        {
            if (seatCapacity != 0)
            {
                Bus bus = new Bus()
                {
                    SeatCapacity = seatCapacity
                };
                _busService.Create(bus);
                TempData["Message"] = Message.CreateMessage("Notification!", "Bus is created.", "success");
                return RedirectToAction("BusList");
            }else
            {
                Bus bus = new Bus()
                {
                    SeatCapacity = seatCapacity
                };
                
                TempData["SeatCapacityError"]= "Please write a valid number!";
                return RedirectToAction("BusList", bus);
            }
        }
        public IActionResult BusDelete(int id)
        {
            Bus bus = _busService.GetById(id);
            if (bus == null)
            {
                return NotFound();
            }
            _busService.Delete(bus);
            TempData["Message"] = Message.CreateMessage("Warning!", "Bus is deleted.", "danger");
            return RedirectToAction("BusList");
        }
        public IActionResult BusEdit(int id)
        {  
            Bus bus = _busService.GetById(id);
            return View(bus);
        }
        [HttpPost]
        public IActionResult BusEdit(Bus bus)
        {
            Bus updatedBus = _busService.GetById(bus.BusId);
            updatedBus.SeatCapacity = bus.SeatCapacity;
            _busService.Update(updatedBus);
            TempData["Message"] = Message.CreateMessage("Notification!", "Bus is updated.", "warning");
            return RedirectToAction("BusList");
        }
        public IActionResult PassengersByTrip(int id)
        {
            var pass = _tripService.PassengersByTrip(id);
            ViewBag.Required = id;
            return View(pass);
        }

        public IActionResult PassengerCreate(int id)
        {
            var tripId = id;
            var result = _tripService.GetSeatCapacity(tripId);
            var busResult = _tripService.GetBusId(tripId);

            List<int> seats = new List<int>();

            for (int i = 1; i < result; i++)
            {
                seats.Add(i);
            }

            var result2 = _ticketService.GetSelectedSeats(id);

            ViewBag.Seats = seats;
            ViewBag.SelectedSeats = result2;

            foreach (var seat in result2)
            {
                seats.Remove(seat);
            }
            ViewBag.AvailableSeats = seats;

            //BuyTicketModel ticketModel = new BuyTicketModel()
            //{
            //    SeatCapacity = result,
            //    TripId = tripId,
            //    BusId = busResult
            //};


            PaymentTicketModel ticket = new PaymentTicketModel()
            {
                SeatCapacity = result,
                TripId = tripId,
                BusId = busResult
            };
            return View(ticket);
        }
       
        public IActionResult PassengerEdit(int id, int tripId)
        {
            Passenger passenger = _passengerService.GetById(id);
            ViewBag.Required = tripId;
            return View(passenger);
        }
        [HttpPost]
        public IActionResult PassengerEdit(Passenger passenger, int id)
        {
            if (ModelState.IsValid)
            {
                var pass = _passengerService.GetById(passenger.PassengerId);
                pass.PassengerName = passenger.PassengerName;
                pass.PassengerSurname = passenger.PassengerSurname;
                pass.IdentityNumber = passenger.IdentityNumber;
                pass.EMail = passenger.EMail;
                _passengerService.Update(pass);
                TempData["Message"] = Message.CreateMessage("Notification!", "Passenger is updated.", "warning");
                return Redirect($"/Admin/PassengersByTrip/{id}");

            }
            return View(passenger);
        }

        public IActionResult PassengerDelete(int id, int tripId)
        {
            Passenger pass = _passengerService.GetById(id);
            if (pass == null)
            {
                return NotFound();
            }
            _passengerService.Delete(pass);
            TempData["Message"] = Message.CreateMessage("Warning!", "Passenger is deleted.", "danger");
            return Redirect($"/Admin/PassengersByTrip/{tripId}");

        }

    }
}
