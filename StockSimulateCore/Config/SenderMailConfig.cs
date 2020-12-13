//
//
//  Copyright (c) SEUNGEE Co.,Ltd. All Rights Reserved.
//
// 	@Author: jeffpan 
// 	@Last Modified by:   jeffpan 
//
//
//
//
using System;
namespace StockSimulateCore.Config
{
    public class SenderMailConfig
    {
        public SenderMailConfig()
        {
        }

        public string SendUserId { get; set; } = "47426568@qq.com";
        public string SendPwd { get; set; } = "qqmm47426568";
        public string DisplayName { get; set; } = "QQMail";
        public string Host { get; set; } = "smtp.qq.com";
        public int Port { get; set; } = 465;

        /// <summary>
        /// 启用SSL加密
        /// </summary>
        public bool EnableSsl { get; set; } = true;

        /// <summary>
        /// 匿名认证
        /// </summary>
        public bool AnonymousAuth { get; set; } = false;

    }
}
