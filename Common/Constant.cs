using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Common
{
    public static class Constant
    {
        public static string KEY = ConfigurationManager.AppSettings["KEY"].ToString();
        public static string GET_ALL = "GET_ALL";
        public static string GET_ANY = "GET_ANY";
        public static string SESSION_USERID = "USERID";
        public static string SESSION_ACCOUNT = "ACCOUNT";
        public static double MAX_SIZE = 4000000;
        public static string URL_AVATAR = "/upload/avatar/";
    }
}
