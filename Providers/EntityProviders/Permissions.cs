using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Dao.Enum;
using Moss.Hospital.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Moss.Hospital.Data.Providers.Repositories;

namespace Moss.Hospital.Data.Entities
{
    public partial class Permission : MossHospitalRepository<Permission, int>, ISqlAction<Permission, int>
    {


        /// <summary>
        /// Hàm khởi tạo đối tượng
        /// </summary>
        public Permission()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "PHANQUYEN";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "phân quyền";
        }

        /// <summary>
        /// Hàm lấy dữ liệu cache nếu đối tượng thuộc danh sách đối tượng cache
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<Permission> CacheData()
        {
            return GlobalCache.Permissions;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.Permission);
        }

        /// <summary>
        /// Lấy ra 1 đối tượng bộ phận theo mã bộ phận
        /// </summary>
        /// <param name="id">Mã bộ phận</param>
        /// <returns>Thành công: trả về 1 bộ phận có mã là ID, Lỗi: trả về giá trị "NULL"</returns>
        public override Permission GetByID(int id)
        {
         return this.context.Set<Permission>().FirstOrDefault(p => p.PermissionsID == id);
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public override Permission Find(Expression<Func<Permission, bool>> match)
        {
            return base.Find(match);
        }

        public IEnumerable<PermisionFeature> Get(int userId)
        {
            var q = (from a in context.Features.Where(p => p.RedirectURL != null && p.RedirectURL != "")
                     join b in this.context.Permissions.Where(p => p.userID == userId) on a.FeatureID equals b.FeatureID into kq
                     from c in kq.DefaultIfEmpty()
                     select new
                     {
                         Deleted = c == null ? false : c.Deleted,
                         Edit = c == null ? false : c.Edit,
                         FeatureID = a.FeatureID,
                         FetureCode = a.FeatureCode,
                         FetureName = a.FeatureName,
                         New = c == null ? false : c.New,
                         PermissionsID = c == null ? 0 : c.PermissionsID,
                         Prints = c == null ? false : c.Prints,
                         userID = c == null ? 0 : c.userID,
                         Views = c == null ? false : c.Views,
                     });
            var do1 =(from c in q
                    select new PermisionFeature
                    {
                        Deleted = c == null ? false : c.Deleted,
                        Edit = c == null ? false : c.Edit,
                        FeatureID = c == null ? 0 : c.FeatureID,
                        FeatureCode = c.FetureCode,
                        FeatureName = c.FetureName,
                        New = c == null ? false : c.New,
                        PermissionsID = c == null ? 0 : c.PermissionsID,
                        Prints = c == null ? false : c.Prints,
                        userID = c == null ? 0 : c.userID,
                        Views = c == null ? false : c.Views
                    });
            return do1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public CoreResult Update(int id, Permission item, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                #region Kiểm tra quyền
                var per = this.CheckPermission(userId, checkPermission, ActionType.Edit);
                if (!per.Item1)
                {
                    return per.Item2;
                }
                #endregion

                Permission atach = this.GetByID(id);
                if (atach != null)
                {
                    atach.Views = item.Views;
                    atach.New = item.New;
                    atach.Edit = item.Edit;
                    atach.Deleted = item.Deleted;
                    atach.Prints = item.Prints;
                    this.context.Entry<Permission>(atach).State = EntityState.Modified;
                }
                int counter = this.context.SaveChanges();
                if (this.GetUseCache())
                {
                    if (counter > 0)
                    {
                        this.RefreshCache();
                        return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.OK, ActionType.Edit) };
                    }
                    else
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.Failed, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Failed, ActionType.Edit) };
                    }
                }
                else
                {
                    if (counter > 0)
                    {
                        this.RefreshCache();
                        return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.OK, ActionType.Edit) };
                    }
                    else
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.Failed, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Failed, ActionType.Edit) };
                    }
                }
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CoreResult Update(int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                return this.Update(this.PermissionsID, this);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public CoreResult Insert(Permission item, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                #region Kiểm tra quyền
                var per = this.CheckPermission(userId, checkPermission, ActionType.Insert);
                if (!per.Item1)
                {
                    return per.Item2;
                }
                #endregion
                var result = base.CoreInsert(item);
                if (result.Item1 == CoreStatusCode.OK)
                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                else
                    return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CoreResult Insert(int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                return this.Insert(this);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CoreResult Delete(int id, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                #region Kiểm tra quyền
                var per = this.CheckPermission(userId, checkPermission, ActionType.Delete);
                if (!per.Item1)
                {
                    return per.Item2;
                }
                #endregion
                var result = base.CoreDelete(id);
                if (result == CoreStatusCode.OK)
                    return new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
                return
                    new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CoreResult Delete(int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                return this.Delete(this.PermissionsID);

            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public CoreResult Delete(int[] ids, int? userId = default(int?), bool checkPermission = false)
        {
            #region Kiểm tra quyền
            var per = this.CheckPermission(userId, checkPermission, ActionType.Delete);
            if (!per.Item1)
            {
                return per.Item2;
            }
            #endregion
            CoreResult resultReturn = new CoreResult();
            using (DbContextTransaction dbTransaction = this.context.Database.BeginTransaction())
            {
                foreach (int id in ids)
                {
                    var permission = this.GetByID(id);
                    if (permission != null)
                    {
                        this.context.Entry<Permission>(permission).State = EntityState.Deleted;
                    }
                }
                try
                {
                    this.context.SaveChanges();
                    dbTransaction.Commit();
                    resultReturn.StatusCode = CoreStatusCode.OK; resultReturn.Message = this.GetMessageByCoreStatusCode(CoreStatusCode.OK, ActionType.Delete);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    resultReturn.StatusCode = CoreStatusCode.Exception; resultReturn.Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex);
                }

            }
            if (resultReturn.StatusCode == CoreStatusCode.OK && this.GetUseCache())
            {
                this.RefreshCache();
            }
            return resultReturn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_permissions"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        public CoreResultRange<CoreResult> InsertOrUpdate(IEnumerable<Permission> _permissions, bool useTransaction = true, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                #region Kiểm tra quyền
                var per = this.CheckPermission(userId, checkPermission, ActionType.Edit);
                if (!per.Item1)
                {
                    return new CoreResultRange<CoreResult> { StatusCode=per.Item2.StatusCode, Data=per.Item2.Data, Message= per.Item2.Message };
                }
                #endregion
                IEnumerable<Permission> _updates = _permissions.Where(p => p.PermissionsID > 0);
                IEnumerable<Permission> _inserts = _permissions.Where(p => p.PermissionsID == 0);
                if (useTransaction)
                {
                    using (DbContextTransaction dbTransaction = this.context.Database.BeginTransaction())
                    {
                        //insert
                        foreach (Permission permission in _inserts)
                        {
                            permission.Approved = false;
                            this.context.Permissions.Add(permission);
                        }
                        //update
                        foreach (Permission permission in _updates)
                        {
                            Permission atach = this.GetByID(permission.PermissionsID);
                            if (atach != null)
                            {
                                atach.Views = permission.Views;
                                atach.New = permission.New;
                                atach.Edit = permission.Edit;
                                atach.Deleted = permission.Deleted;
                                atach.Prints = permission.Prints;
                                this.context.Entry<Permission>(atach).State = EntityState.Modified;
                            }
                        }
                        try
                        {
                            this.context.SaveChanges();
                            dbTransaction.Commit();
                            return new CoreResultRange<CoreResult> { StatusCode = CoreStatusCode.OK, Data = _permissions, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.OK, ActionType.Edit) };
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            return new CoreResultRange<CoreResult> { StatusCode = CoreStatusCode.Exception, Data = _permissions, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
                        }
                    }
                }
                else
                {
                    int sum = _permissions.Count();
                    var resultUpdate = this.UpdateRange(_updates);
                    var resultInsert = this.InsertRange(_inserts);
                    int _success = 0;
                    List<CoreResult> _rSuccess = new List<CoreResult>();
                    List<CoreResult> _rError = new List<CoreResult>();
                    if (resultInsert.StatusCode == CoreStatusCode.OK)
                    {
                        if (resultInsert.Success != null)
                            _rSuccess.AddRange(resultInsert.Success.ToList());
                        if (resultInsert.Error != null)
                            _rError.AddRange(resultInsert.Error.ToList());
                    }
                    else if (resultInsert.StatusCode == CoreStatusCode.Failed)
                    {
                        if (resultInsert.Error != null)
                            _rError.AddRange(resultInsert.Error.ToList());
                    }
                    if (resultUpdate.StatusCode == CoreStatusCode.OK)
                    {
                        if (resultUpdate.Success != null)
                            _rSuccess.AddRange(resultUpdate.Success.ToList());
                        if (resultUpdate.Error != null)
                            _rError.AddRange(resultUpdate.Error.ToList());
                    }
                    else if (resultUpdate.StatusCode == CoreStatusCode.Failed)
                    {
                        if (resultUpdate.Error != null)
                            _rError.AddRange(resultUpdate.Error.ToList());
                    }
                    return new CoreResultRange<CoreResult>
                    {
                        StatusCode = _success == 0 ? CoreStatusCode.OK : CoreStatusCode.Failed,
                        Message = string.Format("Cập nhật thành công {0}/{1} mục", _rSuccess.Count, sum),
                        Success = _rSuccess,
                        Error = _rError
                    };
                }
            }
            catch (Exception ex)
            {
                return new CoreResultRange<CoreResult>
                {
                    StatusCode = CoreStatusCode.Exception,
                    Data = _permissions,
                    Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex)
                };
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_permissions"></param>
        /// <returns></returns>
        protected internal virtual CoreResultRange<CoreResult> InsertRange(IEnumerable<Permission> _permissions)
        {
            try
            {
                int _sum = _permissions.Count();
                int _success = 0;
                List<CoreResult> _lSuccess = new List<CoreResult>();
                List<CoreResult> _lError = new List<CoreResult>();
                foreach (Permission permission in _permissions)
                {
                    var obj = this.Insert(permission);
                    if (obj.StatusCode == CoreStatusCode.OK)
                    {
                        _success++;
                        _lSuccess.Add(obj);
                    }
                    else
                    {
                        _lError.Add(obj);
                    }
                }
                if (_success == 0)
                {
                    return new CoreResultRange<CoreResult> { StatusCode = CoreStatusCode.Failed, Message = string.Format("Không thêm mới được mục nào"), Error = _lError };
                }
                else
                {
                    return new CoreResultRange<CoreResult> { StatusCode = CoreStatusCode.OK, Message = string.Format("Thêm mới thành công {0}/{1} mục", _success, _sum), Success = _lSuccess, Error = _lError };
                }
            }
            catch (Exception ex)
            {
                return new CoreResultRange<CoreResult> { StatusCode = CoreStatusCode.Exception, Data = _permissions, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Insert, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_permissions"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        protected virtual CoreResult InsertRangeWithTransaction(IEnumerable<Permission> _permissions, DbContextTransaction dbTransaction = null)
        {
            if (dbTransaction == null)
            {
                using (dbTransaction = this.context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Permission permission in _permissions)
                        {
                            this.context.Permissions.Add(permission);
                        }
                        try
                        {
                            this.context.SaveChanges();
                            dbTransaction.Commit();
                            return new CoreResult { StatusCode = CoreStatusCode.Failed, Data = _permissions, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Failed, ActionType.Insert) };
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = _permissions, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Insert, ex) };
                        }

                    }
                    catch (Exception ex)
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = _permissions, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Insert, ex) };
                    }
                }
            }
            else
            {
                foreach (Permission permission in _permissions)
                {
                    this.context.Permissions.Add(permission);
                }
                return null;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_permissions"></param>
        /// <returns></returns>
        protected virtual CoreResultRange<CoreResult> UpdateRange(IEnumerable<Permission> _permissions)
        {
            try
            {
                int _sum = _permissions.Count();
                int _success = 0;
                List<CoreResult> _lSuccess = new List<CoreResult>();
                List<CoreResult> _lError = new List<CoreResult>();
                foreach (Permission permission in _permissions)
                {
                    var obj = this.Update(permission.PermissionsID, permission);
                    if (obj.StatusCode == CoreStatusCode.OK)
                    {
                        _success++;
                        _lSuccess.Add(obj);
                    }
                    else
                    {
                        _lError.Add(obj);
                    }
                }
                if (_success == 0)
                {
                    return new CoreResultRange<CoreResult> { StatusCode = CoreStatusCode.Failed, Message = string.Format("Không cập nhật được mục nào"), Error = _lError };
                }
                else
                {
                    return new CoreResultRange<CoreResult> { StatusCode = CoreStatusCode.OK, Message = string.Format("Cập nhật thành công {0}/{1} mục", _success, _sum), Success = _lSuccess, Error = _lError };
                }
            }
            catch (Exception ex)
            {
                return new CoreResultRange<CoreResult> { StatusCode = CoreStatusCode.Exception, Data = _permissions, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_permissions"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        protected virtual CoreResult UpdateRangeWithTransaction(IEnumerable<Permission> _permissions, DbContextTransaction dbTransaction = null)
        {
            if (dbTransaction == null)
            {
                using (dbTransaction = this.context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Permission permission in _permissions)
                        {
                            Permission atach = this.GetByID(permission.PermissionsID);
                            if (atach != null)
                            {
                                this.context.Entry<Permission>(atach).CurrentValues.SetValues(permission);
                            }
                        }
                        try
                        {
                            this.context.SaveChanges();
                            dbTransaction.Commit();
                            return new CoreResult { StatusCode = CoreStatusCode.Failed, Data = _permissions, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Failed, ActionType.Insert) };
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = _permissions, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Insert, ex) };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = _permissions, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Insert, ex) };
                    }
                }
            }
            else
            {
                foreach (Permission permission in _permissions)
                {
                    Permission atach = this.GetByID(permission.PermissionsID);
                    if (atach != null)
                    {
                        this.context.Entry<Permission>(atach).CurrentValues.SetValues(permission);
                    }
                }
                return null;
            }
        }

        public CoreResult Exist()
        {
            throw new NotImplementedException();
        }

        public CoreResult Get()
        {
            throw new NotImplementedException();
        }

        public CoreResult Update(Permission entity, int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Delete(Permission entity, int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Exist(int key)
        {
            throw new NotImplementedException();
        }

        Permission IProvider<Permission, int>.Get(int key)
        {
            throw new NotImplementedException();
        }
    }

    public class PermisionFeature : Permission
    {
        #region Public Properties
        public string FeatureCode { get; set; }
        public string FeatureName { get; set; }
        private string _featureid_permissionid;
        public string FeatureId_PermissionId { get { return string.Format("{0}|{1}", this.FeatureID, this.PermissionsID); } set { this._featureid_permissionid = value; } }
        #endregion
    }
}
