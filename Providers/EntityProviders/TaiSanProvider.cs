using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Entities;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Moss.Hospital.Data.Providers
{
    public sealed class TaiSanProvider : EFRepository<Asset, int>, IProvider<Asset>, IEntityDeleteMultiple<int>,IDisposable
    {
        public TaiSanProvider()
        {
            SetUseCache(GlobalCacheService.IsLoadCache);
            SetCacheType(CacheType.Asset);
            DeleteTemp = true;
        }

        internal override IEnumerable<Asset> CacheData()
        {
            return GlobalCache.Assets;
        }

        protected override string GetNameEntity()
        {
            return "tài sản";
        }
        
        protected override string GetFeatureCode()
        {
            return "TAISAN";
        }
       
        public CoreResult Update(Asset entity, int? userId = null, bool checkPermission = false)
        {
            try
            {
                GetDbContextCRUD();
                var permission = CheckPermission(userId, checkPermission, ActionType.Edit);
                if (permission.Item1)
                {
                    
                    var obj = mossHospitalEntities.Assets.Find(entity.AssetsID);
                    if (obj != null)
                    {
                        #region Set default value
                        obj.AssetsCateID = entity.AssetsCateID;
                        obj.CompanyID = entity.CompanyID;
                        obj.DepartmentsID = entity.DepartmentsID;
                        obj.NgayNhap = entity.NgayNhap;
                        obj.TinhTrangNhap = entity.TinhTrangNhap;
                        obj.SoNamDaSD = entity.SoNamDaSD;
                        obj.NamSanXuat = entity.NamSanXuat;
                        obj.DonGiaNhap = entity.DonGiaNhap;
                        obj.dateUpdated = DateTime.Now;
                        obj.userIDUpdated = userId == null ? entity.userIDCreated : userId.Value;
                        obj.NumberUpdated = Convert.ToByte(obj.NumberUpdated + 1);
                        #endregion
                        var result = Edit(entity, userId,checkPermission);
                        return result;
                    }
                    else
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.NotFound, Data = entity, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Edit) };
                    }
                }
                else
                    return permission.Item2;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult Delete(Asset entity, int? userId = null, bool checkPermission = false)
        {
            try
            {
                GetDbContextCRUD();
                return Delete(entity.AssetsID, userId, checkPermission);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public CoreResult Insert(Asset entity, int? userId = null, bool checkPermission = false)
        {
            try
            {
                GetDbContextCRUD();
                var permission = CheckPermission(userId, checkPermission, ActionType.Insert);
                if (permission.Item1)
                {
                    #region Set default value
                    entity.dateCreated = DateTime.Now;
                    entity.dateUpdated = null;
                    entity.deleted = false;
                    entity.NumberUpdated = 0;
                    entity.userIDCreated = userId == null ? entity.userIDCreated : userId.Value;
                    entity.userIDUpdated = 0;
                    #endregion
                    var result = Add(entity, userId, checkPermission);
                    return result;
                }
                else
                    return permission.Item2;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult Delete(int key, int? userId = null, bool checkPermission = false)
        {
            try
            {
                GetDbContextCRUD();
                var permission = CheckPermission(userId, checkPermission, ActionType.Delete);
                if (permission.Item1)
                {
                    var obj = Get(key);
                    if (obj != null)
                    {
                        var result = DeleteBase(key, userId ?? 0);
                        return result;
                    }
                    else
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.NotFound, Data = key, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Delete) };
                    }
                }
                else
                    return permission.Item2;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult Delete(int[] keys, int? userId = null, bool checkPermission = false)
        {
            try
            {
                GetDbContextCRUD();
                var permission = CheckPermission(userId, checkPermission, ActionType.Delete);
                if (permission.Item1)
                {
                    return DeleteMultipleBase(keys, userId ?? 0);
                }
                else
                    return permission.Item2;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal override void SetValueUpdate(Asset oldValue, Asset newValue)
        {
            
        }

        public Asset Get(int primaryKey)
        {
            try
            {
                if (mossHospitalEntities != null)
                {
                    return mossHospitalEntities.Assets.FirstOrDefault(p=>p.AssetsID==primaryKey);
                }
                else
                {
                    GetDbContext();
                    return mossHospitalEntities.Assets.FirstOrDefault(p => p.AssetsID == primaryKey);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
