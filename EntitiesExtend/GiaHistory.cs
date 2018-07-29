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
    public partial class GiaHistory : Providers.Repositories.MossHospitalRepository<GiaHistory, int>
    {
        /// <summary>
        /// Hàm khởi tạo đối tượng
        /// </summary>
        public GiaHistory()
        {
            this.SetUseCache(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "danh mục giá dịch vụ";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "DM_GIADICHVU";
        }
        /// <summary>
        /// Hàm lấy dữ liệu cache nếu đối tượng thuộc danh sách đối tượng cache
        /// </summary>
        /// <returns></returns>
        //internal override IEnumerable<GiaHistory> CacheData()
        //{
        //    return GlobalCache.dMBenhViens;
        //}
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        //internal override void RefreshCache()
        //{
        //    GlobalCacheService globalcacheservice = new GlobalCacheService();
        //    globalcacheservice.LoadCache(CacheType.GiaHistory);
        //}

        /// <summary>
        /// Lấy ra 1 đối tượng giá dịch vụ theo mã giá dịch vụ
        /// </summary>
        /// <param name="id">Mã giá dịch vụ</param>
        /// <returns>Thành công: trả về 1 giá dịch vụ có mã là ID, Lỗi: trả về giá trị "NULL"</returns>
        public override GiaHistory GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.GiaHistoryID == id);
            //}
            return this.context.GiaHistories.FirstOrDefault(p => p.GiaHistoryID == id);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Update(int id, GiaHistory item, int? userID = null, bool checkPermission = false)
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
                        var per = this.GetPermission(userID.Value, ActionType.Edit);
                        if (per.StatusCode == CoreStatusCode.OK)
                        {
                            ////action
                            GiaHistory gia = this.GetByID(id);
                            if (gia != null)
                            {
                                gia.NgayHetHieuLuc = item.NgayHetHieuLuc;
                                gia.NgayHetHieuLuc = item.NgayHetHieuLuc;
                                gia.Status = item.Status;
                                try
                                {
                                    this.context.Entry(gia).State = EntityState.Modified;
                                    int counter = this.context.SaveChanges();
                                    this.context.Entry<GiaHistory>(gia).GetDatabaseValues();
                                    if (this.GetUseCache())
                                    {
                                        if (counter > 0)
                                        {
                                            this.RefreshCache();
                                            return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.OK, ActionType.Edit) };
                                        }
                                        else
                                        {
                                            return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Failed, ActionType.Edit) };
                                        }
                                    }
                                    return new CoreResult { StatusCode = counter > 0 ? CoreStatusCode.OK : CoreStatusCode.Failed, Message = this.GetMessageByCoreStatusCode(counter > 0 ? CoreStatusCode.OK : CoreStatusCode.Failed, ActionType.Edit) };

                                }
                                catch (Exception ex)
                                {
                                    return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
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
                        GiaHistory gia = this.GetByID(id);
                        if (gia != null)
                        {
                            gia.NgayHetHieuLuc = item.NgayHetHieuLuc;
                            gia.NgayHetHieuLuc = item.NgayHetHieuLuc;
                            gia.Status = item.Status;
                            try
                            {
                                this.context.Entry(gia).State = EntityState.Modified;
                                int counter = this.context.SaveChanges();
                                this.context.Entry<GiaHistory>(gia).GetDatabaseValues();
                                if (this.GetUseCache())
                                {
                                    if (counter > 0)
                                    {
                                        this.RefreshCache();
                                        return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.OK, ActionType.Edit) };
                                    }
                                    else
                                    {
                                        return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Failed, ActionType.Edit) };
                                    }
                                }
                                return new CoreResult { StatusCode = counter > 0 ? CoreStatusCode.OK : CoreStatusCode.Failed, Message = this.GetMessageByCoreStatusCode(counter > 0 ? CoreStatusCode.OK : CoreStatusCode.Failed, ActionType.Edit) };
                            }
                            catch (Exception ex)
                            {
                                return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
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
                return this.Update(this.GiaHistoryID, this, userID, checkPermission);
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
        public CoreResult Insert(GiaHistory item, int? userID = null, bool checkPermission = false)
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
                            if (!this.Any(p => p.DichVuID == item.DichVuID && p.GiaSD == item.GiaSD && p.patientsObjectID == item.patientsObjectID))
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
                        if (!this.Any(p => p.DichVuID == item.DichVuID && p.GiaSD == item.GiaSD && p.patientsObjectID == item.patientsObjectID))
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
                            //GiaHistory benhvien = this.GetByID(id);
                            if (this.Any(p => p.GiaHistoryID == id))
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
                        //GiaHistory benhvien = this.GetByID(id);
                        if (this.Any(p => p.GiaHistoryID == id))
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
                return this.Delete(this.GiaHistoryID, userID, checkPermission);
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
                            var giadichvu = this.GetByID(id);
                            if (giadichvu != null)
                            {
                                //nhacungcap.deleted = true;
                                this.context.Entry<GiaHistory>(giadichvu).State = EntityState.Deleted;
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

        public IEnumerable<GiaHistory> GetByDichvuID(int? dichvuid, DateTime? ngayhieuluc=null, DateTime? ngayhethieuluc = null, bool? status = null)
        {
            return this.FindAll(p => (dichvuid == null ? true : p.DichVuID == dichvuid.Value)
            && (ngayhieuluc == null ? true : p.NgayHieuLuc == ngayhieuluc.Value)
            && (ngayhethieuluc == null ? true : p.NgayHetHieuLuc == ngayhethieuluc.Value)
            && (status == null ? true : p.Status == status.Value));
        }
        public IEnumerable<GiaHistory> GetByDichvuID_Ngay_Status(int? dichvuid, DateTime? ngay = null, bool? status = null)
        {
            return this.FindAll(p => (dichvuid == null ? true : p.DichVuID == dichvuid.Value)
            && (ngay == null ? true : (p.NgayHieuLuc <= ngay.Value && p.NgayHetHieuLuc >= ngay.Value))
            && (status == null ? true : p.Status == status.Value));
        }
    }
}
