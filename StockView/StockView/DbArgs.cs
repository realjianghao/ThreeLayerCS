using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockView
{
    public class DbArgs
    {
        public DateTime beginDate;
        public DateTime endDate;
        public bool openingPrice;
        public bool closingPrice;
        public bool maxPrice;
        public bool minPrice;

        public string toString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(beginDate.ToShortDateString());
            sb.AppendLine(endDate.ToShortDateString());
            sb.AppendLine(openingPrice.ToString());
            sb.AppendLine(closingPrice.ToString());
            sb.AppendLine(maxPrice.ToString());
            sb.AppendLine(minPrice.ToString());
            return sb.ToString();
        }

        public void parse(string str)
        {
            var args = str.Split('\n');
            beginDate = DateTime.Parse(args[0]);
            endDate = DateTime.Parse(args[1]);
            openingPrice = bool.Parse(args[2]);
            closingPrice = bool.Parse(args[3]);

            maxPrice = bool.Parse(args[4]);
            minPrice = bool.Parse(args[5]);
        }
    }
}
