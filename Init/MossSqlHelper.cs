namespace Moss.Hospital.Data.Init
{
    public class MossSqlHelper
    {
        public void Init()
        {
            try
            {
                using (Entities.MossHospitalEntities db = new Entities.MossHospitalEntities())
                {
                    Providers.Repositories.SqlHelper.ConnectString = db.Database.Connection.ConnectionString;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
