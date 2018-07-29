using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moss.Hospital.Data.Common.Enum;

namespace Moss.Hospital.Data.Entities
{

    public partial class KhamBenh : Providers.Repositories.SqlEntityBase<KhamBenh, int>, Providers.Repositories.ISqlAction<KhamBenh>
    {
        public CoreResult Delete(int? userId = default(int?), bool checkPermission = false)
        {
            return this.Delete(this.KhamBenhID,userId);
        }

        public CoreResult Delete(int key, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                this.CreateConnection();
                this.sqlHelper.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = this.sqlHelper.ExecuteReader("[KhamBenh_Delete]", new string[] { "@KhamBenhID", "@userID" }, new object[] { key, userId });
                CoreResult returnValue = this.GetCustomMessage(dr, ActionType.Delete);
                if (returnValue != null)
                {
                    return returnValue;
                }
                else
                    return this.GetResultFromSqlDataReader(dr, ActionType.Delete);
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                //log.Error("GetAll", e);
                return this.GetResultFromStatusCode(CoreStatusCode.Exception, ActionType.Delete, e);
            }
        }

        public CoreResult Delete(KhamBenh entity, int? userId = default(int?), bool checkPermission = false)
        {
            return this.Delete(entity.KhamBenhID, userId);
        }

        public CoreResult Exist(int key)
        {
            throw new NotImplementedException();
        }

        public CoreResult Exist()
        {
            throw new NotImplementedException();
        }

