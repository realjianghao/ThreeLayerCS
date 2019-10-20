using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockView
{
    public class DbAccess
    {
        //数据库连接字符串
        public string connectionStr = "Data Source=PCHAO;" +
            "Initial Catalog=stocktrend;Integrated Security=True;" +
            "Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
            "ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public string ConnectionStr
        {
            get
            {
                return connectionStr;
            }
            set
            {
                connectionStr = value;
            }
        }

        private SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(ConnectionStr);
            connection.Open();
            return connection;
        }
        //根据起始日期，截止日期和参数搜索数据库
        public DataTable Search(Args args)
        {
            string queryArgs = "日期";
            bool[] tmp = { args.OpeningPrice, args.ClosingPrice, args.MaxPrice, args.MinPrice };
            string[] tmpStr = { "[开盘价(元)]", "[收盘价(元)]", "[最高价(元)]", "[最低价(元)]" };

            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i])
                {
                    queryArgs += ", " + tmpStr[i];
                }
            }

            string queryStr = "select " + queryArgs + " from stocktrend where 日期 >= '" + args.BeginDate.ToString("yyyy-MM-dd")
                + "' and 日期 <= '" + args.EndDate.ToString("yyyy-MM-dd") + "'";

            using (var connection = OpenConnection())
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = queryStr;
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
        }

        public DataTable Search2(Args args)
        {
            string queryArgs = "[日期]";
            foreach (var i in args.Items)
            {
                queryArgs += ",[" + i + "]";
            }

            string queryStr = "select " + queryArgs + " from stocktrend where 日期 >= '" + args.BeginDate.ToString("yyyy-MM-dd")
                + "' and 日期 <= '" + args.EndDate.ToString("yyyy-MM-dd") + "'";
            using (var connection = OpenConnection())
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = queryStr;
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
        }
    }

    public class DbAccessClient
    {
        public DbAccessClient(IPEndPoint ep)
        {
            this.ep = ep;

        }

        IPEndPoint ep;

        private TcpClient Connect()
        {
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

                var buffer = new byte[1024 * 1024];
                int len = client.Client.Receive(buffer);

                return Common.decodeDataTable(buffer, len);
            }

        }
    }

    public class DbAccessServer
    {
        public DbAccessServer(IPEndPoint ep)
        {
            server = new TcpListener(ep);
  
        }

        TcpListener server;
        DbAccess da = new DbAccess();

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

                var dt = da.Search2(args);
                buffer = Common.encodeDataTable(dt);
                client.Client.Send(buffer);
                

                Thread.Sleep(200);
            }

        }

        //private void Process(Socket client)
        //{
        //    while (true)
        //    {
        //        byte[] buffer = new byte[1024];
        //        int len = client.Receive(buffer);
        //        Args args = new Args();
        //        dal.Search(args);

        //    }
        //}
    }
}
