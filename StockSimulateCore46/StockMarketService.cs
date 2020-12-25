﻿using StockSimulateCore.Config;
using StockSimulateCore.Service;
using StockSimulateCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockSimulateNetCore
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
        #endregion

        #region 服务启动&停止
        public void Start()
        {
            this.LoadConfigData();

            this.GatherData();

            this.RemindData();
        }

        public void Stop()
        {
            this.CancellationTokenSource.Cancel();
        }
        #endregion

        #region 启动事件

        void LoadConfigData()
        {
            Task.Factory.StartNew(() =>
            {
                StockConfigService.LoadGlobalConfig(RC);
            });
        }

        void GatherData()
        {
            //采集价格数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                while (true)
                {
                    this.ActionLog("采集今日股价数据...");
                    if (RC.DebugMode || ObjectUtil.EffectStockDealTime())
                    {
                        StockGatherService.GatherPriceData((message) =>
                        {
                            this.ActionLog(message);
                        });
                    }
                    Thread.Sleep(RC.GatherStockPriceInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            //计算盈亏数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                while (true)
                {
                    this.ActionLog("计算持有股价盈亏数据...");
                    StockPriceService.CalculateProfit((message) =>
                    {
                        this.ActionLog(message);
                    });
                    Thread.Sleep(RC.UpdateAccountStockProfitInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            //计算均线数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                while (true)
                {
                    this.ActionLog("计算股票均线价格数据...");
                    StockPriceService.CalculateAvgrage((message) =>
                    {
                        this.ActionLog(message);
                    });
                    Thread.Sleep(RC.UpdateAccountStockProfitInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            //采集财务数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10000);
                while (true)
                {
                    this.ActionLog("采集财务指标数据...");
                    StockGatherService.GatherFinanceData((message) =>
                    {
                        this.ActionLog(message);
                    });
                    Thread.Sleep(RC.GatherStockFinanceTargetInterval * 1000);
                }
            }, CancellationTokenSource.Token);

            //采集研报数据
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10000);
                while (true)
                {
                    this.ActionLog("采集机构研报数据...");
                    StockGatherService.GatherReportData((message) =>
                    {
                        this.ActionLog(message);
                    });
                    Thread.Sleep(RC.GatherStockReportInterval * 1000);
                }
            }, CancellationTokenSource.Token);
        }

        void RemindData()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    this.ActionLog("检测股价策略提醒数据...");
                    if (ObjectUtil.EffectStockDealTime())
                    {
                        StockRemindService.CheckAutoRun((message) =>
                        {
                            this.ActionLog(message);
                        });
                    }
                    Thread.Sleep(RC.RemindStockStrategyInterval * 1000);
                }
            }, CancellationTokenSource.Token);
        }

        #endregion

        #region 通知消息日志

        public void ActionLog(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}\n");
        }

        #endregion
    }
}