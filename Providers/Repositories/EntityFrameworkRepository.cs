using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Moss.Hospital.Data.Providers.Repositories
{

    public abstract class EntityFrameworkRepository<T, KeyType> : MossDataLayerBase, IDisposable where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        private bool _useCache;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal virtual IEnumerable<T> CacheData()
        {
            return new List<T>();
        }

        /// <summary>
        /// DbSet
        /// </summary>
        protected DbSet<T> dbSet;

        /// <summary>
        /// DbContext Entities
        /// </summary>
        protected MossHospitalEntities context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_useCache"></param>
        internal virtual void SetUseCache(bool _useCache)
        {
            this._useCache = _useCache;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal virtual bool GetUseCache()
        {
            return this._useCache;
        }

        /// <summary>
        /// Khởi tạo với DbContext
        /// </summary>
        /// <param name="db">DbContext Entities</param>
        public EntityFrameworkRepository(MossHospitalEntities db)
        {
            this.context = db;
            this.dbSet = this.context.Set<T>();
            this.context.Configuration.ProxyCreationEnabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public EntityFrameworkRepository()
        {
        }

        /// <summary>
        /// Đếm số lượng bản ghi
        /// </summary>
        /// <returns>int: trả về số lượng bản ghi có trong bảng</returns>
        internal virtual int CoreCount()
        {
            try
            {
                if (this._useCache)
                {
                    return this.CacheData().Count();
                }
                return this.dbSet.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xóa đối tượng
        /// </summary>
        /// <param name="item">Đối tượng cần xóa</param>
        /// <returns>AccessEntityStatusCode</returns>
        internal virtual CoreStatusCode CoreDelete(T item)
        {
            try
            {
                this.context.Set<T>().Attach(item);
                this.context.Entry<T>(item).State = EntityState.Deleted;
                int counter = this.context.SaveChanges();
                if (_useCache)
                {
                    if (counter > 0)
                    {
                        this.RefreshCache();
                        return CoreStatusCode.OK;
                    }
                    return CoreStatusCode.Failed;
                }
                else
                    return (counter > 0) ? CoreStatusCode.OK : CoreStatusCode.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Xóa bản ghi: thay đổi thuộc tính IsDeleted = true
        /// </summary>
        /// <param name="id"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        internal virtual CoreStatusCode CoreDeleteTemp(KeyType id, bool useTransaction = false)
        {
            try
            {
                var model = this.context.Set<T>().Find(id);
                if (model != null)
                {
                    PropertyInfo propertyDelete = model.GetType().GetProperty("deleted");
                    if (propertyDelete != null)
                    {
                        propertyDelete.SetValue(model, true);
                        this.context.Set<T>().Attach(model);
                        this.context.Entry<T>(model).State = EntityState.Modified;
                        int counter = this.context.SaveChanges();
                        if (_useCache && !useTransaction)
                        {
                            if (counter >= 0)
                            {
                                this.RefreshCache();
                                return CoreStatusCode.OK;
                            }
                            else
                                return CoreStatusCode.Failed;
                        }
                        return counter >= 0 ? CoreStatusCode.OK : CoreStatusCode.Failed;
                    }
                    else
                    {
                        this.context.Set<T>().Attach(model);
                        this.context.Entry<T>(model).State = EntityState.Deleted;
                        int counter = this.context.SaveChanges();
                        if (_useCache && !useTransaction)
                        {
                            if (counter >= 0)
                            {
                                this.RefreshCache();
                                return CoreStatusCode.OK;
                            }
                            else
                                return CoreStatusCode.Failed;
                        }
                        return counter >= 0 ? CoreStatusCode.OK : CoreStatusCode.Failed;
                    }
                }
                else
                {
                    return CoreStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal virtual CoreStatusCode CoreDeleteTemp(T item, bool useTransaction = false)
        {
            try
            {
                if (item != null)
                {
                    PropertyInfo propertyDelete = item.GetType().GetProperty("deleted");
                    if (propertyDelete != null)
                    {
                        propertyDelete.SetValue(item, true);
                        this.context.Set<T>().Attach(item);
                        this.context.Entry<T>(item).State = EntityState.Modified;
                        int counter = this.context.SaveChanges();
                        //this.context.Entry<T>(item).GetDatabaseValues();
                        if (_useCache && !useTransaction)
                        {
                            if (counter >= 0)
                            {
                                this.RefreshCache();
                                return CoreStatusCode.OK;
                            }
                            else
                                return CoreStatusCode.Failed;
                        }
                        return counter >= 0 ? CoreStatusCode.OK : CoreStatusCode.Failed;
                    }
                    else
                    {
                        this.context.Set<T>().Attach(item);
                        this.context.Entry<T>(item).State = EntityState.Deleted;
                        int counter = this.context.SaveChanges();//
                        //this.context.Entry<T>(item).GetDatabaseValues();
                        if (_useCache && !useTransaction)
                        {
                            if (counter >= 0)
                            {
                                this.RefreshCache();
                                return CoreStatusCode.OK;
                            }
                            else
                                return CoreStatusCode.Failed;
                        }
                        return counter >= 0 ? CoreStatusCode.OK : CoreStatusCode.Failed;
                        //throw new Exception(string.Format("{0} not contain property with name is \"{1}\"", item.GetType().Name, "deleted"));
                    }
                }
                else
                {
                    return CoreStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Trả về phần tử đầu tiên phù hợp với 'biểu thức' tìm kiếm
        /// </summary>
        /// <param name="match">Biểu thức tìm kiếm</param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> match)
        {
            try
            {
                if (_useCache)
                {
                    return this.CacheData().FirstOrDefault(match.Compile());
                }
                return this.context.Set<T>().FirstOrDefault(match);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Trả về ICollection phù hợp với 'biểu thức' tìm kiếm
        /// </summary>
        /// <param name="match">Biểu thức tìm kiếm</param>
        /// <returns></returns>
        public virtual ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            try
            {
                if (_useCache)
                {
                    return CacheData().Where(match.Compile()).ToList();
                }
                return this.context.Set<T>().Where(match).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        

        /// <summary>
        /// Tìm kiếm phần tử trong DbSet theo Key Field
        /// </summary>
        /// <param name="id">Key Value</param>
        /// <returns></returns>
        public virtual T GetByID(KeyType id)
        {
            try
            {
                return this.context.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Thêm mới phần tử vào Collections
        /// </summary>
        /// <param name="item">Đối tượng cần thêm</param>
        /// <returns></returns>
        internal virtual Tuple<CoreStatusCode, T> CoreInsert(T item)
        {
            try
            {
                this.context.Set<T>().Add(item);
                int counter = this.context.SaveChanges();
                this.context.Entry(item).GetDatabaseValues();
                if (_useCache)
                {
                    if (counter > 0)
                    {
                        this.RefreshCache();
                        return Tuple.Create(CoreStatusCode.OK, item);
                    }
                    else
                    {
                        return Tuple.Create(CoreStatusCode.Failed, item);
                    }
                }
                return Tuple.Create((counter > 0) ? CoreStatusCode.OK : CoreStatusCode.Failed, item);
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Return DbSet
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> Select()
        {
            return this.dbSet;
        }
        /// <summary>
        /// Cập nhật giá trị cho đối tượng
        /// </summary>
        /// <param name="item">Đối tượng cần cập nhật giá trị</param>
        /// <param name="id">Key Value</param>
        /// <returns></returns>
        internal virtual Tuple<CoreStatusCode, T> CoreUpdate(KeyType id, T item)
        {
            try
            {
                var model = this.GetByID(id);
                if (model != null)
                {
                    this.context.Entry<T>(model).State = EntityState.Detached;
                    this.context.Entry<T>(item).State = EntityState.Modified;
                    int counter = this.context.SaveChanges();
                    this.context.Entry<T>(item).GetDatabaseValues();
                    if (_useCache)
                    {
                        if (counter > 0)
                        {
                            this.RefreshCache();
                            return Tuple.Create(CoreStatusCode.OK, item);
                        }
                        else
                        {
                            return Tuple.Create(CoreStatusCode.Failed, item);
                        }
                    }
                    return Tuple.Create(counter >= 0 ? CoreStatusCode.OK : CoreStatusCode.Failed, item);
                }
                else
                {
                    return Tuple.Create(CoreStatusCode.NotFound, item);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        internal Tuple<CoreStatusCode, T> SaveChange(KeyType id, T item)
        {
            try
            {
                using (var db = new  MossHospitalEntities())
                {
                    var model = db.Set<T>().Find(id);
                    if (model != null)
                    {
                        //db.Entry<T>(model).State = EntityState.Detached;
                        db.Entry<T>(item).State = EntityState.Modified;
                        int counter = db.SaveChanges();
                        db.Entry<T>(item).GetDatabaseValues();
                        if (_useCache)
                        {
                            if (counter > 0)
                            {
                                this.RefreshCache();
                                return Tuple.Create(CoreStatusCode.OK, item);
                            }
                            else
                            {
                                return Tuple.Create(CoreStatusCode.Failed, item);
                            }
                        }
                        return Tuple.Create(counter >= 0 ? CoreStatusCode.OK : CoreStatusCode.Failed, item);
                    }
                    else
                    {
                        return Tuple.Create(CoreStatusCode.NotFound, item);
                    }
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.context.Dispose();
        }
        

        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <returns></returns>
        public virtual ICollection<T> GetAll()
        {
            try
            {
                if (_useCache)
                {
                    return this.CacheData().ToList();
                }
                return this.context.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal CoreStatusCode CoreDelete(KeyType id)
        {
            try
            {
                T item = this.GetByID(id);
                if (item == null)
                {
                    return CoreStatusCode.NotFound;
                }
                else
                {
                    var statusCode = this.CoreDelete(item);
                    return statusCode;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return this.context.Set<T>().Any(predicate);
        }
        internal virtual void RefreshCache()
        {

        }
    }

    public abstract class MossDataLayerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract string GetNameEntity();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract string GetFeatureCode();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="_actionType"></param>
        /// <returns></returns>
        public CoreResult GetPermission(string username, ActionType _actionType)
        {
            return new Permission().GetPermission(username, this.GetFeatureCode(), _actionType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="_actionType"></param>
        /// <returns></returns>
        public CoreResult GetPermission(int userId, ActionType _actionType)
        {
            return new Permission().GetPermission(userId, this.GetFeatureCode(), _actionType);
        }
        /// <summary>
        /// Kiểm tra chuỗi là rỗng, trắng hoặc null
        /// <para>return: true - chuỗi là rỗng, trắng hoặc null; false - chuỗi khác các giá trị rỗng, trắng, null</para>
        /// </summary>
        /// <param name="_string">Chuỗi cần kiểm tra</param>
        /// <returns>return: true - chuỗi là rỗng, trắng hoặc null; false - chuỗi khác các giá trị rỗng, trắng, null</returns>
        internal virtual bool CheckNullEmptyWhiteSpaceString(string _string)
        {
            if (string.IsNullOrEmpty(_string) || string.IsNullOrWhiteSpace(_string))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fetureName"></param>
        /// <param name="_actionType"></param>
        /// <returns></returns>
        private string GetMessagePermission(string fetureName, ActionType _actionType)
        {
            string _nameAction = "";
            switch (_actionType)
            {
                case ActionType.View:
                    _nameAction = "xem";
                    break;
                case ActionType.Insert:
                    _nameAction = "tạo mới";
                    break;
                case ActionType.Edit:
                    _nameAction = "chỉnh sửa";
                    break;
                case ActionType.Delete:
                    _nameAction = "xóa";
                    break;
                case ActionType.Print:
                    _nameAction = "in";
                    break;
                default:
                    break;
            }
            return string.Format("Bạn không có quyền \"{0}\" tại chức năng \"{1}\"", _nameAction, fetureName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fetureCode"></param>
        /// <param name="_actionType"></param>
        /// <returns></returns>
        public CoreResult GetPermission(int userId, string fetureCode, ActionType _actionType)
        {
            try
            {
                using (MossHospitalEntities context = new MossHospitalEntities())
                {

                    var query = (from feture in context.Features
                                 join per in context.Permissions on feture.FeatureID equals per.FeatureID
                                 where feture.FeatureCode == fetureCode && per.userID == userId
                                 select new { per, fetureCode = feture.FeatureCode, featureName = feture.FeatureName }).FirstOrDefault();
                    if (query == null)
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.DontHavePermission, Message = this.GetMessagePermission(fetureCode, _actionType) };
                    }
                    else
                    {
                        switch (_actionType)
                        {
                            case ActionType.View:
                                return new CoreResult { StatusCode = query.per.Views ? CoreStatusCode.OK : CoreStatusCode.DontHavePermission, Message = query.per.Views ? string.Empty : this.GetMessagePermission(query.featureName, _actionType) };
                            case ActionType.Insert:
                                return new CoreResult { StatusCode = query.per.New ? CoreStatusCode.OK : CoreStatusCode.DontHavePermission, Message = query.per.New ? string.Empty : this.GetMessagePermission(query.featureName, _actionType) };
                            case ActionType.Edit:
                                return new CoreResult { StatusCode = query.per.Edit ? CoreStatusCode.OK : CoreStatusCode.DontHavePermission, Message = query.per.Edit ? string.Empty : this.GetMessagePermission(query.featureName, _actionType) };
                            case ActionType.Delete:
                                return new CoreResult { StatusCode = query.per.Deleted ? CoreStatusCode.OK : CoreStatusCode.DontHavePermission, Message = query.per.Deleted ? string.Empty : this.GetMessagePermission(query.featureName, _actionType) };
                            case ActionType.Print:
                                return new CoreResult { StatusCode = query.per.Prints ? CoreStatusCode.OK : CoreStatusCode.DontHavePermission, Message = query.per.Prints ? string.Empty : this.GetMessagePermission(query.featureName, _actionType) };
                            default:
                                return new CoreResult { StatusCode = CoreStatusCode.DontHavePermission, Message = this.GetMessagePermission(query.featureName, _actionType) };
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new CoreResult { StatusCode = CoreStatusCode.DontHavePermission, Message = this.GetMessagePermission(fetureCode, _actionType) }; ;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="fetureName"></param>
        /// <param name="_actionType"></param>
        /// <returns></returns>
        public CoreResult GetPermission(string userName, string fetureName, ActionType _actionType)
        {
            try
            {
                using (MossHospitalEntities context = new MossHospitalEntities())
                {

                    var user = context.Users.FirstOrDefault(p => p.userName == userName);
                    if (user != null)
                    {
                        return this.GetPermission(user.userID, fetureName, _actionType);
                    }
                    else
                    {
                        return new CoreResult { StatusCode = CoreStatusCode.DontHavePermission, Message = this.GetMessagePermission(fetureName, _actionType) };
                    }
                }
            }
            catch (Exception)
            {
                return new CoreResult { StatusCode = CoreStatusCode.DontHavePermission, Message = this.GetMessagePermission(fetureName, _actionType) };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="actionType"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        internal virtual string GetMessageByCoreStatusCode(CoreStatusCode statusCode, ActionType actionType, Exception exception = null)
        {
            switch (statusCode)
            {
                case CoreStatusCode.OK:
                    if (actionType == ActionType.View)
                        return string.Format("Lấy danh sách {0} thành công.", this.GetNameEntity());
                    else if (actionType == ActionType.Insert)
                        return string.Format("Thêm mới {0} thành công.", this.GetNameEntity());
                    else if (actionType == ActionType.Edit)
                        return string.Format("Cập nhật {0} thành công.", this.GetNameEntity());
                    else if (actionType == ActionType.Delete)
                        return string.Format("Xóa {0} thành công.", this.GetNameEntity());
                    else if (actionType == ActionType.Print)
                        return string.Format("In {0} thành công.", this.GetNameEntity());
                    else if (actionType == ActionType.Get)
                        return string.Format("Lấy dữ liệu {0} thành công.", this.GetNameEntity());
                    else return string.Empty;
                case CoreStatusCode.Existed:
                    return string.Format("{0} đã tồn tại trong hệ thống.", GetNameEntity());
                case CoreStatusCode.NotFound:
                    return string.Format("{0} không tồn tại trong hệ thống", GetNameEntity());
                case CoreStatusCode.Failed:
                    if (actionType == ActionType.View)
                        return string.Format("Lấy danh sách {0} không thành công.", this.GetNameEntity());
                    else if (actionType == ActionType.Insert)
                        return string.Format("Thêm mới {0} không thành công.", this.GetNameEntity());
                    else if (actionType == ActionType.Edit)
                        return string.Format("Cập nhật {0} không thành công.", this.GetNameEntity());
                    else if (actionType == ActionType.Delete)
                        return string.Format("Xóa {0} không thành công.", this.GetNameEntity());
                    else if (actionType == ActionType.Print)
                        return string.Format("In {0} không thành công.", this.GetNameEntity());
                    else if (actionType == ActionType.Get)
                        return string.Format("Lấy dữ liệu {0} không thành công.", this.GetNameEntity());
                    else
                        return string.Empty;
                case CoreStatusCode.ModelFailed:
                    return string.Format("Đối tượng {0} không đúng chuẩn dữ liệu.", GetNameEntity());
                case CoreStatusCode.Exception:
                    return this.GetMessageExceptionString(exception);
                case CoreStatusCode.DontHavePermission:
                    return string.Format("Bạn không có quyền để thực hiện thao tác này.", GetNameEntity());
                case CoreStatusCode.Used:
                    return string.Format("Đối tượng \"{0}\" đã được sử dụng, không được phép xóa.", GetNameEntity());
                case CoreStatusCode.SystemError:
                    return "Lỗi hệ thống";
                default:
                    return string.Empty;
            }
        }

        protected ExceptionData GetMessageException(Exception exception)
        {
            if (exception != null)
            {
                string _name = exception.GetType().Name.ToLower();
                switch (_name)
                {
                    case "dbupdateexception":
                        DbUpdateException dbupdateexception = (DbUpdateException)exception;
                        return new ExceptionData { InnerException = dbupdateexception.InnerException.ToString(), Message = dbupdateexception.Message };
                    case "dbentityvalidationexception":
                        DbEntityValidationException dbentityvalidationexception = (DbEntityValidationException)exception;
                        return new ExceptionData { InnerException = dbentityvalidationexception.InnerException?.ToString(), Message = dbentityvalidationexception.Message, ErrorEntities = dbentityvalidationexception.EntityValidationErrors };
                    default:
                        return new ExceptionData { Message = exception.Message, InnerException = exception.InnerException?.ToString() };
                }
            }
            else
                return new ExceptionData { Message = string.Empty, InnerException = null };
        }

        private string GetMessageExceptionString(Exception exception)
        {
            if (exception != null)
            {
                string _name = exception.GetType().Name.ToLower();
                switch (_name)
                {
                    case "dbupdateexception":
                        DbUpdateException dbupdateexception = (DbUpdateException)exception;
                        return dbupdateexception.InnerException.ToString();
                    case "dbentityvalidationexception":
                        DbEntityValidationException dbentityvalidationexception = (DbEntityValidationException)exception;
                        StringBuilder builder = new StringBuilder();
                        foreach (DbEntityValidationResult item in dbentityvalidationexception.EntityValidationErrors)
                        {
                            foreach (DbValidationError item1 in item.ValidationErrors)
                            {
                                builder.Append(string.Format("Property Name: \"{0}\" - Error Message: \"{1}\"", item1.PropertyName, item1.ErrorMessage));
                            }
                            builder.Append("||");
                        }
                        return builder.ToString();
                    default:
                        return exception.Message;
                }
            }
            else
                return "";
        }

        internal Tuple<bool, CoreResult> CheckPermission(int? userId = default(int?), bool checkPermission = false, ActionType _actionType = ActionType.View)
        {
            #region Kiểm tra quyền thao tác
            if (userId == null && checkPermission)
            {
                return Tuple.Create(false, new CoreResult { StatusCode = CoreStatusCode.Failed, Message = "\"UserID\" không được phép để trống." });
            }
            else
            {
                if (checkPermission)
                {
                    var per = this.GetPermission(userId.Value, ActionType.Delete);
                    if (per.StatusCode != CoreStatusCode.OK)
                    {
                        return Tuple.Create(false, per);
                    }
                }
            }
            return Tuple.Create(true, new CoreResult { StatusCode = CoreStatusCode.OK });
            #endregion
        }
    }

    public abstract class MossHospitalRepository<T, KeyType> : EntityFrameworkRepository<T, KeyType> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        public MossHospitalRepository()
        {
            this.context = new MossHospitalEntities();
        }
        
    }
    
}
