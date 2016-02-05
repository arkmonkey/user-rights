using System.Data.SqlClient;

namespace userrightslib
{
    public class DataSourceInterfacer
    {
        private readonly string _connString;
        private readonly string _prefix;
        private readonly DbQueryHelper _queryHelper;

        public DataSourceInterfacer(string connectionSring, string prefix = "")
        {
            _connString = connectionSring;
            _prefix = prefix;
            _queryHelper = new DbQueryHelper(_prefix);
        }

        public string Prefix { get { return _prefix; } }

        public bool TablesExist()
        {
            return DoesIndividualDbTableExists(DbQueryHelper.TableNames.RIGHT) && DoesIndividualDbTableExists(DbQueryHelper.TableNames.USERRIGHT);
        }

        public void BuildTables()
        {
            CreateTable(DbQueryHelper.TableNames.RIGHT);
            CreateTable(DbQueryHelper.TableNames.USERRIGHT);
        }

        private SqlConnection _connection;
        internal SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(_connString);
                }
                return _connection;
            }
        }
        
        /// <summary>
        /// return true if the table exists, false if otherwise
        /// </summary>
        /// <param name="tableRootName"></param>
        /// <returns></returns>
        private bool DoesIndividualDbTableExists(string tableRootName)
        {
            string query = _queryHelper.GenerateQueryForTableExistence(tableRootName);
            SqlCommand cmd = new SqlCommand(query, this.Connection);
            return ((int) cmd.ExecuteScalar()) > 0;
        }

        

        private void CreateTable(string tableRootName)
        {
            string commandText = _queryHelper.GenerateQueryToCreateTable(tableRootName);
            SqlCommand cmd = new SqlCommand(commandText, this.Connection);
            cmd.ExecuteNonQuery();
        }
    }
}
