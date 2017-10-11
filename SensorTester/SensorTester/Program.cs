using SensorTester.ServiceReference1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace SensorTester
{
    class Program
    {
        static void Main(string[] args)
        {

            GetData();
            Console.ReadLine();
            //UdpCapturer();

            /**
             * This function will listen for UDP broadcasts,
             * then send along the received data to a second function
             * which will format the values into something we can work with
             */
            void UdpCapturer()
            {
                const int PORT = 7000;
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, PORT);
                using (UdpClient socket = new UdpClient(endpoint))
                {
                    IPEndPoint newEndpoint = new IPEndPoint(0, 0);
                    while (true)
                    {
                        byte[] received = socket.Receive(ref newEndpoint);
                        string message = Encoding.ASCII.GetString(received, 0, received.Length);
                        Console.WriteLine(message);
                        FilterValues(message);
                    }
                }
            }

            /**
             * This, second function, will be called from the UdpCapturer()
             * and will split apart the integer values, then invoke a third
             * function which will send our newly formatted (integer) values to the db
             */
            void FilterValues(string value)
            {
                string[] values = value.Split(' ');
                string filteredValues = "";
                foreach (string val in values)
                {
                    int isInt;
                    if (int.TryParse(val, out isInt))
                        filteredValues += val + " ";
                }
                SendData(filteredValues);
            }

            /**
             * This, third function, will be called from the FilterValues()
             * and will pass along the data to our Azure database
             */
            void SendData(string value)
            {
                List<int> values = new List<int>();
                string[] split = value.Split(' ');
                foreach(string val in split)
                {
                    int isInt;
                    int.TryParse(val, out isInt);
                    values.Add(isInt);
                }
                using (Service1Client client = new Service1Client())
                {
                    client.Insert(values[1], values[0], values[2], values[3]);
                }
            }

            /**
             * This function will make a call to read all the data in the db.
             * This is a function for it self, and so far you will either be
             * calling the UdpCapturer(), which will set off a chain of function calls
             * that will in the end save the data to a database, or you will call this 
             * function which will read the data instead.
             */
            void GetData()
            {
                using (Service1Client client = new Service1Client())
                {
                    foreach(string val in client.Receive())
                    {
                        Console.WriteLine(val);
                    }
                    Console.WriteLine();
                    Console.WriteLine(client.Avarage(client.Receive()));
                }
            }
        }
    }
}
