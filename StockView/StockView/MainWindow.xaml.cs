using System;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace StockView
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PresentationLayer : Window
    {
        DataTable dataTable = new DataTable();
        private BusinessLogicLayer bll = new BusinessLogicLayer();
        public PresentationLayer()
        {
            //var dal = new DatabaseAccessLayer();
            //var args = new DbArgs();

            //args.beginDate = DateTime.Parse("2001-9-14");
            //args.endDate = DateTime.Parse("2013-10-24");
            //args.openingPrice = true;
            //args.closingPrice = true;
            //args.maxPrice = true;
            //args.minPrice = true;

            //var res = (DataSet)dal.search(args);
            InitializeComponent();

            //dgShow.ItemsSource = res.Tables[0].DefaultView;

            //var chartDemo = (Chart)wfh.Child.Controls[0];
            //var chartDemo = 
            //Series series = chartDemo.Series[0];
            //// 画样条曲线（Spline）
            //series.ChartType = SeriesChartType.Spline;
            //// 线宽2个像素
            //series.BorderWidth = 2;
            //// 线的颜色：红色
            //series.Color = System.Drawing.Color.Red;
            //// 图示上的文字
            //series.LegendText = "演示曲线";

            //// 准备数据
            //float[] values = { 95, 30, 20, 23, 60, 87, 42, 77, 92, 51, 29 };

            //// 在chart中显示数据
            //int x = 0;
            //foreach (float v in values)
            //{
            //    series.Points.AddXY(x, v);
            //    x++;
            //}

            //// 设置显示范围
            //ChartArea chartArea = chartDemo.ChartAreas[0];
            //chartArea.AxisX.Minimum = 0;
            //chartArea.AxisX.Maximum = 10;
            //chartArea.AxisY.Minimum = 0d;
            //chartArea.AxisY.Maximum = 100d;
        }

        //功能1：初始界面显示开盘价、收盘、最低、最高和均价折线图
        //功能2：数据查询，结果表显示或折线显示


        //解析界面输入参数，返回字典
        DbArgs parseArgs()
        {
            try
            {
                var args = new DbArgs();

                string beginDate = txtBoxBeginDate.Text;
                args.beginDate = DateTime.Parse(beginDate);

                string endDate = txtBoxEndDate.Text;
                args.endDate = DateTime.Parse(endDate);

                args.openingPrice = (bool)cbOpeningPrice.IsChecked;
                args.closingPrice = (bool)cbClosingPrice.IsChecked;
                args.maxPrice = (bool)cbMaxPrice.IsChecked;
                args.minPrice = (bool)cbMinPrice.IsChecked;

                return args;
            }
            catch
            {
                return null;
            }
        }

        //根据起始日期，截止日期和参数搜索数据库
        DataSet search(DbArgs args)
        {
            try
            {
                var res = bll.search(args);
                return res;
            }catch 
            {
                return null;
            }
        }
        /// <summary>
        /// 設定Chart Control
        /// </summary>
        //private void SetChart()
        //{
        //    ChartArea ca = new ChartArea("ChartArea1");
        //    this.Chart_HistoryFlow.ChartAreas.Add(ca);
        //    ChartArea ca_Pres = new ChartArea("ChartArea1");
        //    this.Chart_HistoryPres.ChartAreas.Add(ca_Pres);
        //    ChartArea ca_Ratio = new ChartArea("ChartArea1");
        //    this.Chart_HistoryRatio.ChartAreas.Add(ca_Ratio);

        //    //Processor
        //    System.Windows.Forms.DataVisualization.Charting.Legend lgFlow = new System.Windows.Forms.DataVisualization.Charting.Legend("Legend1");
        //    lgFlow.IsTextAutoFit = true;
        //    lgFlow.Docking = Docking.Top;
        //    this.Chart_HistoryFlow.Legends.Add(lgFlow);

        //    System.Windows.Forms.DataVisualization.Charting.Legend lgPres = new System.Windows.Forms.DataVisualization.Charting.Legend("Legend1");
        //    lgPres.IsTextAutoFit = true;
        //    lgPres.Docking = Docking.Top;
        //    this.Chart_HistoryPres.Legends.Add(lgPres);

        //    System.Windows.Forms.DataVisualization.Charting.Legend lgRatio = new System.Windows.Forms.DataVisualization.Charting.Legend("Legend1");
        //    lgRatio.IsTextAutoFit = true;
        //    lgRatio.Docking = Docking.Top;
        //    this.Chart_HistoryRatio.Legends.Add(lgRatio);

        //    SetChartAutoBar(Chart_HistoryFlow);
        //    SetChartAutoBar(Chart_HistoryPres);
        //    SetChartAutoBar(Chart_HistoryRatio);
        //}

        /// <summary>
        /// 设置折线图游标
        /// </summary>
        private void SetChartAutoBar(Chart chart)
        {
            //设置游标
            chart.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart.ChartAreas[0].CursorX.AutoScroll = true;
            chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            //设置X轴是否可以缩放
            chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            //将滚动内嵌到坐标轴中
            chart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            // 设置滚动条的大小
            chart.ChartAreas[0].AxisX.ScrollBar.Size = 10;
            // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            chart.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            // 设置自动放大与缩小的最小量
            chart.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
        }
        /// <summary>
        /// 历史流量折线图鼠标滚动 滚动条对应滑动 最小及最大数据位置停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Chart_HistoryFlow_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        //{

        //    //按住Ctrl，缩放
        //    if ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control)
        //    {
        //        if (e.Delta < 0)
        //            Chart_HistoryFlow.ChartAreas[0].AxisX.ScaleView.Size += 4;
        //        else
        //            Chart_HistoryFlow.ChartAreas[0].AxisX.ScaleView.Size -= 4;
        //    }
        //    //不按Ctrl，滚动
        //    else
        //    {
        //        if (e.Delta < 0)
        //        {
        //            //当前位置+视图长大于最大数据时停止
        //            if (Chart_HistoryFlow.ChartAreas[0].AxisX.ScaleView.Position + Chart_HistoryFlow.ChartAreas[0].AxisX.ScaleView.Size < Chart_HistoryFlow.ChartAreas[0].AxisX.ScaleView.ViewMaximum)
        //                Chart_HistoryFlow.ChartAreas[0].AxisX.ScaleView.Position += 4;
        //        }
        //        else
        //        {
        //            //当前位置小于最小数据时停止
        //            if (Chart_HistoryFlow.ChartAreas[0].AxisX.ScaleView.Position > Chart_HistoryFlow.ChartAreas[0].AxisX.ScaleView.ViewMinimum)
        //                Chart_HistoryFlow.ChartAreas[0].AxisX.ScaleView.Position -= 4;
        //        }

        //    }
        //}

        /// <summary>
        /// 流量脉冲折线图光标显示详细数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chart_HistoryFlow_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                this.Cursor = System.Windows.Input.Cursors.Cross;
                int i = e.HitTestResult.PointIndex;
                string time = this.dataTable.Rows[i]["时间"].ToString();
                string aFlow = this.dataTable.Rows[i]["A脉冲"].ToString();
                string bFlow = this.dataTable.Rows[i]["B脉冲"].ToString();
                string aPressure = this.dataTable.Rows[i]["A压力"].ToString();
                string bPressure = this.dataTable.Rows[i]["B压力"].ToString();
                string abRatio = this.dataTable.Rows[i]["AB比率"].ToString();
                e.Text = $"时  间：{time}\r\nA脉冲：{aFlow}\r\nB脉冲：{bFlow}\r\nA压力：{aPressure}\r\nB压力：{bPressure}\r\nAB比率：{abRatio}";
            }
            else
            {
                this.Cursor = System.Windows.Input.Cursors.Arrow;
            }
        }
        /// <summary>
        /// DataTable数据写入Chart中
        /// </summary>
        /// <param name="dataTable">包含数据的DataTable</param>
        /// <param name="chart">待写入数据的Chart</param>
        public void DataTableToChart(DataTable dataTable, Chart chart, string title, string series1, string series2)
        {
            chart.Series.Clear();   //清空图表中的内容
            chart.Titles.Clear();
            chart.Titles.Add(title);//添加标题
            chart.DataSource = dataTable;

            Series aCodeSeries = chart.Series.Add(series1);//添加第一个表
            aCodeSeries.ChartType = SeriesChartType.Line;//设为折线图显示
            aCodeSeries.YValueMembers = series1;//y轴为数据
            aCodeSeries.XValueMember = "时间";
            Series bCodeSeries = chart.Series.Add(series2);//添加第二个表
            bCodeSeries.ChartType = SeriesChartType.Line;//设为折线图显示
            bCodeSeries.YValueMembers = series2;
        }
        //显示结果表
        void showTable(DataSet dataSet)
        {
            
            //dgShow.ItemsSource = dataSet.Tables[0].DefaultView;

            var dt = dataSet.Tables[0];
            //dt.Columns[0].DataType = DateTime.
            //for (int r = 1; r < dt.Rows.Count; r++)
            //{
            //    var i = dt.Rows[r][0];
            //    var j  = dt.Rows[r][0].ToString().Split(' ');



            //    //dt.Rows[r][0] = dt.Rows[r][0].ToString().Split(' ')[0];
            //    //i = dt.Rows[r][0];
            //}
            //dt.Columns[0].DataType = Type.GetType("String");
            dgShow.ItemsSource = dt.DefaultView;

            // var i = dataSet.Tables[0];
            // var j = i.Rows[0];
            // var k = i.Rows[1];
            //var avg = dt.Compute("avg(开盘价)", null);
            //var i = bll.average(dt);
        }


        //显示结果折线图
        void showLineChart(object args)
        {
            
        }

        

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var args = parseArgs();
            var res = search(args);
            showTable(res);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
