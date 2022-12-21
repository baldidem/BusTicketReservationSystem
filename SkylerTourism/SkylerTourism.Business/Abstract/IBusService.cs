using Microsoft.EntityFrameworkCore;
using SkylerTourism.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Business.Abstract
{
    public interface IBusService
    {
        List<Bus> GetAll();
        public void Create(Bus bus);

        public void Delete(Bus bus);

        public Bus GetById(int id);

        public void Update(Bus bus);

    }
}
