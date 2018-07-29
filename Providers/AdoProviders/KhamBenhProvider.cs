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
    public class KhamBenhProvider : SqlEntityBase<KhamBenh, int>, IProvider<KhamBenh>
    {
        public CoreResult Delete(int key, int? userId = default(int?), bool checkPermission = false)
        {
            return DeleteMultilple("[KhamBenh_Delete]", new string[] { "@KhamBenhID", "@userID" }, new object[] { key, userId }, userId, checkPermission);
        }

        public CoreResult Delete(KhamBenh entity, int? userId = default(int?), bool checkPermission = false)
        {
            return Delete(entity.KhamBenhID, userId, checkPermission);
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

        public KhamBenh Get(int key)
        {
            try
            {
                return this.GetSingleByDapper("[KhamBenh_GetSingle]", new string[] { "@KhamBenhID" }, new object[] { key }, CommandType.StoredProcedure);
            }
            catch
            {
                this.sqlHelper.Close();
                return null;
            }
        }

        public CoreResult Insert(KhamBenh entity, int? userId = default(int?), bool checkPermission = false)
        {
            return AddModel("[KhamBenh_Insert]", new string[] {"@patientsID","@NgayKham","@NgayKetThucKham","@ChanDoanBD","@BenhChinh_MaICD","@BenhKemTheo_MaICDs","@DepartmentsID","@employeesID","@PhuongAn","@GuiKham_DepartmentsIDs","@LoaiKham","@ChuyenKhoa_Code","@userIDCreated"},
                new object[] {
                    entity.patientsID, entity.NgayKham, entity.NgayKetThucKham,
                    entity.ChanDoanBD, entity.BenhChinh_MaICD, entity.BenhKemTheo_MaICDs,
                    entity.DepartmentsID, entity.employeesID, entity.PhuongAn, entity.GuiKham_DepartmentsIDs,
                    entity.LoaiKham, entity.ChuyenKhoa_Code, entity.userIDCreated },
                userId,checkPermission);
        }

        public CoreResult Update(KhamBenh entity, int? userId = default(int?), bool checkPermission = false)
        {
            return UpdateModel("[KhamBenh_Update]", new string[] { "@KhamBenhID", "@patientsID", "@NgayKham", "@NgayKetThucKham", "@ChanDoanBD", "@BenhChinh_MaICD", "@BenhKemTheo_MaICDs", "@DepartmentsID", "@employeesID", "@PhuongAn", "@GuiKham_DepartmentsIDs", "@LoaiKham", "@ChuyenKhoa_Code", "@userIDUpdated" },
                new object[] {
                    entity.KhamBenhID,entity.patientsID,entity.NgayKham,entity.NgayKetThucKham,
                    entity.ChanDoanBD,entity.BenhChinh_MaICD,entity.BenhKemTheo_MaICDs,
                    entity.DepartmentsID,entity.employeesID,entity.PhuongAn,entity.GuiKham_DepartmentsIDs,
                    entity.LoaiKham,entity.ChuyenKhoa_Code,entity.userIDUpdated
                }, userId, checkPermission);
        }

        protected override string GetFeatureCode()
        {
            throw new NotImplementedException();
        }

        protected override string GetNameEntity()
        {
            throw new NotImplementedException();
        }
    }
}
