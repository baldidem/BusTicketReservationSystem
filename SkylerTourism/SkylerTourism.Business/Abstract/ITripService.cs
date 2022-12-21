using SkylerTourism.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Business.Abstract
{
    public interface ITripService
    {
        List<Trip> GetAll();
        List<Trip> GetAvailableTrips(int departureCityId, int arrivalCityId, DateTime tripDate);
        int GetSeatCapacity(int tripId);
        int GetBusId(int tripId);
        void Create(Trip trip);
        Trip GetById(int tripId);
        void Delete(Trip trip);
        void Update(Trip trip);
        List<Passenger> PassengersByTrip(int id);
        decimal GetPrice(int id);



    }
}
