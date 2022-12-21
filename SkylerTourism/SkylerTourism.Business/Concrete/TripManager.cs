using SkylerTourism.Business.Abstract;
using SkylerTourism.Data.Abstract;
using SkylerTourism.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Business.Concrete
{
    public class TripManager : ITripService
    {
        private readonly ITripRepository _tripRepository;

        public TripManager(ITripRepository tripRepository)
        {
            _tripRepository=tripRepository;
        }

        public void Create(Trip trip)
        {
            _tripRepository.Create(trip);

        }

        public void Delete(Trip trip)
        {
            _tripRepository.Delete(trip);
        }

        public List<Trip> GetAll()
        {
            return _tripRepository.GetAll();
        }

        public List<Trip> GetAvailableTrips(int departureCityId, int arrivalCityId, DateTime tripDate)
        {
            return _tripRepository.GetAvailableTrips(departureCityId, arrivalCityId, tripDate);
        }

        public int GetBusId(int tripId)
        {
            return _tripRepository.GetBusId(tripId);
        }

        public Trip GetById(int tripId)
        {
            return _tripRepository.GetById(tripId);
        }

        public decimal GetPrice(int id)
        {
            return _tripRepository.GetPrice(id);
        }

        public int GetSeatCapacity(int tripId)
        {
            return _tripRepository.GetSeatCapacity(tripId);
        }

        public List<Passenger> PassengersByTrip(int id)
        {
            return _tripRepository.PassengersByTrip(id);
        }

        public void Update(Trip trip)
        {
            _tripRepository.Update(trip);
        }
    }
}
