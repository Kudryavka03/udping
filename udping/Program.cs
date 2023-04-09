using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace udping
{
    class Program
    {
        static Socket client;
        public static float min= 0.00f, max= 0.00f, avg= 0.00f, mdev=0.00f;
        public static ArrayList result = new ArrayList();
        static void Main(string[] args)
        {
            string ipaddr;
            string port;
            string wait;
            int portint, waitint;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                ipaddr = args[0];
                port = args[1];
                portint = Convert.ToInt32(port);
                wait = args[2];
                waitint = Convert.ToInt32(wait);
            }
            catch
            {

                try
                {
                    ipaddr = args[0];
                    port = args[1];
                    portint = Convert.ToInt32(port);
                }
                catch
                {
                    Console.WriteLine("未提供必要的实参");
                    return;
                }
                waitint = 1000;
            }
            var IPEndpoint1 = new IPEndPoint(IPAddress.Parse(ipaddr), portint);
            sendMsg(ipaddr,portint,waitint);
        }
        static void sendMsg(string ipaddr,int port,int wait)
        {
            bool is_first = true;

            int count = 0;
            EndPoint point = new IPEndPoint(IPAddress.Parse(ipaddr), port);
            Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);
            while (true)
            {
                string msg = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) ).ToString();
                long startTimeTest = (long)(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000);
                client.SendTo(Encoding.UTF8.GetBytes(msg), point);
                byte[] bytes = Encoding.UTF8.GetBytes(msg);
                client.ReceiveFrom(bytes, ref point);
                string floattimestr = System.Text.Encoding.UTF8.GetString(bytes);
                long endTimeTest = (long)(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000);
                long lantencyTime = endTimeTest - startTimeTest;
                float lantencyTimeFloat = (float)lantencyTime/10000.000f;
                if (!is_first)//第一次延迟往往大一些，所以不记录第一次
                {
                    result.Add(lantencyTimeFloat);
                    count++;
                    Console.WriteLine($"from {ipaddr}：seq={count} time={lantencyTimeFloat.ToString("F3")}ms");
                    max = (max > lantencyTimeFloat) ? max : lantencyTimeFloat;
                    min = (min!=0 && min < lantencyTimeFloat) ? min : lantencyTimeFloat;
                }
                is_first = false;
                if (wait != 0) Thread.Sleep((wait == 0 ? 1000 : wait));
            }
        }
        public static void myHandler(object sender, ConsoleCancelEventArgs args)
        {
            //TODO:Use Lambda
            ArrayList mdevList = new ArrayList();
            float total2 = 0.00f;
            float total = 0.00f;
            //Calc Avg
            foreach (float i in result)
            {
                total += i;
            }
            avg = total/result.Count;
            //calc every num
            foreach (float i in result)
            {
                mdevList.Add((float)Math.Pow((i-avg),2));
            }
            foreach (float i in mdevList)
            {
                total2 += i;
            }
            mdev = (float)Math.Sqrt(total2 / result.Count);
            Console.WriteLine($"round - trip min/avg/max/mdev = {min.ToString("F3")}/{avg.ToString("F3")}/{max.ToString("F3")}/{mdev.ToString("F3")} ms");
        }
    }
}
