using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moss.Hospital.Data.Common.Enum;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace Moss.Hospital.Data.Entities
{
    public partial class RaVien : Providers.Repositories.SqlEntityBase<RaVien, int>, Providers.Repositories.ISqlAction<RaVien>
    {
        #region Properties
        public ChuyenVien ThongtinChuyenvien { get; set; }

        #endregion

        #region Method
        protected override string GetFeatureCode()
        {
            return "RAVIEN";
        }

        protected override string GetNameEntity()
        {
            return "ra viện";
        }

        public CoreResult Delete(int? userId = default(int?), bool checkPermission = false)
        {
            return this.Delete(this.patientsID, userId, checkPermission);
        }
        protected CoreResult GetCustomMessage(SqlDataReader dataReader, ActionType _actionType)
        {
            try
            {
                object _status = null;
                while (dataReader.Read())
                {
                    _status = dataReader["StatusCode"];
                    break;
                }
                if (_status != null)
                {
                    switch (_status.ToString())
                    {
                        case "001":
                            return new CoreResult { StatusCode = CoreStatusCode.Failed, SqlStatusCode = string.Format("{0}", _status), Message = string.Format("Bệnh nhân đã thanh toán, không thể {0} ra viện.", _actionType == ActionType.Insert ? "thêm mới" : _actionType == ActionType.Edit ? "cập nhật" : _actionType == ActionType.Delete ? "xóa" : "") };
                        case "003":
                            return new CoreResult { StatusCode = CoreStatusCode.Failed, SqlStatusCode = string.Format("{0}", _status), Message = "Khoa phòng đã có khám bệnh, không thể thêm mới" };
                        case "004":
                            return new CoreResult { StatusCode = CoreStatusCode.Failed, SqlStatusCode = string.Format("{0}", _status), Message = "Không phải khám bệnh cuối cùng, không thể cập nhật" };
                        case "005":
                            return new CoreResult { StatusCode = CoreStatusCode.Used, SqlStatusCode = string.Format("{0}", _status), Message = "Khám bệnh đã được chỉ định dịch vụ hoặc thuốc, không thể cập nhật." };
                        default:
                            return this.GetResultFromSqlDataReader(dataReader, _actionType);
                    }
                }
                else
                    return null;
            }
            catch// (Exception e)
            {
                return null;
            }
        }

        public CoreResult Delete(int key, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                #region Kiểm tra quyền thao tác
                var result = this.CheckPermission(userId, checkPermission, ActionType.Delete);
                #endregion
                if (result.Item1)
                {
                    var _kiemtrathanhtoan = new ThanhToanProvider().KiemTraThanhToan(key);
                    if (!_kiemtrathanhtoan)
                    {
                        this.CreateConnection();
                        this.sqlHelper.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = this.sqlHelper.ExecuteReader("[RaVien_Delete]", new string[] { "@patientsID" }, new object[] { key });
                        CoreResult returnValue = this.GetCustomMessage(dr, ActionType.Delete);
                        if (returnValue != null)
                        {
                            return returnValue;
                        }
                        else
                            return this.GetResultFromSqlDataReader(dr, ActionType.Delete);
                    }
                    else
                    {
                        //bệnh nhân đã thanh toán
                        return new CoreResult
                        {
                            StatusCode = CoreStatusCode.Failed,
                            Message = "Bệnh nhân đã thanh toán, không được sửa ra viện."
                        };
                    }
                }
                else
                {
                    return result.Item2;
                }
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                return this.GetResultFromStatusCode(CoreStatusCode.Exception, ActionType.Delete, e);
            }
        }

        public CoreResult Delete(RaVien entity, int? userId = default(int?), bool checkPermission = false)
        {
            return this.Delete(entity.patientsID, userId, checkPermission);
        }

        public CoreResult Exist(int key)
        {
            throw new NotImplementedException();
        }

        public CoreResult Exist()
        {
            throw new NotImplementedException();
        }

        public RaVien Get(int key)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(this.sqlHelper.ConnectionString))
                {
                    dbConnection.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@patientsID", key);
                    using (var data = dbConnection.QueryMultiple("[RaVien_GetSingle]", parameters, null, null, CommandType.StoredProcedure))
                    {
                        RaVien _ravien = data.Read<RaVien>().FirstOrDefault();
                        if (_ravien != null)
                        {
                            _ravien.ThongtinChuyenvien = data.Read<ChuyenVien>().FirstOrDefault();
                        }
                        return _ravien;
                    }
                }
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
                RaVien entity = this.Get(this.patientsID);
                if (entity != null)
                {
                    #region Assign value
                    //thong tin ra vien
                    //this.patientsID = entity.patientsID;
                    this.NgayRaVien = entity.NgayRaVien;
                    this.SoNgayDT = entity.SoNgayDT;
                    this.KetQuaDT = entity.KetQuaDT;
                    this.HinhThucRV = entity.HinhThucRV;
                    this.SoRaVien = entity.SoRaVien;
                    this.SoLuuTru = entity.SoLuuTru;
                    this.MaYTe = entity.MaYTe;
                    this.DepartmentID_TK = entity.DepartmentID_TK;
                    this.PhuongPhapDT = entity.PhuongPhapDT;
                    this.TinhTrangRaVien = entity.TinhTrangRaVien;
                    this.DienBienLamSang = entity.DienBienLamSang;
                    this.KetQuaCanLamSang = entity.KetQuaCanLamSang;
                    this.ChuyenKhoa_Code = entity.ChuyenKhoa_Code;
                    this.LoiDan = entity.LoiDan;
                    this.GhiChu = entity.GhiChu;
                    this.userIDCreated = entity.userIDCreated;
                    this.dateCreated = entity.dateCreated;
                    this.dateUpdated = entity.dateUpdated;
                    this.userIDUpdated = entity.userIDUpdated;
                    this.NumberUpdated = entity.NumberUpdated;

                    if (entity.ThongtinChuyenvien != null)
                    {
                        this.ThongtinChuyenvien.patientsID = entity.ThongtinChuyenvien.patientsID;
                        this.ThongtinChuyenvien.MaBenhVien_CV = entity.ThongtinChuyenvien.MaBenhVien_CV;
                        this.ThongtinChuyenvien.TinhTrangCV = entity.ThongtinChuyenvien.TinhTrangCV;
                        this.ThongtinChuyenvien.LyDoCV = entity.ThongtinChuyenvien.LyDoCV;
                        this.ThongtinChuyenvien.HinhThucCV = entity.ThongtinChuyenvien.HinhThucCV;
                        this.ThongtinChuyenvien.HanCV = entity.ThongtinChuyenvien.HanCV;
                        this.ThongtinChuyenvien.ChanDoanCV = entity.ThongtinChuyenvien.ChanDoanCV;
                        this.ThongtinChuyenvien.NguoiChuyen = entity.ThongtinChuyenvien.NguoiChuyen;
                        this.ThongtinChuyenvien.KetQua = entity.ThongtinChuyenvien.KetQua;
                    }
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
            return this.Insert(this, userId, checkPermission);
        }

        public CoreResult Insert(RaVien entity, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                #region Kiểm tra quyền thao tác
                var result = this.CheckPermission(userId, checkPermission, ActionType.Insert);
                #endregion
                if (result.Item1)
                {
                    //kiểm tra Id bộ phận có phải là ID bộ phận trong khám bệnh cuối cùng không
                    if (entity.DepartmentID_TK == new KhamBenh().GetIDBoPhan_TrongKhamBenhCuoiCung(entity.patientsID))
                    {
                        //kiểm tra ngày ra viện
                        var _kiemtraravien = this.KiemtraNgayRavien(entity.patientsID, entity.NgayRaVien);
                        if (_kiemtraravien.Item1)
                        {
                            //kiểm tra dịch vụ cận lâm sàng chưa thực hiện
                            var _kiemtracls = new DichVuChiDinh().KiemtraCanLamSangChuaThucHien(entity.patientsID);
                            if (!_kiemtracls)
                            {
                                //kiểm tra thuốc đã kê đơn nhưng chưa lên phiếu lĩnh
                                var _kiemtrathuoc = new ThuocChiDinh().KiemTraThuocChuaLenPhieuLinh(entity.patientsID);
                                if (!_kiemtrathuoc)
                                {
                                    #region Thêm mới ra viện
                                    this.CreateConnection();
                                    this.sqlHelper.CommandType = CommandType.StoredProcedure;
                                    List<string> paramsName = new List<string> {
                    "@patientsID","@NgayRaVien","@SoNgayDT","@KetQuaDT","@HinhThucRV","@SoRaVien",
                        "@SoLuuTru","@MaYTe","@DepartmentID_TK","@PhuongPhapDT","@TinhTrangRaVien",
                        "@DienBienLamSang","@KetQuaCanLamSang","@ChuyenKhoa_Code","@LoiDan",
                        "@GhiChu","@userIDCreated"
                };
                                    List<object> paramsValue = new List<object> {
                        entity.patientsID,entity.NgayRaVien,entity.SoNgayDT,entity.KetQuaDT,
                    entity.HinhThucRV,entity.SoRaVien,entity.SoLuuTru,entity.MaYTe,entity.DepartmentID_TK,
                    entity.PhuongPhapDT,entity.TinhTrangRaVien,entity.DienBienLamSang,entity.KetQuaCanLamSang,
                    entity.ChuyenKhoa_Code,entity.LoiDan,entity.GhiChu,entity.userIDCreated
                    };
                                    if (entity.HinhThucRV == (byte)Library.Constants.HinhThucRaVien.ChuyenVien)
                                    {
                                        //chuyen vien
                                        paramsName.AddRange(new List<string> { "@MaBenhVien_CV", "@TinhTrangCV", "@LyDoCV", "@HinhThucCV", "@HanCV", "@ChanDoanCV", "@NguoiChuyen", "@KetQua" });
                                        paramsValue.AddRange(new List<object> { entity.ThongtinChuyenvien.patientsID,
                            entity.ThongtinChuyenvien.MaBenhVien_CV, entity.ThongtinChuyenvien.TinhTrangCV,
                            entity.ThongtinChuyenvien.LyDoCV, entity.ThongtinChuyenvien.HinhThucCV, entity.ThongtinChuyenvien.HanCV,
                            entity.ThongtinChuyenvien.ChanDoanCV, entity.ThongtinChuyenvien.NguoiChuyen, entity.ThongtinChuyenvien.KetQua });
                                    }
                                    SqlDataReader dr = this.sqlHelper.ExecuteReader("[RaVien_Insert]", paramsName.ToArray(), paramsValue.ToArray());
                                    CoreResult returnValue = this.GetCustomMessage(dr, ActionType.Insert);
                                    if (returnValue != null)
                                    {
                                        return returnValue;
                                    }
                                    else
                                        return this.GetResultFromSqlDataReader(dr, ActionType.Insert);
                                    #endregion
                                }
                                else
                                {
                                    return new CoreResult
                                    {
                                        //có thuốc đã kê đơn nhưng chưa lên phiếu lĩnh
                                        StatusCode = CoreStatusCode.Failed,
                                        Message = "Bệnh nhân có thuốc đã kê đơn nhưng chưa lên phiếu lĩnh"
                                    };
                                }
                            }
                            else
                            {
                                //có dịch vụ cận lâm sàng đã chỉ định nhưng chưa thực hiện
                                return new CoreResult
                                {
                                    StatusCode = CoreStatusCode.Failed,
                                    Message = "Bệnh nhân có dịch vụ cận lâm sàng nhưng chưa thực hiện"
                                };
                            }
                        }
                        else
                        {
                            return new CoreResult
                            {
                                StatusCode = CoreStatusCode.Failed,
                                Message = _kiemtraravien.Item2
                            };
                        }
                    }
                    else
                    {
                        //không phải ID bộ phận cuối cùng
                        var bophan = new Department().GetByID(entity.DepartmentID_TK);
                        string tenbophan = bophan == null ? string.Empty : bophan.DepartmentsName;
                        return new CoreResult
                        {
                            StatusCode = CoreStatusCode.Failed,
                            Message = string.Format("\"{0}\" không phải là bộ phận khám bệnh cuối cùng, không được cho thêm ra viện cho bệnh nhân này.", tenbophan)
                        };
                    }
                }
                else
                {
                    return result.Item2;
                }
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

        public CoreResult Update(RaVien entity, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                #region Kiểm tra quyền thao tác
                var result = this.CheckPermission(userId, checkPermission, ActionType.Edit);
                #endregion
                if (result.Item1)
                {

                    //kiểm tra Id bộ phận có phải là ID bộ phận trong khám bệnh cuối cùng không
                    if (entity.DepartmentID_TK == new KhamBenh().GetIDBoPhan_TrongKhamBenhCuoiCung(entity.patientsID))
                    {
                        //kiểm tra ngày ra viện
                        var _kiemtrangayravien = this.KiemtraNgayRavien(entity.patientsID, entity.NgayRaVien);
                        if (_kiemtrangayravien.Item1)
                        {
                            //kiểm tra thanh toán(đã thanh toán, không cho cập nhật ra viện)
                            var _kiemtrathanhtoan = new ThanhToanProvider().KiemTraThanhToan(entity.patientsID);
                            if (!_kiemtrathanhtoan)
                            {
                                #region Cập nhật ra viện
                                this.CreateConnection();
                                this.sqlHelper.CommandType = CommandType.StoredProcedure;
                                List<string> paramsName = new List<string> {
                    "@patientsID","@NgayRaVien","@SoNgayDT","@KetQuaDT","@HinhThucRV","@SoRaVien",
                        "@SoLuuTru","@MaYTe","@DepartmentID_TK","@PhuongPhapDT","@TinhTrangRaVien",
                        "@DienBienLamSang","@KetQuaCanLamSang","@ChuyenKhoa_Code","@LoiDan",
                        "@GhiChu","@userIDUpdated"
                };
                                List<object> paramsValue = new List<object> {
                        entity.patientsID,entity.NgayRaVien,entity.SoNgayDT,entity.KetQuaDT,
                    entity.HinhThucRV,entity.SoRaVien,entity.SoLuuTru,entity.MaYTe,entity.DepartmentID_TK,
                    entity.PhuongPhapDT,entity.TinhTrangRaVien,entity.DienBienLamSang,entity.KetQuaCanLamSang,
                    entity.ChuyenKhoa_Code,entity.LoiDan,entity.GhiChu,entity.userIDUpdated
                    };
                                if (entity.HinhThucRV == (byte)Library.Constants.HinhThucRaVien.ChuyenVien)
                                {
                                    //chuyen vien
                                    paramsName.AddRange(new List<string> { "@MaBenhVien_CV", "@TinhTrangCV", "@LyDoCV", "@HinhThucCV", "@HanCV", "@ChanDoanCV", "@NguoiChuyen", "@KetQua" });
                                    paramsValue.AddRange(new List<object> { entity.ThongtinChuyenvien.patientsID,
                            entity.ThongtinChuyenvien.MaBenhVien_CV, entity.ThongtinChuyenvien.TinhTrangCV,
                            entity.ThongtinChuyenvien.LyDoCV, entity.ThongtinChuyenvien.HinhThucCV, entity.ThongtinChuyenvien.HanCV,
                            entity.ThongtinChuyenvien.ChanDoanCV, entity.ThongtinChuyenvien.NguoiChuyen, entity.ThongtinChuyenvien.KetQua });
                                }
                                SqlDataReader dr = this.sqlHelper.ExecuteReader("[RaVien_Update]", paramsName.ToArray(), paramsValue.ToArray());
                                CoreResult returnValue = this.GetCustomMessage(dr, ActionType.Edit);
                                if (returnValue != null)
                                {
                                    return returnValue;
                                }
                                else
                                    return this.GetResultFromSqlDataReader(dr, ActionType.Edit);
                                #endregion
                            }
                            else
                            {
                                //bệnh nhân đã thanh toán
                                return new CoreResult
                                {
                                    StatusCode = CoreStatusCode.Failed,
                                    Message = "Bệnh nhân đã thanh toán, không được sửa ra viện."
                                };
                            }
                        }
                        else
                        {
                            return new CoreResult
                            {
                                StatusCode = CoreStatusCode.Failed,
                                Message = _kiemtrangayravien.Item2
                            };
                        }
                    }
                    else
                    {
                        //không phải ID bộ phận cuối cùng
                        var bophan = new Department().GetByID(entity.DepartmentID_TK);
                        string tenbophan = bophan == null ? string.Empty : bophan.DepartmentsName;
                        return new CoreResult
                        {
                            StatusCode = CoreStatusCode.Failed,
                            Message = string.Format("\"{0}\" không phải là bộ phận khám bệnh cuối cùng, không được cho thêm ra viện cho bệnh nhân này.", tenbophan)
                        };
                    }
                }
                else
                {
                    return result.Item2;
                }

            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                return this.GetResultFromStatusCode(CoreStatusCode.Exception, ActionType.Edit, e);
            }
        }

        public Tuple<bool, string> KiemtraNgayRavien(int mabenhnhan, DateTime ngayravien)
        {
            DateTime _now = DateTime.Now;
            if (ngayravien <= _now)
            {
                try
                {
                    this.CreateConnection();
                    this.sqlHelper.CommandType = CommandType.Text;
                    object obj = this.sqlHelper.ExecuteScalar("SELECT [dbo].[GetNgayYLenh](@mabenhnhan) AS 'NgayYLenh'", new string[] { "@mabenhnhan" }, new object[] { mabenhnhan }, null);
                    if (obj == null)
                    {
                        return Tuple.Create(true, string.Empty);
                    }
                    else
                    {
                        DateTime ngayylenh = DateTime.Now;
                        if (DateTime.TryParse(obj.ToString(), out ngayylenh))
                        {
                            if (ngayravien >= ngayylenh)
                            {
                                return Tuple.Create(true, string.Empty);
                            }
                            else
                                return Tuple.Create(false, string.Format("Ngày ra viện phải lớn hơn ngày y lệnh cuối cùng: \"{0}\"", ngayylenh.ToString("dd/MM/yyyy HH:mm:ss")));
                        }
                        else
                            throw new Exception(string.Format("Ngày y lệnh: \"{0}\" không đúng định dạng Datetime.", obj));
                    }
                }
                catch (Exception e)
                {
                    this.sqlHelper.Close();
                    throw e;
                }
            }
            else
            {
                //ngày ra viện lớn hơn ngày hiện tại: false
                return Tuple.Create(false, string.Format("Ngày ra viện phải nhỏ hơn ngày hiện tại."));
            }
        }


        #endregion
    }
}