        public KhamBenh Get(int key)
        {
            try
            {
                //this.CreateConnection();
                //this.sqlHelper.CommandType = System.Data.CommandType.StoredProcedure;
                return this.GetSingleByDapper("[KhamBenh_GetSingle]", new string[] { "@KhamBenhID" }, new object[] { key }, System.Data.CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                throw e;
            }
        }

        public CoreResult Get()
        {
            try
            {
                //this.CreateConnection();
                //this.sqlHelper.CommandType = System.Data.CommandType.StoredProcedure;
                KhamBenh entity = this.GetSingleByDapper("[KhamBenh_GetSingle]", new string[] { "@KhamBenhID" }, new object[] { this.KhamBenhID }, System.Data.CommandType.StoredProcedure);
                if (entity != null)
                {
                    #region Assign value

                    this.patientsID = entity.patientsID;
                    this.NgayKham = entity.NgayKham;
                    this.NgayKetThucKham = entity.NgayKetThucKham;
                    this.ChanDoanBD = entity.ChanDoanBD;
                    this.BenhChinh_MaICD = entity.BenhChinh_MaICD;
                    this.BenhKemTheo_MaICDs = entity.BenhKemTheo_MaICDs;
                    this.DepartmentsID = entity.DepartmentsID;
                    this.employeesID = entity.employeesID;
                    this.PhuongAn = entity.PhuongAn;
                    this.GuiKham_DepartmentsIDs = entity.GuiKham_DepartmentsIDs;
                    this.LoaiKham = entity.LoaiKham;
                    this.ChuyenKhoa_Code = entity.ChuyenKhoa_Code;
                    this.deleted = entity.deleted;
                    this.userIDCreated = entity.userIDCreated;
                    this.dateCreated = entity.dateCreated;
                    this.dateUpdated = entity.dateUpdated;
                    this.userIDUpdated = entity.userIDUpdated;

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
            return this.Insert(this);
        }

        public CoreResult Insert(KhamBenh entity, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                this.CreateConnection();
                this.sqlHelper.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = this.sqlHelper.ExecuteReader("[KhamBenh_Insert]", new string[] {
                    "@patientsID","@NgayKham","@NgayKetThucKham","@ChanDoanBD",
                    "@BenhChinh_MaICD","@BenhKemTheo_MaICDs","@DepartmentsID","@employeesID",
                    "@PhuongAn","@GuiKham_DepartmentsIDs","@LoaiKham","@ChuyenKhoa_Code",
                    "@userIDCreated"
                }, new object[] {
                    entity.patientsID, entity.NgayKham, entity.NgayKetThucKham,
                    entity.ChanDoanBD, entity.BenhChinh_MaICD, entity.BenhKemTheo_MaICDs,
                    entity.DepartmentsID, entity.employeesID, entity.PhuongAn, entity.GuiKham_DepartmentsIDs,
                    entity.LoaiKham, entity.ChuyenKhoa_Code, entity.userIDCreated });
                CoreResult returnValue = this.GetCustomMessage(dr, ActionType.Insert);
                if (returnValue != null)
                {
                    return returnValue;
                }
                else
                    return this.GetResultFromSqlDataReader(dr, ActionType.Insert);
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                return this.GetResultFromStatusCode(CoreStatusCode.Exception, ActionType.Insert, e);
            }
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
                            return new CoreResult { StatusCode = CoreStatusCode.Failed, SqlStatusCode = string.Format("{0}",_status), Message = string.Format("Bệnh nhân đã ra viện, không thể {0} khám bệnh.", _actionType == ActionType.Insert ? "thêm mới" : _actionType == ActionType.Edit ? "cập nhật" : _actionType == ActionType.Delete ? "xóa" : "") };
                        case "003":
                            return new CoreResult { StatusCode = CoreStatusCode.Failed, SqlStatusCode = string.Format("{0}", _status), Message = "Khoa phòng đã có khám bệnh, không thể thêm mới" };
                        case "004":
                            return new CoreResult { StatusCode = CoreStatusCode.Failed, SqlStatusCode = string.Format("{0}", _status), Message = "Không phải khám bệnh cuối cùng, không thể cập nhật" };
                        case "005":
                            return new CoreResult { StatusCode = CoreStatusCode.Used, SqlStatusCode = string.Format("{0}", _status), Message = "Khám bệnh đã được chỉ định dịch vụ hoặc thuốc, không thể cập nhật." };
                        default:
                            return null;
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

        public CoreResult Update(int? userId = default(int?), bool checkPermission = false)
        {
            return this.Update(this);
        }

        public CoreResult Update(KhamBenh entity, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                this.CreateConnection();
                this.sqlHelper.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = this.sqlHelper.ExecuteReader("[KhamBenh_Update]", new string[] {
                    "@KhamBenhID","@patientsID","@NgayKham","@NgayKetThucKham",
                    "@ChanDoanBD","@BenhChinh_MaICD","@BenhKemTheo_MaICDs",
                    "@DepartmentsID","@employeesID","@PhuongAn",
                    "@GuiKham_DepartmentsIDs","@LoaiKham","@ChuyenKhoa_Code",
                    "@userIDUpdated"
                }, new object[] {
                    entity.KhamBenhID,entity.patientsID,entity.NgayKham,entity.NgayKetThucKham,
                    entity.ChanDoanBD,entity.BenhChinh_MaICD,entity.BenhKemTheo_MaICDs,
                    entity.DepartmentsID,entity.employeesID,entity.PhuongAn,entity.GuiKham_DepartmentsIDs,
                    entity.LoaiKham,entity.ChuyenKhoa_Code
                    ,entity.userIDUpdated });
                CoreResult returnValue = this.GetCustomMessage(dr, ActionType.Edit);
                if (returnValue != null)
                {
                    return returnValue;
                }
                else
                    return this.GetResultFromSqlDataReader(dr, ActionType.Edit);
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                return this.GetResultFromStatusCode(CoreStatusCode.Exception, ActionType.Edit, e);
            }
        }

        public IEnumerable<KhamBenh> GetByMaBenhnhan(int mabenhnhan)
        {
            try
            {
                //this.CreateConnection();
                //this.sqlHelper.CommandType = System.Data.CommandType.StoredProcedure;
                return this.GetListByDapper("[KhamBenh_GetByPatientId]", new string[] { "@PatientId" }, new object[] { mabenhnhan }, System.Data.CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                throw e;
            }
        }
        
        protected override string GetFeatureCode()
        {
            return "KHAMBENH";
        }

        protected override string GetNameEntity()
        {
            return "khám bệnh";
        }

        public int GetIDBoPhan_TrongKhamBenhCuoiCung(int mabenhnhan)
        {
            try
            {
                this.sqlHelper.CommandType = System.Data.CommandType.Text;
                int obj = this.sqlHelper.ExecuteScalar("SELECT [dbo].[KhamBenh_GetBoPhanID_CuoiCung](@mabenhnhan)", new string[] { "@mabenhnhan" }, new object[] { mabenhnhan }, 0);
                return obj;
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                throw e;
            }

        }
    }
}
