using StockSimulateNetCore.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockSimulateNetCore.Data
{
    public class ServiceBase
    {
        RunningConfig RC;

        Repository Repository;

        public ServiceBase(RunningConfig _rc)
        {
            this.RC = _rc;

            Repository = new Repository(_rc);
        }
    }
}
