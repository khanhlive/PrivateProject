using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Providers;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DMDichVu : MossHospitalRepository<DMDichVu, int>, ISqlAction<DMDichVu>
    {
        /// <summary>
        /// 
        /// </summary>
        public DMDichVu()
        {
            this.DichVuChiDinhDetails = new HashSet<DichVuChiDinhDetail>();
            this.DMDichVuCLSangs = new HashSet<DMDichVuCLSang>();
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "DM_DICHVU";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "dich vụ";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<DMDichVu> CacheData()
        {
            return GlobalCache.dMDichVus;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.DMDichVu);
        }

        /// <summary>
        /// Lấy ra 1 dịch vụ theo mã dịch vụ
        /// </summary>
        /// <param name="id">Mã dịch vụ</param>
        /// <returns>Thành công: trả về 1 dịch vụ có mã là ID, Lỗi: trả về giá trị "NULL"</returns>
        public override DMDichVu GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.DichVuID== id);
            //}
            return base.GetByID(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public CoreResult Update(int id, DMDichVu item, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                #region Kiểm tra quyền
                var per = this.CheckPermission(userId, checkPermission, ActionType.Edit);
                if (!per.Item1)
                {
                    return per.Item2;
                }
                #endregion
                DMDichVu dichvu = this.GetByID(id);
                if (dichvu != null)
                {
                    #region MyRegion
                    item.NumberUpdated = Convert.ToByte(dichvu.NumberUpdated + 1);
                    item.dateCreated = dichvu.dateCreated;
                    item.userIDCreated = dichvu.userIDCreated;
                    item.dateUpdated = DateTime.Now;
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
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CoreResult Update(int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                return this.Update(this.DichVuID, this);
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
        /// <returns></returns>
        public CoreResult Insert(DMDichVu item, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {

                #region Kiểm tra quyền
                var per = this.CheckPermission(userId, checkPermission, ActionType.Insert);
                if (!per.Item1)
                {
                    return per.Item2;
                }
                #endregion
                #region MyRegion
                item.Status = true;
                item.deleted = false;
                item.dateCreated = DateTime.Now;
                item.dateUpdated = null;
                item.NumberUpdated = 0;
                item.userIDUpdated = 0;
                #endregion
                var result = base.CoreInsert(item);
                if (result.Item1 == CoreStatusCode.OK)
                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                else
                    return new CoreResult { StatusCode = result.Item1, Data = item, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Data = item, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CoreResult Insert(int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                return this.Insert(this);
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
        /// <returns></returns>
        public CoreResult Delete(int id, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                #region Kiểm tra quyền
                var per = this.CheckPermission(userId, checkPermission, ActionType.Delete);
                if (!per.Item1)
                {
                    return per.Item2;
                }
                #endregion
                DMDichVu dichvu = this.GetByID(id);
                if (dichvu!=null)
                {
                    dichvu.dateUpdated = DateTime.Now;
                    dichvu.deleted = true;
                    dichvu.NumberUpdated = Convert.ToByte(dichvu.NumberUpdated + 1);
                    var result = base.CoreDeleteTemp(dichvu);
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
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CoreResult Delete(int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                return this.Delete(this.DichVuID);

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
        /// <returns></returns>
        public CoreResult Delete(int[] ids, int? userId = default(int?), bool checkPermission = false)
        {
            #region Kiểm tra quyền
            var per = this.CheckPermission(userId, checkPermission, ActionType.Delete);
            if (!per.Item1)
            {
                return per.Item2;
            }
            #endregion
            CoreResult resultReturn = new CoreResult();
            using (DbContextTransaction dbTransaction = this.context.Database.BeginTransaction())
            {
                foreach (int id in ids)
                {
                    var dichvu = this.GetByID(id);
                    if (dichvu != null)
                    {
                        dichvu.deleted = true;
                        dichvu.dateUpdated = DateTime.Now;
                        dichvu.NumberUpdated = Convert.ToByte(dichvu.NumberUpdated + 1);
                        this.context.Entry<DMDichVu>(dichvu).State = EntityState.Modified;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_code"></param>
        /// <param name="exactly"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public virtual DMDichVu GetByCode(string _code,bool exactly=true,bool deleted=false)
        {
            if (_code != null)
            {
                _code = _code.ToLower();
                if (exactly)
                {
                    return this.FindAll(p => p.MaDichVu.ToLower() == (_code)).FirstOrDefault();
                }
                else
                    return this.FindAll(p => p.MaDichVu.ToLower().Contains(_code)).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nhomId"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public virtual IEnumerable<DMDichVu> GetDichVuByNhomDichVu(int nhomId, bool deleted = false)
        {
            try
            {
                DMNhomDichVu nhomdichvu = new DMNhomDichVu().GetByID(nhomId);
                if (nhomdichvu != null)
                {
                    int level = nhomdichvu.GetLevel();
                    if (level==(int)Library.Constants.LevelsNhomDichVu.Nhom)
                    {
                        return (from dichvu in this.FindAll(p => p.deleted == deleted)
                                join tieunhom in nhomdichvu.FindAll(p => (p.NhomDichVuID_TrucThuoc == nhomId)) on dichvu.NhomDichVuID equals tieunhom.NhomDichVuID
                                select dichvu);
                    }
                    else
                    {
                        return (from dichvu in this.FindAll(p =>( p.deleted == deleted )&& (p.NhomDichVuID == nhomId))
                                select dichvu);
                    }
                }
                else
                    return new List<DMDichVu>();
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nhomId"></param>
        /// <param name="text"></param>
        /// <param name="exactly"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public virtual IEnumerable<DMDichVu> GetDichVuByNhomDichVu_TextSearch(int nhomId, string text, bool exactly = false,bool deleted = false)
        {
            try
            {
                var query = this.GetDichVuByNhomDichVu(nhomId, deleted);
                if (query != null)
                {
                    if (!this.CheckNullEmptyWhiteSpaceString(text))
                    {
                        text = text.ToLower();
                        if (exactly)
                        {
                            return (from a in query where (a.TenDichVu.ToLower() == (text) || a.MaDichVu.ToLower() == (text)) select a);
                        }
                        else
                            return (from a in query where (a.TenDichVu.ToLower().Contains(text) || a.MaDichVu.ToLower().Contains(text)) select a);
                    }
                    else
                        return query;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="exactly"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public virtual IEnumerable<DMDichVu> GetDichvuByText(string text, bool exactly = false, bool? deleted = false)
        {
            try
            {
                if (!this.CheckNullEmptyWhiteSpaceString(text))
                {
                    text = text.ToLower();
                    return this.FindAll(p => (exactly ? (p.TenDichVu.ToLower() == text || p.MaDichVu.ToLower() == text) : (p.TenDichVu.ToLower().Contains(text) || p.MaDichVu.ToLower().Contains(text))) && (deleted == null ? true : p.deleted == deleted.Value));
                }
                else
                    return this.FindAll(p => (deleted == null ? true : p.deleted == deleted.Value));
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="madoituong"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public virtual IEnumerable<DMDichVu> GetDichVuByMaDoiTuong(int madoituong, bool deleted=false)
        {
            string _code = string.Format(";{0};", madoituong).ToLower();
            return this.FindAll(p => p.partientsObjectIDs.ToLower().Contains(_code) && p.deleted == deleted);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="madoituongs"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public virtual IEnumerable<DMDichVu> GetDichVuByDanhSachDoiTuong(int[] madoituongs, bool deleted = false)
        {
            if (madoituongs.Length>0)
            {
                if (madoituongs.Length == 1)
                {
                    return this.GetDichVuByMaDoiTuong(madoituongs[0], deleted);
                }
                else
                {
                    var query = this.GetDichVuByMaDoiTuong(madoituongs[0], deleted);
                    for (int i = 1; i < madoituongs.Length; i++)
                    {
                        string _code= string.Format(";{0};", madoituongs[i]).ToLower();
                        query = query.Where(p => p.partientsObjectIDs.ToLower().Contains(_code));
                    }
                    return query;
                }
            }
            else
            {
                return new List<DMDichVu>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mabophan"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public virtual IEnumerable<DMDichVu> GetDichVuByMaBoPhan(int mabophan, bool deleted = false)
        {
            string _code = string.Format(";{0};", mabophan).ToLower();
            return this.FindAll(p => p.DepartmentsIDs.ToLower().Contains(_code) && p.deleted == deleted);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mabophan"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public virtual IEnumerable<DMDichVu> GetDichVuByDanhSachBoPhan(int[] mabophan, bool deleted = false)
        {
            if (mabophan.Length > 0)
            {
                if (mabophan.Length == 1)
                {
                    return this.GetDichVuByMaBoPhan(mabophan[0], deleted);
                }
                else
                {
                    var query = this.GetDichVuByMaBoPhan(mabophan[0], deleted);
                    for (int i = 1; i < mabophan.Length; i++)
                    {
                        string _code = string.Format(";{0};", mabophan[i]).ToLower();
                        query = query.Where(p => p.DepartmentsIDs.ToLower().Contains(_code));
                    }
                    return query;
                }
            }
            else
            {
                return new List<DMDichVu>();
            }
        }

        public CoreResult Exist()
        {
            throw new NotImplementedException();
        }

        public CoreResult Get()
        {
            throw new NotImplementedException();
        }

        public CoreResult Update(DMDichVu entity, int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Delete(DMDichVu entity, int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Exist(int key)
        {
            throw new NotImplementedException();
        }

        public DMDichVu Get(int key)
        {
            throw new NotImplementedException();
        }
    }
}
