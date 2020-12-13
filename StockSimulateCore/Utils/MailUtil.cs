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
using StockSimulateCore.Config;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace StockSimulateCore.Utils
{
    public class MailUtil
    {
        public static bool CheckMail(string recMail)
        {
            var regex = new Regex(@"[\w]+@[\w]+.[\w;]+");
            return regex.IsMatch(recMail);
        }

        /// <summary>
        /// bcc = true, recmail以密送方式
        /// </summary>
        /// <param name="mailConfig"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="recmail"></param>
        /// <param name="ccmail"></param>
        /// <param name="attachFiles"></param>
        /// <param name="bcc"></param>
        /// <returns></returns>
        public static bool SendMailAsync(SenderMailConfig mailConfig, string title, string content, string recmail, string ccmail = null, string[] attachFiles = null, bool bcc = false)
        {
            string sendUserId = mailConfig.SendUserId;
            string sendPwd = mailConfig.SendPwd;
            string displayName = mailConfig.DisplayName;

            var client = new SmtpClient();
            client.EnableSsl = mailConfig.EnableSsl;
            client.Host = mailConfig.Host;
            client.Port = mailConfig.Port;
            if (mailConfig.AnonymousAuth)
            {
                client.UseDefaultCredentials = true;
            }
            else
            {
                client.Credentials = new NetworkCredential(sendUserId, sendPwd);
            }
            MailAddress f = new MailAddress(sendUserId, displayName, System.Text.Encoding.UTF8);
            MailMessage oMail = new MailMessage();
            oMail.From = f;
            string[] to = recmail.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
            foreach (var t in to)
            {
                var m = t.Trim();
                if (!string.IsNullOrEmpty(m) && m.Contains("@"))
                {
                    if (bcc)
                    {
                        oMail.Bcc.Add(m);
                    }
                    else
                    {
                        oMail.To.Add(m);
                    }
                }
            }
            if (oMail.To.Count == 0 & !bcc) throw new Exception($"邮件接收人无效:{recmail}");

            if (attachFiles != null)
            {
                foreach (var file in attachFiles)
                {
                    if (!File.Exists(file)) throw new Exception($"邮件附件不存在:{file}");
                    oMail.Attachments.Add(new Attachment(file));
                }
            }

            oMail.IsBodyHtml = true;
            oMail.BodyEncoding = System.Text.Encoding.UTF8;
            oMail.Priority = MailPriority.High;
            oMail.Subject = title;
            oMail.Body = content;
            if (!string.IsNullOrEmpty(ccmail))
            {
                string[] cclist = ccmail.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                foreach (var c in cclist)
                {
                    var m = c.Trim();
                    if (!string.IsNullOrEmpty(m) && m.Contains("@"))
                    {
                        oMail.CC.Add(m);
                    }
                }
            }
            client.Send(oMail);
            return true;
        }
    }
}
