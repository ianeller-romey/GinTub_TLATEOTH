using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

using Newtonsoft.Json;

using GinTub.Repository.Interface;
using GinTub.Repository.Entities;


namespace GinTub
{

    public static class CheatDictionary
    {
        #region MEMBER FIELDS

        private static readonly string s_connectionString =
            Regex.Match(ConfigurationManager.ConnectionStrings["GinTubEntities"].ConnectionString,
            "provider connection string=\"(?<connectionstring>.+)\"").Groups["connectionstring"].Value;

        private static readonly Dictionary<string, ICheatRule> s_rules =
            new Dictionary<string, ICheatRule>
            {
                {"ResetPlayer", new CheatResetPlayer()}
            };

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER CLASSES

        private interface ICheatRule
        {
            void Process(Guid playerId, dynamic jsonObject);
        }

        private class CheatResetPlayer : ICheatRule
        {
            #region MEMBER METHODS

            #region Public Functionality

            public void Process(Guid playerId, dynamic jsonObject)
            {
                using (var conn = new SqlConnection(s_connectionString))
                {
                    using (var cmd = new SqlCommand("[cheat].[cheat_ResetPlayer]", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@player", playerId));

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public static void DoCheat(string cheat, Guid playerId, string jsonString)
        {
            dynamic jsonObject = (!string.IsNullOrEmpty(jsonString)) ? JsonConvert.DeserializeObject(jsonString) : null;
            if (s_rules.ContainsKey(cheat))
            {
                s_rules[cheat].Process(playerId, jsonObject);
            }
        }

        #endregion


        #region Private Functionality
        #endregion

        #endregion
    }

}