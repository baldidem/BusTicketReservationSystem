using SkylerTourism.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Business.Abstract
{
    public interface ICityService
    {
        List<City> GetAll();
        void Create(City city);

        void Delete(City city);

        City GetById(int id);

        void Update(City city);
    }
}
