using Moss.Hospital.Data.Cache;
using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Entities
{
    public partial class DMNhomDichVu : MossHospitalRepository<DMNhomDichVu, int>
    {
        private Library.Constants.LoaiDichVu _loainhom;
        /// <summary>
        /// 
        /// </summary>
        public DMNhomDichVu(Library.Constants.LoaiDichVu loainhom)
        {
            this._loainhom = loainhom;
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        public DMNhomDichVu()
        {
            this.SetUseCache(GlobalCacheService.IsLoadCache);
        }
        public void SetNhom(Library.Constants.LoaiDichVu loainhom)
        {
            this._loainhom = loainhom;
        }
        protected override string GetFeatureCode()
        {
            if (_loainhom == Library.Constants.LoaiDichVu.DichVu)
                return "DM_NHOMDICHVU";
            else
                return "DM_NHOMDUOC";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetNameEntity()
        {
            if (_loainhom == Library.Constants.LoaiDichVu.DichVu)
                return "danh mục nhóm dịch vụ";
            else
                return "danh mục nhóm dược";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<DMNhomDichVu> CacheData()
        {
            return GlobalCache.dMNhomDichVus;
        }
        /// <summary>
        /// Khởi tạo lại cache
        /// </summary>
        internal override void RefreshCache()
        {
            GlobalCacheService globalcacheservice = new GlobalCacheService();
            globalcacheservice.LoadCache(CacheType.DMNhomDichVu);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DMNhomDichVu GetByID_DichVu(int id)
        {
            var query = this.GetByID(id);
            if (query == null)
            {
                return null;
            }
            else
            {
                int _loai = (int)Library.Constants.LoaiDichVu.DichVu;
                return query.LoaiDichVu == _loai ? query : null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DMNhomDichVu GetByID_Duoc(int id)
        {
            var query = this.GetByID(id);
            if (query == null)
            {
                return null;
            }
            else
            {
                int _loai = (int)Library.Constants.LoaiDichVu.Duoc;
                return query.LoaiDichVu == _loai ? query : null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nhomId"></param>
        /// <returns></returns>
        public virtual int GetLevel(int? nhomId)
        {
            return nhomId != null ? (int)Library.Constants.LevelsNhomDichVu.TieuNhom : (int)Library.Constants.LevelsNhomDichVu.Nhom;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual int GetLevel()
        {
            return this.GetLevel(this.NhomDichVuID_TrucThuoc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        /// <param name="checkPermission"></param>
        /// <returns></returns>
        public CoreResult Update(int id, DMNhomDichVu item, int? userID = null, bool checkPermission = false)
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
                            var obj = this.GetByID(id);
                            if (obj != null)
                            {
                                if (!this.Any(p => p.MaNhomBYT == item.MaNhomBYT && p.NhomDichVuID != item.NhomDichVuID))
                                {
                                    #region Set default value
                                    item.Levels = Convert.ToByte(this.GetLevel(item.NhomDichVuID_TrucThuoc));
                                    item.userIDCreated = obj.userIDCreated;
                                    item.dateCreated = obj.dateCreated;
                                    item.NumberUpdated = Convert.ToByte(obj.NumberUpdated + 1);
                                    item.dateUpdated = DateTime.Now;
                                    #endregion
                                    var result = base.CoreUpdate(id, item);
                                    if (result.Item1 == CoreStatusCode.OK)
                                        return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                                    else
                                        return
                                            new CoreResult { StatusCode = result.Item1, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                                }
                                else
                                {
                                    return new CoreResult { StatusCode = CoreStatusCode.Existed, Message = string.Format("Mã: \"{0}\" đã tồn tại, cập nhật thất bại.") };
                                }
                            }
                            else
                            {
                                return new CoreResult { StatusCode = CoreStatusCode.NotFound, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Edit) };
                            }
                        }
                        else return per;
                    }
                    else
                    {
                        ////action
                        var obj = this.GetByID(id);
                        if (obj != null)
                        {
                            if (!this.Any(p => p.MaNhomBYT == item.MaNhomBYT && p.NhomDichVuID != item.NhomDichVuID))
                            {
                                #region Set default value
                                item.Levels = Convert.ToByte(this.GetLevel(item.NhomDichVuID_TrucThuoc));
                                item.userIDCreated = obj.userIDCreated;
                                item.dateCreated = obj.dateCreated;
                                item.NumberUpdated = Convert.ToByte(obj.NumberUpdated + 1);
                                item.dateUpdated = DateTime.Now;
                                #endregion
                                var result = base.CoreUpdate(id, item);
                                if (result.Item1 == CoreStatusCode.OK)
                                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                                else
                                    return
                                        new CoreResult { StatusCode = result.Item1, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Edit) };
                            }
                            else
                            {
                                return new CoreResult { StatusCode = CoreStatusCode.Existed, Message = string.Format("Mã: \"{0}\" đã tồn tại, cập nhật thất bại.") };
                            }
                        }
                        else
                        {
                            return new CoreResult { StatusCode = CoreStatusCode.NotFound, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.NotFound, ActionType.Edit) };
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// Cập nhật nhóm, tiểu nhóm dịch vụ hiện thời
        /// </summary>
        /// <returns></returns>
        public CoreResult Update(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return this.Update(this.NhomDichVuID, this);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// Thêm mới nhóm, tiểu nhóm dịch vụ
        /// </summary>
        /// <param name="item">Đối tượng nhóm, tiểu nhóm dịch vụ cần thêm mới</param>
        /// <returns></returns>
        public CoreResult Insert(DMNhomDichVu item, int? userID = null, bool checkPermission = false)
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
                            if (!this.Any(p => p.MaNhomBYT == item.MaNhomBYT))
                            {
                                #region MyRegion
                                item.Levels = Convert.ToByte(this.GetLevel(item.NhomDichVuID_TrucThuoc));
                                item.Status = true;
                                item.deleted = false;
                                item.dateCreated = DateTime.Now;
                                item.dateUpdated = null;
                                item.NumberUpdated = 0;
                                #endregion

                                var result = base.CoreInsert(item);
                                if (result.Item1 == CoreStatusCode.OK)
                                    return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                                else
                                    return new CoreResult { StatusCode = result.Item1, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                            }
                            else
                            {
                                return new CoreResult { StatusCode = CoreStatusCode.Existed, Message = string.Format("Mã: \"{0}\" đã tồn tại, thêm mới thất bại.") };
                            }
                        }
                        else return per;
                    }
                    else
                    {
                        ////action
                        if (!this.Any(p => p.MaNhomBYT == item.MaNhomBYT))
                        {
                            #region MyRegion
                            item.Levels = Convert.ToByte(this.GetLevel(item.NhomDichVuID_TrucThuoc));
                            item.Status = true;
                            item.deleted = false;
                            item.dateCreated = DateTime.Now;
                            item.dateUpdated = null;
                            item.NumberUpdated = 0;
                            #endregion

                            var result = base.CoreInsert(item);
                            if (result.Item1 == CoreStatusCode.OK)
                                return new CoreResult { StatusCode = CoreStatusCode.OK, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                            else
                                return new CoreResult { StatusCode = result.Item1, Message = this.GetMessageByCoreStatusCode(result.Item1, ActionType.Insert) };
                        }
                        else
                        {
                            return new CoreResult { StatusCode = CoreStatusCode.Existed, Message = string.Format("Mã: \"{0}\" đã tồn tại, thêm mới thất bại.") };
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// Thêm mới nhóm, tiểu nhóm dịch vụ hiện thời
        /// </summary>
        /// <returns></returns>
        public CoreResult Insert(int? userID = null, bool checkPermission = false)
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
        /// Xóa nhóm, tiểu nhóm dịch vụ theo "ID"
        /// </summary>
        /// <param name="id">Mã nhóm, tiểu nhóm dịch vụ</param>
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
                            var nhomdichvu = this.GetByID(id);
                            if (nhomdichvu != null)
                            {
                                nhomdichvu.deleted = true;
                                nhomdichvu.dateUpdated = DateTime.Now;
                                nhomdichvu.NumberUpdated = Convert.ToByte(nhomdichvu.NumberUpdated + 1);
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
                        var nhomdichvu = this.GetByID(id);
                        if (nhomdichvu != null)
                        {
                            nhomdichvu.deleted = true;
                            nhomdichvu.dateUpdated = DateTime.Now;
                            nhomdichvu.NumberUpdated = Convert.ToByte(nhomdichvu.NumberUpdated + 1);
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
        /// Xóa nhóm, tiểu nhóm dịch vụ hiện thời
        /// </summary>
        /// <returns></returns>
        public CoreResult Delete(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return this.Delete(this.NhomDichVuID, userID, checkPermission);

            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Delete, ex) };
            }
        }
        /// <summary>
        /// Xóa nhóm, tiểu nhóm dịch vụ theo danh sách Mã nhóm, tiểu nhóm dịch vụ
        /// </summary>
        /// <param name="ids">Mảng int[]: danh sách mã nhóm, tiểu nhóm dịch vụ cần xóa</param>
        /// <returns></returns>
        public CoreResult Delete(int[] ids, int? userID = null, bool checkPermission = false)
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
                        var dmnhomdichvu = this.GetByID(id);
                        if (dmnhomdichvu != null)
                        {
                            dmnhomdichvu.dateUpdated = DateTime.Now;
                            dmnhomdichvu.NumberUpdated = Convert.ToByte(dmnhomdichvu.NumberUpdated + 1);
                            dmnhomdichvu.deleted = true;
                            this.context.Entry<DMNhomDichVu>(dmnhomdichvu).State = EntityState.Modified;
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
        /// <summary>
        /// Lấy danh sách nhóm dịch vụ có loại là Nhóm dịch vụ hoặc nhóm dược
        /// </summary>
        /// <param name="loaidichvu">Nhóm dịch vụ, nhóm dược</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMNhomDichVu> GetNhomDichVu(Library.Constants.LoaiDichVu loaidichvu, bool deleted = false)
        {
            int _loai = (int)loaidichvu;
            return this.FindAll(p => p.LoaiDichVu == _loai && p.deleted == deleted);
        }

        #region Nhóm dịch vụ

        public CoreResult Update_LoaiNhomDichVu(int id, DMNhomDichVu item, int? userID = null, bool checkPermission = false)
        {
            try
            {
                if (item.LoaiDichVu == (int)Library.Constants.LoaiDichVu.DichVu)
                {
                    return this.Update(id, item, userID, checkPermission);
                }
                else
                {
                    return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = "Nhóm dịch vụ này không thuộc loại \"Nhóm dịch vụ\", không thể cập nhật" };
                }
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// Cập nhật nhóm, tiểu nhóm dịch vụ có loại nhóm là "Nhóm dịch vụ" hiện thời
        /// </summary>
        /// <returns></returns>
        public CoreResult Update_LoaiNhomDichVu(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return this.Update_LoaiNhomDichVu(this.NhomDichVuID, this, userID, checkPermission);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }

        /// <summary>
        /// Lấy danh sách nhóm dịch vụ có loại là Nhóm dịch vụ: theo điều kiện là nhóm, tiểu nhóm
        /// </summary>
        /// <param name="levelnhomdichvu">Nhóm dịch vụ, tiểu nhóm dịch vụ</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMNhomDichVu> GetNhom_DichVu(Library.Constants.LevelsNhomDichVu levelnhomdichvu, bool deleted = false)
        {
            int _loai = (int)Library.Constants.LoaiDichVu.DichVu;
            int _level = (int)levelnhomdichvu;
            return this.FindAll(p => (p.Levels == _level) && (p.LoaiDichVu == _loai) && (p.deleted == deleted));
        }

        /// <summary>
        /// Lấy danh sách nhóm dịch vụ có loại là Nhóm dịch vụ và theo điều kiện:
        /// <para>Nhóm dịch vụ hoặc tiểu nhóm dịch vụ</para>
        /// <para>Chuỗi tìm kiếm</para>
        /// <para>Loại tìm kiếm</para>
        /// </summary>
        /// <param name="levelnhomdichvu">Nhóm dịch vụ hoặc tiểu nhóm dịch vụ</param>
        /// <param name="manhom">Chuỗi tìm kiếm: mã nhóm, mã tiểu nhóm, tên nhóm, tên tiểu nhóm</param>
        /// <param name="exactly">Độ chính xác: true - tìm kiếm tuyệt đối; false: tìm kiếm tương đối</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMNhomDichVu> GetNhom_DichVuByMaNhomAndText(Library.Constants.LevelsNhomDichVu levelnhomdichvu,byte? manhom=null, string text=null, bool exactly=true, bool deleted = false)
        {
            var query = this.GetNhom_DichVu(levelnhomdichvu, deleted);
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                if (exactly)
                {
                    return (from a in query where (a.TenNhom.ToLower()==text.ToLower() || (manhom==null? true: a.MaNhomBYT == manhom.Value)) select a);
                }
                else
                    return (from a in query where (a.TenNhom.ToLower().Contains(text.ToLower()) || (manhom == null ? true : a.MaNhomBYT == manhom.Value)) select a);
            }
            else
            {
                return query;
            }
        }

        /// <summary>
        /// Lấy danh sách nhóm, tiểu nhóm dịch vụ theo mã nhóm, tiểu nhóm dịch vụ
        /// </summary>
        /// <param name="nhomdichvu">Mã nhóm, mã tiểu dịch vụ</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMNhomDichVu> GetNhom_DichVuByMaNhom(int nhomdichvu, bool deleted = false)
        {
            int _loai = (int)Library.Constants.LoaiDichVu.DichVu;
            return this.FindAll(p => (p.LoaiDichVu == _loai) && (p.deleted == deleted) && (p.NhomDichVuID_TrucThuoc == nhomdichvu));
        }
        /// <summary>
        /// Lấy danh sách nhóm, tiểu nhóm dịch vụ theo mã nhóm, tiểu nhóm dịch vụ
        /// </summary>
        /// <param name="nhomdichvu">Mã nhóm, tiểu nhóm dịch vụ</param>
        /// <param name="manhom">Chuỗi tìm kiếm: mã nhóm, mã tiểu nhóm, tên nhóm, tên tiểu nhóm</param>
        /// <param name="exactly">Độ chính xác: true - tìm kiếm tuyệt đối; false: tìm kiếm tương đối</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMNhomDichVu> GetNhom_DichVuByMaNhomAndText(int nhomdichvu, byte? manhom = null, string text = null, bool exactly = false, bool deleted = false)
        {
            var query = this.GetNhom_DichVuByMaNhom(nhomdichvu, deleted);
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                if (exactly)
                {
                    return (from a in query where (manhom == null ? true : a.MaNhomBYT == manhom.Value) select a);
                }
                else
                    return (from a in query where (a.TenNhom.ToLower().Contains(text) || (manhom == null ? true : a.MaNhomBYT == manhom.Value)) select a);
            }
            else
            {
                return query;
            }
        }

        #endregion

        #region Nhóm dược

        public CoreResult Update_LoaiNhomDuoc(int id, DMNhomDichVu item, int? userID = null, bool checkPermission = false)
        {
            try
            {
                if (item.LoaiDichVu == (int)Library.Constants.LoaiDichVu.Duoc)
                {
                    return this.Update(id, item, userID, checkPermission);
                }
                else
                {
                    return new CoreResult { StatusCode = CoreStatusCode.Failed, Message = "Nhóm dịch vụ này không thuộc loại \"Nhóm dược\", không thể cập nhật" };
                }
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }
        /// <summary>
        /// Cập nhật nhóm, tiểu nhóm dịch vụ có loại nhóm là "Nhóm dược" hiện thời
        /// </summary>
        /// <returns></returns>
        public CoreResult Update_LoaiNhomDuoc(int? userID = null, bool checkPermission = false)
        {
            try
            {
                return this.Update_LoaiNhomDuoc(this.NhomDichVuID, this, userID, checkPermission);
            }
            catch (Exception ex)
            {
                return new CoreResult { StatusCode = CoreStatusCode.Exception, Message = this.GetMessageByCoreStatusCode(CoreStatusCode.Exception, ActionType.Edit, ex) };
            }
        }

        /// <summary>
        /// Lấy danh sách nhóm dịch vụ có loại là Nhóm dược: theo điều kiện là nhóm, tiểu nhóm
        /// </summary>
        /// <param name="levelnhomdichvu">Nhóm dược, tiểu nhóm</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMNhomDichVu> GetNhom_Duoc(Library.Constants.LevelsNhomDichVu levelnhomdichvu, bool deleted = false)
        {
            int _loai = (int)Library.Constants.LoaiDichVu.Duoc;
            int _level = (int)levelnhomdichvu;
            return this.FindAll(p => (p.Levels == _level) && (p.LoaiDichVu == _loai) && (p.deleted == deleted));
        }
        /// <summary>
        /// Lấy danh sách nhóm dịch vụ có loại là Nhóm dược và theo điều kiện:
        /// <para>Nhóm dịch vụ hoặc tiểu nhóm dịch vụ</para>
        /// <para>Chuỗi tìm kiếm</para>
        /// <para>Loại tìm kiếm</para>
        /// </summary>
        /// <param name="levelnhomdichvu">Nhóm dịch vụ hoặc tiểu nhóm dịch vụ</param>
        /// <param name="manhom">Chuỗi tìm kiếm: mã nhóm, mã tiểu nhóm, tên nhóm, tên tiểu nhóm</param>
        /// <param name="exactly">Độ chính xác: true - tìm kiếm tuyệt đối; false: tìm kiếm tương đối</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMNhomDichVu> GetNhom_DuocByMaNhomAndText(Library.Constants.LevelsNhomDichVu levelnhomdichvu, byte? manhom = null, string text = null, bool exactly = false, bool deleted = false)
        {
            var query = this.GetNhom_Duoc(levelnhomdichvu, deleted);
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                if (exactly)
                {
                    return (from a in query where (manhom == null ? true : a.MaNhomBYT == manhom.Value) select a);
                }
                else
                    return (from a in query where (a.TenNhom.ToLower().Contains(text) || (manhom == null ? true : a.MaNhomBYT == manhom.Value)) select a);
            }
            else
            {
                return query;
            }
        }
        /// <summary>
        /// Lấy danh sách nhóm, tiểu nhóm dược theo mã nhóm, tiểu nhóm dược
        /// </summary>
        /// <param name="nhomdichvu">Mã nhóm, mã tiểu dịch vụ</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMNhomDichVu> GetNhom_DuocByMaNhom(int nhomdichvu, bool deleted = false)
        {
            int _loai = (int)Library.Constants.LoaiDichVu.Duoc;
            return this.FindAll(p => (p.LoaiDichVu == _loai) && (p.deleted == deleted) && (p.NhomDichVuID_TrucThuoc == nhomdichvu));
        }
        /// <summary>
        /// Lấy danh sách nhóm, tiểu nhóm dược theo mã nhóm, tiểu nhóm dược
        /// </summary>
        /// <param name="nhomdichvu">Mã nhóm, tiểu nhóm dịch vụ</param>
        /// <param name="manhom">Chuỗi tìm kiếm: mã nhóm, mã tiểu nhóm, tên nhóm, tên tiểu nhóm</param>
        /// <param name="exactly">Độ chính xác: true - tìm kiếm tuyệt đối; false: tìm kiếm tương đối</param>
        /// <param name="deleted">Có hay không lấy các bản ghi đã được xóa</param>
        /// <returns></returns>
        public IEnumerable<DMNhomDichVu> GetNhom_DuocByMaNhomAndText(int nhomdichvu, byte? manhom = null, string text = null, bool exactly = false, bool deleted = false)
        {
            var query = this.GetNhom_DuocByMaNhom(nhomdichvu, deleted);
            if (!this.CheckNullEmptyWhiteSpaceString(text))
            {
                text = text.ToLower();
                if (exactly)
                {
                    return (from a in query where (manhom == null ? true : a.MaNhomBYT == manhom.Value) select a);
                }
                else
                    return (from a in query where (a.TenNhom.ToLower().Contains(text) || (manhom == null ? true : a.MaNhomBYT == manhom.Value)) select a);
            }
            else
            {
                return query;
            }
        }

        #endregion
    }
}
