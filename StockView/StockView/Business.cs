using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockView
{
    

    class Business
    {

        private DbAccess dal = new DbAccess();

        //根据起始日期，截止日期和参数搜索数据库
        public DataTable search(Args args)
        {
            try
            {
                return dal.Search(args);
            }
            catch
            {
                return null;
            }
        }

        public double[] average(DataTable dataTable)
        {
            //int row = dataTable.Rows.Count;
            //int col = dataTable.Columns.Count;

            double[] res = new double[dataTable.Columns.Count];
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 1; j < dataTable.Columns.Count; j++)
                {
                    res[j] += (double)dataTable.Rows[i][j];
                }
            }
            for (int j = 1; j < dataTable.Columns.Count; j++)
            {
                res[j] /= dataTable.Rows.Count;
            }
            return res;
        }

        public double[] variance(DataTable dataTable)
        {
            //dataTable.Compute()
            return null;
        }

        public double[] sum(DataTable dataTable)
        {
            int col = dataTable.Columns.Count;
            var res = new double[col - 1];

            for (int i = 1; i < col; i++)
            {
                var name = dataTable.Columns[i].ColumnName;
                res[i - 1] = (double)dataTable.Compute("sum(" + name + ")", null);
            }
            return res;
        }

        public int count(DataTable dataTable)
        {
            string name = dataTable.Columns[0].ColumnName;
            int res = (int)dataTable.Compute("count(" + name + ")", null);
            return res;
        }
    }

    public class BusClient
    {
        public BusClient(IPEndPoint ep)
        {
            this.ep = ep;
        }

        IPEndPoint ep;
        private TcpClient Connect()
        {
            //var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            //socket.Connect(endPoint);
            //return socket;
            var client = new TcpClient();
            client.Connect(ep);
            return client;
        }

        public DataTable Search(Args args)
        {
            using (var client = Connect())
            {
                var bytes = args.ToBytes();
                client.Client.Send(bytes);
                
                //接收返回的结果
                var buffer = new byte[1024 * 1024];
                int len = client.Client.Receive(buffer);
                
                return Common.decodeDataTable(buffer, len);
            }

        }



    }

    public class BusServer
    {
        public BusServer(IPEndPoint ep)
        {
            var daEp = new IPEndPoint(IPAddress.Parse(daClientIp), daClientPort);
            daClient = new DbAccessClient(daEp);

            server = new TcpListener(ep);
        }

        TcpListener server;
        DbAccessClient daClient;

        public string daClientIp = "127.0.0.1";
        public int daClientPort = 2223;

        public void Listen()
        {
            server.Start(10);
            while (true)
            {
                var client = server.AcceptTcpClient();
                var buffer = new byte[1024];
                int len = client.Client.Receive(buffer);

                var args = new Args();
                args.Parse(buffer, len);

                var dt = daClient.Search(args);
                buffer = Common.encodeDataTable(dt);
                client.Client.Send(buffer);
                

                Thread.Sleep(200);
            }

        }

        //private void Process(TcpClient client)
        //{
        //    byte[] buffer = new byte[1024];
        //    int len = client.Receive(buffer);
        //    Args args = new Args();
        //    daClient.Search(args);


        //}

        //private DataTable Process(string msg)
        //{

        //    return null;
        //}

    }
}
