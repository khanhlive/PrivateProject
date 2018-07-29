using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Dao.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Entities
{
    public partial class DmCompany : Providers.MossHospitalRepository<DmCompany, int>
    {
        /// <summary>
        /// Hàm khởi tạo đối tượng
        /// </summary>
        public DmCompany()
        {
            this.SetUseCache(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "danh mục nhà cung cấp";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "DM_NHACUNGCAP";
        }
        /// <summary>
        /// Hàm lấy dữ liệu cache nếu đối tượng thuộc danh sách đối tượng cache
        /// </summary>
        /// <returns></returns>
        //internal override IEnumerable<DmCompany> CacheData()
        //{
        //    return GlobalCache.dMBenhViens;
        //}
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        //internal override void RefreshCache()
        //{
        //    GlobalCacheService globalcacheservice = new GlobalCacheService();
        //    globalcacheservice.LoadCache(CacheType.DmCompany);
        //}

        /// <summary>
        /// Lấy ra 1 đối tượng nhà cung cấp theo mã nhà cung cấp
        /// </summary>
        /// <param name="id">Mã nhà cung cấp</param>
        /// <returns>Thành công: trả về 1 nhà cung cấp có mã là ID, Lỗi: trả về giá trị "NULL"</returns>
        public override DmCompany GetByID(int id)
        {
            if (this.GetUseCache())
            {
                return this.CacheData().FirstOrDefault(p => p.CompanyID == id);
            }
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
        public CoreResult Update(int id, DmCompany item, int? userID = null, bool checkPermission = false)
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
                            if (this.Any(p => p.CompanyID == id))
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
                        if (this.Any(p => p.CompanyID == id))
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
                return this.Update(this.CompanyID, this, userID, checkPermission);
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
        public CoreResult Insert(DmCompany item, int? userID = null, bool checkPermission = false)
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
                            if (!this.Any(p => p.CompanyCode == item.CompanyCode))
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
                            {
                                return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Existed, ActionType.Insert) };
                            }

                        }
                        else return per;
                    }
                    else
                    {
                        ////action
                        if (!this.Any(p => p.CompanyCode == item.CompanyCode))
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
                        {
                            return new CoreResult { StatusCode = CoreStatusCode.Existed, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Existed, ActionType.Insert) };
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
                            //DmCompany benhvien = this.GetByID(id);
                            if (this.Any(p => p.CompanyID == id))
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
                        //DmCompany benhvien = this.GetByID(id);
                        if (this.Any(p => p.CompanyID == id))
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
                return this.Delete(this.CompanyID, userID, checkPermission);
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
                            var nhacungcap = this.GetByID(id);
                            if (nhacungcap != null)
                            {
                                nhacungcap.deleted = true;
                                this.context.Entry<DmCompany>(nhacungcap).State = EntityState.Modified;
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
        /// Lọc danh sách nhà cung cấp theo tên
        /// </summary>
        /// <param name="name">Tên nhà cung cấp</param>
        /// <param name="exactly">Độ chính xác: tuyệt đối hay tương đối</param>
        /// <param name="status">Trạng thái nhà cung cấp cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DmCompany> GetFilterByName(string name, bool exactly = false, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(name))
            {
                name = name.ToLower();
                return this.FindAll(p => (exactly ? (p.CompanyName.ToLower() == name) : (p.CompanyName.ToLower().Contains(name))) && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
            }
            else
                return this.FindAll(p => (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
        }
        /// <summary>
        /// Lọc danh sách nhà cung cấp theo mã
        /// </summary>
        /// <param name="code">Mã nhà cung cấp</param>
        /// <param name="exactly">Độ chính xác: tuyệt đối hay tương đối</param>
        /// <param name="status">Trạng thái nhà cung cấp cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DmCompany> GetFilterByCode(string code, bool exactly = false, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(code))
            {
                code = code.ToLower();
                return this.FindAll(p => (exactly ? (p.CompanyCode.ToLower() == code) : (p.CompanyCode.ToLower().Contains(code))) && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
            }
            else
                return this.FindAll(p => (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value));
        }
        /// <summary>
        /// Lấy nhà cung cấp theo mã
        /// </summary>
        /// <param name="code">Mã nhà cung cấp</param>
        /// <param name="status">Trạng thái nhà cung cấp cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public DmCompany GetByCode(string code, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(code))
            {
                code = code.ToLower();
                return this.Find(p => (p.CompanyCode.ToLower() == code) && (status == null ? true : p.Status == status.Value )&& (deleted == null ? true : p.deleted == deleted.Value));
            }
            else
                return null;
        }
        /// <summary>
        /// Lọc danh sách nhà cung cấp theo chuỗi lọc, loại, trạng thái, deleted
        /// </summary>
        /// <param name="text">Chuỗi cần tìm kiếm</param>
        /// <param name="loai">Loại</param>
        /// <param name="exactly">Độ chính xác: lọc tuyệt đối hay tương đối</param>
        /// <param name="status">Trạng thái nhà cung cấp</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DmCompany> GetFilter(string text, bool exactly = false, int? loai=null, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                return this.FindAll(p => (loai == null ? true : p.Loai == loai.Value) &&( exactly ? p.CompanyCode.ToLower() == text : p.CompanyCode.ToLower().Contains(text)) && (status == null ? true : p.Status == status.Value )&& (deleted == null ? true : p.deleted == deleted.Value));
            }
            else
                return this.FindAll(p => (loai == null ? true : p.Loai == loai.Value )&&( status == null ? true : p.Status == status.Value) &&( deleted == null ? true : p.deleted == deleted.Value));
        }
    }
}
