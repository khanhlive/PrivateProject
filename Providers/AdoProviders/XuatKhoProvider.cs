using Dapper;
using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Entities;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Moss.Hospital.Data.Providers
{
    public class XuatKhoProvider : SqlEntityBase<XuatKho, int>, IProvider<XuatKho>
    {
        public CoreResult Delete(int key, int? userId = default(int?), bool checkPermission = false)
        {
            return this.DeleteMultilple("[XuatKho_Delete]",
                new string[] { "@XuatKhoID", "@UserID" },
                new object[] { key, userId??0 },
                userId,
                checkPermission
                );
        }

        public CoreResult Delete(XuatKho entity, int? userId = default(int?), bool checkPermission = false)
        {
            return Delete(entity.XuatKhoID, userId, checkPermission);
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

        public XuatKho GetWithDetails(int key)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(this.sqlHelper.ConnectionString))
                {
                    dbConnection.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@XuatKhoID", key);
                    parameters.Add("@IsGetDetail", true);
                    using (var data = dbConnection.QueryMultiple("[XuatKho_GetSingle]", parameters, null, null, CommandType.StoredProcedure))
                    {
                        XuatKho _xuatkho = data.Read<XuatKho>().FirstOrDefault();
                        if (_xuatkho != null)
                        {
                            _xuatkho.XuatKhoDetails = data.Read<XuatKhoDetail>().ToList();
                        }
                        return _xuatkho;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<XuatKho> GetFilter(DateTime? ngaytu=null, DateTime? ngayden=null, int? departmentID = null)
        {
            try
            {
                return GetListByDapper("[XuatKho_GetFilter]", new string[] { "@NgayTu","@NgayDen", "@DepartmentID_XuatKho" }, new object[] {ngaytu,ngayden,departmentID }, CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult DeleteChiTiet(int[] xuatkhochitietIDs, int? userId = default(int?), bool checkPermission = false)
        {
            return this.DeleteMultilple("[XuatKho_DeleteChiTiet]",
                new string[] { "@XuatKhoChiTietIDs", "@UserID" },
                new object[] { GetXMLStringDeleteMultiple(xuatkhochitietIDs), userId ?? 0 },
                userId,
                checkPermission
                );
        }

        public XuatKho Get(int key)
        {
            try
            {
                return this.GetSingleByDapper("[XuatKho_GetSingle]", new string[] { "@XuatKhoID", "@IsGetDetail" }, new object[] { key,false }, CommandType.StoredProcedure);
            }
            catch
            {
                return null;
            }
        }

        public CoreResult Insert(XuatKho entity, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                XElement xml = null;
                #region Create XML string

                if (entity.XuatKhoDetails != null && entity.XuatKhoDetails.Count > 0)
                {
                    xml = new XElement("XuatKhoDetails",
                from detail in entity.XuatKhoDetails
                select new XElement("XuatKhoChiTiet",
                new XAttribute("id", detail.XuatKhoDetailID),
new XElement("XuatKhoDetailID", detail.XuatKhoDetailID),
new XElement("XuatKhoID", detail.XuatKhoID),
new XElement("DuocID", detail.DuocID),
new XElement("GiaVAT", detail.GiaVAT),
new XElement("ThanhTienVAT", detail.ThanhTienVAT),
new XElement("SoLo", detail.SoLo),
new XElement("GiaXuat", detail.GiaXuat),
new XElement("ThanhTienXuat", detail.ThanhTienXuat),
new XElement("TyLeGia", detail.TyLeGia),
new XElement("SoLuong", detail.SoLuong),
new XElement("StoreBalanceID", detail.StoreBalanceID)
));
                }

                #endregion
                return AddModel("[XuatKho_Insert]", 
                    new string[] {"@DepartmentsID_KhoX","@LoaiPhieuXuat","@DepartmentsID_NoiNhan","@patientsID","@NguoiNhan","@NoiDungXuat","@NgayXuat","@NgayCT","@SoCT","@userIDCreated","@XuatKhoChiTiet"}, 
                    new object[] {entity.DepartmentsID_KhoX,entity.LoaiPhieuXuat,entity.DepartmentsID_NoiNhan,entity.patientsID,entity.NguoiNhan,entity.NoiDungXuat,entity.NgayXuat,entity.NgayCT,entity.SoCT,entity.userIDCreated,xml?.ToString()},
                    userId,checkPermission);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult Update(XuatKho entity, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                XElement xml = null;
                #region Create XML string

                if (entity.XuatKhoDetails != null && entity.XuatKhoDetails.Count > 0)
                {
                    xml = new XElement("XuatKhoDetails",
                from detail in entity.XuatKhoDetails
                select new XElement("XuatKhoChiTiet",
                new XAttribute("id", detail.XuatKhoDetailID),
new XElement("XuatKhoDetailID", detail.XuatKhoDetailID),
new XElement("XuatKhoID", detail.XuatKhoID),
new XElement("DuocID", detail.DuocID),
new XElement("GiaVAT", detail.GiaVAT),
new XElement("ThanhTienVAT", detail.ThanhTienVAT),
new XElement("SoLo", detail.SoLo),
new XElement("GiaXuat", detail.GiaXuat),
new XElement("ThanhTienXuat", detail.ThanhTienXuat),
new XElement("TyLeGia", detail.TyLeGia),
new XElement("SoLuong", detail.SoLuong),
new XElement("StoreBalanceID", detail.StoreBalanceID)
));
                }

                #endregion
                return UpdateModel("[XuatKho_Update]",
                    new string[] { "@XuatKhoID", "@DepartmentsID_KhoX", "@LoaiPhieuXuat", "@DepartmentsID_NoiNhan", "@patientsID", "@NguoiNhan", "@NoiDungXuat", "@NgayXuat", "@NgayCT", "@SoCT", "@userIDUpdated", "@XuatKhoChiTiet" },
                    new object[] { entity.XuatKhoID, entity.DepartmentsID_KhoX, entity.LoaiPhieuXuat, entity.DepartmentsID_NoiNhan, entity.patientsID, entity.NguoiNhan, entity.NoiDungXuat, entity.NgayXuat, entity.NgayCT, entity.SoCT, entity.userIDUpdated, xml?.ToString() },
                    userId, checkPermission);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected override string GetFeatureCode()
        {
            return "XUATKHO";
        }

        protected override string GetNameEntity()
        {
            return "xuất kho";
        }
    }
}
