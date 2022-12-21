using SkylerTourism.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Business.Abstract
{
    public interface ITicketService
    {
        List<Ticket> GetAll();

         void Delete(Ticket ticket);

         Ticket GetById(int id);

         void Update(Ticket ticket);
        void Create(Ticket ticket);
        List<int> GetSelectedSeats(int tripId);


    }
}
