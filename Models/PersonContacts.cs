using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleSample.Models
{
    public class PersonContacts
    {
        public string ID { get; set; }
        public string ObjectType { get; set; }
        public string fkTenantID { get; set; }
        public string fkUserID { get; set; }

        public string fkUserTypeID { get; set; }
        public string tempUserTypeID { get; set; }
        public string fkContactID { get; set; }
        public string fkAPVendorID { get; set; }
        public string fkPayGroupID { get; set; }
        public string fkLabourClassID { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ApproverUserID { get; set; }
        public int ApprovalLimit { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Fax { get; set; }
        public string Photo { get; set; }
        public int  IsLoggedIn { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastActiveDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified  { get; set; }

        public int  LoginTimeout { get; set; }
        public string Favorites { get; set; }
        public string Defaults { get; set; }
        public string Theme { get; set; }
        public string Settings { get; set; }
        public string Visible { get; set; }
        public string Department { get; set; }
        public string WageCode { get; set; }
        public string PayLevel { get; set; }
      


       

    }
}