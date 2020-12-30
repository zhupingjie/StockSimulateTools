using Microsoft.AspNetCore.Mvc;
using StockSimulateCore.Controllers;
using StockSimulateCore.Data;
using StockSimulateDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockSimulateWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseController : ControllerBase
    {
    }
}
