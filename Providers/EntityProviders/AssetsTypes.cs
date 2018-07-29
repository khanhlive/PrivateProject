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
    public partial class AssetsType : Providers.MossHospitalRepository<AssetsType, int>
    {
        /// <summary>
        /// Hàm khởi tạo đối tượng
        /// </summary>
        public AssetsType()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "danh mục loại tài sản";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "DM_LOAITAISAN";
        }
        /// <summary>
        /// Hàm lấy dữ liệu cache nếu đối tượng thuộc danh sách đối tượng cache
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<AssetsType> CacheData()
        {
            return GlobalCache.AssetsTypes;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.AssetsType);
        }

        /// <summary>
        /// Lấy ra 1 đối tượng loại tài sản theo mã loại tài sản
        /// </summary>
        /// <param name="id">Mã loại tài sản</param>
        /// <returns>Thành công: trả về 1 loại tài sản có mã là ID, Lỗi: trả về giá trị "NULL"</returns>
        public override AssetsType GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.AssetsTypesID == id);
            //}
            return base.GetByID(id);
        }
        

        protected int GetLevel(int parentId)
        {
            var obj = this.GetByID(parentId);
            return obj == null ? -1 : obj.levels + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Update(int id, AssetsType item, int? userID = null, bool checkPermission = false)
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
                            if (this.Any(p => p.AssetsTypesID == id))
                            {
                                if (item.AssetsTypesID_parents != 0 && item.AssetsTypesID_parents != null)
                                {
                                    item.levels = this.GetLevel(item.AssetsTypesID_parents.Value);
                                }
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
                        if (this.Any(p => p.AssetsTypesID == id))
                        {
                            if (item.AssetsTypesID_parents != 0 && item.AssetsTypesID_parents != null)
                            {
                                item.levels = this.GetLevel(item.AssetsTypesID_parents.Value);
                            }
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
                return this.Update(this.AssetsTypesID, this, userID, checkPermission);
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
        public CoreResult Insert(AssetsType item, int? userID = null, bool checkPermission = false)
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
                            if (!this.Any(p => p.TypesCode == item.TypesCode))
                            {
                                #region Set default value
                                item.Status = true;
                                item.deleted = false;
                                if (item.AssetsTypesID_parents != 0&& item.AssetsTypesID_parents != null)
                                {
                                    item.levels = this.GetLevel(item.AssetsTypesID_parents.Value);
                                }
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
                        if (!this.Any(p => p.TypesCode == item.TypesCode))
                        {
                            #region Set default value
                            item.Status = true;
                            item.deleted = false;
                            if (item.AssetsTypesID_parents != 0 && item.AssetsTypesID_parents != null)
                            {
                                item.levels = this.GetLevel(item.AssetsTypesID_parents.Value);
                            }
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
                            if (this.Any(p => p.AssetsTypesID == id))
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
                        if (this.Any(p => p.AssetsTypesID == id))
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
                return this.Delete(this.AssetsTypesID, userID, checkPermission);
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
                            var loaitaisan = this.GetByID(id);
                            if (loaitaisan != null)
                            {
                                loaitaisan.deleted = true;
                                this.context.Entry<AssetsType>(loaitaisan).State = EntityState.Modified;
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
        public AssetsType GetByCode(string code, bool? deleted = false, bool? status = null)
        {
            return this.Find(p => (p.TypesCode == code) && (deleted == null ? true : p.deleted == deleted.Value) && (status == null ? true : p.Status == status.Value));
        }
        public IEnumerable<AssetsType> GetFilterByCode(string code, bool? deleted = false, bool? status = null)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(code))
            {
                code = code.ToLower();
                return this.FindAll(p => (p.TypesCode == code) && (deleted == null ? true : p.deleted == deleted.Value) && (status == null ? true : p.Status == status.Value));
            }
            else
            {
                return this.FindAll(p => (deleted == null ? true : p.deleted == deleted.Value) && (status == null ? true : p.Status == status.Value));
            }
        }
        public IEnumerable<AssetsType> GetFilterByLevel(int level = 0, bool? deleted = false, bool? status = null)
        {
            return this.FindAll(p => (p.levels == level) && (deleted == null ? true : p.deleted == deleted.Value) && (status == null ? true : p.Status == status.Value));
        }
        public IEnumerable<AssetsType> GetFilterByTypeGroup(int groupType = 0, int? level = null, bool? deleted = false, bool? status = null)
        {
            return this.FindAll(p => (level == null ? true : p.levels == level)
            && (p.TypesGroup == groupType) && (deleted == null ? true : p.deleted == deleted.Value)
            && (status == null ? true : p.Status == status.Value));

        }
        public IEnumerable<AssetsType> GetFilterByParentId(int parentId, int? level = null, bool? deleted = false, bool? status = null)
        {
            return this.FindAll(p => (level == null ? true : p.levels == level)
            && (p.AssetsTypesID_parents == parentId) && (deleted == null ? true : p.deleted == deleted.Value)
            && (status == null ? true : p.Status == status.Value));
        }
        public IEnumerable<AssetsType> GetFilterByName(string name, bool exactly = true, int? level = null, bool? deleted = false, bool? status = null)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(name))
            {
                name = name.ToLower();
                return this.FindAll(p => (level == null ? true : p.levels == level)
           && (exactly ? p.TypesName.ToLower() == name : p.TypesName.ToLower().Contains(name)) && (deleted == null ? true : p.deleted == deleted.Value)
           && (status == null ? true : p.Status == status.Value));
            }
            else
            {
                return this.FindAll(p => (level == null ? true : p.levels == level)
            && (deleted == null ? true : p.deleted == deleted.Value)
           && (status == null ? true : p.Status == status.Value));
            }
        }
        public IEnumerable<AssetsType> GetFilterByText(string text, bool exactly = true, int? level = null, bool? deleted = false, bool? status = null)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                return this.FindAll(p => (level == null ? true : p.levels == level)
           && (exactly ? (p.TypesName.ToLower() == text || p.TypesCode.ToLower() == text) : (p.TypesName.ToLower().Contains(text) || p.TypesCode.ToLower().Contains(text))) && (deleted == null ? true : p.deleted == deleted.Value)
           && (status == null ? true : p.Status == status.Value));
            }
            else
            {
                return this.FindAll(p => (level == null ? true : p.levels == level)
            && (deleted == null ? true : p.deleted == deleted.Value)
           && (status == null ? true : p.Status == status.Value));
            }
        }
    }
}
