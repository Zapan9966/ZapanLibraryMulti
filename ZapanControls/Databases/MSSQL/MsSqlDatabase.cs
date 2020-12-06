namespace ZapanControls.Databases.MSSQL
{
    public sealed class MsSqlDatabase : DbBase
    {
        protected override string ConnectionString { get; set; }

        public MsSqlDatabase(string serverName, string databaseName, int timeout = 30, bool showErrors = true, bool exitAppOnInitError = true)
            : this(string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;", serverName, databaseName),
                  timeout, showErrors, exitAppOnInitError) { }

        public MsSqlDatabase(string serverName, string databaseName, string user, string password, int timeout = 30, bool showErrors = true, bool exitAppOnInitError = true)
            : this(string.Format("Data Source={0};Initial Catalog={1};User id={2};Password={3};", serverName, databaseName, user, password),
                  timeout, showErrors, exitAppOnInitError) { }

        public MsSqlDatabase(string connectionString, int timeout = 30, bool showErrors = true, bool exitAppOnInitError = true)
            : base("System.Data.SqlClient", timeout, showErrors, exitAppOnInitError)
        {
            ConnectionString = connectionString + "MultipleActiveResultSets=True;";
            this.Initialize();
        }
    }
}