using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScheduleSample.Models;
using Syncfusion.EJ2.Schedule;

namespace ScheduleSample.Controllers
{
    public class HomeController : Controller
    {
       

        ScheduleDataDataContext db = new ScheduleDataDataContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BindEvent()
        {
            Models.mysqlConnector objConnector = new Models.mysqlConnector();
            List<SchedulerDataModel> eventListData = new List<SchedulerDataModel>();
            eventListData = objConnector.GetEvents();

            foreach (SchedulerDataModel objemodel in eventListData)
            {
                string[] newArr = new string[] { };
                List<string> list = new List<string>();
                string AttendeesData1 = objemodel.attendeesAsString;
                if (AttendeesData1 != string.Empty)
                {
                    string[] _arr = AttendeesData1.Split(',');
                    foreach (string stringCon in _arr)
                    {
                        list.Add(stringCon);
                    }
                    newArr = list.ToArray();
                    objemodel.Attendees = newArr;
                }
                else
                    objemodel.Attendees = new string[] { };


                objemodel.Subject = objemodel.Job;

            }
            return Json(eventListData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DropView()
        {
            Models.mysqlConnector objConnector = new Models.mysqlConnector();
            ViewBag.treeDataSource = objConnector.GetPersonContacts();
            List<SchedulerDataModel> eventListData = new List<SchedulerDataModel>();
             eventListData = objConnector.GetEvents(); 
            ViewBag.Jobs = objConnector.GetJobs(); 
            foreach (SchedulerDataModel objemodel in eventListData)
            {
                string[] newArr = new string[] { };
                List<string> list = new List<string>();
                string AttendeesData1 = objemodel.attendeesAsString;
                if (AttendeesData1 != string.Empty)
                {
                    string[] _arr = AttendeesData1.Split(',');
                    foreach (string stringCon in _arr)
                    {
                        list.Add(stringCon);
                    }
                    newArr = list.ToArray();
                    objemodel.Attendees = newArr;
                }
                else
                    objemodel.Attendees = new string[] { };

                
                objemodel.Subject = objemodel.Job;
                
            }
            ViewBag.datasource = eventListData;
            ViewBag.Resources = new string[] {  "Jobs" };
            return View();
        }

        public JsonResult LoadData(Params param)
        {
            DateTime start = param.StartDate;
            DateTime end = param.EndDate;
            Models.mysqlConnector objConnector = new Models.mysqlConnector();
            var data = objConnector.GetEvents();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public class Params
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }



        [HttpPost]
        public JsonResult CrudActions(SchedulerDataModel obj) {
            Models.mysqlConnector objConnector = new Models.mysqlConnector();
            var value = obj;
            int intMax = db.ScheduleEventDatas.ToList().Count > 0 ? db.ScheduleEventDatas.ToList().Max(p => p.Id) : 1;
            DateTime startTime = Convert.ToDateTime(value.StartTime);
            DateTime endTime = Convert.ToDateTime(value.EndTime);
           

            string resp = "";
            if (obj.action == "edit")
            {
                SchedulerDataModel appointment = new SchedulerDataModel()
                {
                    Id = value.Id,
                    StartTime = startTime.ToLocalTime(),
                    EndTime = endTime.ToLocalTime(),
                    Subject = value.Job,
                    Attendees = value.Attendees,
                    Event = value.Event,
                    ResourceId = value.ResourceId,

                    IsAllDay = value.IsAllDay,
                    StartTimezone = value.StartTimezone,
                    EndTimezone = value.EndTimezone,
                    RecurrenceRule = value.RecurrenceRule,
                    RecurrenceID = value.RecurrenceID,
                    RecurrenceException = value.RecurrenceException,
                };

                resp = objConnector.UpdateEvent(appointment);
            }
            else if (obj.action == "Add")
            {
                SchedulerDataModel appointment = new SchedulerDataModel()
                {
                    StartTime = startTime.ToLocalTime(),
                    EndTime = endTime.ToLocalTime(),
                    Job = value.Subject,
                    Attendees = value.Attendees,
                    Event = value.Event,
                    ResourceId = value.ResourceId,

                    IsAllDay = value.IsAllDay,
                    StartTimezone = value.StartTimezone,
                    EndTimezone = value.EndTimezone,
                    RecurrenceRule = value.RecurrenceRule,
                    RecurrenceID = value.RecurrenceID,
                    RecurrenceException = value.RecurrenceException,
                };
                resp = objConnector.AddEvent(appointment);

            }
            else if (obj.action == "editFromDrop")
            {
                SchedulerDataModel appointment = new SchedulerDataModel()
                {
                    Id = value.Id,
                    //StartTime = startTime.ToLocalTime(),
                    //EndTime = endTime.ToLocalTime(),
                    attendeesAsString = value.attendeesAsString,
                    //ResourceId = value.ResourceId,
                    
                };
                List<SchedulerDataModel> eventListData2 = new List<SchedulerDataModel>();
                eventListData2 = objConnector.searchEventByTime(appointment);
                if (eventListData2 != null && eventListData2.Count > 0) {
                    string[] _ArrAttendees = eventListData2.FirstOrDefault().attendeesAsString.Split(',');
                    int existCount = 0;
                    foreach (string valueString in _ArrAttendees)
                    {
                        if (valueString == value.attendeesAsString)
                        {
                            existCount++;
                            break;

                        }

                    }

                    if (existCount == 0)
                    {
                        string toModify = eventListData2.FirstOrDefault().attendeesAsString;
                        toModify = toModify + "," + value.attendeesAsString;
                        appointment.attendeesAsString = toModify;
                        appointment.Id = eventListData2.FirstOrDefault().Id;
                        resp = objConnector.updateByDrop(appointment);
                    }
                     
                

                }
                
            }
            ViewData["result"] = resp;
           



            return Json(new string[] { }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateData(EditParams2 param)
        {
            Models.mysqlConnector objConnector = new Models.mysqlConnector();
            if (param.action == "insert" || (param.action == "batch" && param.added != null)) // this block of code will execute while inserting the appointments
            {
                var value = (param.action == "insert") ? param.value : param.added[0];
                int intMax = db.ScheduleEventDatas.ToList().Count > 0 ? db.ScheduleEventDatas.ToList().Max(p => p.Id) : 1;
                DateTime startTime = Convert.ToDateTime(value.StartTime);
                DateTime endTime = Convert.ToDateTime(value.EndTime);
                SchedulerDataModel appointment = new SchedulerDataModel()
                {
                    Id = intMax + 1,
                    StartTime = startTime.ToLocalTime(),
                    EndTime = endTime.ToLocalTime(),
                    Job = value.Job,
                    Attendees = value.Attendees,
                    Event = value.Event,
                    ResourceId = value.ResourceId,

                    IsAllDay = value.IsAllDay,
                    StartTimezone = value.StartTimezone,
                    EndTimezone = value.EndTimezone,
                    RecurrenceRule = value.RecurrenceRule,
                    RecurrenceID = value.RecurrenceID,
                    RecurrenceException = value.RecurrenceException,
                };
                string resp = objConnector.AddEvent(appointment);
                ViewData["result"] = resp;
            }
            
            if (param.action == "remove" || (param.action == "batch" && param.deleted != null)) // this block of code will execute while removing the appointment
            {
                int key = Convert.ToInt32(param.key);
                SchedulerDataModel obj2 = param.deleted.FirstOrDefault();
                SchedulerDataModel appointment = new SchedulerDataModel();
                if (appointment != null)
                {
                    ViewData["result"] = objConnector.DeleteEvent(obj2.Id);
                }
            }
            var data = objConnector.GetEvents();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

     

    }

}



