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
    public class TicketManager : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketManager(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public void Create(Ticket ticket)
        {
            _ticketRepository.Create(ticket);
        }

        public void Delete(Ticket ticket)
        {
            throw new NotImplementedException();
        }


        public List<Ticket> GetAll()
        {
            return _ticketRepository.GetAll();
        }

        public Ticket GetById(int id)
        {
            return _ticketRepository.GetById(id);
        }

        public List<int> GetSelectedSeats(int tripId)
        {
            return _ticketRepository.GetSelectedSeats(tripId);
        }

        public void Update(Ticket ticket)
        {
            throw new NotImplementedException();
        }

    }
}
