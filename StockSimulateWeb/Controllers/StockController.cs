using Microsoft.AspNetCore.Mvc;
using StockSimulateCore.Controllers;
using StockSimulateCore.Data;
using StockSimulateDomain.Entity;
using StockSimulateService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockSimulateWeb.Controllers
{
    public class StockController : BaseController
    {
        [HttpGet]
        public APIResult GetStocks()
        {
            var res = new APIResult();
            res.Result = Repository.Instance.QueryAll<StockEntity>();
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
