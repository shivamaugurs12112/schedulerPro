using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Syncfusion.EJ2.Schedule;

namespace ScheduleSample.Models
{

    public class SchedulerDataModel
    {
       
        public string action { get; set; }


        public bool AllowAdding { get; set; }
        public int Id { get; set; }

        [System.ComponentModel.DefaultValue("")]
        public string Event { get; set; }
        public string Subject { get; set; }

        
        [System.ComponentModel.DefaultValue("")]
        public string Job { get; set; }


        [System.ComponentModel.DefaultValue("")]
        public string[] Attendees { get; set; }

        [System.ComponentModel.DefaultValue("")]
        public string ResourceId { get; set; }


        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [System.ComponentModel.DefaultValue("")]
        public string StartTimezone { get; set; }
        [System.ComponentModel.DefaultValue("")]
        public string EndTimezone { get; set; }

        [System.ComponentModel.DefaultValue(0)]
        public bool? IsAllDay { get; set; } 

        [System.ComponentModel.DefaultValue("")]
        public string RecurrenceRule { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public int? RecurrenceID { get; set; }
        [System.ComponentModel.DefaultValue("")]
        public string RecurrenceException { get; set; }

        public string attendeesAsString { get; set; }




    }
    public class ResourceDataSourceModel {
        public string text { get; set; }

        public int id { get; set; }
        public int groupId { get; set; }
        public string color { get; set; }
    }


    public class ConsultantDataSourceModel {
        public string text { get; set; }

        public int id { get; set; }
        public int groupId { get; set; }
        public string color { get; set; }

       public string designation { get; set; }
    }



    public class Jobs
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string fkTenantID { get; set; }


    }
}