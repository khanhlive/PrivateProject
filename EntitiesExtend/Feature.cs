using Moss.Hospital.Data.Dao.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moss.Hospital.Data.Cache;

namespace Moss.Hospital.Data.Entities
{
    public partial class Feature: Providers.Repositories.MossHospitalRepository<Feature, int>
    {

        /// <summary>
        /// Hàm khởi tạo đối tượng
        /// </summary>
        public Feature()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "danh mục chức năng";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "DM_CHUCNANG";
        }
        /// <summary>
        /// Hàm lấy dữ liệu cache nếu đối tượng thuộc danh sách đối tượng cache
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<Feature> CacheData()
        {
            return GlobalCache.Features;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.Feature);
        }

        /// <summary>
        /// Lấy ra 1 đối tượng chức năng theo mã chức năng
        /// </summary>
        /// <param name="id">Mã chức năng</param>
        /// <returns>Thành công: trả về 1 chức năng có mã là ID, Lỗi: trả về giá trị "NULL"</returns>
        public override Feature GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.FeatureID == id);
            //}
            return base.GetByID(id);
        }

        public CoreResult Update(int id, Feature item, int? userID = null, bool checkPermission = false)
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
                            if (this.Any(p => p.FeatureID == id))
                            {
                                if (item.FeatureID_TrucThuoc != null)
                                {
                                    var objParent = this.Find(p => p.FeatureID == item.FeatureID_TrucThuoc);
                                    if (objParent != null)
                                    {
                                        item.Levels = Convert.ToByte(objParent.Levels + 1);
                                    }
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
                        if (this.Any(p => p.FeatureID == id))
                        {
                            if (item.FeatureID_TrucThuoc != null)
                            {
                                var objParent = this.Find(p => p.FeatureID == item.FeatureID_TrucThuoc);
                                if (objParent != null)
                                {
                                    item.Levels = Convert.ToByte(objParent.Levels + 1);
                                }
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
                return this.Update(this.FeatureID, this, userID, checkPermission);
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
        public CoreResult Insert(Feature item, int? userID = null, bool checkPermission = false)
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
                            if (!this.Any(p => p.FeatureCode == item.FeatureCode))
                            {
                                #region Set default value
                                item.Status = true;
                                if (item.FeatureID_TrucThuoc!=null)
                                {
                                    var objParent = this.Find(p => p.FeatureID == item.FeatureID_TrucThuoc);
                                    if (objParent!=null)
                                    {
                                        item.Levels = Convert.ToByte(objParent.Levels + 1);
                                    }
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
                                return new CoreResult { StatusCode = CoreStatusCode.Existed, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Existed, ActionType.Insert) };
                            }
                        }
                        else return per;
                    }
                    else
                    {
                        ////action
                        if (!this.Any(p => p.FeatureCode == item.FeatureCode))
                        {
                            #region Set default value
                            item.Status = true;
                            if (item.FeatureID_TrucThuoc != null)
                            {
                                var objParent = this.Find(p => p.FeatureID == item.FeatureID_TrucThuoc);
                                if (objParent != null)
                                {
                                    item.Levels = Convert.ToByte(objParent.Levels + 1);
                                }
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
                            if (this.Any(p => p.FeatureID == id))
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
                        if (this.Any(p => p.FeatureID == id))
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
                return this.Delete(this.FeatureID, userID, checkPermission);
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
                            var feture = this.GetByID(id);
                            if (feture != null)
                            {
                                this.context.Entry<Feature>(feture).State = EntityState.Deleted;
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
    }
}
