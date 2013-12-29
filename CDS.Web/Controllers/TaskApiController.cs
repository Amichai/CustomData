using CDS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CDS.Web.Controllers {
    public class TaskApiController : ApiController {
        public List<ButtonTask> GetTasks() {
            List<ButtonTask> taskSet = new List<ButtonTask>();
            using (var db = new TaskCollectorEntities()) {
                var user = User;
                foreach (var userTask in db.UserTasks.Where(i => i.User == user.Identity.Name).ToList()) {
                    var task = userTask.Task1;
                    if (task.Visibility != 2) {
                        taskSet.Add(new ButtonTask() {
                            ID = task.ID,
                            CompletedAfter = task.CompletedAfter.Value,
                            CompletionDisabled = TimeSpan.FromTicks(task.CompletionDisabled.Value),
                            Description = task.Description,
                            HitCount = task.HitCount,
                            HitDisabled = TimeSpan.FromTicks(task.HitDisabled.Value),
                            Name = task.Name,
                            Visibility = task.Visibility
                        });
                    }
                }
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

        [HttpGet]
        public string ConnectionTest() {
            string result = "";
            try {
            } catch (Exception ex) {
                return ex.Message + " " + ex.InnerException.Message;
            }
            return result;
        }

        public List<ButtonTask> PostTask(ButtonTask t) {
            var db = GetDataContext();
            TaskVisibility vis = db.TaskVisibilities.First();
            if (t.ID != null) {
                var found1 = db.Tasks.Where(i => i.ID == t.ID.Value);
                if (found1.Count() > 0) {
                    var found = found1.Single();
                    Set(found, t, vis);
                    db.SaveChanges();
                    return this.GetTasks();
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
            return this.GetTasks();
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
            Dictionary<int, HitData> result = new Dictionary<int, HitData>();
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
