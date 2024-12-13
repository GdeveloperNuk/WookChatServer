using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WookChat.SERVER.Sources
{
	internal class Server
	{
		private static IPAddress serverIp = IPAddress.Parse("192.168.10.199");
		private static int port = 9999;
		private static TcpListener server;
		private static TcpClient client;
		private static NetworkStream stream;

		static void Main()
		{
			Console.WriteLine("서버 콘솔창");
			Console.WriteLine("===========");

			//서버 오픈
			server = new TcpListener(serverIp, port);
			server.Start();

			//서버 기능 쓰레드
			Thread serverRun = new Thread(ServerRun);
			serverRun.Start();
		}

		/// <summary>
		/// 서버 기능 쓰레드
		/// </summary>
		private static void ServerRun()
		{
			while (true)
			{
				try
				{
					client = server.AcceptTcpClient();

					Client clientData = new Client(client);

					clientData.client.GetStream().BeginRead(clientData.data, 0, clientData.data.Length, new AsyncCallback(DataReceived), clientData);

				}
				catch (SocketException ex)
				{
					Console.WriteLine($"서버 오류 : {ex}");
					break;	
				}
			}

		}
		private static void DataReceived(IAsyncResult ar)
		{
			Client callbackClient = ar.AsyncState as Client;

			int bytesRead = callbackClient.client.GetStream().EndRead(ar);

			string readString = Encoding.Default.GetString(callbackClient.data, 0, bytesRead);

			Console.WriteLine("{0}번 사용자 : {1}", callbackClient.clientID, readString);

			callbackClient.client.GetStream().BeginRead(callbackClient.data, 0, callbackClient.data.Length, new AsyncCallback(DataReceived), callbackClient);
		}
	}
}