using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LPSAccountPasswordReset
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("t123yh 制作");
            Console.WriteLine("将会发送验证短信至指定手机号，但无需获取验证码，系统会自动获得。");
            Console.Write("手机号：");
            string phoneNum = Console.ReadLine();
            Console.Write("新密码：");
            string newPwd = Console.ReadLine();
            if(string.IsNullOrWhiteSpace(newPwd))
            {
                Console.WriteLine("新密码不能为空！");
                return;
            }
            else if(newPwd.Length > 6)
            {
                Console.WriteLine("新密码不超过 6 位！");
                return;
            }
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Proxy = null;
                    client.Encoding = Encoding.UTF8;
                    JObject valiCodeJson = JObject.Parse(client.DownloadString($"http://www.lepeisheng.net:38034/ios/v1_0/validatecode.aspx?accountcode={phoneNum}"));
                    Console.WriteLine($"{valiCodeJson["Message"]}");
                    string code = valiCodeJson["obj"]["ValiCode"].Value<string>();
                    Console.WriteLine($"验证码为：{code}");
                    string base64pwd = Convert.ToBase64String(Encoding.ASCII.GetBytes(newPwd));
                    JObject modPwdResult = JObject.Parse(client.DownloadString($"http://www.lepeisheng.net:38034/ios/v1_0/pwd_reset.aspx?accid={phoneNum}&flag=forgot&newpwd={base64pwd}&valicode={code}"));
                    Console.WriteLine($"{modPwdResult["Message"]}");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            Console.WriteLine("按任意键退出...");
            Console.ReadKey(true);
        }
    }
}
