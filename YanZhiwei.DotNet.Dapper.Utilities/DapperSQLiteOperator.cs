﻿namespace YanZhiwei.DotNet.Dapper.Utilities
{
    using System.Data;
    using System.Data.SQLite;
    using System.IO;

    /// <summary>
    /// 基于Dapper的SQLite数据库操作类
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.Dapper.Utilities.DapperDataOperator" />
    public sealed class DapperSQLiteOperator : DapperDataOperator
    {
        #region Fields

        /// <summary>
        /// SQLite 文件位置
        /// </summary>
        public readonly string DbFile;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlLiteDbFile">SQLite 文件位置</param>
        public DapperSQLiteOperator(string sqlLiteDbFile)
            : base(string.Format(@"Data Source={0};Pooling=true;FailIfMissing=false;Version=3", sqlLiteDbFile))
        {
            DbFile = sqlLiteDbFile;
            if (!File.Exists(DbFile))
            {
                SQLiteConnection.CreateFile(DbFile);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 创建SqlConnection连接对象，需要打开
        /// </summary>
        /// <returns>
        /// IDbConnection
        /// </returns>
        /// 时间：2016-01-19 16:22
        /// 备注：
        public override IDbConnection CreateConnection()
        {
            return new SQLiteConnection(ConnectString);
        }

        #endregion Methods
    }
}