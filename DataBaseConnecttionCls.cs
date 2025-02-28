using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;

namespace InvterViewTest
{
    public class DataBaseConnecttionCls
    {
        public IConfiguration configuration;
        public string DbconSting = "";
        private DataTable dataTable = new DataTable();
        public DataBaseConnecttionCls(IConfiguration _configuration) 
        {
            configuration = _configuration;
            DbconSting = _configuration.GetConnectionString("DefaultConnection");

        }

        public List<T> GetData<T>(string query) where T : new()
        {
            var databaseonj = GetDataTableSQL(query);
            return ExtentionClass.ToListFromDataTable<T>(databaseonj);
        }

        private DataTable GetDataTableSQL(string queryString)
        {
            SqlConnection  conn = new SqlConnection(DbconSting);
            SqlCommand cmd = new SqlCommand(queryString, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            return dataTable;

        }

        public Int32 SaveExecuteNonQuery<T>(Dictionary<string, string> parameters) where T : class, new()
        {
            var querystring = "insert into " + typeof(T).Name + " ";
            querystring += "(";
            foreach (var parameter in parameters)
            {
                querystring += parameter.Key + ",";
            }
            if (parameters.Count > 0)
                querystring = querystring.Remove(querystring.LastIndexOf(","));
            querystring += ")";
            // querystring += " output INSERTED.ID ";
            querystring += "values";
            querystring += "(";
            foreach (var parameter in parameters)
            {
                if (parameter.Value != null && parameter.Value != "(NULL)")
                {
                    string val = parameter.Value.ToString().Replace("'", "''").Trim();
                    querystring += "'" + val + "'" + ",";
                }
                else
                {
                    querystring += "NULL,";
                }
            }
            if (parameters.Count > 0)
                querystring = querystring.Remove(querystring.LastIndexOf(","));
            querystring += ");";
            //querystring += "SELECT SCOPE_IDENTITY();";
            return ExecuteScalarInsert(querystring);
        }



        private Int32 ExecuteScalarInsert(string queryString)
        {
            var connection = new SqlConnection();
            var command = new SqlCommand();
            var returnVal = 0;
            try
            {
                connection = new SqlConnection(DbconSting);
                connection.Open();
                command = new SqlCommand(queryString + "; SELECT SCOPE_IDENTITY();", connection);
                returnVal = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                returnVal = 0;
                Console.WriteLine($"Error: {ex.Message}"); // Log the error
            }
            finally
            {
                connection.Dispose();
                command.Dispose();
            }

            return returnVal;
        }


        //private int ExecuteUpdate(string queryString)
        //{
        //    var connection = new MySqlConnection();
        //    var adapter = new MySqlDataAdapter();
        //    var returnVal = 1;
        //    try
        //    {
        //        //connection = new MySqlConnection(_dbSQLStirng);
        //        //connection.Open();
        //        //adapter.UpdateCommand = new SqlCommand(queryString, connection);
        //        ////adapter.UpdateCommand.ExecuteNonQuery();
        //        //connection.Close();

        //        connection = new MySqlConnection(_dbSQLStirng);
        //        connection.Open();
        //        adapter.UpdateCommand = new MySqlCommand(queryString, connection);
        //        adapter.UpdateCommand.ExecuteNonQuery();
        //        connection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        returnVal = 0;
        //    }
        //    finally
        //    {
        //        connection.Dispose();
        //    }

        //    return returnVal;
        //}

        //private int ExecuteDelete(string queryString)
        //{
        //    var connection = new MySqlConnection();
        //    var adapter = new MySqlDataAdapter();
        //    var returnVal = 1;

        //    try
        //    {
        //        connection = new MySqlConnection(_dbSQLStirng);
        //        connection.Open();
        //        adapter.DeleteCommand = new MySqlCommand(queryString, connection);
        //        adapter.DeleteCommand.ExecuteNonQuery();
        //        connection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        returnVal = 0;
        //    }
        //    finally
        //    {
        //        connection.Dispose();
        //    }

        //    return returnVal;
        //}

        //public T GetSingleRecordFromSingleTable<T>(Dictionary<string, string> parameters) where T : class, new()
        //{
        //    var querystring = "select " + Query.getQuery(typeof(T).Name) + " from " + typeof(T).Name + "";
        //    string joiner = string.Empty;
        //    if (parameters.Count > 0)
        //        querystring += " WHERE ";
        //    foreach (var parameter in parameters)
        //    {
        //        if (parameter.Value.Length > 0)
        //            querystring += parameter.Key + "=" + "'" + parameter.Value + "'" + " and ";
        //        else
        //            querystring += parameter.Key;
        //    }
        //    if (parameters.Count > 0)
        //        querystring = querystring.Remove(querystring.LastIndexOf("and"));
        //    var _datatableObj = GetDataTableSQL(querystring);
        //    return Extensions.ToFromDataTable<T>(_datatableObj);
        //}


    }
}
