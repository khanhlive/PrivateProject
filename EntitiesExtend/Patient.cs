using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Paramesters;
using Moss.Hospital.Data.Providers.Repositories;
using Moss.Hospital.Data.Providers;

namespace Moss.Hospital.Data.Entities
{
    public partial class patient : IEntity<patient>
    {
        public CoreResult Delete(int? userId = default(int?), bool checkPermission = false)
        {
            return this.Delete(this.patientsID, userId, checkPermission);
        }

        public CoreResult Delete(int key, int? userId = default(int?), bool checkPermission = false)
        {
            using (BenhNhanProvider provider = new BenhNhanProvider())
            {
                return provider.Delete(key, userId, checkPermission);
            }
        }

        public CoreResult Exist(int key)
        {
            using (BenhNhanProvider provider = new BenhNhanProvider())
            {
                return provider.Exist(key);
            }
        }

        public CoreResult Exist()
        {
            return Exist(patientsID);
        }

        public patient Get(int key)
        {
            using (BenhNhanProvider provider = new BenhNhanProvider())
            {
                return provider.Get(key);
            }
        }

        public CoreResult Get()
        {
            try
            {
                using (BenhNhanProvider provider = new BenhNhanProvider())
                {
                    patient entity = provider.Get(this.patientsID);
                    if (entity != null)
                    {
                        #region Assign value

                        this.patientsCode = entity.patientsCode;
                        this.patientsName = entity.patientsName;
                        this.gender = entity.gender;
                        this.dayOfBirth = entity.dayOfBirth;
                        this.mothOfBirth = entity.mothOfBirth;
                        this.yearOfBirth = entity.yearOfBirth;
                        this.age = entity.age;
                        this.address = entity.address;
                        this.PhoneNumber = entity.PhoneNumber;
                        this.registrationDate = entity.registrationDate;
                        this.partientsObjectID = entity.partientsObjectID;
                        this.cardNumber = entity.cardNumber;
                        this.expirationDateFrom = entity.expirationDateFrom;
                        this.expirationDateTo = entity.expirationDateTo;
                        this.emergency = entity.emergency;
                        this.prioritize = entity.prioritize;
                        this.MaBenhVien_DKKCB = entity.MaBenhVien_DKKCB;
                        this.MaBenhVien_KCB = entity.MaBenhVien_KCB;
                        this.MaBenhVien_GioiThieu = entity.MaBenhVien_GioiThieu;
                        this.ChanDoanNoiGT = entity.ChanDoanNoiGT;
                        this.BHTuyen = entity.BHTuyen;
                        this.BHNoiNgoaiTinh = entity.BHNoiNgoaiTinh;
                        this.BHMaKhuVuc = entity.BHMaKhuVuc;
                        this.BHNgayHanMuc = entity.BHNgayHanMuc;
                        //this.BHLuongCoSo = entity.BHLuongCoSo;
                        this.BHMucHuong = entity.BHMucHuong;
                        this.LoaiBenhAn = entity.LoaiBenhAn;
                        this.TrieuChung = entity.TrieuChung;
                        this.CMThu_HChieu = entity.CMThu_HChieu;
                        this.peopleID = entity.peopleID;
                        this.DepartmentsID_Khoa = entity.DepartmentsID_Khoa;
                        this.DepartmentsID_PhongBuong = entity.DepartmentsID_PhongBuong;
                        this.patientsStatus = entity.patientsStatus;
                        //this.DichVuChiDinhID = entity.DichVuChiDinhID;
                        this.registrationNumber = entity.registrationNumber;
                        this.TinhThanhID = entity.TinhThanhID;
                        this.QuanHuyenID = entity.QuanHuyenID;
                        this.XaPhuongID = entity.XaPhuongID;
                        this.NguoiThan_Ten = entity.NguoiThan_Ten;
                        this.NguoiThan_SoDT = entity.NguoiThan_SoDT;
                        this.QuocTichID = entity.QuocTichID;
                        this.NgheNgiepID = entity.NgheNgiepID;
                        this.CanNang = entity.CanNang;
                        this.OutTime = entity.OutTime;
                        this.partientsPicture = entity.partientsPicture;
                        this.deleted = entity.deleted;
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

        public string GetNextCode(string prefix, int? id = null)
        {
            using (BenhNhanProvider provider = new BenhNhanProvider())
            {
                return provider.GetNextCode(prefix, id);
            }
        }

        public IEnumerable<patient> GetFilter(Patient_ParameterSearch parameters)
        {
            using (BenhNhanProvider provider = new BenhNhanProvider())
            {
                return provider.GetFilter(parameters);
            }
        }

        public CoreResult Insert(int? userId = default(int?), bool checkPermission = false)
        {
            using (BenhNhanProvider provider = new BenhNhanProvider())
            {
                return provider.Insert(this, userId, checkPermission);
            }
        }
        
        public CoreResult Update(int? userId = default(int?), bool checkPermission = false)
        {
            using (BenhNhanProvider provider = new BenhNhanProvider())
            {
                return provider.Update(this, userId, checkPermission);
            }
            
        }
    }
}
