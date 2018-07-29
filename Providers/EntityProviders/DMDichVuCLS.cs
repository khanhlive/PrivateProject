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
    public partial class DMDichVuCLSang : MossHospitalRepository<DMDichVuCLSang, int>
    {

        #region Properties

        public string TenDichVu { get; set; }

        #endregion
        /// <summary>
        /// Hàm khởi tạo đối tượng
        /// </summary>
        public DMDichVuCLSang()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "danh mục dịch vụ cận lâm sàng";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "DM_DICHVUCANLAMSANG";
        }
        /// <summary>
        /// Hàm lấy dữ liệu cache nếu đối tượng thuộc danh sách đối tượng cache
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<DMDichVuCLSang> CacheData()
        {
            return GlobalCache.dMDichVuCLs;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.DMDichVuCL);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Update(int id, DMDichVuCLSang item, int? userID = null, bool checkPermission = false)
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
                            if (this.Any(p => p.DichVuCLS_ID == id))
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
                        if (this.Any(p => p.DichVuCLS_ID == id))
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
                return this.Update(this.DichVuCLS_ID, this, userID, checkPermission);
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
        public CoreResult Insert(DMDichVuCLSang item, int? userID = null, bool checkPermission = false)
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
                        else return per;
                    }
                    else
                    {
                        ////action
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
                            //DMDichVuCLSang dichvucls = this.Find();
                            if (this.Any(p=>p.DichVuCLS_ID==id))
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
                        //DMDichVuCLSang dichvucls = this.GetByID(id);
                        if (this.Any(p => p.DichVuCLS_ID == id))
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
                return this.Delete(this.DichVuCLS_ID, userID, checkPermission);
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
                            var dichvucls = this.GetByID(id);
                            if (dichvucls != null)
                            {
                                dichvucls.deleted = true;
                                this.context.Entry<DMDichVuCLSang>(dichvucls).State = EntityState.Modified;
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
        /// Lọc danh sách dịch vụ cận lâm sàng theo dịch vụ ID
        /// </summary>
        /// <param name="dichvuid">ID Dịch vụ</param>
        /// <param name="status">Trạng thái dịch vụ cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMDichVuCLSang> GetByDichVuID(int dichvuid, bool? status = null, bool? deleted = false)
        {
            var q = (from a in this.context.DMDichVuCLSangs.Where(p => (p.DichVuID == dichvuid) && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value))
                     join b in this.context.DMDichVus on a.DichVuID equals b.DichVuID
                     select new
                     {
                         DichVuCLS_ID = a.DichVuCLS_ID,
                         DichVuID = a.DichVuID,
                         TenDichVuCLS = a.TenDichVuCLS,
                         SoTT_Form = a.SoTT_Form,
                         SoTT_Report = a.SoTT_Report,
                         KetQuaMau = a.KetQuaMau,
                         TSBT_Nam = a.TSBT_Nam,
                         TSBT_Nu = a.TSBT_Nu,
                         deleted = a.deleted,
                         Status = a.Status,
                         TenDichVu = b.TenDichVu
                     }).ToList();

            return (from a in q
                    select new DMDichVuCLSang
                    {
                        DichVuCLS_ID = a.DichVuCLS_ID,
                        DichVuID = a.DichVuID,
                        TenDichVuCLS = a.TenDichVuCLS,
                        SoTT_Form = a.SoTT_Form,
                        SoTT_Report = a.SoTT_Report,
                        KetQuaMau = a.KetQuaMau,
                        TSBT_Nam = a.TSBT_Nam,
                        TSBT_Nu = a.TSBT_Nu,
                        deleted = a.deleted,
                        Status = a.Status,
                        TenDichVu = a.TenDichVu
                    }).ToList();
        }
        /// <summary>
        /// Lọc danh sách dịch vụ cận lâm sàng theo tên dịch vụ cận lâm sàng
        /// </summary>
        /// <param name="name">Tên dịch vụ cần lọc</param>
        /// <param name="exactly">Độ chính xác lọc: tuyệt đối hoặc tương đối</param>
        /// <param name="status">Trạng thái dịch vụ cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMDichVuCLSang> GetByDichVuName(string name, bool exactly = true, bool? status = null, bool? deleted = false)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(name))
            {
                name = name.ToLower();
                var q = (from a in this.context.DMDichVuCLSangs.Where(p => (exactly ? (p.TenDichVuCLS.ToLower() == name) : (p.TenDichVuCLS.ToLower().Contains(name))) && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value))
                         join b in this.context.DMDichVus on a.DichVuID equals b.DichVuID
                         select new
                         {
                             DichVuCLS_ID = a.DichVuCLS_ID,
                             DichVuID = a.DichVuID,
                             TenDichVuCLS = a.TenDichVuCLS,
                             SoTT_Form = a.SoTT_Form,
                             SoTT_Report = a.SoTT_Report,
                             KetQuaMau = a.KetQuaMau,
                             TSBT_Nam = a.TSBT_Nam,
                             TSBT_Nu = a.TSBT_Nu,
                             deleted = a.deleted,
                             Status = a.Status,
                             TenDichVu = b.TenDichVu
                         }).ToList();
                return (from a in q
                        select new DMDichVuCLSang
                        {
                            DichVuCLS_ID = a.DichVuCLS_ID,
                            DichVuID = a.DichVuID,
                            TenDichVuCLS = a.TenDichVuCLS,
                            SoTT_Form = a.SoTT_Form,
                            SoTT_Report = a.SoTT_Report,
                            KetQuaMau = a.KetQuaMau,
                            TSBT_Nam = a.TSBT_Nam,
                            TSBT_Nu = a.TSBT_Nu,
                            deleted = a.deleted,
                            Status = a.Status,
                            TenDichVu = a.TenDichVu
                        }).ToList();
            }
            else
            {
                var q = (from a in this.context.DMDichVuCLSangs.Where(p => status == null ? true : p.Status == status.Value && deleted == null ? true : p.deleted == deleted.Value)
                         join b in this.context.DMDichVus on a.DichVuID equals b.DichVuID
                         select new
                         {
                             DichVuCLS_ID = a.DichVuCLS_ID,
                             DichVuID = a.DichVuID,
                             TenDichVuCLS = a.TenDichVuCLS,
                             SoTT_Form = a.SoTT_Form,
                             SoTT_Report = a.SoTT_Report,
                             KetQuaMau = a.KetQuaMau,
                             TSBT_Nam = a.TSBT_Nam,
                             TSBT_Nu = a.TSBT_Nu,
                             deleted = a.deleted,
                             Status = a.Status,
                             TenDichVu = b.TenDichVu
                         }).ToList();
                return (from a in q
                        select new DMDichVuCLSang
                        {
                            DichVuCLS_ID = a.DichVuCLS_ID,
                            DichVuID = a.DichVuID,
                            TenDichVuCLS = a.TenDichVuCLS,
                            SoTT_Form = a.SoTT_Form,
                            SoTT_Report = a.SoTT_Report,
                            KetQuaMau = a.KetQuaMau,
                            TSBT_Nam = a.TSBT_Nam,
                            TSBT_Nu = a.TSBT_Nu,
                            deleted = a.deleted,
                            Status = a.Status,
                            TenDichVu = a.TenDichVu
                        }).ToList();
            }
        }
        /// <summary>
        /// Lọc danh sách dịch vụ cận lâm sàng theo danh sách mã dịch vụ
        /// </summary>
        /// <param name="dichvuids">Danh sách mã dịch vụ</param>
        /// <param name="status">Trạng thái dịch vụ cần lọc</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMDichVuCLSang> GetByDichVuID(int[] dichvuids, bool? status = null, bool? deleted = false)
        {
            var q = (from a in this.context.DMDichVuCLSangs.Where(p => dichvuids.Contains(p.DichVuID) && (status == null ? true : p.Status == status.Value) && (deleted == null ? true : p.deleted == deleted.Value))
                     join b in this.context.DMDichVus on a.DichVuID equals b.DichVuID
                     select new
                     {
                         DichVuCLS_ID = a.DichVuCLS_ID,
                         DichVuID = a.DichVuID,
                         TenDichVuCLS = a.TenDichVuCLS,
                         SoTT_Form = a.SoTT_Form,
                         SoTT_Report = a.SoTT_Report,
                         KetQuaMau = a.KetQuaMau,
                         TSBT_Nam = a.TSBT_Nam,
                         TSBT_Nu = a.TSBT_Nu,
                         deleted = a.deleted,
                         Status = a.Status,
                         TenDichVu = b.TenDichVu
                     }).ToList();
            return (from a in q
                    select new DMDichVuCLSang
                    {
                        DichVuCLS_ID = a.DichVuCLS_ID,
                        DichVuID = a.DichVuID,
                        TenDichVuCLS = a.TenDichVuCLS,
                        SoTT_Form = a.SoTT_Form,
                        SoTT_Report = a.SoTT_Report,
                        KetQuaMau = a.KetQuaMau,
                        TSBT_Nam = a.TSBT_Nam,
                        TSBT_Nu = a.TSBT_Nu,
                        deleted = a.deleted,
                        Status = a.Status,
                        TenDichVu = a.TenDichVu
                    }).ToList();
        }
    }
}
