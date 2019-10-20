using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockView
{
    class Common
    {
        public static byte[] encodeArgs(Dictionary<string, string> args)
        {
            return null;
        }

        public static Dictionary<string, string> decodeArgs(byte[] args)
        {
            return null;
        }

        public static Dictionary<string, string> decodeArgs(byte[] args, int len)
        {
            return null;
        }




        public static byte[] encodeDataTable(DataTable dataTable)
        {
            var sb = new StringBuilder();
            for (int c = 0; c < dataTable.Columns.Count; c++)
            {
                sb.Append(dataTable.Columns[c].ColumnName + " ");
            }
            sb[sb.Length - 1] = '\n';

            for (int r = 0; r < dataTable.Rows.Count; r++)
            {
                var row = dataTable.Rows[r];
                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    sb.Append(row[c] as string + " ");
                }
                sb[sb.Length - 1] = '\n';
            }
            sb.Remove(sb.Length - 1, 1);
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public static DataTable decodeDataTable(byte[] dataTable)
        {
            return decodeDataTable(dataTable, dataTable.Length);
        }

        public static DataTable decodeDataTable(byte[] dataTable, int len)
        {
            var str = Encoding.UTF8.GetString(dataTable, 0, len);
            var rows = str.Split('\n');

            var dt = new DataTable();

            var columnNames = rows[0].Split(' ');
            foreach (var name in columnNames)
            {
                dt.Columns.Add(name);
            }

            for (int i = 1; i < rows.Length; i++)
            {
                var row = rows[i].Split(' ');
                DataRow dr = dt.NewRow();
                for (int j = 0; j < columnNames.Length; j++)
                {
                    dr[j] = row[j];
                }
                dt.Rows.Add(dr);
                

            }

            

            return dt;
        }
    }

    public class Args
    {
        public DateTime BeginDate;
        public DateTime EndDate;
        public bool OpeningPrice;
        public bool ClosingPrice;
        public bool MaxPrice;
        public bool MinPrice;
        public List<string> Items { get; } = new List<string>();

        override public string ToString()
        {
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine(BeginDate.ToShortDateString());
            //sb.AppendLine(EndDate.ToShortDateString());
            //sb.AppendLine(OpeningPrice.ToString());
            //sb.AppendLine(ClosingPrice.ToString());
            //sb.AppendLine(MaxPrice.ToString());
            //sb.AppendLine(MinPrice.ToString());
            //return sb.ToString();
            return ToString2();
        }

        public string ToString2()
        {
            var sb = new StringBuilder();

            sb.Append(BeginDate.ToShortDateString() + "\n");
            sb.Append(EndDate.ToShortDateString());
            foreach (var i in Items)
            {
                sb.Append("\n" + i);
            }
            return sb.ToString();
        }

        public void Parse(string str)
        {
            var args = str.Split('\n');
            BeginDate = DateTime.Parse(args[0]);
            EndDate = DateTime.Parse(args[1]);
            OpeningPrice = bool.Parse(args[2]);
            ClosingPrice = bool.Parse(args[3]);

            MaxPrice = bool.Parse(args[4]);
            MinPrice = bool.Parse(args[5]);
        }
        public void Parse2(string str)
        {
            var slices = str.Split('\n');
            BeginDate = DateTime.Parse(slices[0]);
            EndDate = DateTime.Parse(slices[1]);
            for (int i = 2; i < slices.Length; i++)
            {
                Items.Add(slices[i]);
            }
        }

        public byte[] ToBytes()
        {
            var str = ToString();
            return Encoding.UTF8.GetBytes(str);
        }

        public void Parse(byte[] buffer, int length)
        {
            var str = Encoding.UTF8.GetString(buffer, 0, length);
            Parse2(str);
        }
    }
}
