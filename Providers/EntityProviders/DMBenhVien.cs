using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Dao.Enum;
using Moss.Hospital.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 
/// </summary>
namespace Moss.Hospital.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DMBenhVien : MossHospitalRepository<DMBenhVien, string>
    {

        /// <summary>
        /// Hàm khởi tạo đối tượng
        /// </summary>
        public DMBenhVien()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "danh mục bệnh viện";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "DM_BENHVIEN";
        }
        /// <summary>
        /// Hàm lấy dữ liệu cache nếu đối tượng thuộc danh sách đối tượng cache
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<DMBenhVien> CacheData()
        {
            return GlobalCache.dMBenhViens;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.DMBenhVien);
        }

        /// <summary>
        /// Lấy ra 1 đối tượng bệnh viện theo mã bệnh viện
        /// </summary>
        /// <param name="id">Mã bệnh viện</param>
        /// <returns>Thành công: trả về 1 bệnh viện có mã là ID, Lỗi: trả về giá trị "NULL"</returns>
        public override DMBenhVien GetByID(string id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.BenhVienID == id);
            //}
            return base.GetByID(id);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<DMBenhVien> FindAll(Expression<Func<DMBenhVien, bool>> match, int? skip=0, int? take=null)
        {
            try
            {
                if (this.GetUseCache())
                {
                    if (take==null)
                    {
                        return CacheData().Where(match.Compile());
                    }
                    else
                    {
                        return CacheData().Where(match.Compile()).Skip(skip==null?0:skip.Value).Take(take.Value);
                    }

                }
                else
                {
                    if (take==null)
                    {
                        return this.context.Set<DMBenhVien>().Where(match);
                    }
                    else
                    {
                        return this.context.Set<DMBenhVien>().Where(match).Skip(skip == null ? 0 : skip.Value).Take(take.Value);
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
        /// <param name="mabenhvien"></param>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Update(string mabenhvien, DMBenhVien item, int? userID = null, bool checkPermission = false)
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
                            if (this.Any(p => p.MaBenhVien == mabenhvien))
                            {
                                var result = base.CoreUpdate(mabenhvien, item);
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
                        if (this.Any(p => p.MaBenhVien == mabenhvien))
                        {
                            var result = base.CoreUpdate(mabenhvien, item);
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
                return this.Update(this.MaBenhVien, this, userID, checkPermission);
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
        public CoreResult Insert(DMBenhVien item, int? userID = null, bool checkPermission = false)
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
                            if (!this.Any(p => p.MaBenhVien == item.MaBenhVien))
                            {
                                #region Set default value
                                item.Status = true;
                                item.deleted = false;
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
                        if (!this.Any(p => p.MaBenhVien == item.MaBenhVien))
                        {
                            #region Set default value
                            item.Status = true;
                            item.deleted = false;
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
        /// <param name="mabenhvien"></param>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Delete(string mabenhvien, int? userID = null, bool checkPermission = false)
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
                            //DMBenhVien benhvien = this.GetByID(id);
                            if (this.Any(p => p.MaBenhVien == mabenhvien))//benhvien != null)
                            {
                                var result = base.CoreDeleteTemp(mabenhvien);
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
                        //DMBenhVien benhvien = this.GetByID(id);
                        if (this.Any(p => p.MaBenhVien == mabenhvien))//benhvien != null)
                        {
                            var result = base.CoreDeleteTemp(mabenhvien);
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
                return this.Delete(this.MaBenhVien, userID, checkPermission);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mabenhvien"></param>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Delete(string[] mabenhvien, int? userID = null, bool checkPermission = false)
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
                        foreach (string id in mabenhvien)
                        {
                            var benhvien = this.GetByID(id);
                            if (benhvien != null)
                            {
                                benhvien.deleted = true;
                                this.context.Entry<DMBenhVien>(benhvien).State = EntityState.Modified;
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
        /// Lấy 1 bệnh viện theo mã bệnh viện
        /// </summary>
        /// <param name="code">Mã bệnh viện</param>
        /// <param name="status">Trạng thái bệnh viện cần lọc dữ liệu</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa, lấy tất cả để giá trị NULL</param>
        /// <returns></returns>
        public DMBenhVien GetByCode(string code, bool? status = null, bool? deleted = false)
        {
            if (code != null)
            {
                code = code.ToLower();
                return this.FindAll(p => (p.MaBenhVien.ToLower() == code) && (status == null ? true : p.Status == status)&& (deleted == null ? true : p.deleted == deleted.Value)).FirstOrDefault();
            }
            else
                return null;
        }

        /// <summary>
        /// Lọc danh sách bệnh viện theo mã bệnh viện
        /// </summary>
        /// <param name="code">Mã bệnh viện cần lọc dữ liệu</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <param name="status">Trạng thái bệnh viện cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa, lấy tất cả để giá trị NULL </param>
        /// <returns></returns>
        public IEnumerable<DMBenhVien> GetFilterByCode(string code, bool exactly = true, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(code))
            {
                code = code.ToLower();
                return this.FindAll(p => (exactly ? p.MaBenhVien.ToLower() == code : p.MaBenhVien.ToLower().Contains(code)) && (status == null ? true : p.Status == status) && (deleted == null ? true : p.deleted == deleted));
            }
            else
            {
                return this.FindAll(p => (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
            }
        }
        /// <summary>
        /// Lọc danh sách bệnh viện theo tên bệnh viện
        /// </summary>
        /// <param name="name">Tên bệnh viện</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <param name="status">Trạng thái bệnh viện cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa, lấy tất cả để giá trị NULL </param>
        /// <returns></returns>
        public IEnumerable<DMBenhVien> GetFilterByName(string name, bool exactly = true, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(name))
            {
                name = name.ToLower();
                return this.FindAll(p => (exactly ? p.TenBenhVien.ToLower() == name : p.TenBenhVien.ToLower().Contains(name)) && (status == null ? true : p.Status == status) && (deleted == null ? true : p.deleted == deleted));
            }
            else
            {
                return this.FindAll(p => status == null ? true : p.Status == status.Value && deleted == null ? true : p.deleted == deleted.Value);
            }
        }
        /// <summary>
        /// Lọc danh sách bệnh viện theo mã, tên bệnh viện
        /// </summary>
        /// <param name="text">Chuỗi cần lọc: tên hoặc mã bệnh viện</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <param name="status">Trạng thái bệnh viện cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa, lấy tất cả để giá trị NULL </param>
        /// <returns></returns>
        public IEnumerable<DMBenhVien> GetFilterByNameAndCode(string text, bool exactly = true, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                return this.FindAll(p => (exactly ? (p.TenBenhVien.ToLower() == text || p.MaBenhVien.ToLower() == text) : (p.TenBenhVien.ToLower().Contains(text) || p.MaBenhVien.ToLower().Contains(text))) && (status == null ? true : p.Status == status) && (deleted == null ? true : p.deleted == deleted));
            }
            else
            {
                return this.FindAll(p => (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
            }
        }
        /// <summary>
        /// Lọc danh sách bệnh viện theo tuyến bệnh viện
        /// </summary>
        /// <param name="tuyen">Tuyến bệnh viện</param>
        /// <param name="connected">Đã được kết nối</param>
        /// <param name="status">Trạng thái bệnh viện cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa, lấy tất cả để giá trị NULL </param>
        /// <returns></returns>
        public IEnumerable<DMBenhVien> GetFilterByTuyen(int tuyen, bool? connected = true, bool? status = null, bool? deleted = false)
        {
            return this.FindAll(p => p.TuyenBV == tuyen && (connected == null ? true : p.Connected == connected.Value) && (status == null ? true : p.Status == status) && (deleted == null ? true : p.deleted == deleted));
        }
        /// <summary>
        /// Lọc danh sách bệnh viện theo hạng bệnh viện
        /// </summary>
        /// <param name="hang">Hạng bệnh viện</param>
        /// <param name="connected">Đã được kết nối</param>
        /// <param name="status">Trạng thái bệnh viện cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa, lấy tất cả để giá trị NULL </param>
        /// <returns></returns>
        public IEnumerable<DMBenhVien> GetFilterByHang(int hang, bool? connected = true, bool? status = null, bool? deleted = false)
        {
            return this.FindAll(p => (p.HangBV == hang) && (connected == null ? true : p.Connected == connected.Value) && (status == null ? true : p.Status == status) && (deleted == null ? true : p.deleted == deleted));
        }
        /// <summary>
        /// Lọc danh sách bệnh viện theo tuyến, hạng bệnh viện
        /// </summary>
        /// <param name="hang">Hạng bệnh viện</param>
        /// <param name="tuyen">Tuyến bệnh viện</param>
        /// <param name="connected">Đã được kết nối</param>
        /// <param name="status">Trạng thái bệnh viện cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa, lấy tất cả để giá trị NULL </param>
        /// <returns></returns>
        public IEnumerable<DMBenhVien> GetFilterByTuyenAndHang(int? hang, int? tuyen, bool? connected = true, bool? status = null, bool? deleted = false)
        {
            return this.FindAll(p => (hang == null ? true : p.HangBV == hang.Value) && (tuyen == null ? true : p.TuyenBV == tuyen.Value) && (connected == null ? true : p.Connected == connected.Value) && (status == null ? true : p.Status == status) && (deleted == null ? true : p.deleted == deleted));
        }

        /// <summary>
        /// Lọc danh sách bệnh viện theo mã chủ quản
        /// </summary>
        /// <param name="machuquan">Mã chủ quản</param>
        /// <param name="connected">Đã được kết nối</param>
        /// <param name="status">Trạng thái bệnh viện cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa, lấy tất cả để giá trị NULL </param>
        /// <returns></returns>
        public IEnumerable<DMBenhVien> GetFilterByMaChuQuan(string machuquan, bool? connected = true, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(machuquan))
            {
                return this.FindAll(p => (p.MaBenhVienChuQuan == machuquan) && (connected == null ? true : p.Connected == connected.Value) && (status == null ? true : p.Status == status) && (deleted == null ? true : p.deleted == deleted));
            }
            else
            {
                return this.FindAll(p => (connected == null ? true : p.Connected == connected.Value) && (status == null ? true : p.Status == status) && (deleted == null ? true : p.deleted == deleted));
            }

        }
    }
}
