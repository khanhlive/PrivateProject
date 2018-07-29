using Moss.Hospital.Data.Dao.Enum;
using Moss.Hospital.Data.Entities;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace Moss.Hospital.Data.Providers
{
    public class LichCongTacProvider : SqlEntityBase<Calendar, int>, IProvider<Calendar>
    {
        public CoreResult Delete(int key, int? userId = default(int?), bool checkPermission = false)
        {
            return this.DeleteMultilple("[Calendar_DeleteMultiple]",
                new string[] { "@PrimaryKeys" },
                new object[] { this.GetXMLStringDeleteMultiple(new int[] { key }) },
                userId,
                checkPermission
                );
        }

        public CoreResult Delete(Calendar entity, int? userId = default(int?), bool checkPermission = false)
        {
            return Delete(entity.CalendarID, userId, checkPermission);
        }

        public CoreResult Exist(int key)
        {
            try
            {
                var obj = this.Get(key);
                return new CoreResult { StatusCode = obj == null ? CoreStatusCode.NotExisted : CoreStatusCode.Existed, Data = obj };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Calendar Get(int key)
        {
            try
            {
                return this.GetSingleByDapper("[Calendar_GetSingle]", new string[] { "@CalendarID" }, new object[] { key }, CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Calendar> GetFilter(DateTime? ngaynhap=null, int? employeeID=null,int? departmentID=null, DateTime? time=null)
        {
            try
            {
                return GetListByDapper("[Calendar_GetFilter]",
                    new string[] { "@CalendarDate", "@EmployeeID", "@DepartmentID", "@Time" },
                    new object[] { ngaynhap, employeeID, departmentID, time },
                    CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                this.sqlHelper.Close();
                return null;
            }
        }

        public CoreResult Insert(Calendar entity, int? userId = default(int?), bool checkPermission = false)
        {
            return AddModel("[Calendar_Insert]",
                new string[] { "@CalendarDate", "@employeesID", "@DepartmentsID", "@TimeFrom", "@TimeTo", "@Description", "@isActivity" },
                new object[] { entity.CalendarDate, entity.employeesID, entity.DepartmentsID, entity.TimeFrom, entity.TimeTo, entity.Description, entity.isActivity },
                userId,
                checkPermission
                );
        }

        public CoreResult Update(Calendar entity, int? userId = default(int?), bool checkPermission = false)
        {
            return UpdateModel("[Calendar_Update]",
                new string[] { "@CalendarID", "@CalendarDate", "@employeesID", "@DepartmentsID", "@TimeFrom", "@TimeTo", "@Description", "@isActivity" },
                new object[] { entity.CalendarID, entity.CalendarDate, entity.employeesID, entity.DepartmentsID, entity.TimeFrom, entity.TimeTo, entity.Description, entity.isActivity },
                userId,checkPermission
                );
        }

        public CoreResult UpdateMultilple(List<Calendar> calendars,int? userId = default(int?), bool checkPermission = false)
        {
            if (calendars!= null && calendars.Count>0)
            {
                #region MyRegion
                XElement Xml = new XElement("Calendars",
                from detail in calendars
                select new XElement("Calendar",
                new XAttribute("id", detail.CalendarID),
new XElement("CalendarID", detail.CalendarID),
new XElement("CalendarDate", detail.CalendarDate),
new XElement("employeesID", detail.employeesID),
new XElement("DepartmentsID", detail.DepartmentsID),
new XElement("TimeFrom", detail.TimeFrom),
new XElement("TimeTo", detail.TimeTo),
new XElement("Description", detail.Description),
new XElement("isActivity", detail.isActivity))

);
                return UpdateModel("[Calendar_UpdateMultiple]",
                    new string[] { "@Calendars" },
                    new object[] { Xml?.ToString() },
                    userId, checkPermission
                    );
                #endregion
            }
            else
            {
                return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = "Danh sách lịch công tác không được NULL hoặc rỗng" };
            }
        }
        public CoreResult InsertMultilple(List<Calendar> calendars, int? userId = default(int?), bool checkPermission = false)
        {
            if (calendars != null && calendars.Count > 0)
            {
                #region MyRegion
                XElement Xml = new XElement("Calendars",
                from detail in calendars
                select new XElement("Calendar",
                new XAttribute("id", detail.CalendarID),
new XElement("CalendarID", detail.isActivity),
new XElement("CalendarDate", detail.CalendarDate),
new XElement("employeesID", detail.employeesID),
new XElement("DepartmentsID", detail.DepartmentsID),
new XElement("TimeFrom", detail.TimeFrom),
new XElement("TimeTo", detail.TimeTo),
new XElement("Description", detail.Description),
new XElement("isActivity", detail.isActivity))

);
                return AddModel("[Calendar_InsertMultiple]",
                    new string[] { "@Calendars" },
                    new object[] { Xml?.ToString() },
                    userId, checkPermission
                    );
                #endregion
            }
            else
            {
                return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = "Danh sách lịch công tác không được NULL hoặc rỗng" };
            }


        }

        protected override string GetFeatureCode()
        {
            return "LICHCONGTAC";
        }

        protected override string GetNameEntity()
        {
            return "lịch công tác";
        }
    }
}
