using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using MySql.Data;
using MySqlConnector;
using System.Web;

namespace ScheduleSample.Models
{
    public class mysqlConnector
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        public mysqlConnector()
        {

        }
        public List<SchedulerDataModel> GetEvents()
        {

            List<SchedulerDataModel> customers = new List<SchedulerDataModel>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "SELECT * FROM schedulerdb.tbleventlist";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            customers.Add(new SchedulerDataModel
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Job = sdr["Job"].ToString(),
                                Event = sdr["Event"].ToString(),
                                ResourceId = sdr["ResourceId"].ToString(),
                                attendeesAsString = sdr["Attendees"].ToString(),
                                StartTime = Convert.ToDateTime(sdr["StartTime"]),
                                EndTime = Convert.ToDateTime(sdr["EndTime"]),
                                StartTimezone = sdr["StartTimezone"].ToString(),
                                EndTimezone = sdr["EndTimezone"].ToString(),
                                IsAllDay = Convert.ToBoolean(sdr["IsAllDay"]),
                                RecurrenceRule = Convert.ToString(sdr["RecurrenceRule"]),
                                RecurrenceID = Convert.ToInt32(sdr["RecurrenceID"]),
                                RecurrenceException = Convert.ToString(sdr["RecurrenceException"])

                            });
                        }
                    }
                    con.Close();
                }
            }

            return customers;

        }


        public List<SchedulerDataModel> searchEventByTime(SchedulerDataModel obj)
        {

            List<SchedulerDataModel> filteredData = new List<SchedulerDataModel>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "SELECT Id,  Attendees   FROM schedulerdb.tbleventlist where Id="+obj.Id+" ";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            filteredData.Add(new SchedulerDataModel
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                attendeesAsString = sdr["Attendees"].ToString(),
                                
                            });
                        }
                    }
                    con.Close();
                }
            }

            return filteredData;

        }

        public List<PersonContacts> GetPersonContacts()
        {

            List<PersonContacts> PersonContactsList = new List<PersonContacts>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "SELECT * FROM schedulerdb.`persons (contacts)`";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            PersonContactsList.Add(new PersonContacts
                            {
                                ID = sdr["ID"].ToString(),
                                Name = sdr["Name"].ToString(),
                              ObjectType=  sdr["ObjectType"].ToString()
                                //Id = Convert.ToInt32(sdr["Id"]),
                                //Subject = sdr["Subject"].ToString(),
                                //StartTime = Convert.ToDateTime(sdr["StartTime"]),
                                //EndTime = Convert.ToDateTime(sdr["EndTime"]),
                                //StartTimezone = sdr["StartTimezone"].ToString(),
                                //EndTimezone = sdr["EndTimezone"].ToString(),
                                //IsAllDay = Convert.ToBoolean(sdr["IsAllDay"]),
                                //RecurrenceRule = Convert.ToString(sdr["RecurrenceRule"]),
                                //RecurrenceID = Convert.ToInt32(sdr["RecurrenceID"]),
                                //RecurrenceException = Convert.ToString(sdr["RecurrenceException"])

                            });
                        }
                    }
                    con.Close();
                }
            }

            return PersonContactsList;

        }
        public List<Jobs> GetJobs()
        {

            List<Jobs> JobList = new List<Jobs>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "SELECT * FROM schedulerdb.jobs LIMIT 50";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            JobList.Add(new Jobs
                            {
                                ID = sdr["ID"].ToString(),
                                Name = sdr["Name"].ToString(),
                                fkTenantID=sdr["fkTenantID"].ToString()

                                //Id = Convert.ToInt32(sdr["Id"]),
                                //Subject = sdr["Subject"].ToString(),
                                //StartTime = Convert.ToDateTime(sdr["StartTime"]),
                                //EndTime = Convert.ToDateTime(sdr["EndTime"]),
                                //StartTimezone = sdr["StartTimezone"].ToString(),
                                //EndTimezone = sdr["EndTimezone"].ToString(),
                                //IsAllDay = Convert.ToBoolean(sdr["IsAllDay"]),
                                //RecurrenceRule = Convert.ToString(sdr["RecurrenceRule"]),
                                //RecurrenceID = Convert.ToInt32(sdr["RecurrenceID"]),
                                //RecurrenceException = Convert.ToString(sdr["RecurrenceException"])

                            });
                        }
                    }
                    con.Close();
                }
            }

            return JobList;

        }
        public string AddEvent(SchedulerDataModel modelObj)
        {
            MySqlConnection conn = new MySqlConnection(constr);
            try
            {
                if (string.IsNullOrEmpty(modelObj.StartTimezone))
                    modelObj.StartTimezone = "";
                if (string.IsNullOrEmpty(modelObj.EndTimezone))
                    modelObj.EndTimezone = "";
                if (string.IsNullOrEmpty(modelObj.RecurrenceRule))
                    modelObj.RecurrenceRule = "";
                if (modelObj.RecurrenceID == null)
                    modelObj.RecurrenceID = 0;
                if (string.IsNullOrEmpty(modelObj.RecurrenceException))
                    modelObj.RecurrenceException = "";
                string m_att = string.Empty;
                if (modelObj.Attendees != null)
                {
                    foreach (string s in modelObj.Attendees)
                    {
                        if (string.IsNullOrEmpty(m_att))
                        {
                            m_att = s;

                        }
                        else
                            m_att += "," + s;

                    }
                }

                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Insert into schedulerdb.tbleventlist (Job,ResourceId,Attendees,Event,StartTime,EndTime,StartTimezone,EndTimezone,IsAllDay,RecurrenceRule,RecurrenceID,RecurrenceException )  " + "values(@Job,@ResourceId,@Attendees,@Event, @StartTime, @EndTime, @StartTimezone,@EndTimezone,@IsAllDay,@RecurrenceRule,@RecurrenceID,@RecurrenceException)", conn);
                cmd.Parameters.AddWithValue("@Job", modelObj.Job);
                cmd.Parameters.AddWithValue("@ResourceId", modelObj.ResourceId);
                cmd.Parameters.AddWithValue("@Attendees", m_att);
                cmd.Parameters.AddWithValue("@Event", modelObj.Event);

                cmd.Parameters.AddWithValue("@StartTime", modelObj.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", modelObj.EndTime);
                cmd.Parameters.AddWithValue("@StartTimezone", modelObj.StartTimezone);
                cmd.Parameters.AddWithValue("@EndTimezone", modelObj.EndTimezone);
                cmd.Parameters.AddWithValue("@IsAllDay", modelObj.IsAllDay);
                cmd.Parameters.AddWithValue("@RecurrenceRule", modelObj.RecurrenceRule);
                cmd.Parameters.AddWithValue("@RecurrenceID", modelObj.RecurrenceID);
                cmd.Parameters.AddWithValue("@RecurrenceException", modelObj.RecurrenceException);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return "registed successfully";

            }
            catch (MySqlException ex)
            {
                return "exception found";
            }
            finally
            {
                conn.Close();
            }


        }
        public string UpdateEvent(SchedulerDataModel modelObj)
        {
            MySqlConnection conn = new MySqlConnection(constr);
            try
            {
                if (string.IsNullOrEmpty(modelObj.StartTimezone))
                    modelObj.StartTimezone = "";
                if (string.IsNullOrEmpty(modelObj.EndTimezone))
                    modelObj.EndTimezone = "";
                if (string.IsNullOrEmpty(modelObj.RecurrenceRule))
                    modelObj.RecurrenceRule = "";
                if (modelObj.RecurrenceID == null)
                    modelObj.RecurrenceID = 0;
                if (string.IsNullOrEmpty(modelObj.RecurrenceException))
                    modelObj.RecurrenceException = "";
                string m_att = string.Empty;
                if (modelObj.Attendees != null)
                {
                    foreach (string s in modelObj.Attendees)
                    {
                        if (string.IsNullOrEmpty(m_att))
                        {
                            m_att = s;

                        }
                        else
                            m_att += "," + s;

                    }
                }

                conn.Open();
                MySqlCommand cmd = new MySqlCommand("update  schedulerdb.tbleventlist set  Job=@Job, Event=@Event,Attendees=@Attendees ,StartTime=@StartTime,EndTime=@EndTime,IsAllDay=@IsAllDay,RecurrenceException=@RecurrenceException where Id=" + modelObj.Id + "   ", conn);
                cmd.Parameters.AddWithValue("@Job", modelObj.Subject);
                cmd.Parameters.AddWithValue("@ResourceId", modelObj.ResourceId);
                cmd.Parameters.AddWithValue("@Attendees", m_att);
                cmd.Parameters.AddWithValue("@Event", modelObj.Event);

                cmd.Parameters.AddWithValue("@StartTime", modelObj.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", modelObj.EndTime);
                cmd.Parameters.AddWithValue("@StartTimezone", modelObj.StartTimezone);
                cmd.Parameters.AddWithValue("@EndTimezone", modelObj.EndTimezone);
                cmd.Parameters.AddWithValue("@IsAllDay", modelObj.IsAllDay);
                cmd.Parameters.AddWithValue("@RecurrenceRule", modelObj.RecurrenceRule);
                cmd.Parameters.AddWithValue("@RecurrenceID", modelObj.RecurrenceID);
                cmd.Parameters.AddWithValue("@RecurrenceException", modelObj.RecurrenceException);
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (i > 0)
                    return "updated successfully";
                else
                    return "not updated";

            }
            catch (MySqlException ex)
            {
                return "exception found";
            }
            finally
            {
                conn.Close();
            }


        }
        public string updateByDrop(SchedulerDataModel modelObj)
        {
            MySqlConnection conn = new MySqlConnection(constr);
            try
            {
              
                string m_att = string.Empty;
                if (modelObj.attendeesAsString != null)
                {
                    string[] _Arr = modelObj.attendeesAsString.Split(',');
                    foreach (string s in _Arr)
                    {
                        if (string.IsNullOrEmpty(m_att))
                        {
                            m_att = s;

                        }
                        else
                            m_att += "," + s;

                    }
                }

                conn.Open();
                MySqlCommand cmd = new MySqlCommand("update  schedulerdb.tbleventlist set  Attendees='"+ m_att + "'  where Id="+modelObj.Id+"  ", conn);
              
                //cmd.Parameters.AddWithValue("@ResourceId", modelObj.ResourceId);
                //cmd.Parameters.AddWithValue("@Attendees", m_att);
               
                //cmd.Parameters.AddWithValue("@StartTime", modelObj.StartTime);
                //cmd.Parameters.AddWithValue("@EndTime", modelObj.EndTime);
               
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (i > 0)
                    return "updated successfully";
                else
                    return "not updated";

            }
            catch (MySqlException ex)
            {
                return "exception found";
            }
            finally
            {
                conn.Close();
            }


        }
        public string DeleteEvent(int Id)
        {
            MySqlConnection conn = new MySqlConnection(constr);
            try
            {


                conn.Open();
                MySqlCommand cmd = new MySqlCommand("delete from schedulerdb.tbleventlist where Id=" + Id + " ", conn);

                int delStatus = cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (delStatus > 0)
                    return "deleted successfully";
                else
                    return "not deleted";
            }
            catch (MySqlException ex)
            {
                return "exception found";
            }
            finally
            {
                conn.Close();
            }


        }
    }
}