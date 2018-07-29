using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Dao.Enum;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Moss.Hospital.Data.Entities
{
    public partial class DMDanhmuc : MossHospitalRepository<DMDanhmuc, int>
    {
        /// <summary>
        /// 
        /// </summary>
        private Library.Constants.LoaiDanhMuc _loaidanhmuc;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_loaidanhmuc"></param>
        public void SetLoaiDanhMuc(Library.Constants.LoaiDanhMuc _loaidanhmuc)
        {
            this._loaidanhmuc = _loaidanhmuc;
        }

        /// <summary>
        /// Khởi tạo đối tượng
        /// </summary>
        public DMDanhmuc(Library.Constants.LoaiDanhMuc _loaidanhmuc)
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
            this._loaidanhmuc = _loaidanhmuc;
        }
        /// <summary>
        /// 
        /// </summary>
        public DMDanhmuc()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
            this._loaidanhmuc = Library.Constants.LoaiDanhMuc.DonViTinh;
        }
        protected override string GetFeatureCode()
        {
            switch (_loaidanhmuc)
            {
                case Library.Constants.LoaiDanhMuc.DonViTinh:
                    return "DM_DONVITINH";
                case Library.Constants.LoaiDanhMuc.DanToc:
                    return "DM_DANTOC";
                case Library.Constants.LoaiDanhMuc.BacHoc:
                    return "DM_BACHOC";
                case Library.Constants.LoaiDanhMuc.NgheNghiep:
                    return "DM_NGHENGHIEP";
                case Library.Constants.LoaiDanhMuc.QuocTich:
                    return "DM_QUOCTICH";
                case Library.Constants.LoaiDanhMuc.DoiTuongBHYT:
                    return "DM_DOITUONGBHYT";
                default:return "DM_DANHMUC";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            switch (_loaidanhmuc)
            {
                case Library.Constants.LoaiDanhMuc.DonViTinh:
                    return "danh mục đơn vị tính";
                case Library.Constants.LoaiDanhMuc.DanToc:
                    return "danh mục dân tộc";
                case Library.Constants.LoaiDanhMuc.BacHoc:
                    return "danh mục bậc học";
                case Library.Constants.LoaiDanhMuc.NgheNghiep:
                    return "danh mục nghề nghiệp";
                case Library.Constants.LoaiDanhMuc.QuocTich:
                    return "danh mục quốc tịch";
                case Library.Constants.LoaiDanhMuc.DoiTuongBHYT:
                    return "danh mục đối tượng bảo hiểm y tế";
                default: return "danh mục";
            }
        }

        ///// <summary>
        ///// Khởi tạo lại cache
        ///// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.DMDanhMuc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<DMDanhmuc> CacheData()
        {
            return GlobalCache.DMDanhmucs;
        }

        /// <summary>
        /// Lấy ra 1 danh mục dân tộc theo ID dân tộc
        /// </summary>
        /// <param name="id">ID dân tộc</param>
        /// <returns>Thành công: trả về 1 dân tộc, Lỗi: trả về giá trị "NULL"</returns>
        public override DMDanhmuc GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    int _loai = (int)this._loaidanhmuc;
            //    return this.CacheData().FirstOrDefault(p => p.DanhMucID == id && p.LoaiDanhMuc == _loai);
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
        public CoreResult Update(int id, DMDanhmuc item, int? userID = null, bool checkPermission = false)
        {
            try
            {
                int _loai = (int)this._loaidanhmuc;
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
                            var obj = this.GetByID(id);
                            if (obj != null)
                            {
                                #region Set value Update
                                item.dateCreated = obj.dateCreated;
                                item.userIDCreated = obj.userIDCreated;
                                item.userIDUpdated = userID;
                                item.dateUpdated = DateTime.Now;
                                item.NumberUpdated = Convert.ToByte(obj.NumberUpdated = 1);
                                item.LoaiDanhMuc = (int)this._loaidanhmuc;
                                #endregion
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
                        var obj = this.GetByID(id);
                        if (obj != null)
                        {
                            #region Set value Update
                            item.dateCreated = obj.dateCreated;
                            item.userIDCreated = obj.userIDCreated;
                            item.userIDUpdated = userID;
                            item.dateUpdated = DateTime.Now;
                            item.LoaiDanhMuc = (int)this._loaidanhmuc;
                            item.NumberUpdated = Convert.ToByte(obj.NumberUpdated = 1);
                            #endregion
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
                return this.Update(this.DanhMucID, this, userID, checkPermission);
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
        public CoreResult Insert(DMDanhmuc item, int? userID = null, bool checkPermission = false)
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
                            item.userIDCreated = userID == null ? item.userIDCreated : userID.Value;
                            item.dateCreated = DateTime.Now;
                            item.dateUpdated = null;
                            item.userIDUpdated = null;
                            item.NumberUpdated = 0;
                            item.LoaiDanhMuc = (int)this._loaidanhmuc;
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
                        item.userIDCreated = userID == null ? item.userIDCreated : userID.Value;
                        item.dateCreated = DateTime.Now;
                        item.dateUpdated = null;
                        item.userIDUpdated = null;
                        item.NumberUpdated = 0;
                        item.LoaiDanhMuc = (int)this._loaidanhmuc;
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
                            int _loai = (int)this._loaidanhmuc;
                            if (this.Any(p => p.DanhMucID == id && p.LoaiDanhMuc == _loai))
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
                        int _loai = (int)this._loaidanhmuc;
                        if (this.Any(p => p.DanhMucID == id && p.LoaiDanhMuc == _loai))
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
                return this.Delete(this.DanhMucID, userID, checkPermission);
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
                        int _loai = (int)this._loaidanhmuc;
                        foreach (int id in ids)
                        {
                            var dantoc = this.Find(p => p.DanhMucID == id && p.LoaiDanhMuc == _loai);
                            if (dantoc != null)
                            {
                                this.context.Entry<DMDanhmuc>(dantoc).State = EntityState.Deleted;
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
        /// Lọc danh sách dân tộc theo tên dân tộc
        /// </summary>
        /// <param name="name">Tên dân tộc</param>
        /// <param name="exactly">Lọc tương đối hay tuyệt đối</param>
        /// <param name="status">Trạng thái dân tộc cần tìm kiếm</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa; Lấy tất cả để NULL</param>
        /// <returns></returns>
        public IEnumerable<DMDanhmuc> GetFilterByName(string name, bool exactly = false)
        {
            int _loai = (int)this._loaidanhmuc;
            if (!this.CheckNullEmptyWhiteSpaceString(name))
            {
                name = name.ToLower();
                return this.FindAll(p => (p.LoaiDanhMuc == _loai) && (exactly ? (p.TenDanhMuc.ToLower() == name) : (p.TenDanhMuc.ToLower().Contains(name))));
            }
            else
            {
                return this.FindAll(p => (p.LoaiDanhMuc == _loai));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICollection<DMDanhmuc> GetAll()
        {
            try
            {
                int _loai = (int)this._loaidanhmuc;
                if (this.GetUseCache())
                {
                    return this.CacheData().Where(p => p.LoaiDanhMuc == _loai).ToList();
                }
                return this.context.Set<DMDanhmuc>().Where(p => p.LoaiDanhMuc == _loai).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lọc danh sách dân tộc theo mã dân tộc
        /// </summary>
        /// <param name="name">Mã dân tộc</param>
        /// <param name="exactly">Lọc tương đối hay tuyệt đối</param>
        /// <param name="status">Trạng thái dân tộc cần tìm kiếm</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa; Lấy tất cả để NULL</param>
        /// <returns></returns>
        public IEnumerable<DMDanhmuc> GetFilterByCode(string code, bool exactly = false)
        {
            int _loai = (int)this._loaidanhmuc;
            if (!this.CheckNullEmptyWhiteSpaceString(code))
            {
                code = code.ToLower();
                return this.FindAll(p => (p.LoaiDanhMuc == _loai) && (exactly ? (p.MaDanhMuc.ToLower() == code) : (p.MaDanhMuc.ToLower().Contains(code))));
            }
            else
            {
                return this.FindAll(p => p.LoaiDanhMuc == _loai);
            }
        }

        /// <summary>
        /// Lọc danh sách dân tộc theo mã, tên dân tộc
        /// </summary>
        /// <param name="text">Chuỗi cần lọc: tên hoặc mã</param>
        /// <param name="exactly">Lọc tương đối hay tuyệt đối</param>
        /// <param name="status">Trạng thái dân tộc cần tìm kiếm</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa; Lấy tất cả để NULL</param>
        /// <returns></returns>
        public IEnumerable<DMDanhmuc> GetFilterByNameAndCode(string text, bool exactly = false)
        {
            int _loai = (int)this._loaidanhmuc;
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                return this.FindAll(p => (exactly ? (p.MaDanhMuc.ToLower() == text || p.TenDanhMuc.ToLower() == text) : (p.TenDanhMuc.ToLower().Contains(text) || p.MaDanhMuc.ToLower().Contains(text))) && (p.LoaiDanhMuc == _loai));
            }
            else
            {
                return this.FindAll(p => p.LoaiDanhMuc == _loai);
            }
        }
    }
}
