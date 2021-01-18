using Microsoft.AspNetCore.Mvc;
using StockSimulateCore.Data;
using StockSimulateDomain.Data;
using StockSimulateDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockSimulateWeb.Base
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseController<TEntity> : ControllerBase where TEntity : BaseEntity, new()
    {
        public APIResult Read(BaseModel model)
        {
            var res = new APIResult();
            res.Result = Repository.Instance.QueryAll<TEntity>(model.Filter, model.OrderBy, model.TakeSize, model.Columns);
            return res;
        }
    }
}
