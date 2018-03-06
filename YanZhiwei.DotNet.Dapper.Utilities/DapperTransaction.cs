namespace YanZhiwei.DotNet.Dapper.Utilities
{
    using System;
    using System.Data;

    /// <summary>
    /// Dapper事物
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public sealed class DapperTransaction : IDisposable
    {
        #region Fields

        /// <summary>
        /// The database connection
        /// </summary>
        public readonly IDbConnection DbConnection = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">IDbConnection</param>
        public DapperTransaction(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
            if (DbConnection.State != ConnectionState.Open)
                DbConnection.Open();
            DbTransaction = DbConnection.BeginTransaction();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// IDbTransaction
        /// </summary>
        public IDbTransaction DbTransaction
        {
            get; private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (DbTransaction != null)
            {
                DbTransaction.Commit();
            }
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            if (DbTransaction != null)
            {
                DbTransaction.Dispose();
                DbTransaction = null;
            }
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void Rollback()
        {
            if (DbTransaction != null)
            {
                DbTransaction.Rollback();
            }
        }

        #endregion Methods
    }
}