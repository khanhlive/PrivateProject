using Moss.Hospital.Data.Common;
using Moss.Hospital.Data.Dao;
using Moss.Hospital.Data.Dao.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Entities
{
    public partial class User : Providers.MossHospitalRepository<User, int>
    {
        /// <summary>
        /// Hàm khởi tạo đối tượng
        /// </summary>
        public User()
        {
            this.SetUseCache(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "Tài khoản";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "DM_TAIKHOAN";
        }
        /// <summary>
        /// Hàm lấy dữ liệu cache nếu đối tượng thuộc danh sách đối tượng cache
        /// </summary>
        /// <returns></returns>
        //internal override IEnumerable<User> CacheData()
        //{
        //    return GlobalCache.Users;
        //}
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        //internal override void RefreshCache()
        //{
        //    GlobalCacheService globalcacheservice = new GlobalCacheService();
        //    globalcacheservice.LoadCache(CacheType.User);
        //}
        /// <summary>
        /// Lấy ra 1 đối tượng tài khoản theo mã tài khoản
        /// </summary>
        /// <param name="id">Mã tài khoản</param>
        /// <returns>Thành công: trả về 1 tài khoản có mã là ID, Lỗi: trả về giá trị "NULL"</returns>
        public override User GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.userID == id);
            //}
            return base.GetByID(id);//base.context.User.FirstOrDefault(p => p.tài khoản_ID == id);
        }

        public CoreResult Update(int id, User item, int? userID = null, bool checkPermission = false)
        {
            try
            {
                if (userID == null && checkPermission)
                {
                    return new CoreResult
                    { StatusCode = CoreStatusCode.Failed, Message = "\"UserID\" không được phép để trống." };
                }
                else
                {
                    if (checkPermission)
                    {
                        var per = this.GetPermission(userID.Value, ActionType.Edit);
                        if (per.StatusCode == CoreStatusCode.OK)
                        {
                            ////action
                            var obj = this.context.Users.Find(id);
                            if (obj != null)//this.Any(p => p.userID == id))
                            {
                                obj.userName = item.userName;
                                obj.Account = item.Account;
                                obj.Password = Cipher.Encrypt(Constant.KEY, item.Password.Trim());
                                obj.Avatar = item.Avatar;
                                obj.Status = item.Status;
                                obj.Note = item.Note;
                                var result = context.SaveChanges() >= 0 ? CoreStatusCode.OK : CoreStatusCode.Failed;
                                if (result == CoreStatusCode.OK)
                                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result, ActionType.Edit) };
                                else
                                    return new CoreResult { StatusCode = result, Data = item, Message = this.GetMessageByCoreStatusCode(result, ActionType.Edit) };
                            }
                            else
                            {
                                return new CoreResult { StatusCode = CoreStatusCode.NotFound, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Edit) };
                            }
                        }
                        else return per;
                    }
                    else
                    {
                        ////action
                        var obj = this.context.Users.Find(id);
                        if (obj!=null)//this.Any(p => p.userID == id))
                        {
                            obj.userName = item.userName;
                            obj.Account = item.Account;
                            obj.Password = Cipher.Encrypt(Constant.KEY, item.Password.Trim());
                            obj.Avatar = item.Avatar;
                            obj.Status = item.Status;
                            obj.Note = item.Note;
                            var result = context.SaveChanges()>=0 ? CoreStatusCode.OK: CoreStatusCode.Failed;
                            if (result == CoreStatusCode.OK)
                                return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result, ActionType.Edit) };
                            else
                                return new CoreResult { StatusCode = result, Data = item, Message = this.GetMessageByCoreStatusCode(result, ActionType.Edit) };
                        }
                        else
                        {
                            return new CoreResult { StatusCode = CoreStatusCode.NotFound, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Edit) };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex), ExceptionError = this.GetMessageException(ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Update(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return this.Update(this.userID, this, userID, checkPermission);
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
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Insert(User item, int? userID = null, bool checkPermission = false)
        {
            try
            {
                if (userID == null && checkPermission)
                {
                    return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = "\"UserID\" không được phép để trống." };
                }
                else
                {
                    if (checkPermission)
                    {
                        var per = this.GetPermission(userID.Value, ActionType.Insert);
                        if (per.StatusCode == CoreStatusCode.OK)
                        {
                            ////action
                            if (!this.Any(p => p.Account == item.Account))
                            {
                                #region Set default value
                                item.Status = true;
                                item.deleted = false;
                                item.Password = Cipher.Encrypt(Constant.KEY, item.Password.Trim());
                                #endregion
                                var result = base.CoreInsert(item);
                                if (result.Item1 == CoreStatusCode.OK)
                                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                                else
                                    return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                            }
                            else
                            {
                                return new CoreResult { StatusCode = CoreStatusCode.Existed, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Existed, ActionType.Insert) };
                            }
                        }
                        else return per;
                    }
                    else
                    {
                        ////action
                        if (!this.Any(p => p.Account == item.Account))
                        {
                            #region Set default value
                            item.Status = true;
                            item.deleted = false;
                            item.Password = Cipher.Encrypt(Constant.KEY, item.Password.Trim());
                            #endregion
                            var result = base.CoreInsert(item);
                            if (result.Item1 == CoreStatusCode.OK)
                                return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                            else
                                return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                        }
                        else
                        {
                            return new CoreResult { StatusCode = CoreStatusCode.Existed, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Existed, ActionType.Insert) };
                        }

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
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Insert(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return this.Insert(this, userID, checkPermission);
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
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Delete(int id, int? userID = null, bool checkPermission = false)
        {
            try
            {
                if (userID == null && checkPermission)
                {
                    return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = "\"UserID\" không được phép để trống." };
                }
                else
                {
                    if (checkPermission)
                    {
                        var per = this.GetPermission(userID.Value, ActionType.Delete);
                        if (per.StatusCode == CoreStatusCode.OK)
                        {
                            ////action
                            if (this.Any(p => p.userID == id))
                            {
                                var result = base.CoreDeleteTemp(id);
                                if (result == CoreStatusCode.OK)
                                    return new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
                                return
                                    new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
                            }
                            else
                            {
                                return new CoreResult { StatusCode = CoreStatusCode.NotFound, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Delete) };
                            }
                        }
                        else return per;
                    }
                    else
                    {
                        ////action
                        if (this.Any(p => p.userID == id))
                        {
                            var result = base.CoreDeleteTemp(id);
                            if (result == CoreStatusCode.OK)
                                return new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
                            return
                                new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
                        }
                        else
                        {
                            return new CoreResult { StatusCode = CoreStatusCode.NotFound, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Delete) };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Delete(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return this.Delete(this.userID, userID, checkPermission);
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
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Delete(int[] ids, int? userID = null, bool checkPermission = false)
        {
            try
            {
                bool flag = false;
                if (userID == null && checkPermission)
                {
                    return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = "\"UserID\" không được phép để trống." };
                }
                else
                {
                    if (checkPermission)
                    {
                        var per = this.GetPermission(userID.Value, ActionType.Delete);
                        if (per.StatusCode == CoreStatusCode.OK)
                        {
                            ////action
                            flag = true;
                        }
                        else return per;
                    }
                    else
                    {
                        ////action
                        flag = true;
                    }
                }
                if (flag)
                {
                    CoreResult resultReturn = new CoreResult();
                    using (DbContextTransaction dbTransaction = this.context.Database.BeginTransaction())
                    {
                        foreach (int id in ids)
                        {
                            var user = this.GetByID(id);
                            if (user != null)
                            {
                                user.deleted = true;
                                this.context.Entry<User>(user).State = EntityState.Modified;
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
                else
                {
                    return new CoreResult { StatusCode = CoreStatusCode.DontHavePermission, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.DontHavePermission, ActionType.Delete) };
                }
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }

        }

        private int LoginFailed(int id)
        {
            using (var dbContext = new MossHospitalEntities())
            {
                var obj = dbContext.Users.Find(id);
                if (obj != null)
                {
                    obj.errorNumber = Convert.ToByte(obj.errorNumber + 1);
                    if (obj.errorNumber == 3)
                    {
                        obj.locked = true;
                        dbContext.SaveChanges();
                        return -1;
                    }
                    else
                    {
                        dbContext.SaveChanges();
                        return obj.errorNumber;
                    }
                }
                else return 0;
            }
        }
        private void LoginSuccess(int id)
        {
            using (var db = new MossHospitalEntities())
            {
                var obj = db.Users.Find(id);
                if (obj != null)
                {
                    obj.errorNumber = 0;
                    obj.locked = false;
                    db.SaveChanges();
                }
            }
        }

        public EmployeeUser GetUserDetailByEmployeeID(int employeeId, bool? status = null, bool? deleted = false)
        {
            var query = (from a in context.Employees
                         join b in context.Users on a.employeesID equals b.employeesID into ps
                         from b in ps.DefaultIfEmpty()
                         where (a.employeesID == employeeId)
                         && (status == null ? true : b.Status == status.Value)
                         && (deleted == null ? true : b.deleted == deleted.Value)
                         select new EmployeeUser { Employee = a, User = b }).FirstOrDefault();
            if (query == null)
            {
                return null;
            }
            else
            {
                return query;
            }
        }

        public IEnumerable<EmployeeUser> GetUserDetail(bool? hasAccount=null)
        {
            return (from a in context.Employees
                    join b in context.Users on a.employeesID equals b.employeesID into ps
                    from b in ps.DefaultIfEmpty()
                    where (hasAccount == null ? true : hasAccount.Value ? b != null : b == null)
                    select new EmployeeUser { Employee = a, User = b });
        }

        public CoreResult ChangePassword(int userId, string oldPassword, string newPassword, string newPasswordRetype)
        {
            try
            {
                string _oldPass = Cipher.Encrypt(Constant.KEY, oldPassword.Trim());
                var user = this.Find(p => p.userID == userId && p.Password == oldPassword);
                if (user != null)
                {
                    if (newPassword != newPasswordRetype)
                        return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = "Mật khẩu không khớp!" };
                    else
                    {
                        string _newPass = Cipher.Encrypt(Constant.KEY, newPassword.Trim());
                        user.Password = _newPass;
                        var re = user.Update();
                        if (re.StatusCode == CoreStatusCode.OK)
                        {
                            return new CoreResult { StatusCode = CoreStatusCode.OK, Message = "Đổi mật khẩu thành công!" };
                        }
                        else
                        {
                            return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = "Đổi mật khẩu không thành công!" };
                        }
                    }
                }
                else
                {
                    return new CoreResult { StatusCode = CoreStatusCode.NotFound, Message = "Tài khoản hoặc mật khẩu không đúng!" };
                }
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = "Lỗi đăng nhập", ExceptionError = this.GetMessageException(ex) };
            }
        }

        public EmployeeUserDepartment Login()
        {
            User user = this.context.Users.FirstOrDefault(m => m.Account.ToLower() == this.Account.ToLower());
            if (user != null)
            {
                if (user.locked)
                {
                    return new EmployeeUserDepartment { StatusCode = CoreStatusCode.Failed, Message = "Tài khoản của bạn đã bị khóa.", Data = -1 };
                }
                else
                {
                    if (Helper.TO_LOWER(user.Account) == Helper.TO_LOWER(this.Account))
                        if (user.Password.Trim() == Cipher.Encrypt(Constant.KEY, this.Password.Trim()))
                        {
                            this.LoginSuccess(user.userID);
                            var userinfo = (from a in this.context.Employees
                                            join b in this.context.Departments on a.DepartmentsID equals b.DepartmentsID
                                            where (a.employeesID == user.employeesID)
                                            select new EmployeeUserDepartment { Employee = a, Department = b }).FirstOrDefault();
                            if (userinfo != null)
                            {
                                userinfo.User = user;
                                userinfo.StatusCode = CoreStatusCode.OK;
                                userinfo.Message = "Đăng nhập thành công";
                                return userinfo;
                            }
                            else
                            {
                                return new EmployeeUserDepartment { StatusCode = CoreStatusCode.OK, Message = "Đăng nhập thành công", User = user };
                            }
                        }
                        else
                        {
                            int counter = user.LoginFailed(user.userID);
                            if (counter == -1)
                            {
                                return new EmployeeUserDepartment { StatusCode = CoreStatusCode.Failed, Message = "Nhập sai mật khẩu. Tài khoản của bạn đã bị khóa.", Data = counter };
                            }
                            else if (counter == 1)
                            {
                                return new EmployeeUserDepartment { StatusCode = CoreStatusCode.Failed, Message = "Sai mật khẩu", Data = counter };
                            }
                            else
                            {
                                return new EmployeeUserDepartment { StatusCode = CoreStatusCode.Failed, Message = string.Format("Nhập sai mật khẩu {0} lần. Nhập sai 3 lần, tài khoản sẽ bị khóa.", counter) };
                            }

                        }
                    else
                        return new EmployeeUserDepartment { StatusCode = CoreStatusCode.Failed, Message = "Sai tên đăng nhập" };
                }
            }
            else
                return new EmployeeUserDepartment { StatusCode = CoreStatusCode.NotFound, Message = "Tài khoản không tồn tại" };
        }
    }
    public class EmployeeUser : CoreResult
    {
        public EmployeeUser()
        {
        }
        public EmployeeUser(Employee employee, User user)
        {
            this.Employee = employee;
            this.User = user;
        }
        public User User { get; set; }
        public Employee Employee { get; set; }
    }
    public class EmployeeUserDepartment : EmployeeUser
    {
        public EmployeeUserDepartment()
        {
        }
        public EmployeeUserDepartment(Employee employee, User user) : base(employee, user)
        {
        }
        public EmployeeUserDepartment(Employee employee, User user, Department department) : base(employee, user)
        {
            this.Department = department;
        }
        public Department Department { get; set; }
    }
}
