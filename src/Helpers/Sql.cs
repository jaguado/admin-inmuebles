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
        private static readonly string sqlConn = Environment.GetEnvironmentVariable("SQL_CONN") ?? throw new ArgumentNullException("ENV SQL_CONN");

        private async static Task<SqlConnection> GetConnection()
        {
            var conn = new SqlConnection(sqlConn);
            await conn.OpenAsync();
            return conn;
        }
        public static async Task<DataSet> GetData(string query)
        {
            SqlConnection conn = null;
            try
            {
                conn = await GetConnection();
                var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                var result = new DataSet();
                new SqlDataAdapter(cmd).Fill(result);
                return result;
            }
            catch(Exception ex)
            {
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
                Console.WriteLine("Error on 'Sql.GetData': {0}", ex.ToString());
                return null;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static async Task<DataSet> Execute(string storedProcedure, IDictionary<string, string> args)
        {
            SqlConnection conn = null;
            try
            {
                conn = await GetConnection();
                var cmd = conn.CreateCommand();
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                args.ToList().ForEach(arg => cmd.Parameters.AddWithValue(arg.Key, arg.Value));
                var result = new DataSet();
                new SqlDataAdapter(cmd).Fill(result);
                if (result == null || result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                    return null;
                return result;
            }
            catch (Exception ex)
            {
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
                Console.WriteLine("Error on 'Sql.Execute': {0}", ex.ToString());
                return null;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static async Task<int> ExecuteScalar(string storedProcedure, IDictionary<string, string> args)
        {
            SqlConnection conn = null;
            try
            {
                conn = await GetConnection();
                var cmd = conn.CreateCommand();
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                args.ToList().ForEach(arg => cmd.Parameters.AddWithValue(arg.Key, arg.Value));
                var result = await cmd.ExecuteScalarAsync();
                return (int)result;
            }
            catch (Exception ex)
            {
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
                Console.WriteLine("Error on 'Sql.Execute': {0}", ex.ToString());
                return -1;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            } 
        }
    }
}
