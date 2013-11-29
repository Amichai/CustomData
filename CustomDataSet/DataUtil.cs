using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomDataSet {
    public class DataUtil {
        public static TaskCollectorEntities GetDataContext() {
            return new TaskCollectorEntities();
        }

        static void Set(CustomDataSet.Task dest, ButtonTask source, TaskVisibility vis) {
            dest.Name = source.Name;
            dest.CompletedAfter = source.CompletedAfter;
            dest.CompletionDisabled = source.CompletionDisabled.Ticks;
            dest.Description = source.Description;
            dest.HitDisabled = source.HitDisabled.Ticks;
            dest.TaskVisibility = vis;
        }

        internal static void AddOrUpdateTask(ButtonTask t) {
            var db = GetDataContext();
            TaskVisibility vis = db.TaskVisibilities.First();
            if(t.ID != null){
                var found1 = db.Tasks.Where(i => i.ID == t.ID.Value);
                if (found1.Count() > 0) {
                    var found = found1.Single();
                    Set(found, t, vis);
                    db.SaveChanges();
                    return;
                }
                Debug.Print(string.Format("Task of ID {0} not found in DB", t.ID.Value));
            }

            var newTask =  new CustomDataSet.Task();
            Set(newTask, t, vis);
            db.Tasks.Add(newTask);
            db.SaveChanges();
            t.ID = newTask.ID;
        }

        internal static void RemoveTask(ButtonTask toRemove) {
            var db = GetDataContext();
            var r = db.Tasks.Where(i => i.ID == toRemove.ID).Single();
            db.Tasks.Remove(r);
            db.SaveChanges();
        }

        internal static void ButtonHit(ButtonTask task, User user) {
            var db = GetDataContext();
            db.Tasks.Where(i => i.ID == task.ID).Single().HitCount++;
            db.TaskHits.Add(new TaskHit() {
                Task = task.ID.Value,
                Timestamp = DateTime.Now,
                User = user.ID
            });
            db.SaveChanges();
        }
    }
}
