using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Entities
{
    public class ThanhToanProvider : SqlRepository
    {
        protected override string GetFeatureCode()
        {
            return "THANHTOAN";
        }

        protected override string GetNameEntity()
        {
            return "thanh toán";
        }
        public bool KiemTraThanhToan(int mabenhnhan)
        {
            try
            {
                this.sqlHelper.CommandType = System.Data.CommandType.Text;
                int obj = this.sqlHelper.ExecuteScalar("SELECT [dbo].[KiemTraBenhNhanThanhToan](@mabenhnhan)", new string[] { "@mabenhnhan" }, new object[] { mabenhnhan }, 0);
                return obj == 1;
            }
            catch (Exception e)
            {
                this.sqlHelper.Close();
                throw e;
            }

        }
    }
}
