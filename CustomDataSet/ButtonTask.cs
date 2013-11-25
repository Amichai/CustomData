using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomDataSet {
    public class ButtonTask : INotifyPropertyChanged {
        public ButtonTask() {
            this.CompletedAfter = 1;
            this.Enabled = true;
            this.HitCount = 0;
        }

        private string _Name;
        public string Name {
            get { return _Name; }
            set {
                _Name = value;
                NotifyPropertyChanged();
            }
        }

        private string _Description;
        public string Description {
            get { return _Description; }
            set {
                _Description = value;
                NotifyPropertyChanged();
            }
        }

        private int _CompletedAfter;
        public int CompletedAfter {
            get { return _CompletedAfter; }
            set {
                _CompletedAfter = value;
                NotifyPropertyChanged();
            }
        }

        public double ProgressVal {
            get {
                var val = HitCount * 100 / (double)CompletedAfter;
                if (val >= 100) {
                    this.Enabled = false;
                }
                return val;
            }
        }

        private TimeSpan _HitDisabled;
        public TimeSpan HitDisabled {
            get { return _HitDisabled; }
            set {
                _HitDisabled = value;
                NotifyPropertyChanged();
            }
        }

        private bool _Enabled;
        public bool Enabled {
            get { return _Enabled; }
            set {
                _Enabled = value;
                NotifyPropertyChanged();
            }
        }

        private TimeSpan _CompletionDisabled;
        public TimeSpan CompletionDisabled {
            get { return _CompletionDisabled; }
            set {
                _CompletionDisabled = value;
                NotifyPropertyChanged();
            }
        }

        private int _HitCount;
        public int HitCount {
            get { return _HitCount; }
            set {
                _HitCount = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("ProgressVal");
            }
        }

        private Timer disabledTimer;

        public void ButtonHit() {
            this.HitCount++;
            this.Enabled = false;
            this.disabledTimer = new Timer(state => { this.Enabled = true; }, null, this.HitDisabled, TimeSpan.FromMilliseconds(-1));
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
