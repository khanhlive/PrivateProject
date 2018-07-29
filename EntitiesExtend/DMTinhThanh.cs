using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Dao.Enum;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Moss.Hospital.Data.Entities
{
    public partial class DMTinhThanh : MossHospitalRepository<DMTinhThanh, int>
    {
        /// <summary>
        /// Khởi tạo đối tượng
        /// </summary>
        public DMTinhThanh()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        protected override string GetFeatureCode()
        {
            return "DM_TINHTHANH";
        }
        protected override string GetNameEntity()
        {
            return "danh mục tỉnh thành";
        }

        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.DMTinhThanh);
        }

        internal override IEnumerable<DMTinhThanh> CacheData()
        {
            return GlobalCache.DMTinhThanhs;
        }

        /// <summary>
        /// Lấy ra 1 danh mục tỉnh thành theo ID tỉnh thành
        /// </summary>
        /// <param name="id">ID tỉnh thành</param>
        /// <returns>Thành công: trả về 1 tỉnh thành, Lỗi: trả về giá trị "NULL"</returns>
        public override DMTinhThanh GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.TinhThanhID == id);
            //}
            return base.GetByID(id);
        }
        /// <summary>
        /// Lấy ra 1 tỉnh thành theo Mã tỉnh thành(string)
        /// </summary>
        /// <param name="_code">Mã tỉnh thành</param>
        /// <param name="exactly">Lấy tương đối hay tuyệt đối mã tỉnh thành (default: true - tuyệt đối)</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>

        public DMTinhThanh GetByCode(string _code, bool? _status = null)
        {
            if (_code != null)
            {
                _code = _code.ToLower();
                return this.FindAll(p => p.TinhThanh_Code.ToLower() == _code && _status == null ? true : p.TinhThanh_Status == _status.Value).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Tìm các tỉnh thành theo danh sách mã tỉnh thành
        /// </summary>
        /// <param name="_code">Mảng mã tỉnh thành cần tìm</param>
        /// <param name="_status">Trạng thái tỉnh thành</param>
        /// <returns></returns>
        public IEnumerable<DMTinhThanh> GetByCode(string[] _code, bool? _status = null)
        {
            if (_code.Length > 0)
            {
                _code.ToList().ForEach(p => p.ToLower());
                return this.FindAll(p => _code.Contains(p.TinhThanh_Code.ToLower()) && _status == null ? true : p.TinhThanh_Status == _status.Value);
            }
            else
                return new List<DMTinhThanh>();
        }

        public CoreResult Update(int id, DMTinhThanh item, int? userID = null, bool checkPermission = false)
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
                            if (this.Any(p => p.TinhThanhID == id))
                            {
                                var result = base.CoreUpdate(id, item);
                                if (result.Item1 == CoreStatusCode.OK)
                                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                                else
                                    return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                            }
                            else return per;
                        }
                        else
                        {
                            return new CoreResult { StatusCode = CoreStatusCode.NotFound, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Edit) };
                        }

                    }
                    else
                    {
                        if (this.Any(p => p.TinhThanhID == id))
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
                return this.Update(this.TinhThanhID, this, userID, checkPermission);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        public CoreResult Insert(DMTinhThanh item, int? userID = null, bool checkPermission = false)
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
                            item.TinhThanh_Status = true;
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
                        item.TinhThanh_Status = true;
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
                            DMTinhThanh tinhthanh = this.GetByID(id);
                            if (tinhthanh != null)
                            {
                                var result = base.CoreDelete(tinhthanh);
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
                        DMTinhThanh tinhthanh = this.GetByID(id);
                        if (tinhthanh != null)
                        {
                            var result = base.CoreDelete(tinhthanh);
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
                return this.Delete(this.TinhThanhID, userID, checkPermission);
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
                            var tinhthanh = this.GetByID(id);
                            if (tinhthanh != null)
                            {
                                this.context.Entry<DMTinhThanh>(tinhthanh).State = EntityState.Deleted;
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
        /// Lọc các tỉnh thành theo mã tỉnh thành, trạng thái(status)
        /// </summary>
        /// <param name="_code">Mã tỉnh thành cần lọc dữ liệu</param>
        /// <param name="_status">Trạng thái tỉnh thành cần lọc</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <returns></returns>
        public IEnumerable<DMTinhThanh> GetFilterByCode(string _code, bool? _status = null, bool exactly = true)
        {
            if (_code != null)
            {
                _code = _code.ToLower();
                if (exactly)
                {
                    return this.FindAll(p => (p.TinhThanh_Code.ToLower() == _code )&& (_status == null ? true : p.TinhThanh_Status == _status.Value));
                }
                else
                    return this.FindAll(p => (p.TinhThanh_Code.ToLower().Contains(_code)) && (_status == null ? true : p.TinhThanh_Status == _status.Value));
            }
            else
                return null;
        }

        /// <summary>
        /// Lọc tỉnh thành theo tên tỉnh thành, trạng thái, độ chính xác lọc dữ liệu
        /// </summary>
        /// <param name="name">Tên tỉnh thành cần lọc</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <param name="_status">Trạng thái tỉnh thành cần lọc</param>
        /// <returns></returns>
        public IEnumerable<DMTinhThanh> GetFilterByName(string name, bool exactly = true, bool? _status = null)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(name))
            {
                name = name.ToLower();
                return this.FindAll(p => (exactly ? (p.TinhThanh_Name.ToLower() == name) : p.TinhThanh_Name.Contains(name) )&&( _status == null ? true : p.TinhThanh_Status == _status.Value));
            }
            else
            {
                return this.FindAll(p => _status == null ? true : p.TinhThanh_Status == _status.Value);
            }
        }

        /// <summary>
        /// Lọc các tỉnh thành theo mã, tên tỉnh thành, trạng thái(status)
        /// </summary>
        /// <param name="text">Chuỗi cần lọc dữ liệu</param>
        /// <param name="exactly">Lọc tuyệt đối hay tương đối</param>
        /// <param name="_status">Trạng thái tỉnh thành cần lọc</param>
        /// <returns></returns>
        public IEnumerable<DMTinhThanh> GetFilterByText(string text, bool exactly = true, bool? _status = null)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                return this.FindAll(p => (exactly ? (p.TinhThanh_Name.ToLower() == text || p.TinhThanh_Code.ToLower() == text) : (p.TinhThanh_Name.Contains(text) || p.TinhThanh_Code.Contains(text)) )&&( _status == null ? true : p.TinhThanh_Status == _status.Value));
            }
            else
            {
                return this.FindAll(p => (_status == null ? true : p.TinhThanh_Status == _status.Value));
            }
        }
    }
}
