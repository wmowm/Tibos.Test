using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Tibos.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {

        private readonly IOptions<AppSettings> _options;

        public ValuesController(IOptions<AppSettings> options) 
        {
            this._options = options;
        }

        [HttpGet("/")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {$"16:15点钟的时候,我修改了代码,并提交,成了!",$"{_options.Value.test}", $"{_options.Value.test2}" };
        }



        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="host">服务器地址</param>
        /// <param name="displayName">发件人昵称</param>
        /// <param name="userName">邮箱账户名</param>
        /// <param name="password">邮箱密码</param>
        /// <param name="strto">收件人地址</param>
        /// <param name="subject">邮箱主题</param>
        /// <param name="body">邮箱正题</param>
        [HttpPost("/SendEmail")]
        private async Task<JsonResult> SendEmail(string host,string userName,string password,string strto,string displayName,string subject,string body)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式    
            client.Host = host;//邮件服务器
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential(userName, password);//用户名、密码
            client.Port = 80;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress(userName, displayName);
            msg.To.Add(strto);

            msg.Subject = subject;//邮件标题   
            msg.Body = body;//邮件内容   
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码   
            msg.IsBodyHtml = true;//是否是HTML邮件   
            msg.Priority = MailPriority.High;//邮件优先级   

            try
            {
                Console.WriteLine("开始发送邮件");
                client.Send(msg);
                return new JsonResult("邮件发送成功!");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                return new JsonResult("邮件发送失败!" + ex.Message);
            }
        }




    }
}
