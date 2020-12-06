namespace ZapanControls.Databases.ODBC
{
    public sealed class OdbcDatabase : DbBase
    {
        protected override string ConnectionString { get; set; }

        public OdbcDatabase(string dbName, string user, string password, int timeout = 30, bool showErrors = true, bool exitAppOnInitError = true)
            : this(string.Format("Dsn={0};Uid={1};Pwd={2};", dbName, user, password), 
                  timeout, showErrors, exitAppOnInitError)
        { }

        public OdbcDatabase(string connectionString, int timeout = 30, bool showErrors = true, bool exitAppOnInitError = true)
            : base("System.Data.Odbc", timeout, showErrors, exitAppOnInitError)
        {
            ConnectionString = connectionString;
            this.Initialize();
        }
    }
}
