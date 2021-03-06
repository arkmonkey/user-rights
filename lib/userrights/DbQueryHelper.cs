﻿using System;
using System.Collections.Generic;
using System.Text;

namespace userrightslib
{
    /// <summary>
    /// Helps out creating queries
    /// </summary>
    internal class DbQueryHelper
    {
        public static class TableNames
        {
            public const string RIGHT = "Right";
            public const string USERRIGHT = "UserRight";
        }
        private readonly string _prefix;

        public DbQueryHelper(string prefix)
        {
            _prefix = prefix;
        }

        internal string GetTableName(string tableName)
        {
            return string.Format("{0}{1}", _prefix, tableName);
        }

        #region DB tables-related
        /// <summary>
        /// generates the query to determine if a table exists
        /// </summary>
        /// <param name="tableRootName"></param>
        /// <returns></returns>
        internal string GenerateQueryForTableExistence(string tableRootName)
        {
            return string.Format(@"
                SELECT COUNT(*) 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_SCHEMA = 'dbo' 
                AND  TABLE_NAME = '" + GetTableName(tableRootName) + "'");
        }


        internal string GenerateQueryToCreateTable(string tableRootName)
        {
            tableRootName = tableRootName.ToLowerInvariant();
            string query;

            if (tableRootName.Equals(TableNames.RIGHT.ToLowerInvariant()))
            {
                query = string.Format(" " +
                    "CREATE TABLE [{0}](" +
                        "RightId			INT	NOT NULL IDENTITY(1,1) PRIMARY KEY," +
                        "RightCategory		VARCHAR(200)," +
                        "RightName			VARCHAR(200) NOT NULL" +
                    ")", GetTableName(TableNames.RIGHT));
            }
            else if (tableRootName.Equals(TableNames.USERRIGHT.ToLowerInvariant()))
            {
                query = string.Format(" " +
                    "CREATE TABLE {0}(" +
                        "UserRightId		INT NOT NULL IDENTITY(1,1) PRIMARY KEY," +
                        "RightCategory	    VARCHAR(200) NULL," +
                        "RightName          VARCHAR(200) NOT NULL," +
                        "ContextId		    INT NOT NULL," +
                        "[Value]			BIT NULL" +
                    ")", GetTableName(TableNames.USERRIGHT));
            }
            else
            {
                throw new Exception(string.Format("table name '{0}' not recognized", GetTableName(tableRootName)));
            }

            return query;
        }
        #endregion //DB tables-related

        #region Rights-Related
        /// <summary>
        /// This is a helper to make it easy to generate the filter for Rights.  Category field could be null, 
        /// and so that has to be taken into account.
        /// </summary>
        /// <param name="right"></param>
        /// <param name="tableAlias">this is the alias in the query</param>
        /// <returns></returns>
        internal string GetRightFilter(Right right, string tableAlias = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            if (!string.IsNullOrWhiteSpace(tableAlias)) { tableAlias = tableAlias + "."; }
            
            if (string.IsNullOrEmpty(right.Category))
            {
                sb.AppendFormat(" {0}RightCategory IS NULL ", tableAlias);
            }
            else
            {
                sb.AppendFormat(" {1}RightCategory = '{0}' ", right.Category, tableAlias);
            }

            sb.AppendFormat(" AND {1}RightName = '{0}' ", right.Name, tableAlias);
            sb.Append(")");

            return sb.ToString();
        }

        internal string GetUpdateOneQuery(int userId, Right right, bool value)
        {
            return string.Format("IF(EXISTS(SELECT * FROM {0} WHERE ContextId = {1} AND {2}))" +
                                 "UPDATE {0} SET Value = {3} WHERE ContextId = {1} AND {2} " +
                                 "ELSE" +
                                 "INSERT {0}(RightCategory, RightName, ContextId, Value) VALUES({4}, {5}, {1}, {3}) ",
                                 GetTableName(TableNames.USERRIGHT),    //{0}
                                 userId,                                //{1}
                                 GetRightFilter(right),                 //{2}
                                 value,                                 //{3}
                                 right.Category,                        //{4}
                                 right.Name);                           //{5}
        }

        internal string GetUserRightQuery(int id, Right right)
        {
            string query = string.Format("SELECT r.Category, r.[Name], ur.[Value] " +
                "FROM {0} r LEFT JOIN {1} ur ON r.RightCategory=ur.RightCategory AND r.RightName=ur.RightName " + 
                "WHERE ContextId = {2} AND {3}"
                , GetTableName(TableNames.RIGHT)
                , GetTableName(TableNames.USERRIGHT)
                , id
                , GetRightFilter(right, "r"));
            return query;
        }

        internal string GetUserRightsQuery(int id)
        {
            string query = string.Format("SELECT r.Category, r.[Name], ur.[Value] " +
                "FROM {0} r LEFT JOIN {1} ur ON r.RightCategory=ur.RightCategory AND r.RightName=ur.RightName " +
                "WHERE ContextId = {2}"
                , GetTableName(TableNames.RIGHT)
                , GetTableName(TableNames.USERRIGHT)
                , id);
            return query;
        }

        internal string GetUserRightsQuery(int id, List<Right> rights)
        {
            // build the where clause for the rights based on the List<Right> passed
            List<string> rightsWhereClause = new List<string>();
            foreach (Right r in rights)
            {
                rightsWhereClause.Add(GetRightFilter(r, "r"));
            }
            string resultingRightsWhereClause = rightsWhereClause.Count > 0
                ? string.Join(" OR ", rightsWhereClause)
                : "1";

            string query = string.Format("SELECT r.Category, r.[Name], ur.[Value] " +
                "FROM {0} r LEFT JOIN {1} ur ON r.RightCategory=ur.RightCategory AND r.RightName=ur.RightName " +
                "WHERE ContextId = {2} AND ({3})"
                , GetTableName(TableNames.RIGHT)
                , GetTableName(TableNames.USERRIGHT)
                , id
                , resultingRightsWhereClause);
            return query;
        }

        #endregion //Rights-related

    }
}
