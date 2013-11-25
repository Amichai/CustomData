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

namespace CustomDataSet {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public MainWindow() {
            InitializeComponent();
            this.AllButtons = new ObservableCollection<ButtonTask>();

            this.AllButtons.Add(new ButtonTask() { 
                HitCount = 5, 
                Name = "Testing", 
                CompletedAfter = 50,
                HitDisabled = TimeSpan.FromSeconds(5),
                Description = "Full description here" });

            this.createEdit.NewTask.Subscribe(i => {
                this.AllButtons.Add(i);
                this.main.Focus();
            });
        }

        private ObservableCollection<ButtonTask> _AllButtons;
        public ObservableCollection<ButtonTask> AllButtons {
            get { return _AllButtons; }
            set {
                _AllButtons = value;
                NotifyPropertyChanged();
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

        private void Hit_Click(object sender, RoutedEventArgs e) {
            ((sender as Button).Tag as ButtonTask).ButtonHit();
        }

        private void Edit_Click(object sender, RoutedEventArgs e) {
            this.createEdit.ThisTask = (sender as Button).Tag as ButtonTask;
            this.creationTab.Focus();
            e.Handled = true;
        }

        private void Delete_Click(object sender, RoutedEventArgs e) {
            var toRemove = (sender as Button).Tag as ButtonTask;
            this.AllButtons.Remove(toRemove);
            e.Handled = true;
        }
    }
}
