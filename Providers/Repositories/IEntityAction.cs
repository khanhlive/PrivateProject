using Moss.Hospital.Data.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Moss.Hospital.Data.Providers.Repositories
{
    /// <summary>
    /// Dùng cho các Đối tượng CSDL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntity<T> where T : class
    {
        CoreResult Insert(int? userId = null, bool checkPermission = false);
        CoreResult Update(int? userId = null, bool checkPermission = false);
        CoreResult Delete(int? userId = null, bool checkPermission = false);
        CoreResult Exist();
        CoreResult Get();
    }
    public interface IInsert<T> where T : class
    {
        CoreResult Insert(T entity, int? userId = null, bool checkPermission = false);
    }
    public interface IUpdate<T> where T : class
    {
        CoreResult Update(T entity, int? userId = null, bool checkPermission = false);
    }
    public interface IDelete<T> where T : class
    {
        CoreResult Delete(T entity, int? userId = null, bool checkPermission = false);
    }

    /// <summary>
    /// Dùng cho các đối tượng Cung cấp thao tác thực thi dữ liệu CSDL(Key type: INT)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProvider<T>:IInsert<T>,IUpdate<T>,IDelete<T> where T : class
    {
        CoreResult Delete(int key, int? userId = null, bool checkPermission = false);
        CoreResult Exist(int key);
        T Get(int key);
    }
    /// <summary>
    /// (Key type: STRING)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProviderStringKey<T> : IInsert<T>, IUpdate<T>, IDelete<T> where T : class
    {
        CoreResult Delete(string key, int? userId = null, bool checkPermission = false);
        CoreResult Exist(string key);
        T Get(string key);
    }
    /// <summary>
    /// Dùng cho Các đối tượng CSDL mở rộng Entity(Key type: INT)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISqlAction<T>: IEntity<T>, IProvider<T> where T : class
    {
        
    }
    /// <summary>
    /// (Key type: STRING)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISqlActionStringKey<T> : IEntity<T>, IProviderStringKey<T> where T : class
    {

    }
    /// <summary>
    /// Xoa nhieu
    /// </summary>
    /// <typeparam name="TKeyType"></typeparam>
    public interface IEntityDeleteMultiple<TKeyType>
    {
        CoreResult Delete(TKeyType[] keys, int? userId = null, bool checkPermission = false);
    }

    public interface IEntityBase<T> where T : class
    {
        T Find(Expression<Func<T, bool>> match);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> match);
        IEnumerable<T> GetAll();
        bool Any(Expression<Func<T, bool>> predicate);
    }
    public interface IEFAction<T>: IEntityBase<T> where T : class
    {
        T GetByPrimaryKey(int primaryKey);
    }
    public interface IEFActionStringKey<T>:IEntityBase<T> where T : class
    {
        T GetByPrimaryKey(string primaryKey);
    }
}
