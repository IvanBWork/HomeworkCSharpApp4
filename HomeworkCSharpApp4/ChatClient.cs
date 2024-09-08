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
    internal class ChatClient
    {
        public static void Client(string[] args)
        {
            string myNickName = args[0];
            string ip = args[1];

            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);

            while (true)
            {
                string messageTextOut;
                do
                {
                    Console.WriteLine("Введите сообщение: ");
                    messageTextOut = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(messageTextOut));

                Message message = new Message()
                {
                    DateTime = DateTime.Now,
                    NickNameFrom = myNickName,
                    NickNameTo = "Server",
                    Text = messageTextOut
                };
                string json = message.SerializeMessageToJson();

                byte[] data = Encoding.UTF8.GetBytes(json);

                if (messageTextOut == "Exit")
                {
                    Console.WriteLine("Чат закрыт.");
                    udpClient.Send(data, data.Length, iPEndPoint);
                    Thread.Sleep(1000);
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    udpClient.Send(data, data.Length, iPEndPoint);
                }

                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                if (buffer != null)
                {
                    var messageTextIn = Encoding.UTF8.GetString(buffer);
                    Console.WriteLine(messageTextIn);
                    Console.WriteLine();
                }
                else Console.WriteLine("Сообщение не дошло");
            }
        }
    }
}
