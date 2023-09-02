using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace WebSocketTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //第一步：为当前项目添加 nutget引用。 工具---nutget包管理器--管理解决方案的nutget程序包
          

            //注意：使用实例2之前，先将实例1模块代码注释掉
         
            #region 实例2
            Console.WriteLine(DateTime.Now.ToString() + "   | 实例2 使用说明:");
            Console.WriteLine("1.先启动本程序，在打开实例2页面建立连接。");
            Console.WriteLine("2.注意：本程序可以接收任意次数的客户端信息，本程序也可以向客户端发送任意次数的消息");

            FleckLog.Level = LogLevel.Debug;
            var allSockets = new List<IWebSocketConnection>();
            var server = new WebSocketServer("ws://192.168.68.146:7181");
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("与客户端已经连接上 Open!");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("与客户端已经断开连接 Close!");
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine(message);
                    allSockets.ToList().ForEach(s => s.Send("客户端 Echo: " + message));
                };
            });


            var input = Console.ReadLine();
            int i = 0;
            while (input != "exit")
            {
                foreach (var socket in allSockets.ToList())
                {
                    //string result = "0" + "," + "曹凌铭"+"," + "2062410124" + "," + "200.0";
                    //socket.Send(result);
                   // Thread.Sleep(1000);
                    string result1 = "1" + "," + "曹凌铭" + "," + "2062410124" + "," + "200.0";
                    socket.Send(result1);
                    
                    Thread.Sleep(1000);
                   // string result2 = "2" + "," + "曹凌铭" + "," + "2062410124" + "," + "200.0";
                   // socket.Send(result2);
                   // Thread.Sleep(1000);
                    i += 1;
                }
            }

            #endregion

        }
    }
}
