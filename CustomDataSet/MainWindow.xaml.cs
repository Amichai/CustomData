using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Xml.Linq;

namespace CustomDataSet {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public MainWindow() {
            InitializeComponent();
            this.TaskSet = new TaskSet();
            var last = Properties.Settings.Default.LastFilepath;
            dv = new DataView();
            this.dataView.Content = dv;
            dv.SetData(this.TaskSet);
            if (last != "") {
                if (open(last)) {
                    this.SavePath = last;
                }
            }

            this.createEdit.NewTask.Subscribe(i => {
                this.TaskSet.Add(i);
                this.main.Focus();
            });
        }

        private string _SavePath;
        public string SavePath {
            get { return _SavePath; }
            set {
                if (_SavePath != value) {
                    _SavePath = value;
                    NotifyPropertyChanged("SavePath");
                }
            }
        }

        private TaskSet _TaskSet;
        public TaskSet TaskSet {
            get { return _TaskSet; }
            set {
                if (_TaskSet != value) {
                    _TaskSet = value;
                    NotifyPropertyChanged("TaskSet");
                }
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged Implementation

        DataView dv;

        private void Hit_Click(object sender, RoutedEventArgs e) {
            ((sender as Button).Tag as ButtonTask).ButtonHit();
        }

        private void Edit_Click(object sender, RoutedEventArgs e) {
            var inspection = (sender as Button).Tag as ButtonTask;
            this.createEdit.ThisTask = inspection;
            this.TaskSet.Remove(inspection);
            this.creationTab.Focus();
            e.Handled = true;
        }

        ///TODO: produce a url for registering hits
        ///TODO: simple, medium and advanced button creation tools -> advanced tools allow completed tasks to spawn new buttons
        ///TODO: public and private access to a button
        ///TOOD: timeseries visualization
        ///Todo: multiple time series
        ///Todo: our chart doesn't show zero hit values correctly

        private void Delete_Click(object sender, RoutedEventArgs e) {
            var toRemove = (sender as Button).Tag as ButtonTask;
            this.TaskSet.Remove(toRemove);
            e.Handled = true;
        }

        private void Open_Click_1(object sender, RoutedEventArgs e) {
            var ofd = new OpenFileDialog();
            ofd.ShowDialog();
            var name = ofd.FileName;
            this.SavePath = name;
            open(name);
        }

        private bool open(string path) {
            try {
                var root = XElement.Load(path);
                this.TaskSet = TaskSet.FromXml(root);
                Properties.Settings.Default.LastFilepath = path;
                Properties.Settings.Default.Save();
                dv.SetData(this.TaskSet);
                return true;
            } catch {
                return false;
            }
        }

        private void Save_Click_1(object sender, RoutedEventArgs e) {
            string name;
            if (string.IsNullOrEmpty(this.SavePath)) {
                var ofd = new SaveFileDialog();
                ofd.ShowDialog();
                name = ofd.FileName;
            } else {
                name = this.SavePath;
            }
            if (string.IsNullOrEmpty(name)) {
                return;
            }
            this.SavePath = name;
            this.TaskSet.ToXml().Save(name);
            Properties.Settings.Default.LastFilepath = name;
            Properties.Settings.Default.Save();
        }
    }
}
