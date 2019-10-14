using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StockView
{


    class BusinessLogicLayer
    {

        private DatabaseAccessLayer dal = new DatabaseAccessLayer();

        //根据起始日期，截止日期和参数搜索数据库
        public DataSet search(DbArgs args)
        {
            try
            {
                return dal.search(args);
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

    public class BusinessClient
    {
        public BusinessClient(string host, int port)
        {
            socket = new Socket(SocketType.Stream, ProtocolType.IPv4);

            socket.Connect(host, port);
            
        }

        Socket socket;

    }

    public class BusinessServer
    {
        public BusinessServer(string host, int port)
        {
            socket = new Socket(SocketType.Stream, ProtocolType.IPv4);
            var ep = new IPEndPoint(IPAddress.Parse(host), port);
            socket.Bind(ep);
        }
        Socket socket;


    }
}
