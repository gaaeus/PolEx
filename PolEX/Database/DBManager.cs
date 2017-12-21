using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PolEX.Model;

// https://code.msdn.microsoft.com/Restful-Application-8b6aafdd
// https://visualstudiogallery.msdn.microsoft.com/88330eb9-102a-47b5-b4cb-e5de7e8686fe?SRC=VSIDE

//http://blogs.msdn.com/b/andy_wigley/archive/2013/11/21/how-to-massively-improve-sqlite-performance-using-sqlwinrt.aspx
//http://www.codeproject.com/Articles/826602/Using-SQLite-as-local-database-with-Universal-Apps

// Note: kept this file as a reference, the one used for test dev is DBController
namespace PolEX
{
    public class DBManager : IDisposable
    {
        #region Properties

        String connection_String { get; set; }
        SQLiteConnection m_dbConnection { get; set; }
        SQLite.SQLiteAsyncConnection m_asyncdbConnection { get; set; }
        Boolean is_connected { get; set; }

        private const String DEFAULT_DATABASE = "PolEX.sqlite";

        public const String DEFAULT_TABLE = "pol_ex";
        public const String CATEGORIES = "pol_ex_categories";
        public const String APP_SETTINGS = "pol_ex_appsettings";
        public const String USER_SETTINGS = "pol_ex_usersettings";
        public const String REQUESTS = "pol_ex_requests";
        private const String TABLE_EXISTS = "SELECT name FROM sqlite_master WHERE type='table' AND name='{0}'";
        private const String TABLE_DROP = "DROP TABLE IF EXISTS {0}";
        #endregion

        #region Constructor

        public DBManager() { }

