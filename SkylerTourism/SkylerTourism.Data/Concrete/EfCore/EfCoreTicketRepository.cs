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
    public class EfCoreTicketRepository : EfCoreGenericRepository<Ticket>, ITicketRepository
    {
        public EfCoreTicketRepository(SkylerTourismContext _dbContext) : base(_dbContext)
        {

        }

        private SkylerTourismContext context
        {
            get
            {
                return _dbContext as SkylerTourismContext;
            }
        }
        
        public List<int> GetSelectedSeats(int tripId)
        {
            var result = context
                .Tickets
                .Where(t => t.TripId == tripId)
                .Select(t=>t.SeatNumber)
                .ToList();
            return result;
        }
    }
}
