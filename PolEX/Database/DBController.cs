using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolEX.Model;
using System.IO;
using SQLite;

namespace PolEX.Database
{
    public class DBController
    {
        #region Properties

        const String DataBaseName = "PolEX.db";
        public SQLiteAsyncConnection DatabaseConnection { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if a file exists, asynchronously
        /// </summary>
        /// <param name="dbName">The filename of the file to check</param>
        /// <returns>True if file exists, false otherwise</returns>
        private async Task<Boolean> CheckDbAsync(String dbName)
        {
            bool dbExists = false;
            try
            {
                dbExists = await Task.Run(() => File.Exists(dbName));
            }
            catch (Exception)
            {
                throw;
            }

            return dbExists;
        }
        
        /// <summary>
        /// Creates a database given a filename
        /// </summary>
        /// <param name="dbName">The filename used to name the database being created.</param>
        private async void CreateDatabase(String dbName)
        {
            await Task.Run(() => {
                try
                {
                    string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    folder = "F:\\Dev\\TESTDB";

                    //SQLiteAsyncConnection conn = new SQLiteAsyncConnection(System.IO.Path.Combine(folder, dbName));

                    TODO: https://github.com/oysteinkrog/SQLite.Net-PCL before anything else
                    
                    string databasePath = System.IO.Path.Combine(folder, dbName);
                    //SQLiteConnectionString connectionString = new SQLiteConnectionString(databasePath, false);

                    //var connectionFactory = new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(connectionString, SQLiteOpenFlags.Create));
                    //var conn = new SQLiteAsyncConnection(connectionFactory);

                    var conn = new SQLiteAsyncConnection(databasePath, SQLiteOpenFlags.Create, false);

                    this.DatabaseConnection = conn;

                    conn.CreateTableAsync<Image>()
                                        .ContinueWith (t => {
                        Console.WriteLine ("Table created!");
                    });
                }
                catch (Exception ex)
                {
                    if (ex.Message != null)
                        throw;
                }    
            });
        }

        /// <summary>
        /// Gets and returns the database connection object. Creates a database first if it doesn't exist
        /// </summary>
        /// <returns></returns>
        public async Task <SQLiteAsyncConnection> GetConnection()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            bool dbExists = await CheckDbAsync(System.IO.Path.Combine(folder, DataBaseName));
            if(!dbExists)
            {
                CreateDatabase(DataBaseName);
            }

            return this.DatabaseConnection;
        }
//http://blogs.u2u.be/diederik/post/2015/09/08/Using-SQLite-on-the-Universal-Windows-Platform.aspx
        private void test(Category category)
        {
            var db = DatabaseConnection;
            {
                db.InsertAsync(category);
            }
        }

        #endregion
    }
}
