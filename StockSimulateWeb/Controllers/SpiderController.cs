using Microsoft.AspNetCore.Mvc;
using StockSimulateDomain.Entity;
using StockSimulateWeb.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockSimulateWeb.Controllers
{
    public class SpiderController : BaseController<StockEntity>
    {
        public APIResult Index()
        {
            var res = new APIResult();
            return res;
        }
    }
}
