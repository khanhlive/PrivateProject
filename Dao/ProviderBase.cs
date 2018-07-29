using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Dao
{
    public class ProviderBase:IDisposable
    {
        protected Entities.MossHospitalEntities _dbContext;
        public ProviderBase()
        {
            this._dbContext = new Entities.MossHospitalEntities();
        }
        public ProviderBase(Entities.MossHospitalEntities dbContext) {
            this._dbContext = dbContext;
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
            this._dbContext = null;
        }

        public Entities.MossHospitalEntities GetContext()
        {
            if (this._dbContext == null)
            {
                this._dbContext = new Entities.MossHospitalEntities();
                return this._dbContext;
            }
            return _dbContext;
        }
    }
}
