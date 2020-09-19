using ScreenCasting.Common;
using System;
using System.Net.Sockets;
using System.Threading;

namespace ScreenCastingClient
{
    class Program
    {
        static ScreenCapture screenCapture;
        static TcpClient client;
        static string ip;
        static int port;
        static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length != 2)
                    return;

                ip = args[0]; // IPAddress.Parse(args[0]);
                port = int.Parse(args[1]);

                Console.WriteLine("Connecting to " + ip + ":" + port);
                //Console.ReadLine();


                screenCapture = new ScreenCapture();
                Timer t = new Timer(timerCallback, null, 0, 100);

                Console.WriteLine("Sent");
                Console.ReadLine();
            }
            catch
            {
                Console.WriteLine("== halted ==");
                Console.ReadLine();
            }
        }

        static void timerCallback(object o)
        {
            DesktopImageCommand cmd = new DesktopImageCommand();
            cmd.Image = screenCapture.CaptureScreen();
            byte[] data = ScreenCastingCommand.Transform(cmd);

            client = new TcpClient();
            client.Connect(ip, port);
            client.GetStream().Write(data, 0, data.Length);
            client.GetStream().Close();
        }
    }
}
