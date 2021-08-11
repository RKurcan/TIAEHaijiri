using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Entity.User
{
    public class EMenu
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public string ParentCode { get; set; }
        public bool IsGroup { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIconCss { get; set; }
        public bool IsWidget { get; set; }
        public int SoftwarePackageType { get; set; }
        public int Order { get; set; }
    }
    public class EMenuAction
    {
        public int Id { get; set; }
        public string MenuCode { get; set; }
        public string ActionCode { get; set; }
        public string Desc { get; set; }
    }
    public class EUserGroupMenuRight
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public virtual EMenu Menu { get; set; }
        public virtual EUserRole Role { get; set; }

    }
    public class EUserGroupActionRight
    {
        public int Id { get; set; }
        public int MenuActionId { get; set; }
        public int RoleId { get; set; }
        public virtual EUserRole Role { get; set; }
        public virtual EMenuAction MenuAction { get; set; }
    }

    public class EUserGroupDataVisibility
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public DataVisibilityLevel DataVisibilityLevel { get; set; }
    }

    public enum DataVisibilityLevel
    {
        Self = 0,
        Section = 1,
        Department = 2,
        Branch = 3,
        All = 4,
        ReportingHierarchy = 5,
    }
}
