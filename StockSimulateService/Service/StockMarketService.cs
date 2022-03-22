using StockSimulateCore.Utils;
using StockSimulateNetService.Serivce;
using StockSimulateService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StockSimulateCore.Config;
using StockSimulateCore.Data;
using StockSimulateDomain.Model;

namespace StockSimulateService.Service
{
    public class StockMarketService
    {
        #region 静态实例

        static readonly StockMarketService instance = new StockMarketService();
        public static StockMarketService Instance
        {
            get
            {
                return instance;
            }
        }
        static StockMarketService()
        {

        }
        private StockMarketService()
        {

        }
        #endregion

        #region 内部属性

        /// <summary>
        /// 线程资源
        /// </summary>
        CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// 环境变量
        /// </summary>
        RunningConfig RC = RunningConfig.Instance;

        /// <summary>
        /// 股票基本信息缓存
        /// </summary>
        List<StockCacheInfo> StockCache = new List<StockCacheInfo>();

        /// <summary>
        /// 股票250日历史价格缓存
        /// </summary>
        List<StockPriceCacheInfo> StockPriceCache = new List<StockPriceCacheInfo>();

        #endregion

        #region 服务启动&停止
        public void Start()
        {
            this.LoadConfigData();

            this.GatherData();

            this.RemindData();

            this.ClearData();
        }

        public void Stop()
        {
            this.CancellationTokenSource.Cancel();

            LogUtil.Debug($"程序停止运行...");
        }

        public void Test()
        {
            StockDebugService.Test();
        }
        #endregion

        #region 启动事件

