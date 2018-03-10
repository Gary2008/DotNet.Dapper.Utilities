# Dapper 辅助类
1. ExecuteDataTable

   `string _sql = "SELECT * FROM [dbo].[Person]";var _actual = sqlHelper.ExecuteDataTable(_sql);Assert.IsNotNull(_actual);Assert.AreEqual(_actual.Rows.Count, 10); string _sqlWhere = @"SELECT *FROM [dbo].[Person]WHERE [Age] = @Age;"; var _actualWhere = sqlHelper.ExecuteDataTable<Person>(_sqlWhere, new Person() { Age = 2 });Assert.IsNotNull(_actualWhere);Assert.AreEqual(_actualWhere.Rows.Count, 1);`