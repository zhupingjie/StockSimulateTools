using StockSimulateCore.Config;
using StockSimulateCore.Data;
using StockSimulateDomain.Entity;
using StockSimulateService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockSimulateService.Service
{
    public class StockFundService
    {

        public static void GatherStockFundFlows()
        {
            var stocks = Repository.Instance.QueryAll<StockEntity>($"Type=0");
            foreach (var stock in stocks)
            {
                GatherStockFundFlow(stock.Code, stock.PriceDate);
            }
        }

        public static void GatherStockFundFlow(string stockCode, string dealDate)
        {
            var fundFlow = EastMoneyUtil.GetStockFundFlows(stockCode, dealDate);
            if (fundFlow == null) return;

            var fund = Repository.Instance.QueryFirst<FundFlowEntity>($"StockCode='{stockCode}' and DealDate='{dealDate}'");
            if(fund == null)
            {
                Repository.Instance.Insert<FundFlowEntity>(fundFlow);
            }
            else
            {
                fundFlow.ID = fund.ID;
                Repository.Instance.Update<FundFlowEntity>(fundFlow, new string[] { "MainAmount", "RetailAmount", "Amount" });
            }
        }

        public static void GatherIndustryFundFlows()
        {
            var stock = Repository.Instance.QueryFirst<StockEntity>($"Code='{RunningConfig.Instance.SHZSOfStockCode}'");
            if (stock == null) return;

            var newFunds = new List<FundFlowEntity>();
            var updFunds = new List<FundFlowEntity>();
            var funds = Repository.Instance.QueryAll<FundFlowEntity>($"StockCode='' and IndustryName<>'' and DealDate='{stock.PriceDate}'");
            var fundFlows = EastMoneyUtil.GetIndustryFundFlows(stock.PriceDate);
            foreach(var item in fundFlows)
            {
                var fund = funds.FirstOrDefault(c=>c.IndustryName == item.IndustryName);
                if(fund == null)
                {
                    newFunds.Add(item);
                }
                else
                {
                    item.ID = fund.ID;
                    updFunds.Add(item);
                }
            }
            Repository.Instance.Insert<FundFlowEntity>(newFunds.ToArray());
            Repository.Instance.Update<FundFlowEntity>(updFunds.ToArray(), new string[] { "MainAmount", "RetailAmount", "Amount"});
        }
    }
}
