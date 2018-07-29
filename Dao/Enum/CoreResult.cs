using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Dao.Enum
{
    public enum CoreStatusCode
    {
        OK = 0,
        Existed = -1,
        NotFound = -2,
        Failed = -3,
        ModelFailed = -4,
        Exception = -5,
        DontHavePermission = -6,
        Used = -7,
        SystemError = -8,
        NotExisted = -9
    }
    public class CoreResult
    {
        public CoreStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
        public string SqlStatusCode { get; set; }
        public ExceptionData ExceptionError { get; set; }

    }
    public class ExceptionData
    {
        public string InnerException { get; set; }
        public string Message { get; set; }
        public IEnumerable<DbEntityValidationResult> ErrorEntities { get; set; }
    }
    public class CoreResultRange<T> : CoreResult
    {
        public IEnumerable<T> Success { get; set; }
        public IEnumerable<T> Error { get; set; }
    }

    public enum ActionType
    {
        View = 1,
        Insert = 2,
        Edit = 3,
        Delete = 4,
        Print = 5,
        Get = 6
    }
}