        public DBManager(String connection_String)
        {
            this.connection_String = connection_String;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens the database connection
        /// </summary>
        private void Connect()
        {
            // If we don't get a connection String, use the default one
            if (String.IsNullOrEmpty(this.connection_String))
                this.connection_String = DEFAULT_DATABASE;

            if (!String.IsNullOrEmpty(this.connection_String))
            {
                try
                {
                    if (!File.Exists(DEFAULT_DATABASE))
                        SQLiteConnection.CreateFile(DEFAULT_DATABASE);

                    m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", this.connection_String));
                    m_dbConnection.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally {
                    is_connected = false;
                }
            }
            else {
                is_connected = false;
            }
        }

        public async void CreateDatabase()
        {
            try
            {
                SQLite.SQLiteAsyncConnection connection = new SQLite.SQLiteAsyncConnection(DEFAULT_DATABASE);
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Opens the database connection asynchronously
        /// </summary>
        private void ConnectAsync()
        {
            // If we don't get a connection String, use the default one
            if (String.IsNullOrEmpty(this.connection_String))
                this.connection_String = DEFAULT_DATABASE;

            if (!String.IsNullOrEmpty(this.connection_String))
            {
                try
                {
                    if (!File.Exists(DEFAULT_DATABASE))
                        CreateDatabase();

                    m_asyncdbConnection = new SQLite.SQLiteAsyncConnection(String.Format("Data Source={0};Version=3;", this.connection_String));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally {
                    is_connected = false;
                }
            }
            else {
                is_connected = false;
            }
        }


        /// <summary>
        /// Closes the database connection
        /// </summary>
        private void Disconnect()
        {
            if (m_dbConnection.State != System.Data.ConnectionState.Closed)
                m_dbConnection.Close();
        }

        #endregion

        #region Data Verification

        public Boolean UserDataExists()
        {
            Boolean ret = false;

            SQLiteTransaction tr = null;
            SQLiteCommand cmd = null;

            try {

                this.Connect();

                using (tr = m_dbConnection.BeginTransaction())
                {
                    using (cmd = m_dbConnection.CreateCommand())
                    {
                        cmd.CommandText = String.Format("SELECT COUNT(*) FROM {0}", USER_SETTINGS);

                        int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                        ret = (rowCount > 0);

                    }
                    tr.Commit();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

                if (tr != null)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (ObjectDisposedException e)
                    {

                    }
                    catch (SQLiteException ex2)
                    {
                        Console.WriteLine("Transaction rollback failed.");
                        Console.WriteLine("Error: {0}", ex2.ToString());
                    }
                    finally
                    {
                        tr.Dispose();
                    }
                }
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                if (tr != null)
                {
                    tr.Dispose();
                }

                if (m_dbConnection != null)
                {
                    try
                    {
                        this.Disconnect();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Closing connection failed.");
                        Console.WriteLine("Error: {0}", ex.ToString());
                    }
                    finally
                    {
                        m_dbConnection.Dispose();
                    }
                }
            }

            return ret;
        }

        #endregion

        #region Schema Creation

        /// <summary>
        /// Creates tables with default values
        /// </summary>
        public void CreateSchema(String table)
        {
            if (String.IsNullOrEmpty(table.Trim()))
            {
                CreateDefaultSchema(true);
            }
            else
            {
                switch(table.Trim())
                {
                    case DEFAULT_TABLE:
                        CreateDefaultTable();
                        break;

                    case CATEGORIES:
                        CreateCategories();
                        break;

                    case USER_SETTINGS:
                        CreateUserSettings();
                        break;

                    case REQUESTS:
                        CreateRequests();
                        break;
                }
            }
        }

        /// <summary>
        /// Creates the default schema and all tables
        /// </summary>
        /// <param name="force"></param>
        private void CreateDefaultSchema(Boolean force = false)
        {
            CreateDefaultTable(force);
            CreateCategories(force);
            CreateAppSettings(force);
            CreateUserSettings(force);
            CreateRequests(force);
            //CreateVideo(force);
            //CreateImage(force);
        }

        /// <summary>
        /// Creates the default application table with default values
        /// </summary>
        /// <param name="forceDrop">Forces existing table to be dropped</param>
        /// <returns>Boolean</returns>
        private void CreateDefaultTable(Boolean forceDrop = false)
        {
            String VERIFY_DEFAULT_TABLE = String.Format(TABLE_EXISTS, DEFAULT_TABLE);

            String strDrop = String.Format(TABLE_DROP, DEFAULT_TABLE);
            String strCreate = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (APP_NAME VARCHAR(128), APP_GUID VARCHAR(128), APP_COPYRIGHT VARCHAR(128), APP_FIRST_VERSION VARCHAR(32), APP_CURRENT_VERSION VARCHAR(32), APP_CURRENT_VERSION_DATE VARCHAR(32), APP_DEVELOPER_ID VARCHAR(128))", DEFAULT_TABLE);
            String strInsert = String.Format(@"INSERT INTO {0} VALUES('PolEx', '{1}', 'Copyright (C) 2015 By Hélio Silva', '0.0.1.0', '0.0.1.0', '{2}', 'Hélio Silva <helio.a.silva@gmail.com>')", DEFAULT_TABLE, Guid.NewGuid().ToString(), DateTime.Now.ToShortDateString());

            int ret = -1;

            SQLiteTransaction tr = null;
            SQLiteCommand cmd = null;

            this.Connect();

            try
            {
                using (tr = m_dbConnection.BeginTransaction())
                {
                    // Drop and recreate
                    cmd = new SQLiteCommand(strDrop, m_dbConnection);
                    cmd.ExecuteNonQuery();

                    cmd = new SQLiteCommand(strCreate, m_dbConnection);
                    cmd.ExecuteNonQuery();

                    // Insert values (1 row only)
                    cmd = new SQLiteCommand(strInsert, m_dbConnection);

                    ret = cmd.ExecuteNonQuery();

                    if (ret > 0) tr.Commit();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

                if (tr != null)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (ObjectDisposedException e)
                    {
                    }
                    catch (SQLiteException ex2)
                    {
                        Console.WriteLine("Transaction rollback failed.");
                        Console.WriteLine("Error: {0}", ex2.ToString());
                    }
                    finally
                    {
                        tr.Dispose();
                    }
                }
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                if (tr != null)
                {
                    tr.Dispose();
                }

                if (m_dbConnection != null)
                {
                    try
                    {
                        this.Disconnect();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Closing connection failed.");
                        Console.WriteLine("Error: {0}", ex.ToString());
                    }
                    finally
                    {
                        m_dbConnection.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Creates the default Categories table with default values
        /// </summary>
        /// <param name="forceDrop">Forces existing table to be dropped</param>
        /// <returns>Boolean</returns>
        private void CreateCategories(Boolean forceDrop = false)
        {
            String VERIFY_CATEGORIES = String.Format(TABLE_EXISTS, CATEGORIES);
            String strDrop = String.Format(TABLE_DROP, CATEGORIES);
            String strCreate = String.Format("CREATE TABLE IF NOT EXISTS {0} (category_id INTEGER PRIMARY KEY, category_identifier VARCHAR(64), category_name VARCHAR(128), category_description VARCHAR(256), parent_category_id INT)", CATEGORIES);

            int ret = -1;

            SQLiteTransaction tr = null;
            SQLiteCommand cmd = null;

            this.Connect();

            try
            {
                using (tr = m_dbConnection.BeginTransaction())
                {
                    // Drop and recreate
                    cmd = new SQLiteCommand(strDrop, m_dbConnection);
                    cmd.ExecuteNonQuery();

                    cmd = new SQLiteCommand(strCreate, m_dbConnection);
                    cmd.ExecuteNonQuery();

                    if (forceDrop)
                    {
                        String strInsertCategory1 = String.Format("INSERT INTO {0} (category_id, category_identifier, category_name, category_description) VALUES (NULL, 'Category1', 'Category 1', 'This is a Category')", CATEGORIES);
                        String strInsertCategory2 = String.Format("INSERT INTO {0} (category_id, category_identifier, category_name, category_description) VALUES (NULL, 'Category2', 'Category 2', 'This is a Category')", CATEGORIES);
                        String strInsertCategory3 = String.Format("INSERT INTO {0} (category_id, category_identifier, category_name, category_description) VALUES (NULL, 'Category3', 'Category 3', 'This is a Category')", CATEGORIES);

                        cmd = new SQLiteCommand(strInsertCategory1, m_dbConnection);
                        ret = cmd.ExecuteNonQuery();

                        cmd = new SQLiteCommand(strInsertCategory2, m_dbConnection);
                        ret = cmd.ExecuteNonQuery();

                        cmd = new SQLiteCommand(strInsertCategory3, m_dbConnection);
                        ret = cmd.ExecuteNonQuery();
                    }

                    if (ret == 1) tr.Commit();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

                if (tr != null)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (ObjectDisposedException e)
                    {

                    }
                    catch (SQLiteException ex2)
                    {
                        Console.WriteLine("Transaction rollback failed.");
                        Console.WriteLine("Error: {0}", ex2.ToString());
                    }
                    finally
                    {
                        tr.Dispose();
                    }
                }
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                if (tr != null)
                {
                    tr.Dispose();
                }

                if (m_dbConnection != null)
                {
                    try
                    {
                        this.Disconnect();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Closing connection failed.");
                        Console.WriteLine("Error: {0}", ex.ToString());
                    }
                    finally
                    {
                        m_dbConnection.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Creates default application settings table
        /// </summary>
        /// <param name="forceDrop"></param>
        private void CreateAppSettings(Boolean forceDrop = false)
        {
            String strDrop = String.Format(TABLE_DROP, APP_SETTINGS);
            String strCreate = String.Format("CREATE TABLE IF NOT EXISTS {0} (MAX_VIDEO_KB INT, USE_COORDINATES INT, SEND_ANONYMOUS INT,SERVER_URL VARCHAR(256),SERVER_PORT VARCHAR(10), SERVER_CONNECTION_TIMEOUT INT, USE_SSL INT)", APP_SETTINGS);
            String strInsert = String.Format("INSERT INTO {0} VALUES (2048, 1, 0, '127.0.0.1', '443', 100000, 0)", APP_SETTINGS);

            int ret = -1;

            SQLiteTransaction tr = null;
            SQLiteCommand cmd = null;

            this.Connect();

            try
            {
                using (tr = m_dbConnection.BeginTransaction())
                {
                    // Drop and recreate
                    cmd = new SQLiteCommand(strDrop, m_dbConnection);
                    cmd.ExecuteNonQuery();

                    cmd = new SQLiteCommand(strCreate, m_dbConnection);
                    cmd.ExecuteNonQuery();

                    ret = cmd.ExecuteNonQuery();

                    cmd = new SQLiteCommand(strInsert, m_dbConnection);
                    cmd.ExecuteNonQuery();

                    if (ret == -1) tr.Commit();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

                if (tr != null)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (ObjectDisposedException e)
                    {

                    }
                    catch (SQLiteException ex2)
                    {
                        Console.WriteLine("Transaction rollback failed.");
                        Console.WriteLine("Error: {0}", ex2.ToString());
                    }
                    finally
                    {
                        tr.Dispose();
                    }
                }
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                if (tr != null)
                {
                    tr.Dispose();
                }

                if (m_dbConnection != null)
                {
                    try
                    {
                        this.Disconnect();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Closing connection failed.");
                        Console.WriteLine("Error: {0}", ex.ToString());
                    }
                    finally
                    {
                        m_dbConnection.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Creates the default user settings table with default values
        /// </summary>
        /// <param name="forceDrop">Forces existing table to be dropped</param>
        /// <returns>Boolean</returns>
        private void CreateUserSettings(Boolean forceDrop = false)
        {
            //String VERIFY_SETTINGS = String.Format(TABLE_EXISTS, USER_SETTINGS);

            String strDrop = String.Format(TABLE_DROP, USER_SETTINGS);
            String strCreate = String.Format("CREATE TABLE IF NOT EXISTS {0} (user_id INTEGER PRIMARY KEY,user_name TEXT,user_alias TEXT,user_email TEXT,user_phone VARCHAR(32),user_address TEXT,user_address2 TEXT,observations TEXT,map_coordinates VARCHAR(128),warning_check1 INT,warning_check2 INT,warning_check3 INT)", USER_SETTINGS);

            int ret = -1;

            SQLiteTransaction tr = null;
            SQLiteCommand cmd = null;

            this.Connect();

            try
            {
                using (tr = m_dbConnection.BeginTransaction())
                {
                    // Drop and recreate
                    cmd = new SQLiteCommand(strDrop, m_dbConnection);
                    cmd.ExecuteNonQuery();

                    cmd = new SQLiteCommand(strCreate, m_dbConnection);
                    cmd.ExecuteNonQuery();

                    ret = cmd.ExecuteNonQuery();

                    if (ret == -1) tr.Commit();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

                if (tr != null)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (ObjectDisposedException e)
                    {

                    }
                    catch (SQLiteException ex2)
                    {
                        Console.WriteLine("Transaction rollback failed.");
                        Console.WriteLine("Error: {0}", ex2.ToString());
                    }
                    finally
                    {
                        tr.Dispose();
                    }
                }
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                if (tr != null)
                {
                    tr.Dispose();
                }

                if (m_dbConnection != null)
                {
                    try
                    {
                        this.Disconnect();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Closing connection failed.");
                        Console.WriteLine("Error: {0}", ex.ToString());
                    }
                    finally
                    {
                        m_dbConnection.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Creates the default requests table (empty)
        /// </summary>
        /// <param name="forceDrop">Forces existing table to be dropped</param>
        /// <returns>Booleanean</returns>
        private void CreateRequests(Boolean forceDrop = false)
        {
            String VERIFY_REQUESTS = String.Format(TABLE_EXISTS, REQUESTS);

            String strDrop = String.Format(TABLE_DROP, REQUESTS);
            String strCreate = String.Format("CREATE TABLE IF NOT EXISTS {0} (col1 typ1, ..., colN typN)", REQUESTS);

            int ret = -1;

            SQLiteTransaction tr = null;
            SQLiteCommand cmd = null;

            this.Connect();

            try
            {
                using (tr = m_dbConnection.BeginTransaction())
                {
                    // Drop and recreate
                    cmd = new SQLiteCommand(strDrop, m_dbConnection);
                    cmd.ExecuteNonQuery();

                    cmd = new SQLiteCommand(strCreate, m_dbConnection);
                    cmd.ExecuteNonQuery();

                    if (ret == -1) tr.Commit();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

                if (tr != null)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (ObjectDisposedException e)
                    {

                    }
                    catch (SQLiteException ex2)
                    {
                        Console.WriteLine("Transaction rollback failed.");
                        Console.WriteLine("Error: {0}", ex2.ToString());
                    }
                    finally
                    {
                        tr.Dispose();
                    }
                }
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                if (tr != null)
                {
                    tr.Dispose();
                }

                if (m_dbConnection != null)
                {
                    try
                    {
                        this.Disconnect();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Closing connection failed.");
                        Console.WriteLine("Error: {0}", ex.ToString());
                    }
                    finally
                    {
                        m_dbConnection.Dispose();
                    }
                }
            }
        }

        #endregion


        #region CRUD

        /// <summary>
        /// Creates or updates the User Settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Boolean CreateUpdateUserSettings(UserSettings settings)
        {
            Boolean ret = false;

            SQLiteTransaction tr = null;
            SQLiteCommand cmd = null;

            Boolean userDataExists = UserDataExists();

            try
            {
                String sqlInsertUser = String.Format(@"INSERT INTO {0} ([user_name], [user_alias],  [user_email], [user_phone], [user_address], [user_address2], [observations], [map_coordinates], [warning_check1], [warning_check1], [warning_check3])
                                                    VALUES (@user_name, @user_alias,@user_email,@user_phone,@user_address,@user_address2,@observations,@map_coordinates,@warning_check1,@warning_check2,@warning_check3);", USER_SETTINGS);

                String sqlUpdateUser = String.Format(@"UPDATE {0} SET [user_name] = @user_name, [user_alias] = @user_alias, [user_email] = @user_email, [user_phone] = @user_phone, [user_address] = @user_address, [user_address2] = @user_address2, 
                                                [observations] = @observations, [map_coordinates] = @map_coordinates,  [warning_check1] = @warning_check1, [warning_check2] = @warning_check2, [warning_check3] = @warning_check3 WHERE [user_id] = @user_id", USER_SETTINGS);

                this.Connect();

                using (tr = m_dbConnection.BeginTransaction())
                {
                    using (cmd = m_dbConnection.CreateCommand())
                    {
                        cmd.CommandText = (userDataExists ? sqlUpdateUser : sqlInsertUser);
                        cmd.Parameters.AddWithValue("@user_id", (userDataExists ? 1 : settings.user_id)); // Always one row
                        cmd.Parameters.AddWithValue("@user_name", settings.user_name);
                        cmd.Parameters.AddWithValue("@user_alias", settings.user_alias);
                        cmd.Parameters.AddWithValue("@user_email", settings.user_email);
                        cmd.Parameters.AddWithValue("@user_phone", settings.user_phone);
                        cmd.Parameters.AddWithValue("@user_address", settings.user_address);
                        cmd.Parameters.AddWithValue("@user_address2", settings.user_address2);
                        cmd.Parameters.AddWithValue("@observations", settings.observations);
                        cmd.Parameters.AddWithValue("@map_coordinates", settings.map_coordinates);
                        cmd.Parameters.AddWithValue("@warning_check1", Convert.ToInt32(settings.warning_check1));
                        cmd.Parameters.AddWithValue("@warning_check2", Convert.ToInt32(settings.warning_check2));
                        cmd.Parameters.AddWithValue("@warning_check3", Convert.ToInt32(settings.warning_check3));

                        ret = (cmd.ExecuteNonQuery() > 0);
                    }
                    tr.Commit();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

                if (tr != null)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (ObjectDisposedException e)
                    {

                    }
                    catch (SQLiteException ex2)
                    {
                        Console.WriteLine("Transaction rollback failed.");
                        Console.WriteLine("Error: {0}", ex2.ToString());
                    }
                    finally
                    {
                        tr.Dispose();
                    }
                }
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                if (tr != null)
                {
                    tr.Dispose();
                }

                if (m_dbConnection != null)
                {
                    try
                    {
                        this.Disconnect();
                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Closing connection failed.");
                        Console.WriteLine("Error: {0}", ex.ToString());
                    }
                    finally
                    {
                        m_dbConnection.Dispose();
                    }
                }
            }

            return ret;
        }

        #endregion

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
