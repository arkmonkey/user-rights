
using System;
using System.Collections.Generic;

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

        public Facade(DataSourceInterfacer dbInterfacer)
        {
            _dbInterfacer = dbInterfacer;
        }

        #region Public Methods
        /// <summary>
        /// Get one right of a user/role
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ContextRight GetContextRight(int Id, Right right)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get entire list of user's/role's rights 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<ContextRight> GetContextRights(int Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get list of user's/role's rights based on a specified list of (independent) rights
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="rightsList"></param>
        /// <returns></returns>
        public List<ContextRight> GetContextRights(int Id, List<Right> rightsList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enable/disable a user's/role's right
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="right"></param>
        /// <param name="value"></param>
        public void SetContextRight(int Id, Right right, bool value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mass setting of rights for a user/role
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="contextRightsList"></param>
        public void SetContextRights(int Id, List<ContextRight> contextRightsList)
        {
            throw new NotImplementedException();
        }

        #endregion //Public Methods

        private static class Queries
        {

            
            public static string GetUpdateOneQuery(int userId, Right right, bool value)
            {
                return string.Format("IF(EXISTS(SELECT * FROM {0} WHERE ContextId = {1} AND {2}))" +
                                     "UPDATE {0} SET Value = {3} WHERE ContextId = {1} AND {2} " +
                                     "ELSE" +
                                     "INSERT {0}(RightCategory, RightName, ContextId, Value) VALUES({4}, {5}, {1}, {6}) ",
                                     );
            }
        }

    }
}
