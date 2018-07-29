using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Dao.Enum;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Entities
{
    public partial class DMQuanHuyen : MossHospitalRepository<DMQuanHuyen, int>
    {
        /// <summary>
        /// Khởi tạo đối tượng
        /// </summary>
        public DMQuanHuyen()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        internal override IEnumerable<DMQuanHuyen> CacheData()
        {
            return GlobalCache.DMQuanHuyens;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.DMQuanHuyen);
        }

        protected override string GetFeatureCode()
        {
            return "DM_QUANHUYEN";
        }
        protected override string GetNameEntity()
        {
            return "danh mục quận huyện";
        }

        ///// <summary>
        ///// Khởi tạo lại cache
        ///// </summary>
        //internal override void RefreshCache()
        //{
        //    GlobalCacheService globalcacheservice = new GlobalCacheService();
        //    globalcacheservice.LoadCache(CacheType.DMQuanHuyen);
        //}

        /// <summary>
        /// Lấy ra 1 danh mục quận huyện theo ID quận huyện
        /// </summary>
        /// <param name="id">ID quận huyện</param>
        /// <returns>Thành công: trả về 1 quận huyện, Lỗi: trả về giá trị "NULL"</returns>
        public override DMQuanHuyen GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.QuanHuyenID == id);
            //}
            return base.GetByID(id);
        }
        /// <summary>
        /// Lấy ra 1 quận huyện theo Mã quận huyện(string)
        /// </summary>
        /// <param name="code">Mã quận huyện</param>
        /// <param name="exactly">Lấy tương đối hay tuyệt đối mã quận huyện (default: true - tuyệt đối)</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>

        public DMQuanHuyen GetByCode(string code, bool? status = null)
        {
            if (code != null)
            {
                code = code.ToLower();
                return this.FindAll(p => p.QuanHuyen_Code.ToLower() == code && status == null ? true : p.QuanHuyen_Status == status.Value).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Tìm các quận huyện theo danh sách mã quận huyện
        /// </summary>
        /// <param name="code">Mảng mã quận huyện cần tìm</param>
        /// <param name="status">Trạng thái quận huyện</param>
        /// <returns></returns>
        public IEnumerable<DMQuanHuyen> GetByCode(string[] code, bool? status = null)
        {
            if (code.Length > 0)
            {
                code.ToList().ForEach(p => p.ToLower());
                return this.FindAll(p => code.Contains(p.QuanHuyen_Code.ToLower()) && status == null ? true : p.QuanHuyen_Status == status.Value);
            }
            else
                return new List<DMQuanHuyen>();
        }
        public CoreResult Update(int id, DMQuanHuyen item, int? userID = null, bool checkPermission = false)
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
                            if (this.Any(p => p.QuanHuyenID == id))
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
                        if (this.Any(p => p.QuanHuyenID == id))
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
                return this.Update(this.QuanHuyenID, this, userID, checkPermission);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        public CoreResult Insert(DMQuanHuyen item, int? userID = null, bool checkPermission = false)
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
                            item.QuanHuyen_Status = true;
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
                        item.QuanHuyen_Status = true;
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
                            DMQuanHuyen quanhuyen = this.GetByID(id);
                            if (quanhuyen != null)
                            {
                                var result = base.CoreDelete(quanhuyen);
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
                        DMQuanHuyen quanhuyen = this.GetByID(id);
                        if (quanhuyen != null)
                        {
                            var result = base.CoreDelete(quanhuyen);
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
                return this.Delete(this.QuanHuyenID, userID, checkPermission);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
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
                            var quanhuyen = this.GetByID(id);
                            if (quanhuyen != null)
                            {
                                this.context.Entry<DMQuanHuyen>(quanhuyen).State = EntityState.Deleted;
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
        /// Lọc các quận huyện theo mã quận huyện, trạng thái(status)
        /// </summary>
        /// <param name="_code">Mã quận huyện cần lọc dữ liệu</param>
        /// <param name="status">Trạng thái quận huyện cần lọc</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <returns></returns>
        public IEnumerable<DMQuanHuyen> GetFilterByCode(string _code, bool? status = null, bool exactly = true)
        {
            if (_code != null)
            {
                _code = _code.ToLower();
                if (exactly)
                {
                    return this.FindAll(p => (p.QuanHuyen_Code.ToLower() == _code) && (status == null ? true : p.QuanHuyen_Status == status.Value));
                }
                else
                    return this.FindAll(p => (p.QuanHuyen_Code.ToLower().Contains(_code)) && (status == null ? true : p.QuanHuyen_Status == status.Value));
            }
            else
                return null;
        }

        /// <summary>
        /// Lọc quận huyện theo tên quận huyện, trạng thái, độ chính xác lọc dữ liệu
        /// </summary>
        /// <param name="name">Tên quận huyện cần lọc</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <param name="status">Trạng thái quận huyện cần lọc</param>
        /// <returns></returns>
        public IEnumerable<DMQuanHuyen> GetFilterByName(string name, bool exactly = true, bool? status = null)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(name))
            {
                name = name.ToLower();
                return this.FindAll(p => (exactly ? (p.QuanHuyen_Name.ToLower() == name) : p.QuanHuyen_Name.Contains(name) )&& (status == null ? true : p.QuanHuyen_Status == status.Value));
            }
            else
            {
                return this.FindAll(p => (status == null ? true : p.QuanHuyen_Status == status.Value));
            }
        }

        /// <summary>
        /// Lọc các quận huyện theo mã, tên quận huyện, trạng thái(status)
        /// </summary>
        /// <param name="text">Chuỗi cần lọc dữ liệu</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <param name="status">Trạng thái quận huyện cần lọc</param>
        /// <returns></returns>
        public IEnumerable<DMQuanHuyen> GetFilterByText(string text, bool exactly = true, bool? status = null)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                return this.FindAll(p => (exactly ? (p.QuanHuyen_Name.ToLower() == text || p.QuanHuyen_Code.ToLower() == text) : (p.QuanHuyen_Name.Contains(text) || p.QuanHuyen_Code.Contains(text))) && (status == null ? true : p.QuanHuyen_Status == status.Value));
            }
            else
            {
                return this.FindAll(p => status == null ? true : p.QuanHuyen_Status == status.Value);
            }
        }

        /// <summary>
        /// Lọc danh sách quận huyện theo ID tỉnh thành
        /// </summary>
        /// <param name="idTinh">ID tỉnh thành</param>
        /// <param name="status">Trạng thái quận huyện cần lọc</param>
        /// <returns></returns>
        public IEnumerable<DMQuanHuyen> GetByTinhID(int idTinh, bool? status = null)
        {
            return this.FindAll(p => (p.TinhThanhID == idTinh) && (status == null ? true : p.QuanHuyen_Status == status.Value));
        }
        /// <summary>
        /// Lọc danh sách quận huyện theo mã tỉnh thành
        /// </summary>
        /// <param name="code">Mã tỉnh thành</param>
        /// <param name="_status">Trạng thái quận huyện cần lọc dữ liệu</param>
        /// <returns></returns>
        public IEnumerable<DMQuanHuyen> GetByTinhCode(string code, bool? _status = null)
        {
            code = code.ToLower();
            return (from a in this.context.DMQuanHuyens.Where(p => (_status == null ? true : p.QuanHuyen_Status == _status.Value)) join b in this.context.DMTinhThanhs.Where(p => (p.TinhThanh_Code.ToLower() == code)) on a.TinhThanhID equals b.TinhThanhID select a);

        }
    }
}
