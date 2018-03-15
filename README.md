####Dapper 辅助类
扩展ExecuteDataTable，ExecuteNonQuery，ExecuteReader，ExecuteScalar，Query，QueryList，QueryPageList方法，并支持事务处理，可以方便项目中快速使用Dapper

单元测试：
```c#
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

        [TestMethod]
        public void ExecuteScalarTest()
        {
            string _sql = @"SELECT p.UserName
            FROM dbo.Person p
            WHERE p.Age = @Age;";
            Assert.IsNotNull(sqlHelper.ExecuteReader(_sql, new { Age = 1 }));
            Assert.IsNotNull(sqlHelper.ExecuteReader(_sql, new Person { Age = 1 }));
            _sql = @"SELECT p.UserName
            FROM dbo.Person p
            WHERE p.Age = 1;";
            Assert.IsNotNull(sqlHelper.ExecuteReader(_sql));
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

        [TestMethod]
        public void QueryListTest()
        {
            string _sql = @"SELECT p.*
            FROM dbo.Person p
            WHERE p.Age > @Age;";
            Assert.IsNotNull(sqlHelper.QueryList<Person>(_sql, new { Age = 1 }));
            Assert.IsNotNull(sqlHelper.QueryList<Person>(_sql, new Person { Age = 1 }));
            _sql = @"SELECT p.*
            FROM dbo.Person p
            WHERE p.Age = 1;";
            Assert.IsNotNull(sqlHelper.QueryList<Person>(_sql));
        }

        [TestMethod]
        public void QueryPageListTest()
        {
            string _sql = SqlServerPageScript.TablePageSQLByRowNumber("Person", "*", "Age", string.Empty, OrderType.Desc, 5, 1);
            var _acutal = sqlHelper.QueryPageList<Person>(_sql, 1, 5);
            Assert.AreEqual(10, _acutal.TotalCount);
            Assert.AreEqual(2, _acutal.TotalPage);
        }

        [TestMethod]
        public void QueryTest()
        {
            string _sql = @"SELECT p.*
            FROM dbo.Person p
            WHERE p.Age = @Age;";
            Assert.IsNotNull(sqlHelper.Query<Person>(_sql, new { Age = 1 }));
            Assert.IsNotNull(sqlHelper.Query<Person>(_sql, new Person { Age = 1 }));
            _sql = @"SELECT p.*
            FROM dbo.Person p
            WHERE p.Age = 1;";
            Assert.IsNotNull(sqlHelper.Query<Person>(_sql));
        }

        #endregion Methods
    }
```
