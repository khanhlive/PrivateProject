using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Entities;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Providers.Repositories
{
    public abstract class EFRepository<T, KeyType> : MossDataLayerBase where T : class
    {
        #region Private Property

        private bool _useCache;

        private CacheType _cacheType;

        protected Dictionary<string, object> valuesDeletes;

        protected MossHospitalEntities mossHospitalEntities;

        public bool DeleteTemp { get; protected set; }

        #endregion

        #region Constructor

        public EFRepository()
        {
            _cacheType = CacheType.None;
            _useCache = false;
            valuesDeletes = null;
            DeleteTemp = false;
            GetDbContext();
        }
        #endregion

        #region Get, Find

        public virtual T FindBase(Expression<Func<T, bool>> match)
        {
            try
            {
                if (mossHospitalEntities != null)
                {
                    return mossHospitalEntities.Set<T>().AsNoTracking<T>().FirstOrDefault(match);
                }
                else
                {
                    GetDbContext();
                    return mossHospitalEntities.Set<T>().AsNoTracking<T>().FirstOrDefault(match);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual IEnumerable<T> FindAllBase(Expression<Func<T, bool>> match)
        {
            try
            {
                if (_useCache)
                {
                    return CacheData().Where(match.Compile()).ToList();
                }
                else
                {
                    if (mossHospitalEntities != null)
                    {
                        return mossHospitalEntities.Set<T>().AsNoTracking<T>().Where(match).ToList();
                    }
                    else
                    {
                        GetDbContext();
                        return mossHospitalEntities.Set<T>().AsNoTracking<T>().Where(match).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual IEnumerable<T> GetAllBase()
        {
            try
            {
                if (_useCache)
                {
                    return CacheData().ToList();
                }
                else
                {
                    if (mossHospitalEntities != null)
                    {
                        return mossHospitalEntities.Set<T>().AsNoTracking<T>().ToList();
                    }
                    else
                    {
                        GetDbContext();
                        return mossHospitalEntities.Set<T>().AsNoTracking<T>().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual bool AnyBase(Expression<Func<T, bool>> predicate)
        {
            if (mossHospitalEntities != null)
            {
                return mossHospitalEntities.Set<T>().Any(predicate);
            }
            else
            {
                GetDbContext();
                return mossHospitalEntities.Set<T>().Any(predicate);
            }
        }

        public CoreResult Exist(KeyType key)
        {
            try
            {
                var obj = GetT(key);
                return new CoreResult { StatusCode = obj != null ? CoreStatusCode.Existed : CoreStatusCode.NotExisted, Data = obj };
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region CRUD

        internal virtual CoreStatusCode DbSaveChanges()
        {
            if (mossHospitalEntities == null)
            {
                throw new ArgumentNullException(string.Format("\"MossHospitalEntities\" không được phép NULL."));
            }
            int counter = mossHospitalEntities.SaveChanges();
            if (_useCache)
            {
                if (counter > 0)
                {
                    RefreshCache();
                    return CoreStatusCode.OK;
                }
                return CoreStatusCode.Failed;
            }
            else
                return (counter > 0) ? CoreStatusCode.OK : CoreStatusCode.Failed;
        }

        internal virtual CoreResult DeleteBase(KeyType primaryKey, int userID)
        {
            try
            {
                if (mossHospitalEntities == null || !CheckAutoDetectChanges(mossHospitalEntities))
                {
                    throw new ArgumentNullException(string.Format("\"MossHospitalEntities\": NULL hoặc \"AutoDetectChanges\" = \"false\"."));
                }
                else
                {
                    var model = mossHospitalEntities.Set<T>().Find(primaryKey);
                    if (model != null)
                    {
                        if (DeleteTemp)
                        {
                            GetValuesUpdateWhenDelete();
                            valuesDeletes.Add("userIDUpdated", userID);
                            SetValuesDelete(model);
                            mossHospitalEntities.Set<T>().Attach(model);
                            mossHospitalEntities.Entry<T>(model).State = EntityState.Modified;
                            var result = DbSaveChanges();
                            return new CoreResult { StatusCode = result, Message = GetMessageByCoreStatusCode(result, ActionType.Delete), Data = primaryKey };
                        }
                        else
                        {
                            mossHospitalEntities.Set<T>().Attach(model);
                            mossHospitalEntities.Entry<T>(model).State = EntityState.Deleted;
                            var result = DbSaveChanges();
                            return new CoreResult { StatusCode = result, Message = GetMessageByCoreStatusCode(result, ActionType.Delete), Data = primaryKey };
                        }
                    }
                    else
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.NotFound, Message = GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Delete), Data = primaryKey };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private T GetT(KeyType primaryKey)
        {
            try
            {
                if (mossHospitalEntities != null)
                {
                    return mossHospitalEntities.Set<T>().Find(primaryKey);
                }
                else
                {
                    GetDbContext();
                    return mossHospitalEntities.Set<T>().Find(primaryKey);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal virtual CoreResult DeleteMultipleBase(KeyType[] primaryKeys, int userID)
        {
            try
            {
                CheckDbContext(mossHospitalEntities);
                CoreResult resultReturn = new CoreResult();
                using (DbContextTransaction dbTransaction = mossHospitalEntities.Database.BeginTransaction())
                {
                    if (DeleteTemp)
                    {
                        GetValuesUpdateWhenDelete();
                        valuesDeletes.Add("userIDUpdated", userID);
                        foreach (KeyType id in primaryKeys)
                        {
                            var model = GetT(id);
                            if (model != null)
                            {
                                SetValuesDelete(model);
                                mossHospitalEntities.Set<T>().Attach(model);
                                mossHospitalEntities.Entry<T>(model).State = EntityState.Modified;
                            }
                        }
                    }
                    else
                    {
                        foreach (KeyType id in primaryKeys)
                        {
                            var model = GetT(id);
                            if (model != null)
                            {
                                mossHospitalEntities.Entry<T>(model).State = EntityState.Deleted;
                            }
                        }
                    }
                    try
                    {
                        mossHospitalEntities.SaveChanges();
                        dbTransaction.Commit();
                        resultReturn.StatusCode = CoreStatusCode.OK;
                        resultReturn.Message = GetMessageByCoreStatusCode(CoreStatusCode.OK, ActionType.Delete);
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        resultReturn.StatusCode = CoreStatusCode.Exception;
                        resultReturn.Message = GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex);
                        resultReturn.ExceptionError = GetMessageException(ex);
                    }
                }
                if (resultReturn.StatusCode == CoreStatusCode.OK && _useCache)
                {
                    RefreshCache();
                }
                return resultReturn;
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = primaryKeys, Message = GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex), ExceptionError = GetMessageException(ex) };
            }
        }

        internal virtual CoreResult Add(T item, int? userId = null, bool checkPermission = false)
        {
            try
            {
                if (mossHospitalEntities == null || !CheckAutoDetectChanges(mossHospitalEntities))
                {
                    throw new ArgumentNullException(nameof(mossHospitalEntities));
                }
                mossHospitalEntities.Set<T>().Add(item);
                var result = DbSaveChanges();
                return new CoreResult { StatusCode = result, Message = GetMessageByCoreStatusCode(result, ActionType.Insert), Data = item };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal virtual CoreResult Edit(T item, int? userId = null, bool checkPermission = false)
        {
            try
            {
                if (mossHospitalEntities == null || !CheckAutoDetectChanges(mossHospitalEntities))
                {
                    throw new ArgumentNullException(string.Format("\"MossHospitalEntities\": NULL hoặc \"AutoDetectChanges\" = \"false\"."));
                }
                else
                {
                    if (item != null)
                    {
                        //mossHospitalEntities.Entry<T>(item).State = EntityState.Detached;
                        //mossHospitalEntities.Set<T>().Attach(item);
                        //mossHospitalEntities.Entry<T>(item).State = EntityState.Modified;
                        var result = DbSaveChanges();
                        //mossHospitalEntities.Entry(item).GetDatabaseValues();
                        return new CoreResult { StatusCode = result, Data = item, Message = GetMessageByCoreStatusCode(result, ActionType.Edit) };
                    }
                    else
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.NotFound, Data = item, Message = GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Edit) };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Cache

        internal virtual void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(_cacheType);
        }

        internal virtual IEnumerable<T> CacheData()
        {
            return new List<T>();
        }

        internal virtual void SetUseCache(bool useCache)
        {
            _useCache = useCache;
        }

        internal virtual bool GetUseCache()
        {
            return _useCache;
        }

        internal virtual void SetCacheType(CacheType cacheType)
        {
            _cacheType = cacheType;
        }

        internal abstract void SetValueUpdate(T oldValue, T newValue);
        #endregion

        #region Common Method

        private bool CheckAutoDetectChanges(MossHospitalEntities mossHospital)
        {
            return mossHospital == null ? false : mossHospital.Configuration.AutoDetectChangesEnabled;
        }

        private void CheckDbContext(MossHospitalEntities mossHospital)
        {
            if (mossHospital == null || !CheckAutoDetectChanges(mossHospital))
            {
                throw new ArgumentNullException(string.Format("\"MossHospitalEntities\": NULL hoặc \"AutoDetectChanges\" = \"false\"."));
            }
        }

        internal void GetDbContextCRUD()
        {
            if (mossHospitalEntities == null)
            {
                mossHospitalEntities = new MossHospitalEntities();
            }
            mossHospitalEntities.Configuration.AutoDetectChangesEnabled = true;
            mossHospitalEntities.Configuration.ProxyCreationEnabled = true;
            //return mossHospitalEntities;
        }

        internal void GetDbContext()
        {
            if (mossHospitalEntities == null)
            {
                mossHospitalEntities = new MossHospitalEntities();
            }
            mossHospitalEntities.Configuration.AutoDetectChangesEnabled = false;
            mossHospitalEntities.Configuration.ProxyCreationEnabled = false;
            //return mossHospitalEntities;
        }

        internal virtual void GetValuesUpdateWhenDelete()
        {
            if (valuesDeletes == null)
                valuesDeletes = new Dictionary<string, object>();
            valuesDeletes.Add("deleted", true);
            valuesDeletes.Add("dateUpdated", DateTime.Now);
            valuesDeletes.Add("NumberUpdated", 0);
        }

        private void SetValuesDelete(T itemDelete)
        {
            foreach (var propertyKeyValue in valuesDeletes)
            {
                PropertyInfo propertyDelete = itemDelete.GetType().GetProperty(propertyKeyValue.Key);
                if (propertyDelete != null)
                {
                    if (propertyKeyValue.Key.ToLower().Equals("numberupdated"))
                        propertyDelete.SetValue(itemDelete, Convert.ToByte(Convert.ToByte(propertyDelete.GetValue(itemDelete)) + 1));
                    else
                        propertyDelete.SetValue(itemDelete, propertyKeyValue.Value);
                }
            }
        }

        #endregion

    }

}
