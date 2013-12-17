using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml.Linq;

namespace CDS.Web.Models {
    public class ButtonTask {
        public ButtonTask() {
            this.CompletedAfter = 1;
            this.Enabled = true;
            this.HitCount = 0;
            this.HitTimes = new List<DateTime>();
            this.ID = null;
        }

        public int? ID { get; set; }

        private string _Name;
        public string Name {
            get { return _Name; }
            set {
                _Name = value;
            }
        }

        private string _Description;
        public string Description {
            get { return _Description; }
            set {
                _Description = value;
            }
        }

        public List<DateTime> HitTimes;

        private int _CompletedAfter;
        public int CompletedAfter {
            get { return _CompletedAfter; }
            set {
                _CompletedAfter = value;
                this.Enabled = ProgressVal < 100;
            }
        }

        public double ProgressVal {
            get {
                var val = HitCount * 100 / (double)CompletedAfter;
                this.Enabled = val < 100;
                return val;
            }
        }

        private TimeSpan _HitDisabled;
        public TimeSpan HitDisabled {
            get { return _HitDisabled; }
            set {
                _HitDisabled = value;
            }
        }

        private bool _Enabled;
        public bool Enabled {
            get { return _Enabled; }
            set {
                _Enabled = value;
            }
        }

        private TimeSpan _CompletionDisabled;
        public TimeSpan CompletionDisabled {
            get { return _CompletionDisabled; }
            set {
                _CompletionDisabled = value;
            }
        }

        private int _HitCount;
        public int HitCount {
            get { return _HitCount; }
            set {
                _HitCount = value;
            }
        }

        private Timer disabledTimer;

        public void ButtonHit() {
            this.HitTimes.Add(DateTime.Now);
            this.HitCount++;
            if (this.Enabled) {
                this.Enabled = false;
                this.disabledTimer = new Timer(state => { this.Enabled = true; }, null, this.HitDisabled, TimeSpan.FromMilliseconds(-1));
            }
        }

        internal XElement ToXml() {
            XElement task = new XElement("Task");
            task.Add(new XAttribute("Name", this.Name));
            task.Add(new XAttribute("Description", this.Description));
            task.Add(new XAttribute("HitCount", this.HitCount));
            task.Add(new XAttribute("CompletedAfter", this.CompletedAfter));
            task.Add(new XAttribute("CompletionDisabled", this.CompletionDisabled.ToString("c")));
            task.Add(new XAttribute("HitDisabled", this.HitDisabled.ToString("c")));
            XElement hitTimes = new XElement("HitTimes");
            foreach (var h in this.HitTimes) {
                XElement hitTime = new XElement("Hit");
                hitTime.Add(new XAttribute("Time", h.ToString()));
                hitTimes.Add(hitTime);
            }
            task.Add(hitTimes);
            return task;
        }

        internal static ButtonTask FromXml(XElement t) {
            var toReturn = new ButtonTask();
            toReturn.Name = t.Attribute("Name").Value;
            toReturn.Description = t.Attribute("Description").Value;
            toReturn.HitCount = int.Parse(t.Attribute("HitCount").Value);
            toReturn.CompletedAfter = int.Parse(t.Attribute("CompletedAfter").Value);
            toReturn.CompletionDisabled = TimeSpan.Parse(t.Attribute("CompletionDisabled").Value);
            toReturn.HitDisabled = TimeSpan.Parse(t.Attribute("HitDisabled").Value);
            XElement hitTimesRoot = t.Element("HitTimes");
            foreach (var hitTimeRoot in hitTimesRoot.Elements("Hit")) {
                toReturn.HitTimes.Add(DateTime.Parse(hitTimeRoot.Attribute("Time").Value));
            }
            return toReturn;
        }

        public int? Visibility { get; set; }
    }
}