using Microsoft.AspNetCore.Mvc;
using StockSimulateCore.Data;
using StockSimulateDomain.Entity;
using StockSimulateService.Service;
using StockSimulateWeb.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockSimulateWeb.Controllers
{
    public class StockController : BaseController<StockEntity>
    {
        [HttpGet]
        public APIResult GetStocks()
        {
            var res = new APIResult();
            res.Result = StockService.GetStockInfos();
            return res;
        }

        [HttpGet]
        public APIResult GetMessages()
        {
            var res = new APIResult();
            res.Result = StockMessageService.GetAppMessages();
            return res;
        }
    }
}
