namespace YanZhiwei.DotNet.Dapper.Utilities
{
    using global::Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using YanZhiwei.DotNet2.Utilities.Model;

    /// <summary>
    /// Dapper 数据库操作帮助类，默认是sql Server
    /// </summary>
    /// 时间：2016-01-19 16:21
    /// 备注：
    public abstract class DapperDataOperator
    {
        #region Fields

        /// <summary>
        /// 连接字符串
        /// </summary>
        public readonly string ConnectString = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// 时间：2016-01-19 16:21
        /// 备注：
        public DapperDataOperator(string connectString)
        {
            ConnectString = connectString;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns>DapperTransaction</returns>
        public DapperTransaction BeginTranscation()
        {
            return new DapperTransaction(CreateConnection()) { };
        }

        /// <summary>
        /// 创建SqlConnection连接对象，需要打开
        /// </summary>
        /// <returns>IDbConnection</returns>
        /// 时间：2016-01-19 16:22
        /// 备注：
        public abstract IDbConnection CreateConnection();

        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>DataTable</returns>
        /// 时间：2016-01-19 16:22
        /// 备注：
        public virtual DataTable ExecuteDataTable<T>(string sql, T parameters)
            where T : class
        {
            using (IDbConnection connection = CreateConnection())
            {
                DataTable _table = new DataTable();
                _table.Load(connection.ExecuteReader(sql, parameters));
                return _table;
            }
        }

        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <param name="sql">Sql语句.</param>
        /// <param name="parameters">与Sql语句关联的参数</param>
        /// <returns>DataTable</returns>
        /// 时间：2018/3/9 19:46
        /// 说明：
        public virtual DataTable ExecuteDataTable(string sql, object parameters)
        {
            using (IDbConnection connection = CreateConnection())
            {
                DataTable _table = new DataTable();
                _table.Load(connection.ExecuteReader(sql, parameters));
                return _table;
            }
        }

        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>DataTable</returns>
        /// 时间:2016/10/15 20:07
        /// 备注:
        public virtual DataTable ExecuteDataTable(string sql)
        {
            return ExecuteDataTable(sql, null);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="tran">DapperTransaction</param>
        /// <param name="sql">Sql</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteNonQuery<T>(DapperTransaction tran, string sql, T parameters)
        {
            return tran.DbConnection.Execute(sql, parameters, tran.DbTransaction);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="tran">DapperTransaction</param>
        /// <param name="sql">Sql 语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响行数</returns>
        /// 时间：2018/3/9 19:55
        /// 说明：
        public virtual int ExecuteNonQuery<T>(DapperTransaction tran, string sql, object parameters)
        {
            return tran.DbConnection.Execute(sql, parameters, tran.DbTransaction);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="tran">DapperTransaction</param>
        /// <param name="sql">sql 语句</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteNonQuery(DapperTransaction tran, string sql)
        {
            return tran.DbConnection.Execute(sql, null, tran.DbTransaction);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>影响行数</returns>
        /// 时间：2016-01-19 16:23
        /// 备注：
        public virtual int ExecuteNonQuery<T>(string sql, T parameters)
            where T : class
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Execute(sql, parameters);
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响行数</returns>
        /// 时间：2018/3/9 19:47
        /// 说明：
        public virtual int ExecuteNonQuery(string sql, object parameters)
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.Execute(sql, parameters);
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteNonQuery<T>(string sql, List<T> parameters)
        {
            int _result = 0;
            using (IDbConnection connection = CreateConnection())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (IDbTransaction tran = connection.BeginTransaction())
                {
                    try
                    {
                        _result += connection.Execute(sql, parameters, tran);
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        _result = 0;
                    }
                }
            }
            return _result;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        /// 时间：2016-01-19 16:24
        /// 备注：
        public virtual IDataReader ExecuteReader<T>(string sql, T parameters)
            where T : class
        {
            IDbConnection connection = CreateConnection();

            return connection.ExecuteReader(sql, parameters);
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        /// 时间：2018/3/9 19:51
        /// 说明：
        public virtual IDataReader ExecuteReader(string sql, object parameters)
        {
            IDbConnection connection = CreateConnection();

            return connection.ExecuteReader(sql, parameters);
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>IDataReader</returns>
        public virtual IDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, null);
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回对象</returns>
        /// 时间：2016-01-19 16:25
        /// 备注：
        public virtual object ExecuteScalar<T>(string sql, T parameters)
            where T : class
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.ExecuteScalar(sql, parameters, null, null, null);
            }
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>object</returns>
        /// 时间：2018/3/9 19:51
        /// 说明：
        public virtual object ExecuteScalar(string sql, object parameters)
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.ExecuteScalar(sql, parameters, null, null, null);
            }
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>返回对象</returns>
        public virtual object ExecuteScalar(string sql)
        {
            using (IDbConnection connection = CreateConnection())
            {
                return connection.ExecuteScalar(sql, null, null, null, null);
            }
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>类型</returns>
        /// 时间：2016-01-19 16:25
        /// 备注：
        public virtual T Query<T>(string sql, T parameters)
            where T : class
        {
            T _result = null;
            using (IDbConnection connection = CreateConnection())
            {
                var _queryresult = connection.Query<T>(sql, parameters);
                if (_queryresult != null && _queryresult.Count() > 0)
                    _result = _queryresult.FirstOrDefault();
            }

            return _result;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <returns>类型</returns>
        /// 时间：2018/3/9 19:57
        /// 说明：
        public virtual T Query<T>(string sql)
            where T : class
        {
            return Query<T>(sql, null);
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>类型</returns>
        /// 时间：2018/3/9 19:52
        /// 说明：
        public virtual T Query<T>(string sql, object parameters)
            where T : class
        {
            T _result = null;
            using (IDbConnection connection = CreateConnection())
            {
                var _queryResult = connection.Query<T>(sql, parameters);
                if (_queryResult != null && _queryResult.Count() > 0)
                    _result = _queryResult.FirstOrDefault();
            }

            return _result;
        }

        /// <summary>
        /// 返回集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>集合</returns>
        /// 时间：2016-01-19 16:25
        /// 备注：
        public virtual List<T> QueryList<T>(string sql, T parameters)
            where T : class
        {
            List<T> _result = null;
            using (IDbConnection connection = CreateConnection())
            {
                var _queryResult = connection.Query<T>(sql, parameters);
                if (_queryResult != null)
                    _result = _queryResult.ToList();
            }

            return _result;
        }

        /// <summary>
        /// QueryList
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>集合</returns>
        /// 时间：2018/3/9 19:53
        /// 说明：
        public virtual List<T> QueryList<T>(string sql, object parameters)
            where T : class
        {
            List<T> _result = null;
            using (IDbConnection connection = CreateConnection())
            {
                var _queryResult = connection.Query<T>(sql, parameters);
                if (_queryResult != null)
                    _result = _queryResult.ToList();
            }

            return _result;
        }


        /// <summary>
        /// QueryList
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <returns>集合</returns>
        /// 时间：2018/3/9 20:05
        /// 说明：
        public virtual List<T> QueryList<T>(string sql)
           where T : class
        {
            return QueryList<T>(sql, null);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>PageList</returns>
        public virtual PageList<T> QueryPageList<T>(string sql, int pageIndex, int pageSize)
            where T : class
        {
            using (var sqlConnection = CreateConnection())
            {
                using (var multi = sqlConnection.QueryMultiple(sql, null, null))
                {
                    PageList<T> _pageList = new PageList<T>();
                    _pageList.Data = multi.Read<T>().ToArray();
                    _pageList.TotalCount = multi.Read<int>().First();
                    _pageList.TotalPage = (int)Math.Ceiling(_pageList.TotalCount / (double)pageSize);

                    return _pageList;
                }
            }
        }

        #endregion Methods
    }
}