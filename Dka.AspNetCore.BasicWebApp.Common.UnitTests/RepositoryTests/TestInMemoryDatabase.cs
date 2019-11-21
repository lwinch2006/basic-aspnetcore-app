using System.Data;
using System.Data.SQLite;
using ServiceStack;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests.RepositoryTests
{
    public class TestSQLiteInMemoryDatabase
    {
        private const string ConnectionString = "Data Source=:memory:";

        private readonly IDbConnection _db;

        public TestSQLiteInMemoryDatabase()
        {
            _db = new SQLiteConnection(ConnectionString);
            _db.Open();
        }

        public IDbConnection GetConnection()
        {
            return _db;
        }
    }
}