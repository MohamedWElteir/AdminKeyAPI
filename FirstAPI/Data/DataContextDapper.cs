using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;



namespace FirstAPI.Data
{

    public class DataContextDapper
    {
        private static DataContextDapper? _instance;
        private static int _count;
        private static readonly object Lock = new();

        public static DataContextDapper GetInstance(IConfiguration configuration) // Singleton pattern
        {
            if (_instance != null) return _instance;
            lock (Lock) // Lock to prevent multiple threads from creating multiple instances
            {
                _instance ??= new DataContextDapper();
            }
            return _instance;
        }

        private DataContextDapper()
        {

           Console.Write($"Dapper instance created: {++_count}");

        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            using IDbConnection dbConnection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            return dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            using IDbConnection dbConnection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            return dbConnection.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            using IDbConnection dbConnection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            return dbConnection.Execute(sql) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            using IDbConnection dbConnection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            return dbConnection.Execute(sql);
        }

        public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> sqlParameters)
        {
            using var dbConnection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            using var sqlCommand = new SqlCommand(sql, dbConnection);
            sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
            dbConnection.Open();
            var rowsAffected = sqlCommand.ExecuteNonQuery();
            dbConnection.Close();

            return rowsAffected > 0;
        }
    }
}