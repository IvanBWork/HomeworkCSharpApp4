using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkCSharpApp4
{
    internal class ChatServer
    {
        static private CancellationTokenSource cts = new CancellationTokenSource();

        static private CancellationToken ct =  cts.Token;

        public static void Server()
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Сервер ожидает сообщение от клиента... ");

            while (!ct.IsCancellationRequested)
            {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                if (buffer == null) break;

                var messageText = Encoding.UTF8.GetString(buffer);
                Message? messageServer = Message.DeserializeFromJsonToMessage(messageText);

                Console.WriteLine(messageServer);

                if (messageText.Contains("Exit")) cts.Cancel();

                if (!string.IsNullOrEmpty(messageText))
                {
                    Thread.Sleep(1000);
                    byte[] data = Encoding.UTF8.GetBytes("Сообщение доставлено.");
                    udpClient.Send(data, data.Length, iPEndPoint);
                }
            }

            Console.WriteLine("Сервер завершает работу...");

            Process.GetCurrentProcess().Kill();
        }
    }
}
