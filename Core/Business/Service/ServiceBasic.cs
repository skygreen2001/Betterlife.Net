using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Core.Service
{
    public class ServiceBasic
    {
        protected static BetterlifeNetEntities db;
        public ServiceBasic(){
            if (db == null) db = DatabaseCenter.Instance();
        }

    }
}
