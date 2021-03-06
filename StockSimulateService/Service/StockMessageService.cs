﻿using StockSimulateDomain.Entity;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using StockSimulateCore.Data;

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
                Price = stock.Price,
                UDPer = stock.UDPer,
                Title = title,
                NoticeTime = DateTime.Now,
            };
            Repository.Instance.Insert<MessageEntity>(message);
        }

        public static void Delete(int[] ids)
        {
            Repository.Instance.Delete<MessageEntity>($"ID in ({string.Join(",", ids)})");
        }

        public static MessageEntity[] GetWinMessages()
        {
            var messages = Repository.Instance.QueryAll<MessageEntity>($"Handled=0 and NoticeTime<='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' and WinReadTime is null", "ID asc");
            foreach (var message in messages)
            {
                message.WinReadTime = DateTime.Now;
            }
            Repository.Instance.Update<MessageEntity>(messages, new string[] { "WinReadTime" });
            return messages;
        }

        public static MessageEntity[] GetAppMessages()
        {
            var messages = Repository.Instance.QueryAll<MessageEntity>($"Handled=0 and NoticeTime<='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' and AppReadTime is null", "ID asc");
            foreach (var message in messages)
            {
                message.AppReadTime = DateTime.Now;
            }
            Repository.Instance.Update<MessageEntity>(messages, new string[] { "AppReadTime" });
            return messages;
        }

        public static void Handled(int[] ids)
        {
            var messages = Repository.Instance.QueryAll<MessageEntity>($"Handled=0 and ReadTime>='{DateTime.Now.ToString("yyyy-MM-dd")}' and ID in ({string.Join(",", ids)})");
            foreach(var message in messages)
            {
                message.Handled = 1;
            }
            Repository.Instance.Update<MessageEntity>(messages);
        }

        public static void AllHandled()
        {
            var messages = Repository.Instance.QueryAll<MessageEntity>($"Handled=0 and ReadTime>='{DateTime.Now.ToString("yyyy-MM-dd")}'");
            foreach (var message in messages)
            {
                message.Handled = 1;
            }
            Repository.Instance.Update<MessageEntity>(messages);
        }
    }
}
