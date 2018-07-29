using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Dao.Enum;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Moss.Hospital.Data.Entities
{
    public partial class Department : MossHospitalRepository<Department, int>
    {
        /// <summary>
        /// Hàm khởi tạo đối tượng
        /// </summary>
        public Department()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "bộ phận";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "DM_PHONGBAN";
        }
        /// <summary>
        /// Hàm lấy dữ liệu cache nếu đối tượng thuộc danh sách đối tượng cache
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<Department> CacheData()
        {
            return GlobalCache.departments;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.Department);
        }

        /// <summary>
        /// Lấy ra 1 đối tượng bộ phận theo mã bộ phận
        /// </summary>
        /// <param name="id">Mã bộ phận</param>
        /// <returns>Thành công: trả về 1 bộ phận có mã là ID, Lỗi: trả về giá trị "NULL"</returns>
        public override Department GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.DepartmentsID == id);
            //}
            return base.GetByID(id);
        }

        /// <summary>
        /// Lấy cấp của bộ phận theo "ID" của bộ phận cha
        /// </summary>
        /// <param name="phongbanId">"ID" của bộ phận cha</param>
        /// <returns></returns>
        public virtual int GetPhongBanLevel(int phongbanId)
        {
            if (phongbanId > 0) return 2;
            return 1;
        }
        /// <summary>
        /// Cập nhật bộ phận
        /// </summary>
        /// <param name="id">"ID" của bộ phận cần cập nhật</param>
        /// <param name="item">Bộ phận cần cập nhật</param>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Update(int id, Department item, int? userID = null, bool checkPermission = false)
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
                            var obj = this.GetByID(id);
                            if (obj != null)//this.Any(p => p.DepartmentsID == id))
                            {
                                #region MyRegion
                                item.Levels = Convert.ToByte(this.GetPhongBanLevel(item.DepartmentsID_TrucThuoc == null ? 0 : (int)item.DepartmentsID_TrucThuoc));
                                item.NumberUpdated = Convert.ToByte(obj.NumberUpdated + 1);
                                item.dateUpdated = DateTime.Now;
                                item.userIDCreated = obj.userIDCreated;
                                item.dateCreated = obj.dateCreated;
                                item.userIDUpdated = userID;
                                #endregion
                                var result = base.CoreUpdate(id, item);
                                if (result.Item1 == CoreStatusCode.OK)
                                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                                else
                                    return
                                        new CoreResult { StatusCode = result.Item1, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
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
                        var obj = this.GetByID(id);
                        if (obj != null)//this.Any(p => p.DepartmentsID == id))
                        {
                            #region MyRegion
                            item.Levels = Convert.ToByte(this.GetPhongBanLevel(item.DepartmentsID_TrucThuoc == null ? 0 : (int)item.DepartmentsID_TrucThuoc));
                            item.NumberUpdated = Convert.ToByte(obj.NumberUpdated + 1);
                            item.dateUpdated = DateTime.Now;
                            item.userIDCreated = obj.userIDCreated;
                            item.dateCreated = obj.dateCreated;
                            item.userIDUpdated = userID;
                            #endregion
                            var result = base.CoreUpdate(id, item);
                            if (result.Item1 == CoreStatusCode.OK)
                                return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                            else
                                return
                                    new CoreResult { StatusCode = result.Item1, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
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
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// Cập nhật bộ phận hiện thời
        /// </summary>
        /// <returns></returns>
        public CoreResult Update(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return this.Update(this.DepartmentsID, this, userID, checkPermission);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// Thêm mới bộ phận
        /// </summary>
        /// <param name="item">Đối tượng bộ phận cần thêm mới</param>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Insert(Department item, int? userID = null, bool checkPermission = false)
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
                            if (!this.Any(p => p.DepartmentsCode == item.DepartmentsCode))
                            {
                                #region MyRegion
                                item.Levels = Convert.ToByte(this.GetPhongBanLevel(item.DepartmentsID_TrucThuoc == null ? 0 : (int)item.DepartmentsID_TrucThuoc));
                                item.Status = true;
                                item.deleted = false;
                                item.dateCreated = DateTime.Now;
                                item.dateUpdated = null;
                                item.NumberUpdated = 0;
                                #endregion

                                var result = base.CoreInsert(item);
                                if (result.Item1 == CoreStatusCode.OK)
                                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                                else
                                    return new CoreResult { StatusCode = result.Item1, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
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
                        if (!this.Any(p => p.DepartmentsCode == item.DepartmentsCode))
                        {
                            #region MyRegion
                            item.Levels = Convert.ToByte(this.GetPhongBanLevel(item.DepartmentsID_TrucThuoc == null ? 0 : (int)item.DepartmentsID_TrucThuoc));
                            item.Status = true;
                            item.deleted = false;
                            item.dateCreated = DateTime.Now;
                            item.dateUpdated = null;
                            item.NumberUpdated = 0;
                            #endregion

                            var result = base.CoreInsert(item);
                            if (result.Item1 == CoreStatusCode.OK)
                                return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                            else
                                return new CoreResult { StatusCode = result.Item1, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
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
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// Thêm mới bộ phận hiện thời
        /// </summary>
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
        /// Xóa bộ phận theo "ID"
        /// </summary>
        /// <param name="id">Mã bộ phận</param>
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
                            if (this.Any(p => p.DepartmentsID == id))
                            {
                                if (!this.Any(p => p.DepartmentsID_TrucThuoc == id))
                                {
                                    var result = base.CoreDeleteTemp(id);
                                    if (result == CoreStatusCode.OK)
                                        return new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
                                    return
                                        new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
                                }
                                else
                                   return new CoreResult { StatusCode = CoreStatusCode.Used, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Used, ActionType.Delete) };
                            }
                            else
                                return new CoreResult { StatusCode = CoreStatusCode.NotFound, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Delete) };
                        }
                        else return per;
                    }
                    else
                    {
                        ////action
                        if (this.Any(p => p.DepartmentsID == id))
                        {
                            if (!this.Any(p => p.DepartmentsID_TrucThuoc == id))
                            {
                                var result = base.CoreDeleteTemp(id);
                                if (result == CoreStatusCode.OK)
                                    return new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
                                return
                                    new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
                            }
                            else
                               return  new CoreResult { StatusCode = CoreStatusCode.Used, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Used, ActionType.Delete) };
                        }
                        else
                           return new CoreResult { StatusCode = CoreStatusCode.NotFound, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Delete) };
                    }
                }

            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
        /// <summary>
        /// Xóa bộ phận hiện thời
        /// </summary>
        /// <returns></returns>
        public CoreResult Delete(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return this.Delete(this.DepartmentsID, userID, checkPermission);

            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
        /// <summary>
        /// Xóa nhóm bộ phận theo danh sách Mã bộ phận
        /// </summary>
        /// <param name="ids">Mảng int[]: danh sách mã bộ phận cần xóa</param>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Delete(int[] ids, int? userID = null, bool checkPermission = false)
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
                        var department = this.GetByID(id);
                        if (department != null && !this.Any(p=>p.DepartmentsID_TrucThuoc==id))
                        {
                            department.deleted = true;
                            this.context.Entry<Department>(department).State = EntityState.Modified;
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
        /// <summary>
        /// Lấy ra danh sách bộ phận theo mã bệnh viện
        /// </summary>
        /// <param name="mabenhvien">Mã bệnh viện</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns>Trả về danh sách bộ phận có mã bệnh viện như trên</returns>
        public IEnumerable<Department> GetByMaBenhVien(string mabenhvien, bool deleted = false)
        {
            return this.FindAll(p => ((mabenhvien == null ? true : p.MaBenhVien_TrucThuoc == mabenhvien) && (p.deleted == deleted)));
        }
        /// <summary>
        /// Lấy ra danh sách bộ phận theo: danh sách mã bệnh viện
        /// </summary>
        /// <param name="mabenhviens">Mảng int[]: chứa danh sách mã bệnh viện cần lấy thông tin bộ phận</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns>Trả về danh sách bộ phận theo điều kiện</returns>
        public IEnumerable<Department> GetByMaBenhVien(string[] mabenhviens, bool deleted = false)
        {
            return this.FindAll(p => (mabenhviens.Contains(p.MaBenhVien_TrucThuoc)) && (p.deleted == deleted));
        }
        /// <summary>
        /// Lấy ra danh sách bộ phận theo: Mã bệnh viện, Cấp độ, Deleted
        /// </summary>
        /// <param name="mabenhvien">Mã bệnh viện</param>
        /// <param name="level">Cấp độ(level)</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns>Trả về danh sách bộ phận theo điều kiện</returns>
        public IEnumerable<Department> GetByMaBenhVien_Level(string mabenhvien, int level, bool deleted = false)
        {
            return this.FindAll(p => (mabenhvien == null ? true : p.MaBenhVien_TrucThuoc == mabenhvien )&& (p.deleted == deleted && p.Levels == level));
        }
        /// <summary>
        /// Lấy ra danh sách bộ phận theo: Cấp độ, Deleted
        /// </summary>
        /// <param name="level">Level: Cấp độ</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns>Trả về danh sách bộ phận theo điều kiện</returns>
        public IEnumerable<Department> GetByLevel(int? level, bool deleted = false)
        {
            return this.FindAll(p => (p.deleted == deleted) && (level == null ? true : p.Levels == level));
        }

        /// <summary>
        /// Lấy ra danh sách bộ phận theo: Mã bệnh viện, chuỗi tìm kiếm, Deleted
        /// </summary>
        /// <param name="mabenhvien">Mã bệnh viện</param>
        /// <param name="text">Chuỗi tìm kiếm: tên bộ phận, Code bộ phận</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns>Trả về danh sách bộ phận theo điều kiện</returns>
        public IEnumerable<Department> GetFilterByMaBenhVien_TextSearch(string mabenhvien, string text, bool exactly = true, bool deleted = false)
        {
            var query = this.GetByMaBenhVien(mabenhvien);
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text))
            {
                string _text = text.ToLower();
                var result = (from de in query.Where(p => (exactly ? (p.DepartmentsCode.ToLower() == (_text) || p.DepartmentsName.ToLower() == (_text)) : ((p.DepartmentsCode.ToLower().Contains(_text) || p.DepartmentsName.ToLower().Contains(_text))))) select de);
                return result;
            }
            else
            {
                return query;
            }
        }
        /// <summary>
        /// Lấy ra danh sách bộ phận theo: danh sách mã bệnh viện, chuỗi tìm kiếm, Deleted
        /// </summary>
        /// <param name="mabenhviens">Danh sách mã bệnh viện</param>
        /// <param name="text">Chuỗi tìm kiếm: tên bộ phận, Code bộ phận</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns>Trả về danh sách bộ phận theo điều kiện</returns>
        public IEnumerable<Department> GetFilterByMaBenhVien_TextSearch(string[] mabenhviens, string text, bool exactly = true, bool deleted = false)
        {
            var query = this.GetByMaBenhVien(mabenhviens);
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text))
            {
                string _text = text.ToLower();
                var result = (from de in query.Where(p =>( exactly ? (p.DepartmentsCode.ToLower() == (_text) || p.DepartmentsName.ToLower() == (_text)) : (p.DepartmentsCode.ToLower().Contains(_text) || p.DepartmentsName.ToLower().Contains(_text)))) select de);
                return result;
            }
            else
            {
                return query;
            }
        }
        /// <summary>
        /// Lấy ra danh sách bộ phận theo: danh sách mã bệnh viện, level(cấp độ), chuỗi tìm kiếm, Deleted
        /// </summary>
        /// <param name="mabenhvien">Mã bệnh viện</param>
        /// <param name="level">Level(cấp độ)</param>
        /// <param name="text">Chuỗi tìm kiếm: tên bộ phận, Code bộ phận</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns>Trả về danh sách bộ phận theo điều kiện</returns>
        public IEnumerable<Department> GetFilterByMaBenhVien_Level_TextSearch(string mabenhvien, int level, string text, bool exactly = true, bool deleted = false)
        {
            var query = this.GetByMaBenhVien_Level(mabenhvien, level, deleted);
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text))
            {
                string _text = text.ToLower();
                var result = (from de in query.Where(p => (exactly ? (p.DepartmentsCode.ToLower() == (_text) || p.DepartmentsName.ToLower() == (_text)) : (p.DepartmentsCode.ToLower().Contains(_text) || p.DepartmentsName.ToLower().Contains(_text)))) select de);
                return result;
            }
            else
            {
                return query;
            }
        }

        /// <summary>
        /// Lấy ra danh sách bộ phận theo: level(cấp độ), chuỗi tìm kiếm, Deleted
        /// </summary>
        /// <param name="level">Level(cấp độ)</param>
        /// <param name="text">Chuỗi tìm kiếm: tên bộ phận, Code bộ phận</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns>Trả về danh sách bộ phận theo điều kiện</returns>
        public IEnumerable<Department> GetFilterByLevel_TextSearch(int level, string text, bool exactly = true, bool deleted = false)
        {
            var query = this.GetByLevel(level);
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text))
            {
                string _text = text.ToLower();
                var result = (from de in query.Where(p => (exactly ? (p.DepartmentsCode.ToLower() == (_text) || p.DepartmentsName.ToLower() == (_text)) :( p.DepartmentsCode.ToLower().Contains(_text) || p.DepartmentsName.ToLower().Contains(_text)))) select de);
                return result;
            }
            else
            {
                return query;
            }
        }

        public IEnumerable<DMBenhVien> GetAllBenhVienExisted(bool? deleted = false)
        {
            var array = (from a in this.context.Departments.Where(p => (deleted == null ? true : (p.deleted == deleted.Value))) group a by a.MaBenhVien_TrucThuoc into g select g.Key.Trim()).ToList();
            return (from a in array join b in context.DMBenhViens on a equals b.MaBenhVien select b);
        }

    }
}
