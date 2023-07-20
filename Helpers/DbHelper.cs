using System;
using System.Data.SqlClient;

namespace TodoList.Helpers
{
    public class DbHelper
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TodoList;");
        }
    }
}
