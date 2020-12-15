using ServiceStack;
using ServiceStack.Text;
using StockSimulateCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSimulateCore.Utils
{
    public class EastMoneyUtil
    {
        #region 股价及基本信息
        public static StockInfo GetStockPrice(string code)
        {
            var secid = GetStockSecid(code);
            var api = $"http://push2.eastmoney.com/api/qt/stock/get?fltt=2&secid={secid}&fields=f43,f57,f58,f169,f170,f46,f44,f51,f168,f47,f164,f163,f116,f60,f45,f52,f50,f48,f167,f117,f71,f161,f49,f530,f135,f136,f137,f138,f139,f141,f142,f144,f145,f147,f148,f140,f143,f146,f149,f55,f62,f162,f92,f173,f104,f105,f84,f85,f183,f184,f185,f186,f187,f188,f189,f190,f191,f192,f206,f207,f208,f209,f210,f211,f212,f213,f214,f215,f86,f107,f111,f86,f177,f78,f110,f262,f263,f264,f267,f268,f250,f251,f252,f253,f254,f255,f256,f257,f258,f266,f269,f270,f271,f273,f274,f275,f292";
            var retStr = api.PostJsonToUrl(string.Empty, requestFilter =>
            {
                requestFilter.Timeout = 5 * 60 * 1000;
            });
            var apiModel = ServiceStack.Text.JsonSerializer.DeserializeFromString<EastMoneyAPIModel>(retStr);
            if (apiModel == null || apiModel.data == null) return null;
            var model = apiModel.data;

            var stockPrice = new StockPriceEntity();
            stockPrice.StockCode = code;
            stockPrice.Price = GetNumberValue(model, "f43");
            stockPrice.UDPer = GetNumberValue(model, "f170");  
            stockPrice.TodayStartPrice = GetNumberValue(model, "f46"); 
            stockPrice.TodayMaxPrice = GetNumberValue(model, "f44"); 
            stockPrice.TodayMinPrice = GetNumberValue(model, "f45"); 
            stockPrice.TodayEndPrice = GetNumberValue(model, "f43"); 
            stockPrice.YesterdayEndPrice = GetNumberValue(model, "f60");
            stockPrice.DealQty = GetNumberValue(model, "f47"); 
            stockPrice.DealAmount = GetMillionValue(model, "f48"); 
            stockPrice.Capital = GetMillionValue(model, "f116"); 
            stockPrice.Amount = GetMillionValue(model, "f117");
            stockPrice.TTM = GetNumberValue(model, "f163");
            stockPrice.PE = GetNumberValue(model, "f162");

            var stock = new StockEntity();
            stock.Code = code;
            stock.Name = GetStringValue(model, "f58");
            stock.UDPer = GetNumberValue(model, "f170");
            stock.EPS = GetNumberValue(model, "f55");
            stock.BVPS = GetNumberValue(model, "f92");
            stock.PE = GetNumberValue(model, "f162");
            stock.TTM = GetNumberValue(model, "f163");
            stock.PB = GetNumberValue(model, "f167");
            stock.PEG = 0;
            stock.ROE = 0;
            stock.ROIC = 0;
            stock.Capital = GetMillionValue(model, "f116");
            stock.Amount = GetMillionValue(model, "f117"); 
            stock.Price = GetNumberValue(model, "f43");
            stock.DebtRage = GetNumberValue(model, "f188");
            stock.GrossRate = GetNumberValue(model, "f186");
            stock.NetRate = GetNumberValue(model, "f187");
            stock.NetProfit = GetMillionValue(model, "f105");
            stock.TotalRevenue = GetMillionValue(model, "f183");
            stock.RevenueGrewPer = GetNumberValue(model, "f184");
            stock.ProfitGrewPer = GetNumberValue(model, "f185");

            return new StockInfo()
            {
                Stock = stock,
                DayPrice = stockPrice
            };
        }

        #endregion

        #region 财务主要指标

        public static MainTargetEntity[] GetMainTargets(string stockCode, int type)
        {
            var josnParam = ServiceStack.Text.JsonSerializer.SerializeToString(new
            {
                type = type,
                code = stockCode
            });
            var retStr = "http://f10.eastmoney.com/NewFinanceAnalysis/MainTargetAjax".PostJsonToUrl(josnParam, requestFilter => {
                requestFilter.Timeout = 5 * 60 * 1000;
            });
            retStr = retStr.Replace("亿", "");
            var result = ServiceStack.Text.JsonSerializer.DeserializeFromString<MainTargetEntity[]>(retStr);
            if(result != null)
            {
                result.ToList().ForEach(x =>
                {
                    x.StockCode = stockCode;
                    x.Rtype = type;
                });
            }
            return result;
        }

        public static DataTable ConvertMainTargetData(MainTargetEntity[] data)
        {
            var dt = new DataTable();
            dt.Columns.Add("指标");
            var dates = data.GroupBy(c => c.Date).Select(c => c.Key).OrderByDescending(c => c).ToArray();
            foreach (var date in dates)
            {
                dt.Columns.Add(date);
            }
            dt.Columns.Add("平均增长");
            dt.Columns.Add("近期增长");

            //基本每股收益
            BuildTargetRow(dt, "Jbmgsy", dates, data);
            //每股净资产(元)
            BuildTargetRow(dt, "Mgjzc", dates, data);
            //每股公积金(元)
            BuildTargetRow(dt, "Mggjj", dates, data);
            //每股未分配利润(元)
            BuildTargetRow(dt, "Mgwfply", dates, data);
            //每股经营现金流(元)
            BuildTargetRow(dt, "Mgjyxjl", dates, data);
            //营业总收入(元)
            BuildTargetRow(dt, "Yyzsr", dates, data);
            //归属净利润(元)
            BuildTargetRow(dt, "Gsjlr", dates, data);
            //扣非净利润(元)
            BuildTargetRow(dt, "Kfjlr", dates, data);
            //营业总收入同比增长(%)
            BuildTargetRow(dt, "Yyzsrtbzz", dates, data);
            //归属净利润同比增长(%)
            BuildTargetRow(dt, "Gsjlrtbzz", dates, data);
            //毛利率(%)
            BuildTargetRow(dt, "Mll", dates, data);
            //资产负债率(%)
            BuildTargetRow(dt, "Zcfzl", dates, data);

            return dt;
        }

        static void BuildTargetRow(DataTable dt, string field, string[] dates, MainTargetEntity[] data)
        {
            //基本每股收益
            var dr = dt.NewRow();
            dr["指标"] = ObjectUtil.GetPropertyDesc(typeof(MainTargetEntity), field);//"基本每股收益(元)";

            var vals = new List<decimal>();
            var percent = new List<decimal>();
            foreach (var date in dates)
            {
                var item = data.FirstOrDefault(c => c.Date == date);

                var value = ObjectUtil.GetPropertyValue<decimal>(item, field);
                dr[date] = value;
                if (vals.Count == 0)
                {
                    vals.Add(value);
                }
                else if (vals.Count == 1)
                {
                    if (value != 0)
                    {
                        var p = Math.Round((vals.FirstOrDefault() - value) / value, 2) * 100;
                        percent.Add(p);
                    }
                    else
                    {
                        percent.Add(0);
                    }
                    vals.Clear();
                }
            }
            dr["平均增长"] = $"{percent.Sum() / percent.Count}%";
            dr["近期增长"] = $"{percent.FirstOrDefault()}%";
            dt.Rows.Add(dr);
        }

        #endregion

        #region 辅助方法

        static string GetStockSecid(string code)
        {
            var secid = code.Substring(0, 2) == "SZ" ? "0" : "1";

            return $"{secid}.{code.Substring(2, 6)}";
        }

        static string GetStringValue(Dictionary<string, object> model, string field)
        {
            if (!model.ContainsKey(field)) throw new Exception($"API接口未返回键名:{field}");

            return ObjectUtil.ToValue<string>(model[field], "");
        }

        static decimal GetNumberValue(Dictionary<string, object> model, string field)
        {
            if (!model.ContainsKey(field)) throw new Exception($"API接口未返回键名:{field}");

            var val = ObjectUtil.ToValue<decimal>(model[field], 0);
            return Math.Round(val, 3);
        }

        static decimal GetMillionValue(Dictionary<string, object> model, string field)
        {
            if (!model.ContainsKey(field)) throw new Exception($"API接口未返回键名:{field}");

            var val = ObjectUtil.ToValue<decimal>(model[field], 0);
            return Math.Round(val / 100000000, 3);
        }

        #endregion
    }

    public class EastMoneyAPIModel
    {
        public Dictionary<string, object> data { get; set; }
    }

    public class StockInfo
    {
        public StockEntity Stock { get; set; }

        public StockPriceEntity DayPrice { get; set; }
    }
}
