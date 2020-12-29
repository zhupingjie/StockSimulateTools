using System;
namespace StockSimulateCore.Config
{
    public class SenderMailConfig
    {
        public SenderMailConfig()
        {
        }

        public string SendUserId { get; set; } = "zhupj@foxmail.com";
        public string SendPwd { get; set; } = "oznnqmdunobhbjai";
        public string DisplayName { get; set; } = "QQMail";
        public string Host { get; set; } = "smtp.qq.com";
        public int Port { get; set; } = 587;

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