        void LoadConfigData()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    this.ActionLog("加载全局配置数据...");
                    try
                    {
                        StockConfigService.LoadGlobalConfig(RC);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error($"LoadGlobalConfig Error:{ex.Message}");
                    }
                Thread.Sleep(RC.LoadGlobalConfigInterval * 1000);
                }
            }, CancellationTokenSource.Token);
        }

        void GatherData()
        {
            //采集价格数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                while (true)
                {
                    if (RC.DebugMode || ObjectUtil.EffectStockDealTime())
                    {
                        this.ActionLog("采集今日股价数据...");
                        try
                        {
                            StockGatherService.GatherPriceData((message) =>
                            {
                                this.ActionLog(message);
                            });
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error($"GatherPriceData Error:{ex.Message}");
                        }
                    }
                    Thread.Sleep(RC.GatherStockPriceInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            //采集历史价格数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                while (true)
                {
                    if (RC.GatherHistoryStockPrice && ObjectUtil.EffectStockDealTime())
                    {
                        this.ActionLog("采集历史股价数据...");
                        try
                        {
                            StockGatherService.GatherAllHistoryPriceData((message) =>
                            {
                                this.ActionLog(message);
                            });
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error($"GatherPriceData Error:{ex.Message}");
                        }
                        RC.GatherHistoryStockPrice = false;
                        StockConfigService.SaveGlobalConfig("GatherHistoryStockPrice", false);
                    }
                    Thread.Sleep(RC.UpdateStockAssistTargetInterval * 1000);
                }
            }, CancellationTokenSource.Token);


            //计算盈亏数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                while (true)
                {
                    if (RC.DebugMode || ObjectUtil.EffectStockDealTime())
                    {
                        this.ActionLog("计算持有股价盈亏数据...");
                        try
                        {
                            StockPriceService.CalculateProfit((message) =>
                            {
                                this.ActionLog(message);
                            });
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error($"CalculateProfit Error:{ex.Message}");
                        }
                    }
                    Thread.Sleep(RC.UpdateAccountStockProfitInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            //计算均线数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                while (true)
                {
                    if (RC.DebugMode || ObjectUtil.EffectStockDealTime())
                    {
                        this.ActionLog("计算股票均线价格数据...");
                        try
                        {
                            StockPriceService.CalculateAllAvgrage();
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error($"CalculateNowAvgrage Error:{ex.Message}");
                        }
                    }
                    Thread.Sleep(RC.UpdateStockAssistTargetInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            //计算MACD
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                while (true)
                {
                    if (RC.DebugMode || ObjectUtil.EffectStockDealTime())
                    {
                        this.ActionLog("计算股票MACD数据...");
                        try
                        {
                            StockPriceService.CalculateAllMACD();
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error($"CalculateAllMACD Error:{ex.Message}");
                        }
                    }
                    Thread.Sleep(RC.UpdateStockAssistTargetInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            //计算资金流向
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                while (true)
                {
                    if (RC.DebugMode || ObjectUtil.EffectStockDealTime())
                    {
                        this.ActionLog("计算股票资金流向数据...");
                        try
                        {
                            StockFundService.GatherStockFundFlows();
                            StockFundService.GatherIndustryFundFlows();
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error($"GatherStockFundFlows Error:{ex.Message}");
                        }
                    }
                    Thread.Sleep(RC.UpdateStockAssistTargetInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            //采集基金持仓比例
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                while (true)
                {
                    this.ActionLog("采集基金持仓比例数据...");
                    try
                    {
                        StockGatherService.GatherFundStockData((message) =>
                        {
                            this.ActionLog(message);
                        });
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error($"GatherFundStockData Error:{ex.Message}");
                    }
                    Thread.Sleep(RC.GatherStockFinanceReportInterval * 60 * 60 * 1000);
                }
            }, CancellationTokenSource.Token);

            //采集财务数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10000);
                while (true)
                {
                    this.ActionLog("采集财务指标数据...");
                    try
                    {
                        StockGatherService.GatherFinanceData((message) =>
                        {
                            this.ActionLog(message);
                        });
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error($"GatherFinanceData Error:{ex.Message}");
                    }
                    Thread.Sleep(RC.GatherStockFinanceReportInterval * 60 * 60 * 1000);
                }
            }, CancellationTokenSource.Token);

            //采集研报数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10000);
                while (true)
                {
                    this.ActionLog("采集机构研报数据...");
                    try
                    {
                        StockGatherService.GatherReportData((message) =>
                        {
                            this.ActionLog(message);
                        });
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error($"GatherReportData Error:{ex.Message}");
                    }
                    Thread.Sleep(RC.GatherStockFinanceReportInterval * 60 * 60 * 1000);
                }
            }, CancellationTokenSource.Token);

            //机构预测数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10000);
                while (true)
                {
                    this.ActionLog("采集机构预测数据...");
                    try
                    {
                        StockGatherService.GatherForecastData((message) =>
                        {
                            this.ActionLog(message);
                        });
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error($"GatherForecastData Error:{ex.Message}");
                    }
                    Thread.Sleep(RC.GatherStockFinanceReportInterval * 60 * 60 * 1000);
                }
            }, CancellationTokenSource.Token);
        }

        void RemindData()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                while (true)
                {
                    if (RC.DebugMode || ObjectUtil.EffectStockDealTime(0))
                    {
                        this.ActionLog("检测股价策略提醒数据...");
                        try
                        {
                            StockRemindService.CheckAutoRun((message) =>
                            {
                                this.ActionLog(message);
                            });
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error($"RemindData Error:{ex.Message}");
                        }
                    }
                    Thread.Sleep(RC.RemindStockStrategyInterval * 1000);
                }
            }, CancellationTokenSource.Token);
        }

        void ClearData()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                while (true)
                {
                    if (DateTime.Now.Hour == 8 || DateTime.Now.Hour == 17)
                    {
                        this.ActionLog("清除无效历史数据...");
                        try
                        {
                            LogUtil.Clear(30);
                            StockPriceService.Clear();
                            StockRemindService.Clear();
                        }
                        catch(Exception ex)
                        {
                            LogUtil.Error($"ClearData Error:{ex.Message}");
                        }
                    }
                    Thread.Sleep(30 * 60 * 1000);
                }
            }, CancellationTokenSource.Token);
        }
        #endregion

        #region 通知消息日志

        public void ActionLog(string message)
        {
            //Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}\n");

            LogUtil.Debug(message);
        }

        #endregion
    }
}
