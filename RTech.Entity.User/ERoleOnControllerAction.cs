using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.User.Entity
{
    public class ERoleOnControllerAction
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        //public AppController Controller { get; set; }
        public AppActionName Action { get; set; }
        public virtual EUserRole Role { get; set; }
    }
    public class ActionController
    {
        public AppActionName ActionId { get; set; }
        public AppController ControllerId { get; set; }
    }

    public static class ActionControllerData
    {
        public static List<ActionController> GetData()
        {
            return new List<ActionController>()
            {
#region Office 
                new ActionController(){ActionId=AppActionName.List, ControllerId=AppController.branch},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.branch},
                new ActionController (){ActionId=AppActionName.Update, ControllerId=AppController.branch},
                new ActionController (){ActionId=AppActionName.Delete, ControllerId=AppController.branch},

                new ActionController(){ActionId=AppActionName.Update,ControllerId=AppController.department},
                new ActionController () { ActionId=AppActionName.List, ControllerId=AppController.department},
                new ActionController (){ ActionId=AppActionName.Create, ControllerId=AppController.department},
                new ActionController () { ActionId=AppActionName.Delete, ControllerId=AppController.department},

                new ActionController(){ActionId=AppActionName.Delete, ControllerId=AppController.section},
                new ActionController(){ActionId=AppActionName.List, ControllerId=AppController.section},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.section},
                new ActionController (){ ActionId=AppActionName.Update, ControllerId=AppController.section},

                new ActionController(){ActionId=AppActionName.List, ControllerId=AppController.designation},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.designation},
                new ActionController(){ActionId=AppActionName.Update,ControllerId=AppController.designation},
                new ActionController(){ActionId=AppActionName.Delete,ControllerId=AppController.designation},
              
             
                new ActionController(){ActionId=AppActionName.List, ControllerId=AppController.shift},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.shift},
                new ActionController (){ActionId=AppActionName.Update, ControllerId=AppController.shift},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.shift},

                new ActionController(){ActionId=AppActionName.List, ControllerId=AppController.leavemaster},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.leavemaster},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.leavemaster},
                new ActionController (){ActionId=AppActionName.Update, ControllerId=AppController.leavemaster},


                new ActionController(){ActionId=AppActionName.List, ControllerId=AppController.holiday},
                new ActionController (){ActionId=AppActionName.Create, ControllerId=AppController.holiday},
                new ActionController (){ActionId=AppActionName.Update, ControllerId=AppController.holiday},
                new ActionController (){ActionId=AppActionName.Delete, ControllerId=AppController.holiday},

                new ActionController(){ActionId=AppActionName.List, ControllerId=AppController.bank},
                new ActionController (){ActionId=AppActionName.Create, ControllerId=AppController.bank},
                new ActionController (){ActionId=AppActionName.Update, ControllerId=AppController.bank},
                new ActionController (){ActionId=AppActionName.Delete, ControllerId=AppController.bank},

                new ActionController(){ActionId=AppActionName.List, ControllerId=AppController.notice},
                new ActionController (){ActionId=AppActionName.Create, ControllerId=AppController.notice},
                new ActionController (){ActionId=AppActionName.Update, ControllerId=AppController.notice},
                new ActionController (){ActionId=AppActionName.Delete, ControllerId=AppController.notice},

#endregion

#region employee

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.employee},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.employee},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.employee},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.employee},

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.roster},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.roster},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.roster},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.roster},

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.leavebalance},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.leavebalance},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.leavebalance},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.leavebalance},

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.leaveapplication},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.leaveapplication},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.leaveapplication},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.leaveapplication},

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.leaveapplication},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.leaveapplication},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.leaveapplication},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.leaveapplication},

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.manualpunch},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.manualpunch},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.manualpunch},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.manualpunch},
#endregion

#region Payroll

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.payroll},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.payroll},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.payroll},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.payroll},
#endregion
               
#region User Setup

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.userrole},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.userrole},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.userrole},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.userrole},

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.user},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.user},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.user},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.user},
#endregion

#region Report
                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.attendancereport},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.attendancereport},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.attendancereport},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.attendancereport},

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.payrollreport},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.payrollreport},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.payrollreport},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.payrollreport},
#endregion

#region Device

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.device},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.device},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.device},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.device},

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.deviceassignment},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.deviceassignment},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.deviceassignment},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.deviceassignment},

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.companydevice},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.companydevice},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.companydevice},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.companydevice},

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.companydeviceassignment},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.companydeviceassignment},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.companydeviceassignment},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.companydeviceassignment},
                
                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.model},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.model},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.model},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.model},

                new ActionController(){ActionId=AppActionName.List,ControllerId=AppController.realtime},
                new ActionController(){ActionId=AppActionName.Create, ControllerId=AppController.realtime},
                new ActionController(){ ActionId=AppActionName.Update, ControllerId=AppController.realtime},
                new ActionController (){ ActionId=AppActionName.Delete, ControllerId=AppController.realtime},
              
#endregion

            };
        }
    }
}
