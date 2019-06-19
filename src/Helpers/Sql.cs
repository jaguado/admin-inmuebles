using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Helpers
{
    public static class Sql
    {
        private async static Task<SqlConnection> GetConnection()
        {
            var conn = new SqlConnection(Environment.GetEnvironmentVariable("SQL_CONN"));
            await conn.OpenAsync();
            return conn;
        }
        public static async Task<DataSet> GetData(string query)
        {
            var cmd = (await GetConnection()).CreateCommand();
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            var result = new DataSet();
            new SqlDataAdapter(cmd).Fill(result);
            return result;
        }
    }
}
