using SkylerTourism.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Data.Abstract
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        List<int> GetSelectedSeats(int tripId);

    }
}
