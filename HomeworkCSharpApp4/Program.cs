using System.Net.Sockets;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace HomeworkCSharpApp4
{
    internal class Program
    {
        static void Save(string[] args)
        {
            string path = "D:\\GeekBrains\\C#\\serverData.txt";

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                string text = String.Join(" ", args);
                writer.Write(text);
            }
        }

        static string Load()
        {
            string path = "D:\\GeekBrains\\C#\\serverData.txt";

            using (StreamReader reader = new StreamReader(path))
            {
                string text = reader.ReadToEnd();

                return text;
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ChatServer.Server();
            }
            else
            {
                if (args.Length == 2)
                {
                    Save(args);
                    ChatClient.Client(args);
                }
                else
                {
                    string data = Load();
                    string[] dataArray = data.Split(' ');
                    ChatClient.Client(dataArray);
                }
            }
        }
    }
}
