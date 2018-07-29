using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Entities;
using Moss.Hospital.Data.Paramesters;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Moss.Hospital.Data.Providers
{
    public class BenhNhanProvider : SqlEntityBase<patient, int>, IProvider<patient>
    {
        public CoreResult Delete(int key, int? userId = default(int?), bool checkPermission = false)
        {
            return DeleteMultilple("[Patients_Delete]", new string[] { "@patientsID" }, new object[] { key }, userId, checkPermission);
        }

        public CoreResult Delete(patient entity, int? userId = default(int?), bool checkPermission = false)
        {
            return Delete(entity.patientsID, userId, checkPermission);
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

        public patient Get(int key)
        {
            try
            {
                return this.GetSingleByDapper("[Patients_GetSingle]", new string[] { "@patientsID" }, new object[] { key }, CommandType.StoredProcedure);
            }
            catch
            {
                this.sqlHelper.Close();
                return null;
            }
        }

        private string GetAddress(string sonhathonxom, int IDxa, int IDhuyen, int IDtinh)
        {
            try
            {
                this.CreateConnection();
                this.sqlHelper.CommandType = CommandType.Text;
                return this.sqlHelper.ExecuteScalar("SELECT [dbo].[Patient_GetAddress](@idxa, @idhuyen, @idtinh);", new string[] { "@idxa", "@idhuyen", "@idtinh" }, new object[] { IDxa, IDhuyen, IDtinh }, string.Empty);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public CoreResult Insert(patient entity, int? userId = default(int?), bool checkPermission = false)
        {
            entity.address = GetAddress(entity.SoNhaThoPho, entity.XaPhuongID ?? 0, entity.QuanHuyenID ?? 0, entity.TinhThanhID ?? 0);
            return AddModel("[Patients_Insert]", new string[] {
                    "@patientsID","@patientsCode","@patientsName","@gender","@dayOfBirth","@mothOfBirth","@yearOfBirth","@age","@address",
                    "@PhoneNumber","@registrationDate","@partientsObjectID","@cardNumber","@expirationDateFrom","@expirationDateTo","@emergency","@prioritize",
                    "@MaBenhVien_DKKCB","@MaBenhVien_KCB","@MaBenhVien_GioiThieu","@ChanDoanNoiGT","@BHTuyen","@BHNoiNgoaiTinh","@BHMaKhuVuc","@BHNgayHanMuc",
                    "@BHMucHuong","@LoaiBenhAn","@TrieuChung","@CMThu_HChieu","@peopleID","@DepartmentsID_Khoa","@DepartmentsID_PhongBuong","@patientsStatus"
                    ,"@registrationNumber","@TinhThanhID","@QuanHuyenID","@XaPhuongID","@NguoiThan_Ten","@NguoiThan_SoDT","@QuocTichID","@NgheNgiepID",
                    "@CanNang","@OutTime","@partientsPicture","@deleted","@userIDCreated","@dateCreated","@dateUpdated","@userIDUpdated","@NumberUpdated"
                }, new object[] { entity.patientsID, entity.patientsCode, entity.patientsName, entity.gender, entity.dayOfBirth, entity.mothOfBirth, entity.yearOfBirth,
                    entity.age, entity.address, entity.PhoneNumber, entity.registrationDate, entity.partientsObjectID, entity.cardNumber, entity.expirationDateFrom, entity.expirationDateTo,
                    entity.emergency, entity.prioritize, entity.MaBenhVien_DKKCB, entity.MaBenhVien_KCB, entity.MaBenhVien_GioiThieu, entity.ChanDoanNoiGT, entity.BHTuyen, entity.BHNoiNgoaiTinh,
                    entity.BHMaKhuVuc, entity.BHNgayHanMuc, entity.BHMucHuong, entity.LoaiBenhAn, entity.TrieuChung, entity.CMThu_HChieu, entity.peopleID, entity.DepartmentsID_Khoa,
                    entity.DepartmentsID_PhongBuong, entity.patientsStatus, entity.registrationNumber, entity.TinhThanhID, entity.QuanHuyenID, entity.XaPhuongID,
                    entity.NguoiThan_Ten, entity.NguoiThan_SoDT, entity.QuocTichID, entity.NgheNgiepID, entity.CanNang,
                    entity.OutTime, entity.partientsPicture, entity.deleted, entity.userIDCreated, entity.dateCreated, entity.dateUpdated, entity.userIDUpdated, entity.NumberUpdated },
                userId, checkPermission);
        }

        public CoreResult Update(patient entity, int? userId = default(int?), bool checkPermission = false)
        {
            entity.address = GetAddress(entity.SoNhaThoPho, entity.XaPhuongID ?? 0, entity.QuanHuyenID ?? 0, entity.TinhThanhID ?? 0);
            return UpdateModel("[Patients_Insert]", new string[] {
                    "@patientsID","@patientsCode","@patientsName","@gender","@dayOfBirth","@mothOfBirth","@yearOfBirth","@age","@address",
                    "@PhoneNumber","@registrationDate","@partientsObjectID","@cardNumber","@expirationDateFrom","@expirationDateTo","@emergency","@prioritize",
                    "@MaBenhVien_DKKCB","@MaBenhVien_KCB","@MaBenhVien_GioiThieu","@ChanDoanNoiGT","@BHTuyen","@BHNoiNgoaiTinh","@BHMaKhuVuc","@BHNgayHanMuc",
                    "@BHMucHuong","@LoaiBenhAn","@TrieuChung","@CMThu_HChieu","@peopleID","@DepartmentsID_Khoa","@DepartmentsID_PhongBuong","@patientsStatus"
                    ,"@registrationNumber","@TinhThanhID","@QuanHuyenID","@XaPhuongID","@NguoiThan_Ten","@NguoiThan_SoDT","@QuocTichID","@NgheNgiepID",
                    "@CanNang","@OutTime","@partientsPicture","@deleted","@userIDCreated","@dateCreated","@dateUpdated","@userIDUpdated","@NumberUpdated"
                },
                new object[] { entity.patientsID, entity.patientsCode, entity.patientsName, entity.gender, entity.dayOfBirth, entity.mothOfBirth, entity.yearOfBirth,
                    entity.age, entity.address, entity.PhoneNumber, entity.registrationDate, entity.partientsObjectID, entity.cardNumber, entity.expirationDateFrom, entity.expirationDateTo,
                    entity.emergency, entity.prioritize, entity.MaBenhVien_DKKCB, entity.MaBenhVien_KCB, entity.MaBenhVien_GioiThieu, entity.ChanDoanNoiGT, entity.BHTuyen, entity.BHNoiNgoaiTinh,
                    entity.BHMaKhuVuc, entity.BHNgayHanMuc, entity.BHMucHuong, entity.LoaiBenhAn, entity.TrieuChung, entity.CMThu_HChieu, entity.peopleID, entity.DepartmentsID_Khoa,
                    entity.DepartmentsID_PhongBuong, entity.patientsStatus, entity.registrationNumber, entity.TinhThanhID, entity.QuanHuyenID, entity.XaPhuongID,
                    entity.NguoiThan_Ten, entity.NguoiThan_SoDT, entity.QuocTichID, entity.NgheNgiepID, entity.CanNang,
                    entity.OutTime, entity.partientsPicture, entity.deleted, entity.userIDCreated, entity.dateCreated, entity.dateUpdated, entity.userIDUpdated, entity.NumberUpdated },
                userId, checkPermission);
        }

        public string GetNextCode(string prefix, int? id = null)
        {
            try
            {
                this.CreateConnection();
                this.sqlHelper.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = this.sqlHelper.ExecuteReader("[Patients_GetCode]", new string[] { "@Prefix", "@ID" }, new object[] { prefix, id });
                string _code = string.Empty;
                while (dr.Read())
                {
                    _code = dr["NextPatientCode"].ToString();
                }
                return _code;
            }
            catch
            {
                this.sqlHelper.Close();
                return string.Empty;
            }
        }

        public IEnumerable<patient> GetFilter(Patient_ParameterSearch parameters)
        {
            try
            {
                return this.GetListByDapper("[Patients_GetFilter]", new string[] {"@NgayTu", "@NgayDen", "@DoituongBenhnhan", "@DanhsachKhoaphong", "@LoaiBenhAn", "@TrangthaiBenhNhan","@Uutien", "@Capcuu", "@MaBenhVienKCB", "@BuongGiuong", "@NgoaigioHanhchinh" },
                    new object[] {
                        parameters.NgayTu,parameters.NgayDen,
                        parameters.DoituongBenhnhan ==null?null:string.Format(",{0},",string.Join(",", parameters.DoituongBenhnhan)),
                        parameters.Bophan==null?null:string.Format(",{0},",string.Join(",", parameters.Bophan)),
                        parameters.LoaiBenhAn==null?null:string.Format(",{0},",string.Join(",",parameters.LoaiBenhAn)),
                        parameters.TrangthaiBenhnhan==null?null:string.Format(",{0},",string.Join(",",parameters.TrangthaiBenhnhan)),
                        parameters.Uutien==null?null:string.Format(",{0},",string.Join(",",parameters.Uutien)), parameters.CapCuu,
                        parameters.MaBenhVienKCB,
                        parameters.BuongGiuong==null?null:string.Format(",{0},",string.Join(",",parameters.BuongGiuong)),
                        parameters.NgoaigioHanhchinh },
                    CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                throw e;
            }
        }

        protected override string GetFeatureCode()
        {
            return "BENHNHAN";
        }

        protected override string GetNameEntity()
        {
            return "bệnh nhân";
        }
    }
}
