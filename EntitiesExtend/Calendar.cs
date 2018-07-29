using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Providers;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;

namespace Moss.Hospital.Data.Entities
{
    public partial class Calendar : IEntity<CateBed>
    {

        #region Method

        public CoreResult Delete(int? userId = default(int?), bool checkPermission = false)
        {
            using (LichCongTacProvider provider=new LichCongTacProvider())
            {
                return provider.Delete(this.CalendarID, userId, checkPermission);
            }
        }

        public CoreResult Exist()
        {
            using (LichCongTacProvider provider = new LichCongTacProvider())
            {
                return provider.Exist(this.CalendarID);
            }
        }

        public CoreResult Get()
        {
            using (LichCongTacProvider provider = new LichCongTacProvider())
            {
                var entity = provider.Get(this.CalendarID);
                if (entity != null)
                {
                    #region Assign value
                    this.CalendarID = entity.CalendarID;
                    this.CalendarDate = entity.CalendarDate;
                    this.employeesID = entity.employeesID;
                    this.DepartmentsID = entity.DepartmentsID;
                    this.TimeFrom = entity.TimeFrom;
                    this.TimeTo = entity.TimeTo;
                    this.Description = entity.Description;
                    this.isActivity = entity.isActivity;
                    #endregion
                    return provider.GetResultFromStatusCode(CoreStatusCode.OK, ActionType.Get);
                }
                else
                    return provider.GetResultFromStatusCode(CoreStatusCode.Failed, ActionType.Get);
            }
        }

        public Calendar Get(int key)
        {
            using (LichCongTacProvider provider = new LichCongTacProvider())
            {
                return provider.Get(key);
            }
        }

        public CoreResult Insert(int? userId = default(int?), bool checkPermission = false)
        {
            using (LichCongTacProvider provider = new LichCongTacProvider())
            {
                return provider.Insert(this, userId, checkPermission);
            }
        }

        public CoreResult Update(int? userId = default(int?), bool checkPermission = false)
        {
            using (LichCongTacProvider provider = new LichCongTacProvider())
            {
                return provider.Update(this, userId, checkPermission);
            }
        }

        public IEnumerable<Calendar> GetFilter(DateTime? ngaynhap = null, int? employeeID = null, int? departmentID = null, DateTime? time = null)
        {
            using (LichCongTacProvider provider=new LichCongTacProvider())
            {
                return provider.GetFilter(ngaynhap, employeeID, departmentID, time);
            }
        }

        #endregion
    }
}
