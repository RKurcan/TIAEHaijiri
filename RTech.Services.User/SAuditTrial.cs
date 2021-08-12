using Riddhasoft.Entity.User;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Services.User
{
    public class SAuditTrial
    {

        string _menuCode;

        string _actionCode;

        DateTime _systemTime;
        int _userId;
        string _message;
        int _targetId;

        Riddhasoft.DB.RiddhaDBContext db = null;

        public SAuditTrial()
        {
            db = new DB.RiddhaDBContext();
        }

        public SAuditTrial(string menuCode, string actionCode, DateTime systemTime, int userId, int targetId, string message)
        {
            this._menuCode = menuCode;
            this._actionCode = actionCode;
            this._systemTime = systemTime;
            this._userId = userId;
            this._message = message;
            this._targetId = targetId;

            db = new DB.RiddhaDBContext();
        }
        public ServiceResult<IQueryable<EAuditTrial>> List()
        {
            return new ServiceResult<IQueryable<EAuditTrial>>()
            {
                Data = db.AuditTrial,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        //public ServiceResult<EAuditTrial> Add(EAuditTrial log)
        //{
        //    db.AuditTrial.Add(log);
        //    db.SaveChanges();
        //    return new ServiceResult<EAuditTrial>() { Status = ResultStatus.Ok };
        //}
        //public ServiceResult<EAuditTrial> Add()
        //{
        //    db.AuditTrial.Add(new EAuditTrial()
        //    {
        //        MenuCode = _menuCode,
        //        ActionCode = _actionCode,
        //        LogTime = System.DateTime.Now,
        //        SystemTime = _systemTime,
        //        UserId = _userId,
        //        TargetId = _targetId,
        //        Message = _message
        //    });
        //    db.SaveChanges();
        //    return new ServiceResult<EAuditTrial>() { Status = ResultStatus.Ok };
        //}
        public ServiceResult<EAuditTrial> Add(string menuCode, string actionCode, DateTime systemTime, int userId, int targetId, string message)
        {
            db.AuditTrial.Add(new EAuditTrial()
            {
                MenuCode = menuCode,
                ActionCode = actionCode,
                LogTime = System.DateTime.Now,
                SystemTime = systemTime,
                UserId = userId,
                TargetId = targetId,
                Message = message
            });
            db.SaveChanges();
            return new ServiceResult<EAuditTrial>() { Status = ResultStatus.Ok };
        }
        public ServiceResult<EAuditTrial> Add()
        {
            db.AuditTrial.Add(new EAuditTrial()
            {
                MenuCode = _menuCode,
                ActionCode = _actionCode,
                LogTime = System.DateTime.Now,
                SystemTime = _systemTime,
                UserId = _userId,
                TargetId = _targetId,
                Message = _message
            });
            db.SaveChanges();
            return new ServiceResult<EAuditTrial>() { Status = ResultStatus.Ok };
        }
    }
}
