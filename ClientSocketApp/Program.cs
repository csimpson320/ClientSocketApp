using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientSocketApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteClient();
        }

        static void ExecuteClient() 
        {
            try
            {
                //Create remote endpoint for the socket. Using port 62000 on local host.
				IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint localHost = new IPEndPoint(ipAddr, 62000);

                //Create TCP/IP socket:
                Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                //Connect client to the server:
                sender.Connect(localHost);
                Console.WriteLine("[CLIENT] Connected to {0} ", sender.RemoteEndPoint.ToString());

                //Capture a message from keyboard, store in a byte array and send to server:
                Console.Write("[CLIENT] Enter a message for server: ");
                string message = Console.ReadLine();
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                Console.Write("\n[CLIENT] Transmitting....");
                int sendBytes = sender.Send(messageBytes);

                //Capture message sent back from the server:
                byte[] serverMessage = new byte[1024];
                int receivedBytes = sender.Receive(serverMessage);
                Console.WriteLine("[CLIENT] Message from {ipAddr}: {0}", Encoding.ASCII.GetString(serverMessage, 0, receivedBytes));

                //Close socket
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }

            catch (SocketException se)
            {
                Console.WriteLine("SocketException: {0}", se.ToString());
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected execption: {0}", e.ToString());
            }
        }
    }
}
