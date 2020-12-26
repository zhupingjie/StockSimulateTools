using StockSimulateDomain.Entity;
using StockSimulateService.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockSimulateService.Service
{
    public class StockMessageService
    {
        public static void SendMessage(StockEntity stock, string title)
        {
            var message = new MessageEntity()
            {
                StockCode = stock.Code,
                StockName = stock.Name,
                Title = title,
                NoticeTime = DateTime.Now,
            };
            MySQLDBUtil.Instance.Insert<MessageEntity>(message);
        }

        public static void Delete(int[] ids)
        {
            MySQLDBUtil.Instance.Delete<MessageEntity>($"ID in ({string.Join(",", ids)})");
        }

        public static void Handled(int[] ids)
        {
            var messages = MySQLDBUtil.Instance.QueryAll<MessageEntity>($"Handled=0 and ReadTime>='{DateTime.Now.ToString("yyyy-MM-dd")}' and ID in ({string.Join(",", ids)})");
            foreach(var message in messages)
            {
                message.Handled = true;
            }
            MySQLDBUtil.Instance.Update<MessageEntity>(messages);
        }

        public static void AllHandled()
        {
            var messages = MySQLDBUtil.Instance.QueryAll<MessageEntity>($"Handled=0 and ReadTime>='{DateTime.Now.ToString("yyyy-MM-dd")}'");
            foreach (var message in messages)
            {
                message.Handled = true;
            }
            MySQLDBUtil.Instance.Update<MessageEntity>(messages);
        }
    }
}
