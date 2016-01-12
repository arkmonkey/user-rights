
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace userrights
{
    /// <summary>
    /// This is the main class that consumers will see and use most often.  
    /// Has calls to: 
    ///     Determine if an ID has a certain privilege
    ///     Grant/take away privilege to/from an ID
    ///     Get list of all user rights
    /// </summary>
    public class Facade
    {
        private readonly DataSourceInterfacer _dbInterfacer;
        private readonly DbQueryHelper _queryHelper;

        public Facade(DataSourceInterfacer dbInterfacer)
        {
            _dbInterfacer = dbInterfacer;
            _queryHelper = new DbQueryHelper(_dbInterfacer.Prefix);
        }

        #region Public Methods
        /// <summary>
        /// Get one right of a user/role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public UserRight GetRight(int id, Right right)
        {
            string query = _queryHelper.GetUserRightQuery(id, right);

            
            SqlCommand cmd = new SqlCommand(query, _dbInterfacer.Connection);
            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                bool val = (bool) reader["Value"];
                return new UserRight { Id = id, Right = right, Value = val };
            }
            else
            {
                return new UserRight { Id = id, Right = right, Value = false };
            }
        }

        /// <summary>
        /// Get entire list of user's/role's rights 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<UserRight> GetRights(int Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get list of user's/role's rights based on a specified list of (independent) rights
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="rightsList"></param>
        /// <returns></returns>
        public List<UserRight> GetRights(int Id, List<Right> rightsList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enable/disable a user's/role's right
        /// </summary>
        /// <param name="id"></param>
        /// <param name="right"></param>
        /// <param name="value"></param>
        public void SetRight(int id, Right right, bool value)
        {
            string query = _queryHelper.GetUpdateOneQuery(id, right, value);

            SqlCommand cmd = new SqlCommand(query, _dbInterfacer.Connection);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Mass setting of rights for a user/role
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="contextRightsList"></param>
        public void SetRights(int Id, List<UserRight> contextRightsList)
        {
            throw new NotImplementedException();
        }

        #endregion //Public Methods

        

    }
}
