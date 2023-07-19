using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ZapanControls.Libraries;

namespace ZapanControls.Databases
{
    public abstract class DbBase
    {
        private readonly bool _exitAppOnInitError;
        private readonly bool _showErrors;
        private readonly int _commandTimeout;
        private readonly IsolationLevel _isolation;

        private int _retryCount;

        public DbProviderFactory Factory { get; }

        protected abstract string ConnectionString { get; set; }

        #region Constructors

        protected DbBase(string providerName, int commandTimeout, bool showErrors, bool exitAppOnInitError, IsolationLevel isolation = IsolationLevel.ReadCommitted)
            : this(DbProviderFactories.GetFactory(providerName), commandTimeout, showErrors, exitAppOnInitError, isolation)
        { }

        protected DbBase(DbProviderFactory factory, int commandTimeout, bool showErrors, bool exitAppOnInitError, IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            Factory = factory;
            _exitAppOnInitError = exitAppOnInitError;
            _showErrors = showErrors;
            _commandTimeout = commandTimeout;
            _isolation = isolation;
            _retryCount = 0;
        }

        public DbConnection CreateConnection()
        {
            DbConnection conn = Factory.CreateConnection();
            conn.ConnectionString = ConnectionString;

            return conn;
        }

        internal void Initialize()
        {
            try
            {
                using (DbConnection conn = CreateConnection())
                {
                    conn.Open();
                    conn.Close();
                }
            }
            catch (DbException ex)
            {
                if (_showErrors)
                {
                    ActivateWindow();

                    StackTrace stackTrace = new StackTrace(ex, true);

                    MessageBox.Show($"Erreur initialisation base de données.\r\n\r\nDétails: {ex.Message}\r\nFactory: {Factory}" +
                        $"\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}", "Erreur initialisation base de données",
                        MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                    throw;

                if (_exitAppOnInitError)
                {
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                if (_showErrors)
                {
                    ActivateWindow();

                    StackTrace stackTrace = new StackTrace(ex, true);

                    MessageBox.Show($"Erreur initialisation base de données.\r\n\r\nDétails: {ex.Message}\r\nFactory: {Factory}" +
                        $"\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}", "Erreur initialisation base de données",
                        MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                    throw;
            }
        }

        #endregion

        #region Queries

        /// <summary>
        /// Récupère le résultat d'une requête SQL renvoyant plusieurs lignes de résultat.
        /// </summary>
        /// <typeparam name="T">Doit être une classe dérivée de <see cref="Databases.ModelBase{T}"/>.</typeparam>
        /// <param name="create">Expression lambda permettant l'ajout d'un élément dans la collection.ex: <code>x => new Class(x)</code>.</param>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi une collection enumérable d'objets représentés par <seealso cref="T"/>.</returns>
        public IEnumerable<T> ExecuteReader<T>(Func<DbDataReader, T> create, string query, Dictionary<string, object> parameters = null)
        {
            List<T> items = new List<T>();

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    conn.Open();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.Text);

                        using (DbDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                items.Add(create(r));
                            }
                        }
                        cmd.Transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}", 
                            "Erreur ExecuteReader",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// Récupère le résultat d'une requête SQL renvoyant une seule ligne de résultat.
        /// </summary>
        /// <typeparam name="T">Doit être une classe dérivée de <see cref="Databases.ModelBase{T}"/>.</typeparam>
        /// <param name="create">Expression lambda permettant l'ajout d'un élément dans la collection.ex: <code>x => new Class(x)</code>.</param>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi un object représenté par <seealso cref="T"/>.</returns>
        public T ExecuteReaderSingleResult<T>(Func<DbDataReader, T> create, string query, Dictionary<string, object> parameters = null)
        {
            T item = default;

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    conn.Open();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.Text);

                        using (DbDataReader r = cmd.ExecuteReader())
                        {
                            r.Read();

                            if (r.HasRows)
                            {
                                item = create != null ? create(r) : default;
                            }
                        }
                        cmd.Transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            "Erreur ExecuteReaderAsyncSingleResult",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();

                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
            return item;
        }

        /// <summary>
        /// Récupère le résultat d'une requête SQL renvoyant une seule valeur.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string query, Dictionary<string, object> parameters = null)
        {
            T item = default;

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    conn.Open();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.Text);

                        object result = cmd.ExecuteScalar();
                        item = result == null || Convert.IsDBNull(result) ? default : (T)result;

                        cmd.Transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}", 
                            "Erreur ExecuteScalarAsync",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return item;
        }

