namespace YanZhiwei.DotNet.Dapper.Utilities
{
    using System;
    using System.Data;
    using System.Data.SQLite;

    /// <summary>
    /// SQLite 数据库备份
    /// </summary>
    public sealed class DapperSQLiteBackup
    {
        #region Fields

        /// <summary>
        /// 备份路径
        /// </summary>
        public readonly string BackupSqlitePath = null;

        /// <summary>
        /// 需要备份的Sqlite文件路径
        /// </summary>
        public readonly string SourceSqlitePath = null;

        /// <summary>
        /// 备份Sqlite连接字符串
        /// </summary>
        private string backupSqliteDaoString = null;

        /// <summary>
        /// 需要备份Sqlite连接字符串
        /// </summary>
        private string sourceSqliteDaoString = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceSqlitePath">需要备份的Sqlite文件路径</param>
        /// <param name="backupSqlitePath">备份路径</param>
        public DapperSQLiteBackup(string sourceSqlitePath, string backupSqlitePath)
        {
            SourceSqlitePath = sourceSqlitePath;
            BackupSqlitePath = backupSqlitePath;
            sourceSqliteDaoString = string.Format(@"Data Source={0};Pooling=true;FailIfMissing=false;Version=3", sourceSqlitePath);
            backupSqliteDaoString = string.Format(@"Data Source={0};Pooling=true;FailIfMissing=false;Version=3", backupSqlitePath);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///备份数据库
        /// </summary>
        /// <returns>备份是否成功</returns>
        public bool BackupDatabase()
        {
            bool _result = false;

            try
            {
                using(var source = new SQLiteConnection(sourceSqliteDaoString))
                {
                    using(var destination = new SQLiteConnection(backupSqliteDaoString))
                    {
                        source.Open();
                        destination.Open();
                        source.BackupDatabase(destination, "main", "main", -1, null, 0);
                        _result = true;
                    }
                }
            }

            catch(Exception)
            {
                _result = false;
            }

            return _result;
        }

        /// <summary>
        /// 清空数据库
        /// </summary>
        /// <returns>
        /// 影响行数
        /// </returns>
        public int ClearDB()
        {
            DapperDataOperator _sqlHelper = new DapperSQLiteOperator(SourceSqlitePath);
            int _result = 0;
            DataTable _tables = _sqlHelper.ExecuteDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");

            if(_tables != null && _tables.Rows.Count > 0)
            {
                using(var trans = _sqlHelper.BeginTranscation())
                {
                    try
                    {
                        foreach(DataRow table in _tables.Rows)
                        {
                            string _tableName = table["NAME"].ToString();
                            string _sql = string.Format("delete from {0};", _tableName);
                            _result += _sqlHelper.ExecuteNonQuery(trans, _sql);
                        }

                        trans.Commit();
                    }

                    catch(Exception)
                    {
                        trans.Rollback();
                        _result = 0;
                    }
                }
            }

            return _result;
        }

        #endregion Methods
    }
}