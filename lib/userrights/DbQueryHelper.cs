using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace userrights
{
    /// <summary>
    /// Helps out creating queries
    /// </summary>
    internal class DbQueryHelper
    {
        private readonly string _prefix;

        public DbQueryHelper(string prefix)
        {
            _prefix = prefix;
        }

        internal string GetTableName(string tableName)
        {
            return string.Format("{0}{1}", _prefix, tableName);
        }

        /// <summary>
        /// This is a helper to make it easy to generate the filter for Rights.  Category field could be null, 
        /// and so that has to be taken into account.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        internal string GetRightFilter(Right right)
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(right.Category))
            {
                sb.AppendFormat(" RightCategory IS NULL ");
            }
            else
            {
                sb.AppendFormat(" RightCategory = '{0}' ", right.Category);
            }

            sb.AppendFormat(" AND RightName = '{0} ", right.Name);

            return sb.ToString();
        }

        internal string GetUpdateOneQuery(int userId, Right right, bool value)
        {
            //TODO: for testing
            return string.Format("IF(EXISTS(SELECT * FROM {0} WHERE ContextId = {1} AND {2}))" +
                                 "UPDATE {0} SET Value = {3} WHERE ContextId = {1} AND {2} " +
                                 "ELSE" +
                                 "INSERT {0}(RightCategory, RightName, ContextId, Value) VALUES({4}, {5}, {1}, {3}) ",
                                 GetTableName("UserRight"),     //{0}
                                 userId,                        //{1}
                                 GetRightFilter(right),         //{2}
                                 value,                         //{3}
                                 right.Category,                //{4}
                                 right.Name);                   //{5}
        }
    }
}
