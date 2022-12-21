using SkylerTourism.Data.Abstract;
using SkylerTourism.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Data.Concrete.EfCore
{
    public class EfCorePassengerRepository : EfCoreGenericRepository<Passenger>, IPassengerRepository
    {
        public EfCorePassengerRepository(SkylerTourismContext _dbContext) : base(_dbContext)
        {

        }

        private SkylerTourismContext context
        {
            get
            {
                return _dbContext as SkylerTourismContext;
            }
        }

    }
}
