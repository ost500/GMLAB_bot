using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApplication1;

namespace WindowsFormsApplication1
{
    public class IPchange
    {
        public static Socket listener = null;
        public static string data = null;
        private Form1 context;
        private sql_connection_manager conn;
        private string current_IP;

        public IPchange(Form1 context, sql_connection_manager conn)
        {
            this.context = context;
            this.conn = conn;

            current_IP = GetComputer_InternetIP();
        }

        public void StartListening()
        {
            //waiting start
            context.ip_changing = true;

            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the 
            // host running the application.
            IPHostEntry ipHostInfo =  Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 2222);

            // Create a TCP/IP socket.
            listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);



            context.log("아이피 변경을 시작합니다");
            context.log("모바일에서 다음의 아이피와 포트를 입력해주세요");
            context.log(localEndPoint.Address.ToString() + " port  " + localEndPoint.Port.ToString());


            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {

                listener.Bind(localEndPoint);

                listener.Listen(10);



                // Start listening for connections.


                context.log("연결을 기다리는 중입니다");


                // Program is suspended while waiting for an incoming connection.
                Socket handler = listener.Accept();
                data = null;

                //// An incoming connection needs to be processed.
                //while (true)
                //{
                //    bytes = new byte[1024];
                //    int bytesRec = handler.Receive(bytes);
                //    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                //    if (data.IndexOf("<EOF>") > -1)
                //    {
                //        break;
                //    }
                //}

                //// Show the data on the console.
                //Console.WriteLine("Text received : {0}", data);

                // Echo the data back to the client.
                byte[] msg = Encoding.ASCII.GetBytes("change");

                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                context.log(e.ToString());
            }

            context.log("아이피 변경 명령을 내리는 중입니다...");
            Thread.Sleep(3000);

            while (true)
            {
                try
                {
                    context.log("현재 아이피 : " + GetComputer_InternetIP() + "\n");
                    context.log("연결 성공!");
                    break;
                }
                catch (Exception ex)
                {
                    context.log("연결 실패 다시 시도중...");
                    Thread.Sleep(1500);
                }
            }
            

            //mysql 재연결
            conn.mysql_refresh();

            //시작 버튼 활성화 시도
            context.start_button_valid("phone");
            //waiting done
            context.ip_changing = false;

        }

        public void send_change()
        {
            context.ip_changing = true;
            try
            {

                // Start listening for connections.


                context.log("\n" + "IP 변경 명령을 내리는 중입니다");


                // Program is suspended while waiting for an incoming connection.
                Socket handler = listener.Accept();
                data = null;

                //// An incoming connection needs to be processed.
                //while (true)
                //{
                //    bytes = new byte[1024];
                //    int bytesRec = handler.Receive(bytes);
                //    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                //    if (data.IndexOf("<EOF>") > -1)
                //    {
                //        break;
                //    }
                //}

                //// Show the data on the console.
                //Console.WriteLine("Text received : {0}", data);

                // Echo the data back to the client.
                byte[] msg = Encoding.ASCII.GetBytes("change");

                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                context.log(e.ToString());
            }

            int IPchange_count = 1;

            while (true)
            {
                //Sleep if IP was not changed
                //Sleep if IPchange_count is not Zero
                if (GetComputer_InternetIP() == current_IP && IPchange_count > 3)
                {
                    IPchange_count--;
                    context.log("IP변경 시도중 " + IPchange_count);

                    Thread.Sleep(1000);

                }
                else
                {
                    break;
                }
            }


            //mysql 재연결
            conn.mysql_refresh();
            context.log("데이터베이스 연결 완료");


            if (IPchange_count > 3)
            {
                context.log("현재 아이피 : " + GetComputer_InternetIP());
                context.log("변경 실패");
            }
            else
            {
                context.log("현재 아이피 : " + GetComputer_InternetIP());
                context.log("변경 완료");
            }




            context.ip_changing = false;
            
        }


        public void sleep_thread() { }

        public void refresh_connection()
        {
            listener.Close();

        }


        public string GetComputer_InternetIP()
        {
            string externalip = new WebClient().DownloadString("http://easygram.kr/ip.php");

            return externalip.Replace("\n", "");
        }

        //public bool socket_check()
        //{
        //    return listener.Connected;
        //}

        public bool socket_check()
        {
            bool part1 = listener.Poll(1000, SelectMode.SelectRead);
            bool part2 = (listener.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

    }
}