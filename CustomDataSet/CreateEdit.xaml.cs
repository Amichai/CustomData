using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
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
    /// Interaction logic for CreateEdit.xaml
    /// </summary>
    public partial class CreateEdit : UserControl, INotifyPropertyChanged {
        public CreateEdit() {
            InitializeComponent();
            this.ThisTask = new ButtonTask();
            this.DataContext = this;
            this.NewTask = new Subject<ButtonTask>();
        }

        public Subject<ButtonTask> NewTask { get; set; }

        private void Create_Click(object sender, RoutedEventArgs e) {
            this.NewTask.OnNext(ThisTask);
            this.ThisTask = new ButtonTask();
        }

        private ButtonTask _ThisTask;
        public ButtonTask ThisTask {
            get { return _ThisTask; }
            set {
                _ThisTask = value;
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
    }
}
