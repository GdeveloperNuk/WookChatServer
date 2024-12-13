using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WookChat.SERVER
{
	public partial class Client
	{
		public TcpClient client { get; set; }
		public byte[] data { get; set; }
		public int clientID;

		public Client(TcpClient client)
		{
			this.client = client;
			this.data = new byte[1024];

			// 아래부분이 1:1비동기서버에서 추가된 부분입니다.
			// 127.0.0.1:9999에서 포트번호 직전 마지막번호를 클라이언트 번호로 지정해줍니다.
			string clientEndPoint = client.Client.LocalEndPoint.ToString();
			char[] point = { '.', ':' };
			string[] splitedData = clientEndPoint.Split(point);
			this.clientID = int.Parse(splitedData[3]);
			Console.WriteLine("{0}번 사용자 접속성공", clientID);
		}
	}
}
