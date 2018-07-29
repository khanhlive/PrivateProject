using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moss.Hospital.Data.Dao.Enum;
using System.Data;
using Dapper;
using System.Xml.Linq;

namespace Moss.Hospital.Data.Entities
{
    public partial class NhapKho : Providers.Repositories.SqlEntityBase<NhapKho, int>, Providers.Repositories.ISqlAction<NhapKho>
    {
        public NhapKho()
        {
            this.NhapKhoDetails = new List<NhapKhoDetail>();
        }
        #region Extend Properties
        public List<NhapKhoDetail> NhapKhoDetails { get; set; }

        #endregion

        #region public Method


        public CoreResult Delete(int? userId = default(int?), bool checkPermission = false)
        {
            return this.Delete(this.NhapKhoID, userId, checkPermission);
        }

        public CoreResult Delete(int key, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                this.CreateConnection();
                this.sqlHelper.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = this.sqlHelper.ExecuteReader("[NhapKho_Delete]", new string[] { "@NhapKhoID" }, new object[] { key });
                return this.GetResultFromSqlDataReader(dr, ActionType.Delete);
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                return this.GetResultFromStatusCode(CoreStatusCode.Exception, ActionType.Delete, e);
            }
        }
        public CoreResult DeleteChiTiet(int[] keys,int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                XElement xml = null;
                #region Create XMl string
                if (keys != null && keys.Length > 0)
                {
                    xml = new XElement("items", from key in keys select new XElement("item", key));
                    this.CreateConnection();
                    this.sqlHelper.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = this.sqlHelper.ExecuteReader("[NhapKho_DeleteChiTiet]", new string[] { "@NhapKhoChiTietIDs", "@UserID" }, new object[] { xml?.ToString(), userId });
                    return this.GetResultFromSqlDataReader(dr, ActionType.Delete);
                }
                else
                {
                    return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = "Keys không được NULL" };
                }
                #endregion
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                return this.GetResultFromStatusCode(CoreStatusCode.Exception, ActionType.Delete, e);
            }
        }
        public NhapKho GetWithNhapKhoDetails(int nhapkhoId)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(this.sqlHelper.ConnectionString))
                {
                    dbConnection.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@NhapKhoID", nhapkhoId);
                    parameters.Add("@IsGetDetail", true);
                    using (var data = dbConnection.QueryMultiple("[NhapKho_GetSingle]", parameters, null, null, CommandType.StoredProcedure))
                    {
                        NhapKho _nhapkho = data.Read<NhapKho>().FirstOrDefault();
                        if (_nhapkho != null)
                        {
                            _nhapkho.NhapKhoDetails = data.Read<NhapKhoDetail>().ToList();
                        }
                        return _nhapkho;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IEnumerable<NhapKho> GetByFilter(DateTime? ngaytu=null, DateTime? ngayden=null, int? departmentID_Nhap=null)
        {
            try
            {
                return this.GetListByDapper("[NhapKho_GetFilter]", new string[] { "@NgayTu", "@NgayDen", "@MaBoPhanNhap" }, new object[] { ngaytu, ngayden, departmentID_Nhap }, CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public CoreResult Delete(NhapKho entity,int? userId = default(int?), bool checkPermission = false)
        {
            return this.Delete(entity.NhapKhoID, userId,checkPermission);
        }

        public CoreResult Exist(int key)
        {
            throw new NotImplementedException();
        }

        public CoreResult Exist()
        {
            throw new NotImplementedException();
        }

        public NhapKho Get(int key)
        {
            try
            {
                return this.GetSingleByDapper("[NhapKho_GetSingle]", new string[] { "@NhapKhoID", "@IsGetDetail" }, new object[] { key, false }, CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult Get()
        {
            try
            {
                NhapKho entity = this.GetSingleByDapper("[NhapKho_GetSingle]", new string[] { "@NhapKhoID", "@IsGetDetail" }, new object[] { this.NhapKhoID, false }, CommandType.StoredProcedure);
                if (entity != null)
                {
                    #region Assign value
                    this.NhapKhoID = entity.NhapKhoID;
                    this.LoaiPhieuNhap = entity.LoaiPhieuNhap;
                    this.DepartmentsID_KhoN = entity.DepartmentsID_KhoN;
                    this.DepartmentsID_KhoXT = entity.DepartmentsID_KhoXT;
                    this.CompanyID = entity.CompanyID;
                    this.XuatKhoID = entity.XuatKhoID;
                    this.SoPhieuLinh = entity.SoPhieuLinh;
                    this.NgayNhap = entity.NgayNhap;
                    this.NgayCT = entity.NgayCT;
                    this.SoCT = entity.SoCT;
                    this.NguoiGiao = entity.NguoiGiao;
                    this.NoiDung = entity.NoiDung;
                    this.ThanhToan_status = entity.ThanhToan_status;
                    this.ThanhToan_Date = entity.ThanhToan_Date;
                    this.Duyet_status = entity.Duyet_status;
                    this.Duyet_employeesID = entity.Duyet_employeesID;
                    this.Duyet_Date = entity.Duyet_Date;
                    this.deleted = entity.deleted;
                    this.userIDCreated = entity.userIDCreated;
                    this.dateCreated = entity.dateCreated;
                    this.dateUpdated = entity.dateUpdated;
                    this.userIDUpdated = entity.userIDUpdated;
                    this.NumberUpdated = entity.NumberUpdated;

                    #endregion

                    return this.GetResultFromStatusCode(CoreStatusCode.OK, ActionType.Get);
                }
                else
                    return this.GetResultFromStatusCode(CoreStatusCode.Failed, ActionType.Get);
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                return this.GetResultFromStatusCode(CoreStatusCode.Exception, ActionType.Get, e);
            }
        }

        public CoreResult Insert(int? userId = default(int?), bool checkPermission = false)
        {
            return this.Insert(this,userId,checkPermission);
        }

        public CoreResult Insert(NhapKho entity, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                XElement xml = null;

                #region Create XML string

                if (entity.NhapKhoDetails != null && entity.NhapKhoDetails.Count > 0)
                {
                    xml = new XElement("NhapKhoDetails",
                from detail in entity.NhapKhoDetails
                select new XElement("NhapKhoChiTiet",
                new XAttribute("id", detail.NhapKhoDetailsID),
new XElement("NhapKhoDetailsID", detail.NhapKhoDetailsID),
new XElement("NhapKhoID", detail.NhapKhoID),
new XElement("DuocID", detail.DuocID),
new XElement("GiaNhap", detail.GiaNhap),
new XElement("VatNhap", detail.VatNhap),
new XElement("SoLuong", detail.SoLuong),
new XElement("ThanhTienNhap", detail.ThanhTienNhap),
new XElement("GiaVAT", detail.GiaVAT),
new XElement("ThanhTienVAT", detail.ThanhTienVAT),
new XElement("SoLo", detail.SoLo),
new XElement("HanDung", detail.HanDung)
));
                }

                #endregion

                this.CreateConnection();
                this.sqlHelper.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = this.sqlHelper.ExecuteReader("[NhapKho_Insert]", new string[] {
                    "@LoaiPhieuNhap","@DepartmentsID_KhoN","@DepartmentsID_KhoXT","@CompanyID",
                    "@XuatKhoID","@SoPhieuLinh","@NgayNhap","@NgayCT","@SoCT","@NguoiGiao","@NoiDung",
                    "@userIDCreated","@NhapKhoChiTiet"
                }, new object[] {
                    entity.LoaiPhieuNhap,entity.DepartmentsID_KhoN,entity.DepartmentsID_KhoXT,
                    entity.CompanyID,entity.XuatKhoID,entity.SoPhieuLinh,entity.NgayNhap,entity.NgayCT,entity.SoCT,
                    entity.NguoiGiao,entity.NoiDung,entity.userIDCreated,xml?.ToString()
                     });
                return this.GetResultFromSqlDataReader(dr, ActionType.Insert);
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                return this.GetResultFromStatusCode(CoreStatusCode.Exception, ActionType.Insert, e);
            }
        }

        public CoreResult Update(int? userId = default(int?), bool checkPermission = false)
        {
            return this.Update(this, userId, checkPermission);
        }

        public CoreResult Update(NhapKho entity, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                XElement Xml = null;

                #region Create XML string

                if (entity.NhapKhoDetails != null && entity.NhapKhoDetails.Count > 0)
                {
                    Xml = new XElement("NhapKhoDetails",
                from detail in entity.NhapKhoDetails
                select new XElement("NhapKhoChiTiet",
                new XAttribute("id", detail.NhapKhoDetailsID),
new XElement("NhapKhoDetailsID", detail.NhapKhoDetailsID),
new XElement("NhapKhoID", detail.NhapKhoID),
new XElement("DuocID", detail.DuocID),
new XElement("GiaNhap", detail.GiaNhap),
new XElement("VatNhap", detail.VatNhap),
new XElement("SoLuong", detail.SoLuong),
new XElement("ThanhTienNhap", detail.ThanhTienNhap),
new XElement("GiaVAT", detail.GiaVAT),
new XElement("ThanhTienVAT", detail.ThanhTienVAT),
new XElement("SoLo", detail.SoLo),
new XElement("HanDung", detail.HanDung)
));
                }

                #endregion

                this.CreateConnection();
                this.sqlHelper.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = this.sqlHelper.ExecuteReader("[NhapKho_Update]", new string[] {
                    "@NhapKhoID","@LoaiPhieuNhap","@DepartmentsID_KhoN","@DepartmentsID_KhoXT","@CompanyID",
                    "@XuatKhoID","@SoPhieuLinh","@NgayNhap","@NgayCT","@SoCT","@NguoiGiao","@NoiDung",
                    "@userIDUpdated","@NhapKhoChiTiet"
                }, new object[] {
                    entity.NhapKhoID, entity.LoaiPhieuNhap,entity.DepartmentsID_KhoN,entity.DepartmentsID_KhoXT,
                    entity.CompanyID,entity.XuatKhoID,entity.SoPhieuLinh,entity.NgayNhap,entity.NgayCT,entity.SoCT,
                    entity.NguoiGiao,entity.NoiDung,entity.userIDUpdated,Xml?.ToString()
                     });
                return this.GetResultFromSqlDataReader(dr, ActionType.Edit);
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                return this.GetResultFromStatusCode(CoreStatusCode.Exception, ActionType.Edit, e);
            }
        }
        
        protected override string GetFeatureCode()
        {
            return "NHAPKHO";
        }

        protected override string GetNameEntity()
        {
            return "nhập kho";
        }
        #endregion

    }
}
