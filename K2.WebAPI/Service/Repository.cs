using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace K2.WebAPI.Service
{
    public partial class Repository
    {
        private string ConStr;
        public Repository()
        {
            ConStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();
        }
        public (List<T> List, string ErrorMsg) DynamicParamQuery<T>(string query, DynamicParameters param = null, IDbTransaction Dbtransact = null,
              bool isStoredProcedure = true, CommandType CmdType = CommandType.StoredProcedure)
        {
            var connection = new SqlConnection(ConStr);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                var _a = connection.QueryAsync<T>(query, param, null, 130
                    , isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text).ConfigureAwait(true).GetAwaiter().GetResult().ToList();
                return (_a, string.Empty);
            }
            catch (Exception ex)
            {

                return (null, ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task<(List<T> List, string ErrorMsg)> QueryAsync<T>(string query, Dictionary<string, object> param = null, bool isStoredProcedure = true) where T : class
        {
            var connection = new SqlConnection(ConStr);
            try
            {
                if (connection.State == ConnectionState.Open)
                { }
                else
                {
                    connection.Open();
                }
                var _a = await connection.QueryAsync<T>(query, param, null, 1800, isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text);
                //WriteToFile("Dapper Success: " + _a.ToList().Count().ToString() + " Data Retrieve");
                return (_a.ToList(), string.Empty);
            }
            catch (Exception ex)
            {
                //WriteToFile("Dapper Error : " + ex.Message);
                return (null, ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }

    }
}