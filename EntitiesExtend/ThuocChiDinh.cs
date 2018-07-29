using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moss.Hospital.Data.Common.Enum;
using System.Data.SqlClient;

namespace Moss.Hospital.Data.Entities
{
    public partial class ThuocChiDinh : Providers.Repositories.SqlEntityBase<ThuocChiDinh, int>, Providers.Repositories.ISqlAction<DichVuChiDinh>
    {
        #region MyRegion

        public CoreResult Delete(int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Delete(int key, int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Delete(DichVuChiDinh entity, int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Exist(int key)
        {
            throw new NotImplementedException();
        }

        public CoreResult Exist()
        {
            throw new NotImplementedException();
        }

        public DichVuChiDinh Get(int key)
        {
            throw new NotImplementedException();
        }

        public CoreResult Get()
        {
            throw new NotImplementedException();
        }

        public CoreResult Insert(int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Insert(DichVuChiDinh entity, int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Update(int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }

        public CoreResult Update(DichVuChiDinh entity, int? userId = default(int?), bool checkPermission = false)
        {
            throw new NotImplementedException();
        }
        

        protected override string GetFeatureCode()
        {
            return "THUOCCHIDINH";
        }

        protected override string GetNameEntity()
        {
            return "chỉ định thuốc";
        }

        public bool KiemTraThuocChuaLenPhieuLinh(int mabenhnhan)
        {
            try
            {
                this.sqlHelper.CommandType = System.Data.CommandType.Text;
                int obj = this.sqlHelper.ExecuteScalar("SELECT [dbo].[ThuocChiDinh_KiemTraThuocChuaLenPhieuLinh](@mabenhnhan)", new string[] { "@mabenhnhan" }, new object[] { mabenhnhan }, 0);
                return obj == 1;
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
