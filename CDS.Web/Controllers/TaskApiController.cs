using CDS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CDS.Web.Controllers
{
    public class TaskApiController : ApiController
    {
        public List<ButtonTask> GetTasks() {
            var db = new TaskCollectorEntities();
            List<ButtonTask> taskSet = new List<ButtonTask>();
            foreach (var t in db.Tasks.Where(i => i.Visibility != 2)) {
                taskSet.Add(new ButtonTask() {
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
            return taskSet;
        }

        static void Set(Task dest, ButtonTask source, TaskVisibility vis) {
            dest.Name = source.Name;
            dest.CompletedAfter = source.CompletedAfter;
            dest.CompletionDisabled = source.CompletionDisabled.Ticks;
            dest.Description = source.Description;
            dest.HitDisabled = source.HitDisabled.Ticks;
            dest.TaskVisibility = vis;
        }

        private TaskCollectorEntities GetDataContext() {
            return new TaskCollectorEntities();
        }

        public void PostTask(ButtonTask t) {
            var db = GetDataContext();
            TaskVisibility vis = db.TaskVisibilities.First();
            if (t.ID != null) {
                var found1 = db.Tasks.Where(i => i.ID == t.ID.Value);
                if (found1.Count() > 0) {
                    var found = found1.Single();
                    Set(found, t, vis);
                    db.SaveChanges();
                    return;
                }
            }

            var newTask = new Task();
            Set(newTask, t, vis);
            db.Tasks.Add(newTask);
            db.SaveChanges();
            var relationship = db.TaskRelationships.First();
            db.UserTasks.Add(new UserTask() {
                User = User.Identity.Name,
                Task = newTask.ID,
                TaskRelationship = relationship
            });
            db.SaveChanges();

            t.ID = newTask.ID;
        }

        public void TaskDelete(ButtonTask task) {
            var db = GetDataContext();
            var toRemove = db.Tasks.Where(i => i.ID == task.ID).Single();
            toRemove.Visibility = 2;
            db.SaveChanges();
        }

        public void TaskClick(ButtonTask task) {
            var db = GetDataContext();
            db.Tasks.Where(i => i.ID == task.ID).Single().HitCount++;
            db.TaskHits.Add(new TaskHit() {
                Task = task.ID.Value,
                Timestamp = DateTime.Now,
                User = User.Identity.Name
            });
            db.SaveChanges();
        }

        [HttpPost]
        public List<HitData> getHits(List<int> indicies) {
            var db = GetDataContext();
            Dictionary<int ,HitData> result = new Dictionary<int, HitData>();
            foreach (var h in db.TaskHits) {
                var taskID = h.Task;
                if (!indicies.Contains(taskID)) {
                    continue;
                }
                if (!result.ContainsKey(taskID)) {
                    result[taskID] = new HitData() { ID = taskID };
                }
                result[taskID].Times.Add(h.Timestamp);
            }
            return result.Values.ToList();
        }
    }
}
