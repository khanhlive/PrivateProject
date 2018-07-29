using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Entities
{
    public partial class Employee: Providers.Repositories.MossHospitalRepository<Employee,int>
    {
        /// <summary>
        /// Hàm khởi tạo đối tượng
        /// </summary>
        public Employee()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "danh mục cán bộ";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "DM_CANBO";
        }
        /// <summary>
        /// Hàm lấy dữ liệu cache nếu đối tượng thuộc danh sách đối tượng cache
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<Employee> CacheData()
        {
            return GlobalCache.Employees;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.Employee);
        }

        /// <summary>
        /// Lấy ra 1 đối tượng cán bộ theo mã cán bộ
        /// </summary>
        /// <param name="id">Mã cán bộ</param>
        /// <returns>Thành công: trả về 1 cán bộ có mã là ID, Lỗi: trả về giá trị "NULL"</returns>
        public override Employee GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.employeesID == id);
            //}
            return base.GetByID(id);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Update(int id, Employee item, int? userID = null, bool checkPermission = false)
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
                            if (this.Any(p => p.employeesID == id))
                            {
                                if (!this.Any(p => p.employeesCode == item.employeesCode && p.employeesID!=id && p.employeesCode!=null && p.employeesCode!=""))
                                {
                                    if (!this.Any(p => p.MaCCHN == item.MaCCHN && p.employeesID != id && p.MaCCHN != null && p.MaCCHN != ""))
                                    {
                                        var result = base.CoreUpdate(id, item);
                                        if (result.Item1 == CoreStatusCode.OK)
                                            return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                                        else
                                            return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                                    }
                                    else
                                        return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = item, Message = string.Format("Đã có 1 cán bộ có mã Chứng chỉ hành nghề là: \"{0}\", hãy nhập mã khác.", item.employeesCode) };
                                }
                                else
                                {
                                    return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = item, Message = string.Format("Đã có 1 cán bộ có mã là: \"{0}\", hãy nhập mã khác.", item.employeesCode) };
                                }

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
                        if (this.Any(p => p.employeesID == id))
                        {
                            if (!this.Any(p => p.employeesCode == item.employeesCode && p.employeesID != id && p.employeesCode != null && p.employeesCode != ""))
                            {
                                if (!this.Any(p => p.MaCCHN == item.MaCCHN && p.employeesID != id && p.MaCCHN != null && p.MaCCHN != ""))
                                {
                                    var result = base.CoreUpdate(id, item);
                                    if (result.Item1 == CoreStatusCode.OK)
                                        return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                                    else
                                        return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                                }
                                else
                                    return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = item, Message = string.Format("Đã có 1 cán bộ có mã Chứng chỉ hành nghề là: \"{0}\", hãy nhập mã khác.", item.employeesCode) };
                            }
                            else
                            {
                                return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = item, Message = string.Format("Đã có 1 cán bộ có mã là: \"{0}\", hãy nhập mã khác.", item.employeesCode) };
                            }
                            
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
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
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
                return this.Update(this.employeesID, this, userID, checkPermission);
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
        public CoreResult Insert(Employee item, int? userID = null, bool checkPermission = false)
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
                            if (!this.Any(p => p.employeesCode == item.employeesCode && p.employeesCode != null && p.employeesCode != ""))
                            {
                                if (!this.Any(p => p.MaCCHN == item.MaCCHN && p.MaCCHN != null && p.MaCCHN != ""))
                                {
                                    #region Set default value
                                    item.Status = true;
                                    #endregion
                                    var result = base.CoreInsert(item);
                                    if (result.Item1 == CoreStatusCode.OK)
                                        return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                                    else
                                        return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                                }
                                else
                                    return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = item, Message = string.Format("Đã có 1 cán bộ có mã Chứng chỉ hành nghề là: \"{0}\", hãy nhập mã khác.", item.employeesCode) };
                            }
                            else
                            {
                                return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = item, Message = string.Format("Đã có 1 cán bộ có mã là: \"{0}\", hãy nhập mã khác.", item.employeesCode) };
                            }

                        }
                        else return per;
                    }
                    else
                    {
                        ////action
                        if (!this.Any(p => p.employeesCode == item.employeesCode && p.employeesCode != null && p.employeesCode != ""))
                        {
                            if (!this.Any(p => p.MaCCHN == item.MaCCHN && p.MaCCHN != null && p.MaCCHN != ""))
                            {
                                #region Set default value
                                item.Status = true;
                                #endregion
                                var result = base.CoreInsert(item);
                                if (result.Item1 == CoreStatusCode.OK)
                                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                                else
                                    return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                            }
                            else
                                return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = item, Message = string.Format("Đã có 1 cán bộ có mã Chứng chỉ hành nghề là: \"{0}\", hãy nhập mã khác.", item.employeesCode) };
                        }
                        else
                        {
                            return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = item, Message = string.Format("Đã có 1 cán bộ có mã là:\"{0}\", hãy nhập mã khác.", item.employeesCode) };
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Insert, ex) };
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
                            //Employee benhvien = this.GetByID(id);
                            if (this.Any(p => p.employeesID == id))
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
                        //Employee benhvien = this.GetByID(id);
                        if (this.Any(p => p.employeesID == id))
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
                return this.Delete(this.employeesID, userID, checkPermission);
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
                            var canbo = this.GetByID(id);
                            if (canbo != null)
                            {
                                canbo.deleted = true;
                                this.context.Entry<Employee>(canbo).State = EntityState.Modified;
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

        /// <summary>
        /// Lọc danh sách cán bộ theo tên
        /// </summary>
        /// <param name="name">Tên cán bộ cần lọc</param>
        /// <param name="exactly">Độ chính xác: tuyệt đối hay tương đối</param>
        /// <param name="departmentId">ID bộ phận</param>
        /// <param name="gender">Giới tính</param>
        /// <param name="status">Trạng thái cán bộ</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<Employee> GetFilterByName(string name, bool exactly=true, int? departmentId=null,int? gender=null,bool? status=null, bool? deleted=false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(name))
            {
                name = name.ToLower();
                return this.FindAll(p => (exactly ? p.employeesName.ToLower() == name : p.employeesName.ToLower().Contains(name)) && (departmentId == null ? true : p.DepartmentsID == departmentId.Value)
                && (gender == null ? true : p.gender == gender.Value) && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
            }
            else
            {
                return this.FindAll(p => (departmentId == null ? true : p.DepartmentsID == departmentId.Value) && (gender == null ? true : p.gender == gender.Value)
                && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="exactly"></param>
        /// <param name="departmentId"></param>
        /// <param name="gender"></param>
        /// <param name="status"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public IEnumerable<Employee> GetFilterByText(string text, bool exactly = true, int? departmentId = null, int? gender = null, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                return this.FindAll(p => (exactly ? (p.employeesName.ToLower() == text || p.employeesCode.ToLower() == text) : (p.employeesName.ToLower().Contains(text)|| p.employeesCode.ToLower().Contains(text)))
                && (departmentId == null ? true : p.DepartmentsID == departmentId.Value) && (gender == null ? true : p.gender == gender.Value) && (status == null ? true : p.Status == status.Value)
                && (deleted == null ? true : p.deleted == deleted.Value));
            }
            else
            {
                return this.FindAll(p => (departmentId == null ? true : p.DepartmentsID == departmentId.Value) && (gender == null ? true : p.gender == gender.Value )&& (status == null ? true : p.Status == status.Value )&& (deleted == null ? true : p.deleted == deleted.Value));
            }
        }
        /// <summary>
        /// Lọc danh sách cán bộ theo mã
        /// </summary>
        /// <param name="code">Mã cán bộ</param>
        /// <param name="exactly">Độ chính xác: tuyệt đối hay tương đối</param>
        /// <param name="departmentId">ID bộ phận</param>
        /// <param name="gender">Giới tính</param>
        /// <param name="status">Trạng thái cán bộ</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<Employee> GetFilterByCode(string code, bool exactly = true, int? departmentId = null, int? gender = null, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(code))
            {
                code = code.ToLower();
                return this.FindAll(p => (exactly ? p.employeesCode.ToLower() == code : p.employeesCode.ToLower().Contains(code)) && (departmentId == null ? true : p.DepartmentsID == departmentId.Value)
                && (gender == null ? true : p.gender == gender.Value) && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
            }
            else
            {
                return this.FindAll(p => (departmentId == null ? true : p.DepartmentsID == departmentId.Value) && (gender == null ? true : p.gender == gender.Value)
                && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
            }
        }
        /// <summary>
        /// Lấy thông tin cán bộ theo mã
        /// </summary>
        /// <param name="code">Mã cán bộ</param>
        /// <param name="departmentId">ID bộ phận</param>
        /// <param name="gender">Giới tính</param>
        /// <param name="status">Trạng thái cán bộ</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public Employee GetByCode(string code, int? departmentId = null, int? gender = null, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(code))
            {
                code = code.ToLower();
                return this.Find(p => (p.employeesCode.ToLower() == code) && (departmentId == null ? true : p.DepartmentsID == departmentId.Value)
                && (gender == null ? true : p.gender == gender.Value) && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
            }
            else
            {
                return this.Find(p => (departmentId == null ? true : p.DepartmentsID == departmentId.Value) && (gender == null ? true : p.gender == gender.Value) && 
                (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
            }
        }
        /// <summary>
        /// Lọc danh sách cán bộ theo ID bộ phận
        /// </summary>
        /// <param name="departmentId">ID bộ phận</param>
        /// <param name="gender">Giới tính</param>
        /// <param name="status">Trạng thái cán bộ</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<Employee> GetFilterByDepartmentId(int? departmentId, int? gender = null, bool? status = null, bool? deleted = false)
        {
            return this.FindAll(p => (departmentId == null ? true : p.DepartmentsID == departmentId.Value)
            && (gender == null ? true : p.gender == gender.Value) && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
            
        }
        /// <summary>
        /// Lọc danh sách cán bộ theo danh sách ID bộ phận
        /// </summary>
        /// <param name="departmentId">ID bộ phận</param>
        /// <param name="gender">Giới tính</param>
        /// <param name="status">Trạng thái cán bộ</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<Employee> GetFilterByDepartmentId(int[] departmentIds, int? gender = null, bool? status = null, bool? deleted = false)
        {
            return this.FindAll(p => (departmentIds.Contains(p.DepartmentsID)) && (gender == null ? true : p.gender == gender.Value)
            && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
        }
        /// <summary>
        /// Lọc danh sách cán bộ theo ID bệnh viện
        /// </summary>
        /// <param name="benhvienId">Mã bệnh viện</param>
        /// <param name="gender">Giới tính</param>
        /// <param name="status">Trạng thái cán bộ</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<Employee> GetFilterByBenhVienID(int benhvienId, int? gender = null, bool? status = null, bool? deleted = false)
        {
            return (from a1 in context.DMBenhViens.Where(p => p.BenhVienID == benhvienId)
                    join b1 in context.Departments on a1.MaBenhVien equals b1.MaBenhVien_TrucThuoc
                    join c in context.Employees on b1.DepartmentsID equals c.DepartmentsID
                    where ((gender == null ? true : c.gender == gender.Value) 
                    && (status == null ? true : c.Status == status.Value) 
                    && (deleted == null ? true : c.deleted == deleted.Value))
                    select c);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chucvuId"></param>
        /// <param name="gender"></param>
        /// <param name="status"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public IEnumerable<Employee> GetFilterByChucVuId(int chucvuId, int? gender = null, bool? status = null, bool? deleted = false)
        {
            return (from a in context.DMChucVus.Where(p => p.ChucVuID == chucvuId)
                    join c in context.Employees on a.ChucVuID equals  c.ChucVuID
                    where ((gender == null ? true : c.gender == gender.Value)
                    && (status == null ? true : c.Status == status.Value)
                    && (deleted == null ? true : c.deleted == deleted.Value))
                    select c);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chucvuIds"></param>
        /// <param name="gender"></param>
        /// <param name="status"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public IEnumerable<Employee> GetFilterByChucVuId(int[] chucvuIds, int? gender = null, bool? status = null, bool? deleted = false)
        {
            return (from a in context.DMChucVus.Where(p =>  chucvuIds.Contains(p.ChucVuID))
                    join c in context.Employees on a.ChucVuID equals c.ChucVuID
                    where ((gender == null ? true : c.gender == gender.Value)
                    && (status == null ? true : c.Status == status.Value)
                    && (deleted == null ? true : c.deleted == deleted.Value))
                    select c);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chucvuCode"></param>
        /// <param name="gender"></param>
        /// <param name="status"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public IEnumerable<Employee> GetFilterByChucVuCode(string chucvuCode, int? gender = null, bool? status = null, bool? deleted = false)
        {
            return (from a in context.DMChucVus.Where(p => p.ChucVuCode == chucvuCode)
                    join c in context.Employees on a.ChucVuID equals c.ChucVuID
                    where ((gender == null ? true : c.gender == gender.Value)
                    && (status == null ? true : c.Status == status.Value)
                    && (deleted == null ? true : c.deleted == deleted.Value))
                    select c);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chucvuCodes"></param>
        /// <param name="gender"></param>
        /// <param name="status"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public IEnumerable<Employee> GetFilterByChucVuCode(string[] chucvuCodes, int? gender = null, bool? status = null, bool? deleted = false)
        {
            return (from a in context.DMChucVus.Where(p => chucvuCodes.Contains(p.ChucVuCode))
                    join c in context.Employees on a.ChucVuID equals c.ChucVuID
                    where ((gender == null ? true : c.gender == gender.Value)
                    && (status == null ? true : c.Status == status.Value)
                    && (deleted == null ? true : c.deleted == deleted.Value))
                    select c);
        }
    }
}
