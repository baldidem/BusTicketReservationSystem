using SkylerTourism.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Business.Abstract
{
    public interface IPassengerService
    {
        List<Passenger> GetAll();
        public void Create(Passenger passenger);

        public void Delete(Passenger passenger);

        public Passenger GetById(int id);

        public void Update(Passenger passenger);


    }
}
