using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Entities;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Providers
{
    public sealed class AssetCategoryProvider : EFRepository<AssetsCate, int>, IProvider<AssetsCate>, IEntityDeleteMultiple<int>,IProviderCode<AssetsCate>
    {
        public AssetCategoryProvider() : base()
        {
            SetUseCache(GlobalCacheService.IsLoadCache);
            SetCacheType(CacheType.AssetsCate);
            DeleteTemp = true;
        }

        internal override IEnumerable<AssetsCate> CacheData()
        {
            return GlobalCache.AssetsCates;
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
                        return new CoreResult { StatusCode = CoreStatusCode.NotFound, Data = key, Message = GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Delete) };
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

        public CoreResult Delete(AssetsCate entity, int? userId = null, bool checkPermission = false)
        {
            try
            {
                GetDbContextCRUD();
                return Delete(entity.AssetsCateID, userId, checkPermission);
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

        public AssetsCate Get(int primaryKey)
        {
            try
            {
                if (mossHospitalEntities != null)
                {
                    return mossHospitalEntities.AssetsCates.FirstOrDefault(p => p.AssetsCateID == primaryKey);
                }
                else
                {
                    GetDbContext();
                    return mossHospitalEntities.AssetsCates.FirstOrDefault(p => p.AssetsCateID == primaryKey);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CoreResult Insert(AssetsCate entity, int? userId = null, bool checkPermission = false)
        {
            try
            {
                GetDbContextCRUD();
                var permission = CheckPermission(userId, checkPermission, ActionType.Insert);
                if (permission.Item1)
                {
                    if (!Any(p => p.AssetsCode == entity.AssetsCode))
                    {
                        var result = Add(entity, userId, checkPermission);
                        return result;
                    }
                    else
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = entity, Message = string.Format("Mã tài sản: \"{0}\" đã tồn tại trong hệ thống.",entity.AssetsCode) };
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

        public CoreResult Update(AssetsCate entity, int? userId = null, bool checkPermission = false)
        {
            try
            {
                GetDbContextCRUD();
                var permission = CheckPermission(userId, checkPermission, ActionType.Edit);
                if (permission.Item1)
                {
                    if (Any(p => p.AssetsCateID != entity.AssetsCateID && p.AssetsCode == entity.AssetsCode && p.AssetsCode != null))
                    {
                        var obj = mossHospitalEntities.AssetsCates.Find(entity.AssetsCateID);
                        if (obj != null)
                        {
                            #region Set default value
                            obj.AssetsTypesID = entity.AssetsTypesID;
                            obj.AssetsCode = entity.AssetsCode;
                            obj.AssetsName = entity.AssetsName;
                            obj.SoNamSuDung = entity.SoNamSuDung;
                            obj.TyLeHaoMon = entity.TyLeHaoMon;
                            obj.GiaQuyUoc = entity.GiaQuyUoc;
                            #endregion
                            var result = Edit(entity, userId, checkPermission);
                            return result;
                        }
                        else
                        {
                            return new CoreResult { StatusCode = CoreStatusCode.NotFound, Data = entity, Message = GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Edit) };
                        }
                    }
                    else
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = entity, Message = string.Format("Mã tài sản: \"{0}\" đã tồn tại trong hệ thống.", entity.AssetsCode) };
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

        protected override string GetFeatureCode()
        {
            return "DM_TAISAN";
        }

        protected override string GetNameEntity()
        {
            return "danh mục tài sản";
        }

        public AssetsCate GetByCode(string code)
        {
            try
            {
                if (mossHospitalEntities != null)
                {
                    return mossHospitalEntities.AssetsCates.FirstOrDefault(p => p.AssetsCode == code);
                }
                else
                {
                    GetDbContext();
                    return mossHospitalEntities.AssetsCates.FirstOrDefault(p => p.AssetsCode == code);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<AssetsCate> GetFilterByCode(string code, bool exactly = true, int? assetType = null)
        {
            if (!CheckNullEmptyWhiteSpaceString(code))
            {
                code = code.ToLower();
                return FindAll(p => (exactly ? (p.AssetsCode.ToLower() == code) : (p.AssetsCode.ToLower().Contains(code))) && (assetType == null ? true : p.AssetsTypesID == assetType));
            }
            else
                return FindAll(p => (assetType == null ? true : p.AssetsTypesID == assetType));

        }

        public IEnumerable<AssetsCate> GetFilterByName(string name, bool exactly = true, int? assetType = null)
        {
            if (!CheckNullEmptyWhiteSpaceString(name))
            {
                name = name.ToLower();
                return FindAll(p => (exactly ? (p.AssetsName.ToLower() == name) : (p.AssetsName.ToLower().Contains(name))) && (assetType == null ? true : p.AssetsTypesID == assetType));
            }
            else
                return FindAll(p => (assetType == null ? true : p.AssetsTypesID == assetType));

        }

        public IEnumerable<AssetsCate> GetFilterByAssetTypeCode(string assetTypeCode, bool exactly = true)
        {
            if (!CheckNullEmptyWhiteSpaceString(assetTypeCode))
            {
                assetTypeCode = assetTypeCode.ToLower();
                return (from a in GetAll()
                        join b in mossHospitalEntities.AssetsTypes.Where(p => (exactly ? (p.TypesCode.ToLower() == assetTypeCode) : (p.TypesCode.ToLower().Contains(assetTypeCode)))) on a.AssetsTypesID equals b.AssetsTypesID
                        select a);
            }
            else
                return new List<AssetsCate>();

        }

        public IEnumerable<AssetsCate> GetFilterByText(string text, bool exactly = true, int? assetType = null)
        {
            if (!CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                return FindAll(p => (exactly ? (p.AssetsName.ToLower() == text || p.AssetsCode.ToLower() == text) : (p.AssetsName.ToLower().Contains(text) || p.AssetsCode.ToLower().Contains(text))) && (assetType == null ? true : p.AssetsTypesID == assetType));
            }
            else
                return FindAll(p => (assetType == null ? true : p.AssetsTypesID == assetType));

        }
    }
}
