using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Providers;

namespace Moss.Hospital.Data.Entities
{
    public partial class XuatKho : IEntity<XuatKho>
    {
        public XuatKho()
        {
            this.XuatKhoDetails = new List<XuatKhoDetail>();
        }

        #region Properties

        public List<XuatKhoDetail> XuatKhoDetails{ get; set; }

        #endregion

        #region Method
        
        public CoreResult Delete(int? userId = default(int?), bool checkPermission = false)
        {
            using (XuatKhoProvider provider = new XuatKhoProvider())
            {
                return provider.Delete(this.XuatKhoID, userId, checkPermission);
            }
        }

        public CoreResult Exist()
        {
            using (XuatKhoProvider provider = new XuatKhoProvider())
            {
                return provider.Exist(this.XuatKhoID);
            }
        }

        public CoreResult Get()
        {
            try
            {
                return Get(XuatKhoID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult Insert(int? userId = default(int?), bool checkPermission = false)
        {
            using (XuatKhoProvider provider = new XuatKhoProvider())
            {
                return provider.Insert(this, userId, checkPermission);
            }
        }

        public CoreResult Update(int? userId = default(int?), bool checkPermission = false)
        {
            using (XuatKhoProvider provider = new XuatKhoProvider())
            {
                return provider.Update(this, userId, checkPermission);
            }
        }

        public CoreResult Get(int xuatkhoid)
        {
            try
            {
                using (XuatKhoProvider provider = new XuatKhoProvider())
                {
                    XuatKho entity = provider.Get(xuatkhoid);
                    if (entity != null)
                    {
                        #region Assign value

                        this.XuatKhoID = entity.XuatKhoID;
                        this.DepartmentsID_KhoX = entity.DepartmentsID_KhoX;
                        this.LoaiPhieuXuat = entity.LoaiPhieuXuat;
                        this.DepartmentsID_NoiNhan = entity.DepartmentsID_NoiNhan;
                        this.patientsID = entity.patientsID;
                        this.NguoiNhan = entity.NguoiNhan;
                        this.NoiDungXuat = entity.NoiDungXuat;
                        this.NgayXuat = entity.NgayXuat;
                        this.NgayCT = entity.NgayCT;
                        this.SoCT = entity.SoCT;
                        this.Duyet_status = entity.Duyet_status;
                        this.Duyet_employeesID = entity.Duyet_employeesID;
                        this.Duyet_Date = entity.Duyet_Date;
                        this.userIDCreated = entity.userIDCreated;
                        this.dateCreated = entity.dateCreated;
                        this.dateUpdated = entity.dateUpdated;
                        this.userIDUpdated = entity.userIDUpdated;
                        this.NumberUpdated = entity.NumberUpdated;

                        #endregion
                        return provider.GetResultFromStatusCode(CoreStatusCode.OK, ActionType.Get);
                    }
                    else
                        return provider.GetResultFromStatusCode(CoreStatusCode.Failed, ActionType.Get);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult GetWithDetail()
        {
            return GetWithDetail(XuatKhoID);
        }

        public CoreResult GetWithDetail(int xuatkhoid)
        {
            try
            {
                using (XuatKhoProvider provider = new XuatKhoProvider())
                {
                    XuatKho entity = provider.GetWithDetails(xuatkhoid);
                    if (entity != null)
                    {
                        #region Assign value

                        this.XuatKhoID = entity.XuatKhoID;
                        this.DepartmentsID_KhoX = entity.DepartmentsID_KhoX;
                        this.LoaiPhieuXuat = entity.LoaiPhieuXuat;
                        this.DepartmentsID_NoiNhan = entity.DepartmentsID_NoiNhan;
                        this.patientsID = entity.patientsID;
                        this.NguoiNhan = entity.NguoiNhan;
                        this.NoiDungXuat = entity.NoiDungXuat;
                        this.NgayXuat = entity.NgayXuat;
                        this.NgayCT = entity.NgayCT;
                        this.SoCT = entity.SoCT;
                        this.Duyet_status = entity.Duyet_status;
                        this.Duyet_employeesID = entity.Duyet_employeesID;
                        this.Duyet_Date = entity.Duyet_Date;
                        this.userIDCreated = entity.userIDCreated;
                        this.dateCreated = entity.dateCreated;
                        this.dateUpdated = entity.dateUpdated;
                        this.userIDUpdated = entity.userIDUpdated;
                        this.NumberUpdated = entity.NumberUpdated;
                        this.XuatKhoDetails = entity.XuatKhoDetails;
                        #endregion
                        return provider.GetResultFromStatusCode(CoreStatusCode.OK, ActionType.Get);
                    }
                    else
                        return provider.GetResultFromStatusCode(CoreStatusCode.Failed, ActionType.Get);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult DeleteChiTiet(int[] xuatkhochitietIDs, int? userId = default(int?), bool checkPermission = false)
        {
            using (XuatKhoProvider provider = new XuatKhoProvider())
            {
                return provider.DeleteChiTiet(xuatkhochitietIDs, userId, checkPermission);
            }
        }
        #endregion
    }
}
