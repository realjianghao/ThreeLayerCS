using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StockView
{
    public class DatabaseAccessLayer
    {



        //数据库连接字符串
        const string ConnectionStr = "Data Source=PCHAO;" +
            "Initial Catalog=stocktrend;Integrated Security=True;" +
            "Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
            "ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        private SqlConnection openConnection()
        {
            var connection = new SqlConnection(ConnectionStr);
            connection.Open();
            return connection;
        }
        //根据起始日期，截止日期和参数搜索数据库
        public DataSet search(DbArgs args)
        {
            string queryArgs = "日期";
            bool[] tmp = { args.openingPrice, args.closingPrice, args.maxPrice, args.minPrice };
            string[] tmpStr = { "开盘价", "收盘价", "最高价", "最低价" };

            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i])
                {
                    queryArgs += ", " + tmpStr[i];
                }
            }

            string queryStr = "select " + queryArgs + " from stock where 日期 >= '" + args.beginDate.ToShortDateString()
                + "' and 日期 <= '" + args.endDate.ToShortDateString() + "'";

            using (var connection = openConnection())
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = queryStr;
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
    }

    public class DbAccessClient
    {
        public DbAccessClient(string host, int port)
        {
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var ep = new IPEndPoint(IPAddress.Parse(host), port);
            socket.Connect(ep);
        }
        Socket socket;
    }

    public class DbAccessServer
    {
        public DbAccessServer(string host, int port)
        {
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var ep = new IPEndPoint(IPAddress.Parse(host), port);
            socket.Bind(ep);
        }
        Socket socket;
    }
}
