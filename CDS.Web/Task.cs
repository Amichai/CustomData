//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CDS.Web
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task
    {
        public Task()
        {
            this.UserTasks = new HashSet<UserTask>();
            this.TaskHits = new HashSet<TaskHit>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HitCount { get; set; }
        public Nullable<int> CompletedAfter { get; set; }
        public Nullable<long> HitDisabled { get; set; }
        public Nullable<long> CompletionDisabled { get; set; }
        public Nullable<int> Visibility { get; set; }
    
        public virtual TaskVisibility TaskVisibility { get; set; }
        public virtual ICollection<UserTask> UserTasks { get; set; }
        public virtual ICollection<TaskHit> TaskHits { get; set; }
    }
}
