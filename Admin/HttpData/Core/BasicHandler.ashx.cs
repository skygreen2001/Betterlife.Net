﻿using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminManage.HttpData.Core
{
    /// <summary>
    /// Summary description for BasicHandler
    /// </summary>
    public class BasicHandler
    {
        protected static BetterlifeNetEntities db = DatabaseCenter.Instance();

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}