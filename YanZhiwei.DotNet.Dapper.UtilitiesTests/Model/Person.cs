using System;

namespace YanZhiwei.DotNet.Dapper.UtilitiesTests.Model
{
    public class Person
    {
        public string ID
        {
            get;    // varchar(36)
            set;
        }

        public string UserName
        {
            get;    // nvarchar(20)
            set;
        }

        public string Address
        {
            get;    // nvarchar(100)
            set;
        }

        public byte Age
        {
            get;    // tinyint
            set;
        }

        public string Password
        {
            get;    // nvarchar(20)
            set;
        }

        public DateTime RegisterDate
        {
            get;    // datetime
            set;
        }
    }
}