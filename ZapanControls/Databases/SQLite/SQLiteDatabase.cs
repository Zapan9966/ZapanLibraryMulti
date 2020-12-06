namespace ZapanControls.Databases.SQLite
{
    public sealed class SQLiteDatabase : DbBase
    {
        protected override string ConnectionString { get; set; }

        public SQLiteDatabase(string basePath, bool compress, int version, int timeout = 30, bool showErrors = true, bool exitAppOnInitError = true)
            : this($"Data Source={basePath};Version={version};Compress={compress};Journal Mode=Off;", timeout, showErrors, exitAppOnInitError)
        { }

        public SQLiteDatabase(string connectionString, int timeout = 30, bool showErrors = true, bool exitAppOnInitError = true)
            : base(System.Data.SQLite.SQLiteFactory.Instance, timeout, showErrors, exitAppOnInitError)
        {
            ConnectionString = connectionString;
            Initialize();
        }
    }
}
