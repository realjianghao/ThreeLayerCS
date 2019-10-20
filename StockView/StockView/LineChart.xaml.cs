using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using LiveCharts;
using LiveCharts.Wpf;


namespace StockView
{
    /// <summary>
    /// LineChart.xaml 的交互逻辑
    /// </summary>
    public partial class LineChart : Window
    {
        public LineChart()
        {
            InitializeComponent();
            chart.AxisX.CollectionChanged += 
                new NotifyCollectionChangedEventHandler(
                (object o, NotifyCollectionChangedEventArgs args) => chart.Update());

            DataContext = this;
        }

        public SeriesCollection LineSeries { get; set; }
        public List<string> XLabels { get; set; } 

        public void Update()
        {
            //chart.Series.Clear();
            //chart.Series = LineSeries;
        }
    }
}
