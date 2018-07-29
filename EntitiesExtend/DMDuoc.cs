using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Dao.Enum;
using Moss.Hospital.Data.Providers;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DMDuoc : MossHospitalRepository<DMDuoc, int>, ISqlAction<DMDuoc>
    {
        /// <summary>
        /// Hàm khởi tạo đối tượng
        /// </summary>
        public DMDuoc()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            return "danh mục dược";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetFeatureCode()
        {
            return "DM_DUOC";
        }
        /// <summary>
        /// Hàm lấy dữ liệu cache nếu đối tượng thuộc danh sách đối tượng cache
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<DMDuoc> CacheData()
        {
            return GlobalCache.DMDuocs;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.DMDuoc);
        }

        /// <summary>
        /// Lấy ra 1 đối tượng danh mục dược theo mã danh mục dược
        /// </summary>
        /// <param name="id">Mã danh mục dược</param>
        /// <returns>Thành công: trả về 1 danh mục dược có mã là ID, Lỗi: trả về giá trị "NULL"</returns>
        public override DMDuoc GetByID(int id)
        {
            //if (this.GetUseCache())
            //{
            //    return this.CacheData().FirstOrDefault(p => p.DuocID == id);
            //}
            return base.GetByID(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public CoreResult Update(int id, DMDuoc item, int? userId = default(int?), bool checkPermission = false)
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
                DMDuoc duoc = this.GetByID(id);
                if (duoc != null)
                {
                    #region Set default value update
                    item.dateCreated = duoc.dateCreated;
                    item.userIDCreated = duoc.userIDCreated;
                    item.NumberUpdated = duoc.NumberUpdated++;
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
        public CoreResult Update(int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                return this.Update(this.DuocID, this);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        public CoreResult Insert(DMDuoc item, int? userId = default(int?), bool checkPermission = false)
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
                #region Set default value

                item.deleted = false;
                item.Status = true;
                item.dateCreated = DateTime.Now;
                item.userIDUpdated = 0;
                item.dateUpdated = null;
                item.NumberUpdated = 0;

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
                var result = base.CoreDeleteTemp(id);
                if (result == CoreStatusCode.OK)
                    return new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
                return
                    new CoreResult { StatusCode = result, Message = this.GetMessageByCoreStatusCode(result, ActionType.Delete) };
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
        public CoreResult Delete(int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                return this.Delete(this.DuocID);

            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
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
                    var duoc = this.GetByID(id);
                    if (duoc != null)
                    {
                        duoc.deleted = true;
                        this.context.Entry<DMDuoc>(duoc).State = EntityState.Modified;
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

        public virtual DMDuoc GetByCode(string _code, bool exactly = false, bool deleted = false)
        {
            if (_code != null)
            {
                _code = _code.ToLower();
                if (exactly)
                {
                    return this.FindAll(p => p.MaDuoc.ToLower() == (_code)).FirstOrDefault();
                }
                else
                    return this.FindAll(p => p.MaDuoc.ToLower().Contains(_code)).FirstOrDefault();
            }
            else
                return null;
        }

        public virtual IEnumerable<DMDuoc> GetDuocByNhomDichVu(int nhomId, bool deleted = false)
        {
            try
            {
                DMNhomDichVu nhomdichvu = new DMNhomDichVu().GetByID_Duoc(nhomId);
                if (nhomdichvu != null)
                {
                    int level = nhomdichvu.GetLevel();
                    if (level == (int)Library.Constants.LevelsNhomDichVu.Nhom)
                    {
                        return (from duoc in this.FindAll(p => p.deleted == deleted)
                                join tieunhom in nhomdichvu.FindAll(p => p.NhomDichVuID_TrucThuoc == nhomId) on duoc.NhomDichVuID equals tieunhom.NhomDichVuID
                                select duoc);
                    }
                    else
                    {
                        return (from duoc in this.FindAll(p => (p.deleted == deleted) && (p.NhomDichVuID == nhomId))
                                select duoc);
                    }
                }
                else
                    return new List<DMDuoc>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual IEnumerable<DMDuoc> GetDuocByNhomDichVu_TextSearch(int nhomId, string text, bool exactly = false, bool deleted = false)
        {
            try
            {
                var query = this.GetDuocByNhomDichVu(nhomId, deleted);
                if (query != null)
                {
                    if (!this.CheckNullEmptyWhiteSpaceString(text))
                    {
                        text = text.ToLower();
                        if (exactly)
                        {
                            return (from a in query where (a.TenDuoc.ToLower() == (text) || a.MaDuoc.ToLower() == (text)) select a);
                        }
                        else
                            return (from a in query where (a.TenDuoc.ToLower().Contains(text) || a.MaDuoc.ToLower().Contains(text)) select a);
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
        public virtual IEnumerable<DMDuoc> GetDuocByText(string text, bool exactly = false, bool? deleted = false)
        {
            try
            {
                if (!this.CheckNullEmptyWhiteSpaceString(text))
                {
                    text = text.ToLower();
                    return this.FindAll(p => (exactly ? (p.TenDuoc.ToLower() == text || p.MaDuoc.ToLower() == text) : (p.TenDuoc.ToLower().Contains(text) || p.MaDuoc.ToLower().Contains(text))) && (deleted == null ? true : p.deleted == deleted.Value));
                }
                else
                    return this.FindAll(p => (deleted == null ? true : p.deleted == deleted.Value));
            }
            catch (Exception)
            {
                return null;
            }
        }
        public virtual IEnumerable<DMDuoc> GetDuocByMaDoiTuong(int madoituong, bool deleted = false)
        {
            string _code = string.Format(";{0};", madoituong).ToLower();
            return this.FindAll(p => (p.partientsObjectIDs.ToLower().Contains(_code)) && (p.deleted == deleted));
        }

        public virtual IEnumerable<DMDuoc> GetDuocByDanhSachDoiTuong(int[] madoituongs, bool deleted = false)
        {
            if (madoituongs.Length > 0)
            {
                if (madoituongs.Length == 1)
                {
                    return this.GetDuocByMaDoiTuong(madoituongs[0], deleted);
                }
                else
                {
                    var query = this.GetDuocByMaDoiTuong(madoituongs[0], deleted);
                    for (int i = 1; i < madoituongs.Length; i++)
                    {
                        string _code = string.Format(";{0};", madoituongs[i]).ToLower();
                        query = query.Where(p => p.partientsObjectIDs.ToLower().Contains(_code));
                    }
                    return query;
                }
            }
            else
            {
                return new List<DMDuoc>();
            }
        }

        public virtual IEnumerable<DMDuoc> GetDuocByMaBoPhan(int mabophan, bool deleted = false)
        {
            string _code = string.Format(";{0};", mabophan).ToLower();
            return this.FindAll(p => (p.DepartmentsIDs.ToLower().Contains(_code)) && (p.deleted == deleted));
        }

        public virtual IEnumerable<DMDuoc> GetDuocByDanhSachBoPhan(int[] mabophan, bool deleted = false)
        {
            if (mabophan.Length > 0)
            {
                if (mabophan.Length == 1)
                {
                    return this.GetDuocByMaBoPhan(mabophan[0], deleted);
                }
                else
                {
                    var query = this.GetDuocByMaBoPhan(mabophan[0], deleted);
                    for (int i = 1; i < mabophan.Length; i++)
                    {
                        string _code = string.Format(";{0};", mabophan[i]).ToLower();
                        query = query.Where(p => (p.DepartmentsIDs.ToLower().Contains(_code)));
                    }
                    return query;
                }
            }
            else
            {
                return new List<DMDuoc>();
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

        public CoreResult Update(DMDuoc entity, int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Delete(DMDuoc entity, int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Exist(int key)
        {
            throw new NotImplementedException();
        }

        public DMDuoc Get(int key)
        {
            throw new NotImplementedException();
        }
    }
}
