using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
namespace FirstAPI.Data
{
    class DataContextDapper
    {
        private readonly IConfiguration _config;
        public DataContextDapper(IConfiguration configuration)
        {
            _config = configuration;

        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
           return dbConnection.Execute(sql) > 0;

        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql);

        }
        public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> sqlParameters)
        {
            SqlCommand sqlCommand = new SqlCommand(sql);
            sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
            var dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            dbConnection.Open();
            sqlCommand.Connection = dbConnection;
            var rowsAffected = sqlCommand.ExecuteNonQuery();
            dbConnection.Close();

            return rowsAffected > 0;
        }



    }

}

