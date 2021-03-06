﻿using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomDataSet {
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataView : UserControl, INotifyPropertyChanged {
        PlotModel plot = new PlotModel();
        public DataView() {
            InitializeComponent();
            var db = DataUtil.GetDataContext();
            this.CurrentUser = db.Users.First();

            this.binSize.ItemsSource = Enum.GetValues(typeof(binType)).Cast<binType>();
            this.binSize.SelectedItem = binType.Second;
            var xAxis = new DateTimeAxis();
            xAxis.Position = AxisPosition.Bottom;
            plot.Axes.Add(xAxis);
            var yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            plot.Axes.Add(yAxis);
            this.root.Model = plot;
        }

        private void update() {
            this.plot.Series.Clear();
            double minVal = double.MaxValue;
            double maxVal = double.MinValue;
            
            foreach (var d in this.selectedTasks.SelectedItems.Cast<Task>()) {
                var hitTimes = DataUtil.GetHitsForTask(d);
                this.AddSeries(hitTimes, d.Name, ref minVal, ref maxVal);
            }
            OnPropertyChanged("AllTasks");
        }

        private DateTime Floor(DateTime dateTime, TimeSpan interval) {
            return dateTime.AddTicks(-(dateTime.Ticks % interval.Ticks));
        }

        private DateTime Ceiling(DateTime dateTime, TimeSpan interval) {
            return dateTime.AddTicks(interval.Ticks - (dateTime.Ticks % interval.Ticks));
        }

        private DateTime Round(DateTime dateTime, TimeSpan interval) {
            var halfIntervelTicks = ((interval.Ticks + 1) >> 1);
            return dateTime.AddTicks(halfIntervelTicks - ((dateTime.Ticks + halfIntervelTicks) % interval.Ticks));
        }

        public void AddSeries(List<DateTime> hits, string name, ref double minVal, ref double maxVal) {
            var s = new LineSeries();
            Dictionary<DateTime, int> idxCounter = new Dictionary<DateTime, int>();
            var xAxis = (this.plot.Axes[0] as DateTimeAxis);

            xAxis.ShowMinorTicks = true;
            foreach (var h in hits) {
                if (h < StartTime) {
                    continue;
                }
                DateTime binIdx;
                switch (this.SelectedBinType) {
                    case binType.Day:
                        binIdx = Floor(h, TimeSpan.FromDays(1));
                        break;
                    case binType.Month:
                        binIdx = Floor(h, TimeSpan.FromDays(30));
                        break;
                    case binType.Year:
                        binIdx = Floor(h, TimeSpan.FromDays(365));
                        break;
                    case binType.Second:
                        binIdx = Floor(h, TimeSpan.FromSeconds(1));
                        break;
                    case binType.Minute:
                        binIdx = Floor(h, TimeSpan.FromMinutes(1));
                        break;
                    case binType.Hour:
                        binIdx = Floor(h, TimeSpan.FromHours(1));
                        break;
                    default:
                        throw new Exception();

                }
                if (!idxCounter.ContainsKey(binIdx)) {
                    idxCounter[binIdx] = 1;
                } else {
                    idxCounter[binIdx]++;
                }
            }
            List<IDataPoint> points = new List<IDataPoint>();
            foreach (var i in idxCounter) {
                var asDouble = DateTimeAxis.ToDouble(i.Key);
                if(asDouble < minVal){
                    minVal = asDouble;
                }
                if(asDouble > maxVal){
                    maxVal = asDouble;
                }
                points.Add(new DataPoint(asDouble, i.Value));

            }
            var ls = new LineSeries() { Points = points };
            ls.CanTrackerInterpolatePoints = false;
            xAxis.Zoom(minVal, maxVal);
            this.plot.RefreshPlot(true);
            this.plot.Series.Add(ls);
        }

        private DateTime _StartTime;
        public DateTime StartTime {
            get { return _StartTime; }
            set {
                if (_StartTime != value) {
                    _StartTime = value;
                    OnPropertyChanged("StartTime");
                }
            }
        }

        private binType _SelectedBinType;
        public binType SelectedBinType {
            get { return _SelectedBinType; }
            set {
                if (_SelectedBinType != value) {
                    _SelectedBinType = value;
                    OnPropertyChanged("SelectedBinType");
                }
            }
        }
        public enum binType { Day, Month, Quarter, Year, Hour, Minute, Second };

        private TimeSpan _BinSize;
        public TimeSpan BinSize {
            get { return _BinSize; }
            set {
                if (_BinSize != value) {
                    _BinSize = value;
                    OnPropertyChanged("BinSize");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) {
            var eh = PropertyChanged;
            if (eh != null) {
                eh(this, new PropertyChangedEventArgs(name));
            }
        }

        private User _currentUser;

        public User CurrentUser {
            get { return _currentUser; }
            set { 
                _currentUser = value;
                OnPropertyChanged("AllTasks");
            }
        }
        
        public List<Task> AllTasks {
            get {
                if (CurrentUser == null) {
                    return new List<Task>();
                }
                var db = DataUtil.GetDataContext();
                var toReturn = db.UserTasks.Where(i => i.User == CurrentUser.ID).Select(i => i.Task1).ToList();
                return toReturn;
            }
        }

        //private TaskSet _AllButtons;
        //public TaskSet AllButtons {
        //    get { return _AllButtons; }
        //    set {
        //        if (_AllButtons != value) {
        //            _AllButtons = value;
        //            OnPropertyChanged("AllButtons");
        //        }
        //    }
        //}

        private void binSize_SelectionChanged_1(object sender, SelectionChangedEventArgs e) {
            this.SelectedBinType = (binType)(sender as ComboBox).SelectedItem;
            update();
        }

        private void Refresh_Click_1(object sender, RoutedEventArgs e) {
            update();
        }
    }
}
