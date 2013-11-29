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
            var db = DataUtil.GetDataContext();
            this.currentUser = db.Users.First();
            foreach (var t in db.Tasks) {
                this.TaskSet.Add(new ButtonTask() {
                    ID = t.ID,
                    CompletedAfter = t.CompletedAfter.Value,
                    CompletionDisabled = TimeSpan.FromTicks(t.CompletionDisabled.Value),
                    Description = t.Description,
                    HitCount = t.HitCount,
                    HitDisabled = TimeSpan.FromTicks(t.HitDisabled.Value),
                    Name = t.Name,
                    Visibility = t.Visibility
                });

            }

            this.createEdit.NewTask.Subscribe(i => {
                this.TaskSet.Add(i);
                DataUtil.AddOrUpdateTask(i, this.currentUser);
                this.main.Focus();
            });
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
        User currentUser;

        private void Hit_Click(object sender, RoutedEventArgs e) {
            var task = ((sender as Button).Tag as ButtonTask);
            DataUtil.ButtonHit(task, currentUser);
            task.ButtonHit();
        }

        private void Edit_Click(object sender, RoutedEventArgs e) {
            var inspection = (sender as Button).Tag as ButtonTask;
            this.createEdit.ThisTask = inspection;
            this.TaskSet.Remove(inspection);
            this.creationTab.Focus();
            e.Handled = true;
        }

        ///TODO: get the list of buttons by db query, don't duplicate the store...
        ///TODO: produce a url for registering hits
        ///TODO: simple, medium and advanced button creation tools -> advanced tools allow completed tasks to spawn new buttons
        ///TODO: public and private access to a button
        ///Todo: our chart doesn't show zero hit values correctly

        private void Delete_Click(object sender, RoutedEventArgs e) {
            var toRemove = (sender as Button).Tag as ButtonTask;
            DataUtil.RemoveTask(toRemove);
            this.TaskSet.Remove(toRemove);
            e.Handled = true;
        }
    }
}
