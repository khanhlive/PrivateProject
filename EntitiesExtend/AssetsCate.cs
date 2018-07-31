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
    public partial class AssetsCate : IEntity<AssetsCate>, IEntityCode<AssetsCate>, IEntityDeleteMultiple<int>
    {
        public CoreResult Update(int? userID = null, bool checkPermission = false)
        {
            try
            {
                using (AssetCategoryProvider provider = new AssetCategoryProvider())
                {
                    return provider.Update(this, userID, checkPermission);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CoreResult Insert(int? userID = null, bool checkPermission = false)
        {
            try
            {
                using (AssetCategoryProvider provider = new AssetCategoryProvider())
                {
                    return provider.Insert(this, userID, checkPermission);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CoreResult Delete(int id, int? userID = null, bool checkPermission = false)
        {
            try
            {
                using (AssetCategoryProvider provider = new AssetCategoryProvider())
                {
                    return provider.Delete(id, userID, checkPermission);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CoreResult Delete(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return Delete(AssetsCateID, userID, checkPermission);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CoreResult Delete(int[] ids, int? userID = null, bool checkPermission = false)
        {
            try
            {
                using (AssetCategoryProvider provider= new AssetCategoryProvider())
                {
                    return provider.Delete(ids, userID, checkPermission);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void GetByCode(string code)
        {
            try
            {
                using (AssetCategoryProvider provider = new AssetCategoryProvider())
                {
                    AssetsCate assetsCate = provider.GetByCode(code);
                    if (assetsCate != null)
                    {
                        #region Set values
                        AssetsCateID = assetsCate.AssetsCateID;
                        AssetsTypesID = assetsCate.AssetsTypesID;
                        AssetsCode = assetsCate.AssetsCode;
                        AssetsName = assetsCate.AssetsName;
                        SoNamSuDung = assetsCate.SoNamSuDung;
                        TyLeHaoMon = assetsCate.TyLeHaoMon;
                        GiaQuyUoc = assetsCate.GiaQuyUoc;
                        #endregion
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult Exist()
        {
            try
            {
                using (AssetCategoryProvider provider = new AssetCategoryProvider())
                {
                    return provider.Exist(AssetsCateID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult Get(int assetCategoryId)
        {
            try
            {
                using (AssetCategoryProvider provider = new AssetCategoryProvider())
                {
                    AssetsCate assetsCate = provider.Get(assetCategoryId);
                    if (assetsCate != null)
                    {
                        #region Set values
                        //AssetsCateID = assetsCate.AssetsCateID;
                        AssetsTypesID = assetsCate.AssetsTypesID;
                        AssetsCode = assetsCate.AssetsCode;
                        AssetsName = assetsCate.AssetsName;
                        SoNamSuDung = assetsCate.SoNamSuDung;
                        TyLeHaoMon = assetsCate.TyLeHaoMon;
                        GiaQuyUoc = assetsCate.GiaQuyUoc;
                        #endregion
                        return new CoreResult { StatusCode = CoreStatusCode.OK, Message = provider.GetMessageByCoreStatusCode(CoreStatusCode.OK, ActionType.Get) };
                    }
                    else
                        return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = provider.GetMessageByCoreStatusCode(CoreStatusCode.Failed, ActionType.Get) };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult Get()
        {
            return Get(AssetsCateID);
        }
    }
}
