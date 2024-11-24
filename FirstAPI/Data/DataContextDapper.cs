using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;



namespace FirstAPI.Data
{

    public class DataContextDapper
    {
        private static DataContextDapper? _instance;
        private static  IDbConnection _dbConnection;
        private static readonly object Lock = new();

        public static DataContextDapper GetInstance() // Singleton pattern
        {
            if (_instance != null) return _instance;
            lock (Lock) // Lock to prevent multiple threads from creating multiple instances
            {
                _dbConnection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
                _instance ??= new DataContextDapper();
            }


            return _instance;
        }

        public async Task<IEnumerable<T>> LoadDataAsync<T>(string sql)
        {
            return await _dbConnection.QueryAsync<T>(sql);
        }


        public IEnumerable<T> LoadData<T>(string sql)
        {

            return _dbConnection.Query<T>(sql);
        }
        public IEnumerable<T> LoadData<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.Text)
        {
            return _dbConnection.Query<T>(sql, parameters, commandType: commandType);
        }


        public T LoadDataSingle<T>(string sql)
        {

            return _dbConnection.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {

            return _dbConnection.Execute(sql) > 0;
        }
        public bool ExecuteSql(string sql, DynamicParameters parameters, CommandType commandType = CommandType.Text)
        {

            return _dbConnection.Execute(sql, parameters, commandType: commandType) > 0;
        }



        public bool ExecuteSqlWithParameters(string sql, DynamicParameters parameters, CommandType commandType = CommandType.Text)
        {
            return _dbConnection.Execute(sql, parameters, commandType: commandType) > 0;
        }


        public T LoadDataSingle<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.Text)
        {

            return _dbConnection.QuerySingle<T>(sql, parameters, commandType: commandType);
        }

    }
}