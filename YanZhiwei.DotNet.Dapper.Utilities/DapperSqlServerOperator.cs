namespace YanZhiwei.DotNet.Dapper.Utilities
{
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    ///  Dapper Sql Server数据库操作帮助类
    /// </summary>
    /// 时间：2016-01-19 16:33
    /// 备注：
    public sealed class DapperSqlServerOperator : DapperDataOperator
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlConnectString">连接字符串</param>
        /// 时间：2016-01-19 16:33
        /// 备注：
        public DapperSqlServerOperator(string sqlConnectString)
            : base(sqlConnectString)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 创建SqlConnection连接对象，需要打开
        /// </summary>
        public override IDbConnection CreateConnection()
        {
            IDbConnection sqlConnection = new SqlConnection(base.ConnectString);

            if(sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();

            return sqlConnection;
        }

        #endregion Methods
    }
}