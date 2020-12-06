namespace ZapanControls.Databases.MYSQL
{
    public sealed class MySqlDatabase : DbBase
    {
        protected override string ConnectionString { get; set; }

        public MySqlDatabase(string serverName, string databaseName, string user, string password, int timeout = 30, bool showErrors = true, bool exitAppOnInitError = true)
            : this(string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3}", serverName, databaseName, user, password),
                  timeout, showErrors, exitAppOnInitError) { }

        public MySqlDatabase(string connectionString, int timeout = 30, bool showErrors = true, bool exitAppOnInitError = true)
            : base (MySql.Data.MySqlClient.MySqlClientFactory.Instance, timeout, showErrors, exitAppOnInitError)
        {
            ConnectionString = connectionString;
            Initialize();
        }
    }
}
