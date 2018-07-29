using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Moss.Hospital.Data.Entities;
using Moss.Hospital.Data.Dao.Enum;
using System.Data;

namespace Moss.Hospital.Data.Providers
{
    public class GiuongProvider : SqlEntityBase<CateBed, int>, IProvider<CateBed>
    {
        #region Method

        protected override string GetFeatureCode()
        {
            return "DM_GIUONG";
        }

        protected override string GetNameEntity()
        {
            return "danh mục giường";
        }

        public CoreResult Delete(int key, int? userId = default(int?), bool checkPermission = false)
        {
            return this.DeleteMultilple("[CateBed_DeleteMultiple]", new string[] { "@IDs" }, new object[] { this.GetXMLStringDeleteMultiple(new int[] { key }) }, userId, checkPermission);
        }

        public CoreResult Delete(CateBed entity, int? userId = default(int?), bool checkPermission = false)
        {
            return this.Delete(entity.BedID, userId, checkPermission);
        }

        public CoreResult Exist(int key)
        {
            try
            {
                var obj = this.Get(key);
                return new CoreResult { StatusCode = obj == null ? CoreStatusCode.NotExisted : CoreStatusCode.Existed, Data = obj };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult Exist(string code)
        {
            try
            {
                var obj = this.GetSingleByIDAndCode(null, code);
                return new CoreResult { StatusCode = obj == null ? CoreStatusCode.NotExisted : CoreStatusCode.Existed, Data = obj };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CateBed GetSingleByIDAndCode(int? key = null, string code = null)
        {
            try
            {
                return this.GetSingleByDapper("[CateBed_GetSingle]", new string[] { "@Key", "@Code" }, new object[] { key, code }, CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CateBed Get(int key)
        {
            try
            {
                return this.GetSingleByIDAndCode(key, null);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CoreResult Insert(CateBed entity, int? userId = default(int?), bool checkPermission = false)
        {
            return AddModel("[CateBed_Insert]",
                new string[] {"@BedCode","@BedName","@PersonNumberMax","@Discription","@DepartmentsID","@isActivity"},
                new object[] {entity.BedCode,entity.BedName,entity.PersonNumberMax,entity.Discription,entity.DepartmentsID,entity.isActivity},
                userId, checkPermission
                );
        }

        public CoreResult Update(CateBed entity, int? userId = default(int?), bool checkPermission = false)
        {
            return UpdateModel("[CateBed_Update]",
                   new string[] {
                    "@BedID","@BedCode","@BedName","@PersonNumberMax","@Discription","@DepartmentsID","@isActivity"
                   },
                   new object[] {
                       entity.BedID,entity.BedCode,entity.BedName,entity.PersonNumberMax,entity.Discription,entity.DepartmentsID,entity.isActivity
                       }, userId, checkPermission);
        }

        public IEnumerable<CateBed> GetByFilter(string code = null, string name = null, bool? status = true)
        {
            try
            {
                return this.GetListByDapper("[CateBed_GetFilter]", new string[] { "@Code", "@Name", "@Status" },
                    new object[] { code, name, status });
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                throw e;
            }
        }
        #endregion
    }
}
