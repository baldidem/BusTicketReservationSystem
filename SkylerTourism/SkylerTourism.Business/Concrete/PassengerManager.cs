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
    public class PassengerManager : IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;

        public PassengerManager(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public void Create(Passenger passenger)
        {
            _passengerRepository.Create(passenger);

        }


        public void Delete(Passenger passenger)
        {
            _passengerRepository.Delete(passenger);
        }

        public List<Passenger> GetAll()
        {
            throw new NotImplementedException();
        }

        public Passenger GetById(int id)
        {
            return _passengerRepository.GetById(id);
        }

        public void Update(Passenger passenger)
        {
            _passengerRepository.Update(passenger);
        }
    }
}
