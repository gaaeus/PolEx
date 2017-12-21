using SQLite.Net;
using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolEx.DB
{
    public class Class1
    {
        void myClassStuff()
        {
            string databasePath = "path";
            var connectionFactory = new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(databasePath, storeDateTimeAsTicks: false)));
            var asyncConnection = new SQLiteAsyncConnection(connectionFactory);
        }

    }
}
