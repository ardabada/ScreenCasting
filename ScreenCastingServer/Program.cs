using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using ScreenCasting.Common;
using System.Drawing;

namespace ScreenCastingServer
{
    class Program
    {
        static BackgroundWorker bw = new BackgroundWorker();
        static MainForm mainForm = new MainForm();

        [STAThread]
        static void Main(string[] args)
        {
            //bool x = UPnP.NAT.Discover();
            //UPnP.NAT.ForwardPort(8502, ProtocolType.Tcp, "test");

            NATUPNPLib.UPnPNATClass upnpnat = new NATUPNPLib.UPnPNATClass();
            NATUPNPLib.IStaticPortMappingCollection mappings = upnpnat.StaticPortMappingCollection;
            //Добавление порта
            mappings.Add(12345, "TCP", 12345, GetLocalIPAddress(), true, "Some name");

            //Console.WriteLine(UPnP.NAT.GetExternalIP().ToString());
            bw.DoWork += Bw_DoWork;
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.RunWorkerAsync();


            Application.Run(mainForm);
            //mainForm.Show();

            Console.ReadLine();
            //Удаление порта
            mappings.Remove(12345, "TCP");
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }


        private static void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            mainForm.SetImage(e.UserState as Image);
        }

        private static void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            IPAddress localAddr = IPAddress.Parse(GetLocalIPAddress());
            int port = 12345;
            TcpListener server = new TcpListener(localAddr, port);
            server.Start();
            while (true)
            {
                Console.WriteLine("Ожидание подключений... ");

                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Подключен клиент. Выполнение запроса...");
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = new byte[1024];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int numBytesRead;
                        while ((numBytesRead = stream.Read(data, 0, data.Length)) > 0)
                        {
                            ms.Write(data, 0, numBytesRead);
                        }
                        byte[] result = ms.ToArray();
                        DesktopImageCommand cmd = ScreenCastingCommand.GetCommand(result) as DesktopImageCommand;
                        bw.ReportProgress(0, cmd.Image);
                        Console.WriteLine("Image set");
                    }
                }
            }
        }

        static void doWork()
        {
            //create a new server
            //var server = new UdpServer();

            //Console.WriteLine("Screen casting server is ready");
            //mainForm.Show();

            //start listening for messages and copy the messages back to the client
            //Task.Factory.StartNew(async () => {
            //    while (true)
            //    {
            //        var received = await server.Receive();
            //        mainForm.Draw(received.Message);
            //    }
            //});
        }
    }
}
