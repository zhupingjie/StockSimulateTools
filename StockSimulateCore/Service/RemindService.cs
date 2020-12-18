using StockSimulateCore.Entity;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Service
{
    public class RemindService
    {
        public static void Handle(string stockCode, int rType, decimal target)
        {
            var remind = SQLiteDBUtil.Instance.QueryFirst<RemindEntity>($"StockCode='{stockCode}' and RType={rType} and Target={target} and Handled='False'");
            if (remind != null)
            {
                remind.Handled = true;
                SQLiteDBUtil.Instance.Update<RemindEntity>(remind);
            }
        }

        public static void Cancel(string stockCode, int rType, decimal target)
        {
            var remind = SQLiteDBUtil.Instance.QueryFirst<RemindEntity>($"StockCode='{stockCode}' and RType={rType} and Target={target}");
            if (remind != null)
            {
                remind.Handled = true;
                SQLiteDBUtil.Instance.Delete<RemindEntity>(remind);
            }
        }
    }
}
