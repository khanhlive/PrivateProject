using Moss.Hospital.Data.Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moss.Hospital.Data.Common.Enum;
using Moss.Hospital.Data.Providers;

namespace Moss.Hospital.Data.Entities
{
    public partial class CateBed : IEntity<CateBed>
    {   
        #region Method
        
        public CoreResult Delete(int? userId = default(int?), bool checkPermission = false)
        {
            return this.Delete(this.BedID, userId, checkPermission);
        }

        public CoreResult Delete(int key, int? userId = default(int?), bool checkPermission = false)
        {
            using (GiuongProvider giuongprovider = new GiuongProvider())
            {
                return giuongprovider.Delete(key, userId, checkPermission);
            }
        }
        
        public CoreResult Exist()
        {
            using (GiuongProvider giuongprovider = new GiuongProvider())
            {
                return giuongprovider.Exist(this.BedID);
            }
        }

        public CoreResult ExistByCode()
        {
            using (GiuongProvider giuongprovider = new GiuongProvider())
            {
                return giuongprovider.Exist(this.BedCode);
            }
        }

        public CateBed Get(int key)
        {
            using(GiuongProvider giuongprovider = new GiuongProvider())
            {
                return giuongprovider.Get(key);
            }
        }

        public CoreResult Get()
        {
            using (GiuongProvider giuongprovider = new GiuongProvider())
            {
                var entity= giuongprovider.Get(this.BedID);
                if (entity != null)
                {
                    #region Assign value
                    this.BedID = entity.BedID;
                    this.BedCode = entity.BedCode;
                    this.BedName = entity.BedName;
                    this.PersonNumberMax = entity.PersonNumberMax;
                    this.Discription = entity.Discription;
                    this.DepartmentsID = entity.DepartmentsID;
                    this.isActivity = entity.isActivity;
                    #endregion
                    return giuongprovider.GetResultFromStatusCode(CoreStatusCode.OK, ActionType.Get);
                }
                else
                    return giuongprovider.GetResultFromStatusCode(CoreStatusCode.Failed, ActionType.Get);
            }
        }

        public CoreResult Insert(int? userId = default(int?), bool checkPermission = false)
        {
            using (GiuongProvider giuongprovider = new GiuongProvider())
            {
                return giuongprovider.Insert(this,userId,checkPermission);
            }
        }
        
        public CoreResult Update(int? userId = default(int?), bool checkPermission = false)
        {
            using (GiuongProvider giuongprovider = new GiuongProvider())
            {
                return giuongprovider.Update(this, userId, checkPermission);
            }
        }
        
        #endregion
    }
}
