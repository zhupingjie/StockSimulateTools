using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Utils
{
    public class LogUtil
    {
        static LogUtil()
        {
            logger = log4net.LogManager.GetLogger(Assembly.GetEntryAssembly(), "SS");
        }
        private LogUtil()
        {
        }

        private static ILog logger = null;
        public static ILog Logger
        {
            get
            {
                return logger;
            }
        }
    }
}
