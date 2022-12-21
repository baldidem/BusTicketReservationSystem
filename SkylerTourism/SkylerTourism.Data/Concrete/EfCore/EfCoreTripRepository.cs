using Microsoft.EntityFrameworkCore;
using SkylerTourism.Data.Abstract;
using SkylerTourism.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Data.Concrete.EfCore
{
    public class EfCoreTripRepository :  EfCoreGenericRepository<Trip>, ITripRepository
    {
        public EfCoreTripRepository(SkylerTourismContext _dbContext) : base(_dbContext)
        {

        }

        private SkylerTourismContext context
        {
            get
            {
                return _dbContext as SkylerTourismContext;
            }
        }


        public List<Trip> GetAvailableTrips(int departureCityId, int arrivalCityId, DateTime tripDate)
        {
            var list = context
                .Trips
                .Where(t => t.DepartureCityId == departureCityId && t.ArrivalCityId == arrivalCityId && t.TripDate == tripDate)
                .Include(t=>t.DepartureCity)
                .Include(t=>t.ArrivalCity)
                .ToList();
            return list;
        }

        public int GetBusId(int tripId)
        {
            int result = context
                .Trips
                .Where(t => t.TripId == tripId)
                .Select(t => t.BusId)
                .FirstOrDefault();
            return result;

        }

        public decimal GetPrice(int id)
        {
            var result = context
                .Trips
                .Where(t => t.TripId == id)
                .Select(t => t.Price)
                .FirstOrDefault();
            return result;



        }

        public int GetSeatCapacity(int tripId)
        {
            var result= context
                .Trips
                .Where(t => t.TripId == tripId)
                .Select(t => t.Bus.SeatCapacity)
                .FirstOrDefault();
            return result;
        }

        public List<Passenger> PassengersByTrip(int id)
        {
            var passengers = context
                .Tickets
                .Where(t => t.TripId == id)
                .Select(t => t.Passenger)
                .ToList();

            return passengers;
        }

    }
}
