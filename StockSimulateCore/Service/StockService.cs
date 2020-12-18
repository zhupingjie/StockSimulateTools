using StockSimulateCore.Entity;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Service
{
    public class StockService
    {
        public static void Foucs(string stockCode)
        {
            var stock = SQLiteDBUtil.Instance.QueryFirst<StockEntity>($"Code='{stockCode}'");
            if (stock == null) return;

            if (stock.Foucs == 0) stock.Foucs = 1;
            else stock.Foucs = 0;
            SQLiteDBUtil.Instance.Update<StockEntity>(stock);
        }
    }
}
