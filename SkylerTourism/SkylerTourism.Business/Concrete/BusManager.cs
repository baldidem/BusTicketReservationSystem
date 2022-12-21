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
    public class BusManager : IBusService
    {
        private readonly IBusRepository _busRepository;

        public BusManager(IBusRepository busRepository)
        {
            _busRepository = busRepository;
        }

        public void Create(Bus bus)
        {
            _busRepository.Create(bus);
        }

        public void Delete(Bus bus)
        {
            _busRepository.Delete(bus);
        }

        public List<Bus> GetAll()
        {
            return _busRepository.GetAll();
        }

        public Bus GetById(int id)
        {
            return _busRepository.GetById(id);
        }

        public void Update(Bus bus)
        {
            _busRepository.Update(bus);
        }
    }
}
