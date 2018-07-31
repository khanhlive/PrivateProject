using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Providers;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Entities
{
    public partial class Asset : IEntity<Asset>
    {
        public CoreResult Delete(int? userId = null, bool checkPermission = false)
        {
            CheckValidEntity();
            using (TaiSanProvider provider = new TaiSanProvider())
            {
                return provider.Delete(AssetsID, userId, checkPermission);
            }
        }

        public CoreResult Exist()
        {
            CheckValidEntity();
            using (TaiSanProvider provider = new TaiSanProvider())
            {
                return provider.Exist(AssetsID);
            }
        }

        public CoreResult Get()
        {
            CheckValidEntity();
            using (TaiSanProvider provider = new TaiSanProvider())
            {
                var entity = provider.Get(AssetsID);
                if (entity != null)
                {
                    #region Set Value
                    AssetsCateID = entity.AssetsCateID;
                    CompanyID = entity.CompanyID;
                    DepartmentsID = entity.DepartmentsID;
                    NgayNhap = entity.NgayNhap;
                    TinhTrangNhap = entity.TinhTrangNhap;
                    SoNamDaSD = entity.SoNamDaSD;
                    NamSanXuat = entity.NamSanXuat;
                    DonGiaNhap = entity.DonGiaNhap;
                    deleted = entity.deleted;
                    userIDCreated = entity.userIDCreated;
                    dateCreated = entity.dateCreated;
                    dateUpdated = entity.dateUpdated;
                    userIDUpdated = entity.userIDUpdated;
                    NumberUpdated = entity.NumberUpdated;
                    #endregion
                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = provider.GetMessageByCoreStatusCode(CoreStatusCode.OK, ActionType.Get) };
                }
                else
                    return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = provider.GetMessageByCoreStatusCode(CoreStatusCode.Failed, ActionType.Get) };
            }
        }

        public CoreResult Insert(int? userId = null, bool checkPermission = false)
        {
            using (TaiSanProvider provider = new TaiSanProvider())
            {
                return provider.Insert(this, userId, checkPermission);
            }
        }

        public CoreResult Update(int? userId = null, bool checkPermission = false)
        {
            using (TaiSanProvider provider = new TaiSanProvider())
            {
                return provider.Update(this, userId, checkPermission);
            }
        }

        private void CheckValidEntity()
        {
            if (AssetsID <= 0)
            {
                throw new Exception("\"AssetsID\" chưa được thiết lập giá trị.");
            }
        }
    }
}
