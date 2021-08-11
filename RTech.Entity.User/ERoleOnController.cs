using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.User.Entity
{
    public class ERoleOnController
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public AppController AppController { get; set; }
        public virtual EUserRole Role { get; set; }

    }
    public enum AppController
    {
        companydevice,
        reseller,
        company,
        companyprofile,
        branch,
        department,
        section,
        fiscalyear,
        designation,
        shift,
        leavemaster,
        holiday,
        bank,
        employee,
        roster,
        leavebalance,
        leaveapplication,
        manualpunch,
        payroll,
        userrole,
        user,
        attendancereport,
        payrollreport,
        modulepermission,
        controllerpermission,
        actionpermission,
        device,
        deviceassignment,
        companydeviceassignment,
        model,
        realtime,
        notice,
        gradegroup,
        leavesettlement,
        officevisit
    }

    public enum AppActionName
    {
        List,//get
        Create,//create
        Update,//put
        Delete,//delete
        Approve,//approve

    }


    public class ModuleController
    {
        public AppController ControllerId { get; set; }
        public Module ModuleId { get; set; }
    }
    public static class ModuleControllerData
    {
        public static List<ModuleController> GetData()
        {
            return new List<ModuleController>()
            {
                #region office 
                                new ModuleController(){ControllerId=AppController.companyprofile,ModuleId=Module.Office},
                                new ModuleController(){ControllerId=AppController.branch,ModuleId=Module.Office},
                                new ModuleController(){ControllerId=AppController.department,ModuleId=Module.Office},
                                new ModuleController(){ControllerId=AppController.section,ModuleId=Module.Office},
                                new ModuleController(){ControllerId=AppController.fiscalyear,ModuleId=Module.Office},
                                new ModuleController(){ControllerId=AppController.designation,ModuleId=Module.Office},
                                new ModuleController(){ControllerId=AppController.shift, ModuleId=Module.Office},
                                new ModuleController(){ControllerId=AppController.leavemaster, ModuleId=Module.Office},
                                new ModuleController(){ControllerId=AppController.holiday, ModuleId=Module.Office},
                                new ModuleController(){ControllerId=AppController.bank, ModuleId=Module.Office},
                                 new ModuleController(){ControllerId=AppController.notice, ModuleId=Module.Office},
                                 new ModuleController(){ControllerId=AppController.gradegroup, ModuleId=Module.Office},
                #endregion

                #region Employee
                                new ModuleController(){ControllerId=AppController.employee, ModuleId=Module.Employee},
                                new ModuleController(){ControllerId=AppController.roster, ModuleId=Module.Employee},
                                new ModuleController(){ControllerId=AppController.leavebalance, ModuleId=Module.Employee},
                                new ModuleController(){ControllerId=AppController.leaveapplication, ModuleId=Module.Employee},
                                new ModuleController(){ControllerId=AppController.manualpunch, ModuleId=Module.Employee},
                                new ModuleController(){ControllerId=AppController.leavesettlement, ModuleId=Module.Employee},
                                new ModuleController(){ControllerId=AppController.officevisit, ModuleId=Module.Employee},
                #endregion

                #region PayRoll
                                new ModuleController(){ControllerId=AppController.payroll, ModuleId=Module.Payroll},
                #endregion

                #region User Setup
                                new ModuleController(){ControllerId=AppController.userrole, ModuleId=Module.UserSetup},
                                new ModuleController(){ControllerId=AppController.user, ModuleId=Module.UserSetup},
                                new ModuleController(){ControllerId=AppController.modulepermission, ModuleId=Module.UserSetup},
                                new ModuleController(){ControllerId=AppController.controllerpermission, ModuleId=Module.UserSetup},
                                new ModuleController(){ControllerId=AppController.actionpermission, ModuleId=Module.UserSetup},
                #endregion

                #region Report
                                new ModuleController(){ControllerId=AppController.attendancereport, ModuleId=Module.Report},
                                new ModuleController(){ControllerId=AppController.payrollreport, ModuleId=Module.Report},
                #endregion

                #region Device
                                new ModuleController(){ControllerId=AppController.companydevice, ModuleId=Module.Device},
                                new ModuleController(){ControllerId=AppController.device, ModuleId=Module.Device},
                                new ModuleController(){ControllerId=AppController.deviceassignment, ModuleId=Module.Device},
                                new ModuleController(){ControllerId=AppController.companydeviceassignment, ModuleId=Module.Device},
                                new ModuleController(){ControllerId=AppController.model, ModuleId=Module.Device},
                                new ModuleController(){ControllerId=AppController.realtime, ModuleId=Module.Device},

                #endregion
                

            };
        }
    }
}
