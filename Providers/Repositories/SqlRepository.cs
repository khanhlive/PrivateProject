using Moss.Hospital.Data.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using System.Linq;
using System.Xml.Linq;

namespace Moss.Hospital.Data.Providers.Repositories
{
    public abstract class SqlRepository : MossDataLayerBase, IDisposable
    {
        protected SqlHelper sqlHelper;
        public SqlRepository()
        {
            this.sqlHelper = new SqlHelper();
            this.sqlHelper.Error += new SqlHelper.ErrorEventHander(SqlHelper_ErrorHander);
        }
        private void SqlHelper_ErrorHander(object sender, SqlHelperException e)
        {
            ////log error sql
        }
        protected void CreateConnection()
        {
            if (this.sqlHelper == null)
            {
                this.sqlHelper = new SqlHelper();
                this.sqlHelper.Error += new SqlHelper.ErrorEventHander(SqlHelper_ErrorHander);
            }
        }
        public virtual IEnumerable<T> GetListByDapper<T>(string query, string[] paramsName = null, object[] paramsValue = null, CommandType commandType = CommandType.Text)
        {
            if (!this.CheckNullEmptyWhiteSpaceString(this.sqlHelper.ConnectionString))
            {
                if ((paramsName != null && paramsValue != null) || (paramsName == null && paramsValue == null))
                {
                    using (IDbConnection dbConnection = new SqlConnection(this.sqlHelper.ConnectionString))
                    {
                        dbConnection.Open();
                        if (paramsValue != null && paramsName != null)
                        {
                            if (paramsValue.Length != paramsName.Length)
                            {
                                throw new Exception(string.Format("Số lượng có trong \"paramsName({0})\" và \"paramsValue{1}\" không bằng nhau", paramsName.Length, paramsValue.Length));
                            }
                            else
                            {
                                DynamicParameters parameters = new DynamicParameters();
                                for (int i = 0; i < paramsValue.Length; i++)
                                {
                                    parameters.Add(paramsName[i], paramsValue[i]);
                                }
                                return dbConnection.Query<T>(query, parameters, null, true, null, commandType);
                            }
                        }
                        else
                        {
                            return dbConnection.Query<T>(query, null, null, true, null, commandType);
                        }
                    }
                }
                else if (paramsValue == null)
                {
                    throw new NullReferenceException("paramsValue == NULL and paramsName != NULL");
                }
                else
                {
                    throw new NullReferenceException("paramsName == NULL and paramsValue != NULL");
                }

            }
            else
                throw new NullReferenceException("Connection String is null.");
        }
        public virtual IEnumerable<T> GetListByDapper<T>(string query, CommandType commandType = CommandType.Text)
        {
            try
            {
                return this.GetListByDapper<T>(query, null, null, commandType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual IEnumerable<T> GetListByDapper<T>(string query)
        {
            try
            {
                return this.GetListByDapper<T>(query, null, null, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            this.sqlHelper.Close();
            this.sqlHelper = null;
        }
    }

    public abstract class SqlEntityBase<T, TKeyType> : SqlRepository where T : class
    {
        private bool _useCustomError = false;
        protected internal void SetUseCustomError(bool _value)
        {
            _useCustomError = _value;
        }
        protected SqlConnection GetSqlConnection()
        {
            if (this.sqlHelper == null)
            {
                this.CreateConnection();
            }
            return new SqlConnection(this.sqlHelper.ConnectionString);
        }
        public virtual IEnumerable<T> GetListByDapper(string query, string[] paramsName = null, object[] paramsValue = null, CommandType commandType = CommandType.Text)
        {
            return base.GetListByDapper<T>(query, paramsName, paramsValue, commandType);
        }
        public virtual IEnumerable<T> GetListByDapper(string query, CommandType commandType = CommandType.Text)
        {
            return this.GetListByDapper(query, null, null, commandType);
        }
        public virtual IEnumerable<T> GetListByDapper(string query)
        {
            return this.GetListByDapper(query, CommandType.Text);
        }
        public virtual T GetSingleByDapper(string query, string[] paramsName = null, object[] paramsValue = null, CommandType commandType = CommandType.Text)
        {
            return base.GetListByDapper<T>(query, paramsName, paramsValue, commandType).FirstOrDefault();
        }
        public virtual T GetSingleByDapper(string query, CommandType commandType = CommandType.Text)
        {
            return this.GetSingleByDapper(query, null, null, commandType);
        }
        public virtual T GetSingleByDapper(string query)
        {
            return this.GetSingleByDapper(query, CommandType.Text);
        }
        internal virtual string GetValueMember
        {
            get
            {
                PropertyInfo keyProperty = null;
                foreach (PropertyInfo pi in typeof(T).GetProperties())
                {
                    object[] attrs = pi.GetCustomAttributes(typeof(ComboItemValueAttribute), false);
                    if (attrs != null && attrs.Length == 1)
                    {
                        keyProperty = pi;
                        break;
                    }
                }

                return keyProperty == null ? string.Empty : keyProperty.Name;
            }
        }
        internal virtual string GetDisplayMember
        {
            get
            {
                PropertyInfo displayrProperty = null;
                foreach (PropertyInfo pi in typeof(T).GetProperties())
                {
                    object[] attrs = pi.GetCustomAttributes(typeof(ComboItemTextAttribute), false);
                    if (attrs != null && attrs.Length == 1)
                    {
                        displayrProperty = pi;
                        break;
                    }
                }

                return displayrProperty == null ? string.Empty : displayrProperty.Name;
            }
        }
        public virtual string[] GetParamsName(string[] excludes = null)
        {
            var properties = typeof(T).GetProperties();
            List<string> propertiesName = new List<string>();
            if (excludes != null && excludes.Length != 0)
            {
                foreach (PropertyInfo property in properties)
                {
                    if (!excludes.Contains(property.Name) && !property.PropertyType.IsGenericType)
                    {
                        propertiesName.Add(string.Format("@{0}", property.Name));
                    }
                    else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertiesName.Add(string.Format("@{0}", property.Name));
                    }
                }
            }
            else
            {
                foreach (PropertyInfo property in properties)
                {
                    if (!property.PropertyType.IsGenericType)
                    {
                        propertiesName.Add(string.Format("@{0}", property.Name));
                    }
                    else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertiesName.Add(string.Format("@{0}", property.Name));
                    }
                }
            }
            return propertiesName.ToArray();
        }
        public virtual object[] GetParamsValue(T model, string[] excludes = null)
        {
            if (model != null)
            {
                var properties = typeof(T).GetProperties();
                List<object> propertiesValue = new List<object>();
                if (excludes != null && excludes.Length != 0)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        if (!excludes.Contains(property.Name) && !property.PropertyType.IsGenericType)
                        {
                            propertiesValue.Add(property.GetValue(model));
                        }
                        else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertiesValue.Add(property.GetValue(model));
                        }
                    }
                }
                else
                {
                    foreach (PropertyInfo property in properties)
                    {
                        if (!property.PropertyType.IsGenericType)
                        {
                            propertiesValue.Add(property.GetValue(model));
                        }
                        else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertiesValue.Add(property.GetValue(model));
                        }
                    }
                }
                return propertiesValue.ToArray();
            }
            else
            {
                throw new NullReferenceException("Đối tượng <model> không được NULL");
            }
        }
        internal virtual CoreResult GetResultFromSqlDataReader(SqlDataReader dataReader, ActionType _actionType)
        {
            if (dataReader.FieldCount > 1)
            {
                //exception
                object _status = null;
                object _message = null;
                while (dataReader.Read())
                {
                    _status = dataReader["StatusCode"];
                    _message = dataReader["ErrorMessage"];
                    break;
                }
                var result = this.GetResultFromStatusCode(this.GetStatusCode(_status), _actionType);
                result.Message = _message == null ? "" : _message.ToString();
                return result;
            }
            else
            {
                //success
                object _status = null;
                while (dataReader.Read())
                {
                    _status = dataReader[0];
                    break;
                }
                return this.GetResultFromStatusCode(this.GetStatusCode(_status), _actionType);
            }
        }
        internal CoreStatusCode GetStatusCode(object status)
        {
            if (status != null)
            {
                int _value = -999999;
                if (int.TryParse(status.ToString(), out _value))
                {
                    if (_value > 0)
                    {
                        return CoreStatusCode.OK;
                    }
                    else
                    {
                        switch (_value)
                        {
                            case 0:
                                return CoreStatusCode.OK;
                            case -1:
                                return CoreStatusCode.Existed;
                            case -2:
                                return CoreStatusCode.NotFound;
                            case -3:
                                return CoreStatusCode.Failed;
                            case -4:
                                return CoreStatusCode.ModelFailed;
                            case -5:
                                return CoreStatusCode.Exception;
                            case -6:
                                return CoreStatusCode.DontHavePermission;
                            case -7:
                                return CoreStatusCode.Used;
                            default:
                                return CoreStatusCode.SystemError;
                        }
                    }
                }
                else return CoreStatusCode.SystemError;
            }
            else
                return CoreStatusCode.SystemError;
        }
        internal virtual CoreResult GetResultFromStatusCode(CoreStatusCode statusCode, ActionType _actionType, Exception exception = null)
        {
            return new CoreResult
            {
                StatusCode = statusCode,
                Message = this.GetMessageByCoreStatusCode(statusCode, _actionType, exception),
                ExceptionError = exception == null ? null : this.GetMessageException(exception)
            };
        }
        internal string GetXMLStringDeleteMultiple(int[] primaryKeys)
        {
            try
            {
                XElement xml = null;
                if (primaryKeys != null && primaryKeys.Length > 0)
                {
                    xml = new XElement("items", from key in primaryKeys select new XElement("item", key));
                    return xml.ToString();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        internal string GetXMLStringDeleteMultiple(string[] primaryKeys)
        {
            try
            {
                XElement xml = null;
                if (primaryKeys != null && primaryKeys.Length > 0)
                {
                    xml = new XElement("items", from key in primaryKeys select new XElement("item", key));
                    return xml.ToString();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        internal string GetXMLStringDeleteMultiple(object[] primaryKeys)
        {
            try
            {
                XElement xml = null;
                if (primaryKeys != null && primaryKeys.Length > 0)
                {
                    xml = new XElement("items", from key in primaryKeys select new XElement("item", key?.ToString()));
                    return xml.ToString();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        protected CoreResult DeleteMultilple(string storeName, string[] paramsName, object[] paramsValue, int? userId = default(int?), bool checkPermission = false)
        {
            return this.CRUDBase(storeName, paramsName, paramsValue, ActionType.Delete, userId, checkPermission);
        }
        protected CoreResult AddModel(string storeName, string[] paramsName, object[] paramsValue, int? userId = default(int?), bool checkPermission = false)
        {
            return this.CRUDBase(storeName, paramsName, paramsValue, ActionType.Insert, userId, checkPermission);
        }
        protected CoreResult UpdateModel(string storeName, string[] paramsName, object[] paramsValue, int? userId = default(int?), bool checkPermission = false)
        {
            return this.CRUDBase(storeName, paramsName, paramsValue, ActionType.Edit, userId, checkPermission);
        }
        private CoreResult CRUDBase(string storeName, string[] paramsName, object[] paramsValue,ActionType _actionType, int? userId = default(int?), bool checkPermission = false)
        {
            try
            {
                #region Kiểm tra quyền thao tác
                var result = CheckPermission(userId, checkPermission, _actionType);
                #endregion
                if (result.Item1)
                {
                    CreateConnection();
                    sqlHelper.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = sqlHelper.ExecuteReader(storeName, paramsName, paramsValue);
                    if (this._useCustomError)
                        return GetCustomMessage(dr, _actionType);
                    else
                        return GetResultFromSqlDataReader(dr, _actionType);
                }
                else
                {
                    return result.Item2;
                }
            }
            catch (Exception e)
            {
                sqlHelper.Close();
                return GetResultFromStatusCode(CoreStatusCode.Exception, _actionType, e);
            }
        }
        protected CoreResult DefineCustomMessageError(object statusCode)
        {
            return null;
        }
        private CoreResult GetCustomMessage(SqlDataReader dataReader, ActionType _actionType)
        {
            try
            {
                object _status = null;
                while (dataReader.Read())
                {
                    _status = dataReader["StatusCode"];
                    break;
                }
                if (_status != null)
                {
                    return DefineCustomMessageError(_status);
                }
                else
                    return this.GetResultFromSqlDataReader(dataReader, _actionType); ;
            }
            catch (Exception e)
            {
                return GetResultFromStatusCode(CoreStatusCode.Exception, _actionType, e);
            }
        }
    }
}
