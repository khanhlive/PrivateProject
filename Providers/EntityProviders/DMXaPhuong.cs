using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Dao.Enum;
using Moss.Hospital.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Entities
{
    public partial class DMXaPhuong : MossHospitalRepository<DMXaPhuong, int>
    {/// <summary>
     /// Khởi tạo đối tượng
     /// </summary>
        public DMXaPhuong()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        protected override string GetFeatureCode()
        {
            return "DM_XAPHUONG";
        }
        protected override string GetNameEntity()
        {
            return "danh mục xã phường";
        }
        internal override IEnumerable<DMXaPhuong> CacheData()
        {
            return GlobalCache.DMXaPhuongs;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.DMXaPhuong);
        }

        ///// <summary>
        ///// Khởi tạo lại cache
        ///// </summary>
        //internal override void RefreshCache()
        //{
        //    GlobalCacheService globalcacheservice = new GlobalCacheService();
        //    globalcacheservice.LoadCache(CacheType.DMXaPhuong);
        //}

        /// <summary>
        /// Lấy ra 1 danh mục xã phường theo ID xã phường
        /// </summary>
        /// <param name="id">ID xã phường</param>
        /// <returns>Thành công: trả về 1 xã phường, Lỗi: trả về giá trị "NULL"</returns>
        public override DMXaPhuong GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.XaPhuongID == id);
            //}
            return base.GetByID(id);
        }
        /// <summary>
        /// Lấy ra 1 xã phường theo Mã xã phường(string)
        /// </summary>
        /// <param name="code">Mã xã phường</param>
        /// <param name="exactly">Lấy tương đối hay tuyệt đối mã xã phường (default: true - tuyệt đối)</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>

        public DMXaPhuong GetByCode(string code, bool? status = null)
        {
            if (code != null)
            {
                code = code.ToLower();
                return this.FindAll(p => p.XaPhuong_Code.ToLower() == code && status == null ? true : p.XaPhuong_Status == status.Value).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Tìm các xã phường theo danh sách mã xã phường
        /// </summary>
        /// <param name="code">Mảng mã xã phường cần tìm</param>
        /// <param name="status">Trạng thái xã phường</param>
        /// <returns></returns>
        public IEnumerable<DMXaPhuong> GetByCode(string[] code, bool? status = null)
        {
            if (code.Length > 0)
            {
                code.ToList().ForEach(p => p.ToLower());
                return this.FindAll(p => code.Contains(p.XaPhuong_Code.ToLower()) && status == null ? true : p.XaPhuong_Status == status.Value);
            }
            else
                return new List<DMXaPhuong>();
        }

        public CoreResult Update(int id, DMXaPhuong item, int? userID = null, bool checkPermission = false)
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
                            if (this.Any(p => p.XaPhuongID == id))
                            {
                                var result = base.CoreUpdate(id, item);
                                if (result.Item1 == CoreStatusCode.OK)
                                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                                else
                                    return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
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
                        if (this.Any(p => p.XaPhuongID == id))
                        {
                            var result = base.CoreUpdate(id, item);
                            if (result.Item1 == CoreStatusCode.OK)
                                return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                            else
                                return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
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
        public CoreResult Update(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return this.Update(this.XaPhuongID, this, userID, checkPermission);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        public CoreResult Insert(DMXaPhuong item, int? userID = null, bool checkPermission = false)
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
                            #region MyRegion
                            item.XaPhuong_Status = true;
                            #endregion
                            var result = base.CoreInsert(item);
                            if (result.Item1 == CoreStatusCode.OK)
                                return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                            else
                                return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                        }
                        else return per;
                    }
                    else
                    {
                        ////action
                        #region MyRegion
                        item.XaPhuong_Status = true;
                        #endregion
                        var result = base.CoreInsert(item);
                        if (result.Item1 == CoreStatusCode.OK)
                            return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                        else
                            return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                    }
                }

            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
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
                            DMXaPhuong xaphuong = this.GetByID(id);
                            if (xaphuong != null)
                            {
                                var result = base.CoreDelete(xaphuong);
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
                        DMXaPhuong xaphuong = this.GetByID(id);
                        if (xaphuong != null)
                        {
                            var result = base.CoreDelete(xaphuong);
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
        public CoreResult Delete(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return this.Delete(this.XaPhuongID, userID, checkPermission);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
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
                        var xaphuong = this.GetByID(id);
                        if (xaphuong != null)
                        {
                            this.context.Entry<DMXaPhuong>(xaphuong).State = EntityState.Deleted;
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
        /// Lọc các xã phường theo mã xã phường, trạng thái(status)
        /// </summary>
        /// <param name="_code">Mã xã phường cần lọc dữ liệu</param>
        /// <param name="status">Trạng thái xã phường cần lọc</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <returns></returns>
        public IEnumerable<DMXaPhuong> GetFilterByCode(string _code, bool? status = null, bool exactly = true)
        {
            if (_code != null)
            {
                _code = _code.ToLower();
                if (exactly)
                {
                    return this.FindAll(p => (p.XaPhuong_Code.ToLower() == _code) && (status == null ? true : p.XaPhuong_Status == status.Value));
                }
                else
                    return this.FindAll(p => (p.XaPhuong_Code.ToLower().Contains(_code)) && (status == null ? true : p.XaPhuong_Status == status.Value));
            }
            else
                return null;
        }

        /// <summary>
        /// Lọc xã phường theo tên xã phường, trạng thái, độ chính xác lọc dữ liệu
        /// </summary>
        /// <param name="name">Tên xã phường cần lọc</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <param name="status">Trạng thái xã phường cần lọc</param>
        /// <returns></returns>
        public IEnumerable<DMXaPhuong> GetFilterByName(string name, bool exactly = true, bool? status = null)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(name))
            {
                name = name.ToLower();
                return this.FindAll(p => (exactly ? (p.XaPhuong_Name.ToLower() == name) : p.XaPhuong_Name.Contains(name) )&&( status == null ? true : p.XaPhuong_Status == status.Value));
            }
            else
            {
                return this.FindAll(p => (status == null ? true : p.XaPhuong_Status == status.Value));
            }
        }

        /// <summary>
        /// Lọc các xã phường theo mã, tên xã phường, trạng thái(status)
        /// </summary>
        /// <param name="text">Chuỗi cần lọc dữ liệu</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <param name="status">Trạng thái xã phường cần lọc</param>
        /// <returns></returns>
        public IEnumerable<DMXaPhuong> GetFilterByText(string text, bool exactly = true, bool? status = null)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                return this.FindAll(p => (exactly ? (p.XaPhuong_Name.ToLower() == text || p.XaPhuong_Code.ToLower() == text) : (p.XaPhuong_Name.Contains(text) || p.XaPhuong_Code.Contains(text))) &&( status == null ? true : p.XaPhuong_Status == status.Value));
            }
            else
            {
                return this.FindAll(p => (status == null ? true : p.XaPhuong_Status == status.Value));
            }
        }

        /// <summary>
        /// Lọc danh sách xã phường theo ID quận huyện
        /// </summary>
        /// <param name="idHuyen">ID quận huyện</param>
        /// <param name="status">Trạng thái xã phường cần lọc</param>
        /// <returns></returns>
        public IEnumerable<DMXaPhuong> GetByHuyenID(int idHuyen, bool? status = null)
        {
            return this.FindAll(p => (p.XaPhuongID == idHuyen) && (status == null ? true : p.XaPhuong_Status == status.Value));
        }
        /// <summary>
        /// Lọc danh sách xã phường theo mã quận huyện
        /// </summary>
        /// <param name="code">Mã quận huyện</param>
        /// <param name="_status">Trạng thái xã phường cần lọc dữ liệu</param>
        /// <returns></returns>
        public IEnumerable<DMXaPhuong> GetByQuanHuyenCode(string code, bool? _status = null)
        {
            code = code.ToLower();
            return (from a in this.context.DMXaPhuongs.Where(p => (_status == null ? true : p.XaPhuong_Status == _status.Value))
                    join b in this.context.DMQuanHuyens.Where(p => (p.QuanHuyen_Code.ToLower() == code)) on a.QuanHuyenID equals b.QuanHuyenID
                    select a);

        }
        /// <summary>
        /// Lọc danh sách xã phường theo danh sách ID quận huyện
        /// </summary>
        /// <param name="ids">Mảng int[]: danh sách ID quận huyện cần lọc dữ liệu</param>
        /// <param name="_status">Trạng thái xã phường cần lọc</param>
        /// <returns></returns>
        public IEnumerable<DMXaPhuong> GetByHuyenID(int[] ids, bool? _status = null)
        {
            return (from a in this.context.DMXaPhuongs.Where(p => (ids.Contains(p.QuanHuyenID)) && (_status == null ? true : p.XaPhuong_Status == _status.Value))
                    select a);

        }
        /// <summary>
        /// Lọc danh sách xã phường theo mã tỉnh thành
        /// </summary>
        /// <param name="code">Mã tỉnh thành</param>
        /// <param name="_status">Trạng thái xã phường cần lọc</param>
        /// <returns></returns>
        public IEnumerable<DMXaPhuong> GetByTinhThanhCode(string code, bool? _status = null)
        {
            code = code.ToLower();
            return (from a in this.context.DMXaPhuongs.Where(p => (_status == null ? true : p.XaPhuong_Status == _status.Value))
                    join b in this.context.DMQuanHuyens on a.QuanHuyenID equals b.QuanHuyenID
                    join c in this.context.DMTinhThanhs on b.TinhThanhID equals c.TinhThanhID
                    where (c.TinhThanh_Code.ToLower() == code)
                    select a);
        }

        /// <summary>
        /// Lọc danh sách xã phường theo ID tỉnh thành
        /// </summary>
        /// <param name="idTinhThanh">ID tỉnh thành</param>
        /// <param name="_status">Trạng thái xã phường cần lọc</param>
        /// <returns></returns>
        public IEnumerable<DMXaPhuong> GetByTinhThanhID(int idTinhThanh, bool? _status = null)
        {
            return (from a in this.context.DMXaPhuongs.Where(p => (_status == null ? true : p.XaPhuong_Status == _status.Value))
                    join b in this.context.DMQuanHuyens on a.QuanHuyenID equals b.QuanHuyenID
                    where (b.TinhThanhID == idTinhThanh)
                    select a);
        }
    }
}
