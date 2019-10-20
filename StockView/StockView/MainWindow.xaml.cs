using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
namespace StockView
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Presentation : Window
    {
        private Business bll = new Business();
        BusClient busClient;

        public Args Args { get; set; }

        const string busIp = "127.0.0.1";
        const int busPort = 2222;

        public bool LastClosingPrice { get; set; } = true;
        public bool OpeningPrice { get; set; } = true;
        public bool ClosingPrice { get; set; } = true;
        public bool MaxPrice { get; set; } = true;
        public bool MinPrice { get; set; } = true;
        public bool AvgPrice { get; set; } = true;
        public bool Volumn { get; set; } = false;
        public bool Turnover { get; set; } = false;
        public bool UpDown { get; set; } = false;
        public bool UpDownChange { get; set; } = false;


        public Presentation()
        {
            InitializeComponent();
            var ep = new IPEndPoint(IPAddress.Parse(busIp), busPort);
            busClient = new BusClient(ep);
            DataContext = this;

            new Task(() => runBusServer()).Start();
            new Task(() => runDbAccessServer()).Start();

        }

        //功能1：初始界面显示开盘价、收盘、最低、最高和均价折线图
        //功能2：数据查询，结果表显示或折线显示


        //解析界面输入参数，返回字典
        Args parseArgs()
        {
            try
            {
                var args = new Args();

                string beginDate = txtBoxBeginDate.Text;
                args.BeginDate = DateTime.Parse(beginDate);

                string endDate = txtBoxEndDate.Text;
                args.EndDate = DateTime.Parse(endDate);

                //args.OpeningPrice = (bool)cbOpeningPrice.IsChecked;
                //args.ClosingPrice = (bool)cbClosingPrice.IsChecked;
                //args.MaxPrice = (bool)cbMaxPrice.IsChecked;
                //args.MinPrice = (bool)cbMinPrice.IsChecked;
                bool[] bools = { LastClosingPrice, OpeningPrice,
                    ClosingPrice, MaxPrice, MinPrice, AvgPrice , Volumn, Turnover,
                UpDown, UpDownChange};
                string[] strs = { "前收盘价(元)", "开盘价(元)",
                "收盘价(元)", "最高价(元)", "最低价(元)", "均价(元)","成交量(股)",
                "成交金额(元)","涨跌(元)", "涨跌幅(%)"};
                for (int i = 0; i < bools.Length; i++)
                {
                    if (bools[i])
                    {
                        args.Items.Add(strs[i]);
                    }
                }

                return args;
            }
            catch
            {
                return null;
            }
        }

        //根据起始日期，截止日期和参数搜索数据库
        DataTable search(Args args)
        {
            try
            {
                //var res = bll.search(args);
                var res = busClient.Search(args);
                return res;
            }catch 
            {
                return null;
            }
        }

        //DataTable search(Dictionary<string, string> args)
        //{
        //    return busClient.search(args);
        //}
        
        //显示结果表
        void showTable(DataTable dataTable)
        {
            Table table = new Table();
            table.ItemsSource = dataTable.DefaultView;
            //dgShow.ItemsSource = dataTable.DefaultView;
            table.Show();
            //var str = dt.DisplayExpression;
        }


        //显示结果折线图
        void showLineChart(DataTable dataTable)
        {
            int max = 100;
            int rows = Math.Min(dataTable.Rows.Count, max);
            int cols = Math.Min(dataTable.Columns.Count, max);
            LineChart chart = new LineChart();
            var labels = new List<string>();
            for (int r = 0; r < rows; r++)
            {
                labels.Add(dataTable.Rows[r][0] as string);

            }
            chart.XLabels = labels;

            SeriesCollection series = new SeriesCollection();

            for (int c = 1; c < cols; c++)
            {
                LineSeries line = new LineSeries();
                line.Title = dataTable.Columns[c].ColumnName;
                line.Values = new ChartValues<double>();
                for (int r = 0; r < rows; r++)
                {
                    line.Values.Add(double.Parse(dataTable.Rows[r][c] as string));
                }
                series.Add(line);
            }
            chart.LineSeries = series;
            //chart.Update();
            chart.Show();
            
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var args = parseArgs();
            var res = search(args);
            showTable(res);
            showLineChart(res);
        }

        

        void runBusServer()
        {
            var ip = "127.0.0.1";
            int port = 2222;
            var ep = new IPEndPoint(IPAddress.Parse(ip), port);
            BusServer server = new BusServer(ep);
            server.Listen();
        }
        void runDbAccessServer()
        {
            string ip = "127.0.0.1";
            int port = 2223;
            var ep = new IPEndPoint(IPAddress.Parse(ip), port);
            var server = new DbAccessServer(ep);
            server.Listen();
        }

        private void TxtBoxBeginDate_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
