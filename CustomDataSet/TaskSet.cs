using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomDataSet {
    public class TaskSet : ObservableCollection<ButtonTask> {
        public XElement ToXml() {
            XElement root = new XElement("TaskSet");
            foreach (var t in this) {
                root.Add(t.ToXml());
            }
            return root;
        }

        public static TaskSet FromXml(XElement xml) {
            var ts = new TaskSet();
            foreach (var t in xml.Elements("Task")) {
                ts.Add(ButtonTask.FromXml(t));
            }
            return ts;
        }
    }
}
