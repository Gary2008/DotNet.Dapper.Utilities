namespace YanZhiwei.DotNet.Dapper.Utilities.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using YanZhiwei.DotNet.Dapper.UtilitiesTests.Model;
    using YanZhiwei.DotNet2.Utilities.Common;

    [TestClass]
    public class DapperSqlServerOperatorTests
    {
        #region Fields

        private DapperDataOperator sqlHelper = null;

        #endregion Fields

        #region Methods

        [TestMethod]
        public void ExecuteDataTableTest()
        {
            string _sql = "SELECT * FROM [dbo].[Person]";
            var _actual = sqlHelper.ExecuteDataTable(_sql);
            Assert.IsNotNull(_actual);
            Assert.AreEqual(_actual.Rows.Count, 10);

            string _sqlWhere = @"SELECT *
            FROM [dbo].[Person]
            WHERE [Age] = @Age;";

            var _actualWhere = sqlHelper.ExecuteDataTable<Person>(_sqlWhere, new Person() { Age = 2 });
            Assert.IsNotNull(_actualWhere);
            Assert.AreEqual(_actualWhere.Rows.Count, 1);
        }

        [TestInitialize]
        public void Init()
        {
            sqlHelper = new DapperSqlServerOperator(@"Server=DESKTOP-T36VH59;Database=Sample;uid=sa;pwd=sasa;");
            sqlHelper.ExecuteNonQuery("DELETE [dbo].[Person]");
            string _sql = @"INSERT INTO [dbo].[Person]
               ([ID]
               ,[UserName]
               ,[Address]
               ,[Age]
               ,[Password]
               ,[RegisterDate])
             VALUES
               (@ID
               ,@UserName
               ,@Address
               ,@Age
               ,@Password
               ,@RegisterDate)";
            for (int i = 0; i < 10; i++)
            {
                Person _person = new Person();
                _person.Address = RandomHelper.NextHexString(10);
                _person.Age = (byte)i;
                _person.ID = Guid.NewGuid().ToString();
                _person.UserName = RandomHelper.NextHexString(10);
                _person.Password = RandomHelper.NextHexString(10);
                _person.RegisterDate = RandomHelper.NextDateTime();
                sqlHelper.ExecuteNonQuery<Person>(_sql, _person);
            }
        }

        #endregion Methods
    }
}