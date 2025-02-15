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
        public DataBaseConnecttionCls(IConfiguration _configuration) {
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
    }
}
