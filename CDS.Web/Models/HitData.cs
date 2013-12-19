using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Web.Models {
    public class HitData {
        public HitData() {
            this.Times = new List<DateTime>();
        }
        public int ID { get; set; }
        public List<DateTime> Times { get; set; }
    }
}