```c#
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
```