using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceTools.Utils
{
    public class LogHelper
    {
        static LogHelper()
        {
            logger = log4net.LogManager.GetLogger("logAppender");
        }
        private LogHelper()
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
