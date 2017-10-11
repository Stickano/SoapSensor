using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// Using TcpClient
namespace UdpBroadcastSender
{
    class Program
    {
        public const int Port = 7000;
        static void Main()
        {
            Random rand = new Random();

            UdpClient socket = new UdpClient();
            socket.EnableBroadcast = true; // IMPORTANT
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, Port);
            while (true)
            {
                int[] randValues = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    randValues[i] = rand.Next(200);
                }
                string message = "Temp: " + randValues[0] + " Light: " + randValues[1] + " Resistence: " + randValues[2] + " Analog: " + randValues[3];
                byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
                socket.Send(sendBuffer, sendBuffer.Length, endPoint);
                Console.WriteLine("Message sent to broadcast address {0} port {1}", endPoint.Address, Port);
                Thread.Sleep(5000);
            }
        }
    }
}