        /// <summary>
        /// Exécute une requête SQL qui ne retourne pas de valeur.
        /// </summary>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi un object de type <see cref="int"/> indiquant le résultat de la requête.</returns>
        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            int result = 0;

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    conn.Open();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.Text);
                        result = cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}", 
                            "Erreur ExecuteScalarAsync",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return result;
        }

        #endregion

        #region Queries Async

        /// <summary>
        /// Version asynchrone de <see cref="DbBase.ExecuteReader"/> qui récupère le résultat d'une requête SQL renvoyant plusieurs lignes de résultat.
        /// </summary>
        /// <typeparam name="T">Doit être une classe dérivée de <see cref="Databases.ModelBase{T}"/>.</typeparam>
        /// <param name="create">Expression lambda permettant l'ajout d'un élément dans la collection.ex: <code>x => new Class(x)</code>.</param>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi une collection enumérable d'objets représentés par <seealso cref="T"/>.</returns>
        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(Func<DbDataReader, T> create, string query, Dictionary<string, object> parameters = null)
        {
            List<T> items = new List<T>();

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    await conn.OpenAsync();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.Text);

                        using (DbDataReader r = await cmd.ExecuteReaderAsync())
                        {
                            while (await r.ReadAsync())
                                items.Add(create(r));
                        }
                        cmd.Transaction.Commit();
                    }
                }
                catch (DbException ex)
                {
                    if (ex.ErrorCode == 1205 && _retryCount < 3)
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();

                        _retryCount++;
                        return await ExecuteReaderAsync(create, query, parameters);
                    }
                    else if (_showErrors)
                    {
                        _retryCount = 0;
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.ErrorCode} - {ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}", 
                            "Erreur ExecuteReaderAsync", 
                            MessageBoxButton.OK, MessageBoxImage.Error,  
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}", 
                            "Erreur ExecuteReaderAsync",
                            MessageBoxButton.OK, MessageBoxImage.Error, 
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// Version asynchrone de <see cref="DbBase.ExecuteReaderSingleResult"/> qui récupère le résultat d'une requête SQL renvoyant une seule ligne de résultat.
        /// </summary>
        /// <typeparam name="T">Doit être une classe dérivée de <see cref="Databases.ModelBase{T}"/>.</typeparam>
        /// <param name="create">Expression lambda permettant l'ajout d'un élément dans la collection.ex: <code>x => new Class(x)</code>.</param>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi un object représenté par <seealso cref="T"/>.</returns>
        public async Task<T> ExecuteReaderAsyncSingleResult<T>(Func<DbDataReader, T> create, string query, Dictionary<string, object> parameters = null)
        {
            T item = default;

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    await conn.OpenAsync();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.Text);

                        using (DbDataReader r = await cmd.ExecuteReaderAsync())
                        {
                            await r.ReadAsync();

                            if (r.HasRows)
                            {
                                item = create != null ? create(r) : default;
                            }
                        }
                        cmd.Transaction.Commit();
                    }
                }
                catch (DbException ex)
                {
                    if (ex.ErrorCode == 1205 && _retryCount < 3)
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();

                        _retryCount++;
                        return await ExecuteReaderAsyncSingleResult(create, query, parameters);
                    }
                    else if (_showErrors)
                    {
                        _retryCount = 0;
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.ErrorCode} - {ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            "Erreur ExecuteReaderAsyncSingleResult",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            "Erreur ExecuteReaderAsyncSingleResult",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return item;
        }

        /// <summary>
        /// Version asynchrone de <see cref="DbBase.ExecuteScalar"/> qui récupère le résultat d'une requête SQL renvoyant une seule valeur.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string query, Dictionary<string, object> parameters = null)
        {
            T item = default;

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    await conn.OpenAsync();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.Text);

                        object result = await cmd.ExecuteScalarAsync();
                        item = result == null || Convert.IsDBNull(result) ? default : (T)result;

                        cmd.Transaction.Commit();
                    }
                }
                catch (DbException ex)
                {
                    if (ex.ErrorCode == 1205 && _retryCount < 3)
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }

                        _retryCount++;
                        return await ExecuteScalarAsync<T>(query, parameters);
                    }
                    else if (_showErrors)
                    {
                        _retryCount = 0;
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.ErrorCode} - {ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}", 
                            "Erreur ExecuteScalarAsync",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            "Erreur ExecuteScalarAsync",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return item;
        }

        /// <summary>
        /// Version asynchrone de <see cref="DbBase.ExecuteNonQuery"/> qui exécute une requête SQL qui ne retourne pas de valeur.
        /// </summary>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi un object de type <see cref="int"/> indiquant le résultat de la requête.</returns>
        public async Task<int> ExecuteNonQueryAsync(string query, Dictionary<string, object> parameters = null)
        {
            int result = 0;

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    await conn.OpenAsync();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.Text);
                        result = await cmd.ExecuteNonQueryAsync();
                        cmd.Transaction.Commit();
                    }
                }
                catch (DbException ex)
                {
                    if (ex.ErrorCode == 1205 && _retryCount < 3)
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        _retryCount++;
                        return await ExecuteNonQueryAsync(query, parameters);
                    }
                    else if (_showErrors)
                    {
                        _retryCount = 0;
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.ErrorCode} - {ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            "Erreur ExecuteNonQueryAsync",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            "Erreur ExecuteNonQueryAsync",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return result;
        }

        #endregion

        #region Procedures

        /// <summary>
        /// Récupère le résultat d'une requête SQL renvoyant plusieurs lignes de résultat.
        /// </summary>
        /// <typeparam name="T">Doit être une classe dérivée de <see cref="Databases.ModelBase{T}"/>.</typeparam>
        /// <param name="create">Expression lambda permettant l'ajout d'un élément dans la collection.ex: <code>x => new Class(x)</code>.</param>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi une collection enumérable d'objets représentés par <seealso cref="T"/>.</returns>
        public IEnumerable<T> ProcedureExecuteReader<T>(Func<DbDataReader, T> create, string procedureName, Dictionary<string, object> parameters = null)
        {
            List<T> items = new List<T>();

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    conn.Open();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string query = CreateProcedureQuery(procedureName, parameters);
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.StoredProcedure);

                        using (DbDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                items.Add(create(r));
                            }
                        }
                        cmd.Transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            $"Erreur ProcedureExecuteReaderAsyncSingleResult - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// Récupère le résultat d'une requête SQL renvoyant une seule ligne de résultat.
        /// </summary>
        /// <typeparam name="T">Doit être une classe dérivée de <see cref="Databases.ModelBase{T}"/>.</typeparam>
        /// <param name="create">Expression lambda permettant l'ajout d'un élément dans la collection.ex: <code>x => new Class(x)</code>.</param>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi un object représenté par <seealso cref="T"/>.</returns>
        public T ProcedureExecuteReaderSingleResult<T>(Func<DbDataReader, T> create, string procedureName, Dictionary<string, object> parameters = null)
        {
            T item = default;

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    conn.Open();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string query = CreateProcedureQuery(procedureName, parameters);
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.StoredProcedure);

                        using (DbDataReader r = cmd.ExecuteReader())
                        {
                            r.Read();

                            if (r.HasRows)
                            {
                                item = create != null ? create(r) : default;
                            }
                        }
                        cmd.Transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            $"Erreur ProcdedureExecuteReaderSingleResult - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return item;
        }

        /// <summary>
        /// Récupère le résultat d'une requête SQL renvoyant une seule valeur.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ProcedureExecuteScalar<T>(string procedureName, Dictionary<string, object> parameters = null)
        {
            T item = default;

            using (DbConnection conn = Factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                try
                {
                    conn.Open();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string query = CreateProcedureQuery(procedureName, parameters);
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.StoredProcedure);

                        object result = cmd.ExecuteScalar();
                        item = result == null || Convert.IsDBNull(result) ? default : (T)result;

                        cmd.Transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            $"Erreur ProcedureExecuteScalar - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return item;
        }

        /// <summary>
        /// Exécute une requête SQL qui ne retourne pas de valeur.
        /// </summary>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi un object de type <see cref="int"/> indiquant le résultat de la requête.</returns>
        public int ProcedureExecuteNonQuery(string procedureName, Dictionary<string, object> parameters = null)
        {
            int result = 0;

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    conn.Open();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string query = CreateProcedureQuery(procedureName, parameters);
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.StoredProcedure);
                        result = cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            $"Erreur ProcedureExecuteNonQuery - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return result;
        }

        #endregion

        #region Procedures Async

        /// <summary>
        /// Version asynchrone de <see cref="DbBase.ProcedureExecuteReader"/> qui récupère le résultat d'une procédure SQL renvoyant plusieurs lignes de résultat.
        /// </summary>
        /// <typeparam name="T">Doit être une classe dérivée de <see cref="Databases.ModelBase{T}"/>.</typeparam>
        /// <param name="create">Expression lambda permettant l'ajout d'un élément dans la collection.ex: <code>x => new Class(x)</code>.</param>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi une collection enumérable d'objets représentés par <seealso cref="T"/>.</returns>
        public async Task<IEnumerable<T>> ProcedureExecuteReaderAsync<T>(Func<DbDataReader, T> create, string procedureName, Dictionary<string, object> parameters = null)
        {
            List<T> items = new List<T>();

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    await conn.OpenAsync();

                    using (IDbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string query = CreateProcedureQuery(procedureName, parameters);
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.StoredProcedure);

                        using (DbDataReader r = await cmd.ExecuteReaderAsync())
                        {
                            while (await r.ReadAsync())
                            {
                                items.Add(create(r));
                            }
                        }
                        cmd.Transaction.Commit();
                    }
                }
                catch (DbException ex)
                {
                    if (ex.ErrorCode == 1205 && _retryCount < 3)
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        _retryCount++;
                        return await ProcedureExecuteReaderAsync(create, procedureName, parameters);
                    }
                    else if (_showErrors)
                    {
                        _retryCount = 0;
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.ErrorCode} - {ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}", 
                            $"Erreur ProcedureExecuteReaderAsync - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            $"Erreur ProcedureExecuteReaderAsync - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// Version asynchrone de <see cref="DbBase.ProcdedureExecuteReaderSingleResult"/> qui récupère le résultat d'une procédure SQL renvoyant une seule ligne de résultat.
        /// </summary>
        /// <typeparam name="T">Doit être une classe dérivée de <see cref="Databases.ModelBase{T}"/>.</typeparam>
        /// <param name="create">Expression lambda permettant l'ajout d'un élément dans la collection.ex: <code>x => new Class(x)</code>.</param>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi un object représenté par <seealso cref="T"/>.</returns>
        public async Task<T> ProcedureExecuteReaderAsyncSingleResult<T>(Func<DbDataReader, T> create, string procedureName, Dictionary<string, object> parameters = null)
        {
            T item = default;

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    await conn.OpenAsync();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string query = CreateProcedureQuery(procedureName, parameters);
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.StoredProcedure);

                        using (DbDataReader r = await cmd.ExecuteReaderAsync())
                        {
                            await r.ReadAsync();

                            if (r.HasRows)
                            {
                                item = create != null ? create(r) : default;
                            }
                        }
                        cmd.Transaction.Commit();
                    }
                }
                catch (DbException ex)
                {
                    if (ex.ErrorCode == 1205 && _retryCount < 3)
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        _retryCount++;
                        return await ProcedureExecuteReaderAsyncSingleResult(create, procedureName, parameters);
                    }
                    else if (_showErrors)
                    {
                        _retryCount = 0;
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.ErrorCode} - {ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}", 
                            $"Erreur ProcedureExecuteReaderAsyncSingleResult - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            $"Erreur ProcedureExecuteReaderAsyncSingleResult - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return item;
        }

        /// <summary>
        /// Version asynchrone de <see cref="DbBase.ProcedureExecuteScalar"/> qui récupère le résultat d'un requête SQL renvoyant une seule valeur.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> ProcedureExecuteScalarAsync<T>(string procedureName, Dictionary<string, object> parameters = null)
        {
            T item = default;

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    await conn.OpenAsync();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string query = CreateProcedureQuery(procedureName, parameters);
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.StoredProcedure);

                        object result = await cmd.ExecuteScalarAsync();
                        item = result == null || Convert.IsDBNull(result) ? default : (T)result;

                        cmd.Transaction.Commit();
                    }
                }
                catch (DbException ex)
                {
                    if (ex.ErrorCode == 1205 && _retryCount < 3)
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        _retryCount++;
                        return await ProcedureExecuteScalarAsync<T>(procedureName, parameters);
                    }
                    else if (_showErrors)
                    {
                        _retryCount = 0;
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.ErrorCode} - {ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            $"Erreur ProcedureExecuteReaderAsyncSingleResult - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            $"Erreur ProcedureExecuteReaderAsyncSingleResult - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return item;
        }

        /// <summary>
        /// Version asynchrone de <see cref="DbBase.ProcedureExecuteNonQuery"/> qui exécute une requête SQL qui ne retourne pas de valeur.
        /// </summary>
        /// <param name="query">Chaîne représentant la requête SQL.</param>
        /// <param name="parameters">(Optionnel) Paramètres de la requête SQL.</param>
        /// <returns>Renvoi un object de type <see cref="int"/> indiquant le résultat de la requête.</returns>
        public async Task<int> ProcedureExecuteNonQueryAsync(string procedureName, Dictionary<string, object> parameters = null)
        {
            int result = 0;

            using (DbConnection conn = CreateConnection())
            {
                try
                {
                    await conn.OpenAsync();

                    using (DbTransaction transaction = conn.BeginTransaction(_isolation))
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string query = CreateProcedureQuery(procedureName, parameters);
                        PrepareCommand(cmd, transaction, query, parameters, CommandType.StoredProcedure);
                        result = await cmd.ExecuteNonQueryAsync();
                        cmd.Transaction.Commit();
                    }
                }
                catch (DbException ex)
                {
                    if (ex.ErrorCode == 1205 && _retryCount < 3)
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();

                        _retryCount++;
                        return await ProcedureExecuteNonQueryAsync(procedureName, parameters);
                    }
                    else if (_showErrors)
                    {
                        _retryCount = 0;
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.ErrorCode} - {ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}", 
                            $"Erreur ProcedureExecuteReaderAsyncSingleResult - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    if (_showErrors)
                    {
                        ActivateWindow();

                        StackTrace stackTrace = new StackTrace(ex, true);

                        MessageBox.Show($"{ex.Message}\r\nFactory: {Factory}\r\nMethod: {stackTrace.GetFrame(1).GetMethod().Name}",
                            $"Erreur ProcedureExecuteReaderAsyncSingleResult - {procedureName}",
                            MessageBoxButton.OK, MessageBoxImage.Error,
                            MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return result;
        }

        #endregion

        #region Command Preparation

        /// <summary>
        /// Méthode qui gère la préparation de la commande SQL.
        /// </summary>
        /// <param name="command">La commande SQL.</param>
        /// <param name="transaction">La transaction SQL.</param>
        /// <param name="commandText">La requête SQL.</param>
        /// <param name="parameters">Liste des paramètres SQL.</param>
        /// <param name="cmdType">Type de la commande.</param>
        private void PrepareCommand(IDbCommand command, IDbTransaction transaction, string commandText, Dictionary<string, object> parameters, CommandType cmdType)
        {
            command.Transaction = transaction;
            command.CommandType = cmdType;
            command.CommandText = commandText;
            command.CommandTimeout = _commandTimeout;

            if (parameters != null)
            {
                AttachParameters(command, parameters);
            }
        }

        /// <summary>
        /// Méthode qui gère l'attribution des paramètres SQL.
        /// </summary>
        /// <param name="command">La commande SQL.</param>
        /// <param name="parameters">Liste des paramètres SQL.</param>
        private static void AttachParameters(IDbCommand command, Dictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                IDbDataParameter param = command.CreateParameter();
                param.ParameterName = parameter.Key;
                param.Value = parameter.Value.CheckDbNull();

                command.Parameters.Add(param);
            }
        }

        /// <summary>
        /// Méthode qui gère la création de la requête pour l'exécution d'une procédure.
        /// </summary>
        /// <param name="procedureName">Nom de la procédure SQL.</param>
        /// <param name="parameters">Liste des paramètres SQL.</param>
        /// <returns>Une chaîne de carctère représentant la requête SQL à exécuter.</returns>
        private static string CreateProcedureQuery(string procedureName, Dictionary<string, object> parameters)
        {
            string param = string.Empty;
            int paramCount = parameters != null ? parameters.Count : 0;

            for (int i = 0; i < paramCount; i++)
            {
                param += i == 0 ? "(" : null;
                param += i + 1 < paramCount ? "?," : "?)";
            }
            return $@"{{call {procedureName}{param}}}";
        }

        #endregion

        private static void ActivateWindow()
        {
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA
                && !Thread.CurrentThread.IsThreadPoolThread
                && !Thread.CurrentThread.IsBackground
                && Thread.CurrentThread.IsAlive)
            {
                if (Application.Current.MainWindow != null)
                    Application.Current.MainWindow.Activate();
            }
        }
    }
}
