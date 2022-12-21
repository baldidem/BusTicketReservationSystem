using SkylerTourism.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Data.Abstract
{
    public interface ITripRepository : IRepository<Trip>
    {
        List<Trip> GetAvailableTrips(int departureCityId, int arrivalCityId, DateTime tripDate);
        int GetSeatCapacity(int tripId);
        int GetBusId(int tripId);
        List<Passenger> PassengersByTrip(int id);
        decimal GetPrice(int id);


    }
}
