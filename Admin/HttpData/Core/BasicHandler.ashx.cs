using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.HttpData.Core
{
    /// <summary>
    /// Summary description for BasicHandler
    /// </summary>
    public class BasicHandler
    {
        protected static BetterlifeNetEntities db = new BetterlifeNetEntities();

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}