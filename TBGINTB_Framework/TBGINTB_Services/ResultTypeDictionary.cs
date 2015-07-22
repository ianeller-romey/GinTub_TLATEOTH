using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using GinTub.Repository.Interface;
using GinTub.Repository.Entities;


namespace GinTub
{

    public static class ResultTypeDictionary
    {
        #region MEMBER FIELDS

        private static IGinTubRepository m_repository = new GinTub.Repository.GinTubRepository();
        private static List<ResultTypeRule> m_rules =
            new List<ResultTypeRule>
            {
                new ResultTypeRule("Room XYZ Movement", ResultTypeRule.ResultTypePriority.Last),
                new ResultTypeRule("Room XYZ Teleport", ResultTypeRule.ResultTypePriority.Last),
                new ResultTypeRule("Room Id Teleport", ResultTypeRule.ResultTypePriority.Last),
                new ResultTypeRule("Area Id Room XYZ Teleport", ResultTypeRule.ResultTypePriority.Last),
                new ResultTypeRule("Area Id Room Id Teleport", ResultTypeRule.ResultTypePriority.Last),
                new ResultTypeRule("Item Acquisition", ResultTypeRule.ResultTypePriority.First),
                new ResultTypeRule("Event Acquisition", ResultTypeRule.ResultTypePriority.First),
                new ResultTypeRule("Character Acquisition", ResultTypeRule.ResultTypePriority.First),
                new ResultTypeRule("Paragraph State Change", ResultTypeRule.ResultTypePriority.First),
                new ResultTypeRule("Room State Change", ResultTypeRule.ResultTypePriority.First),
                new ResultTypeRule("Message Activation", ResultTypeRule.ResultTypePriority.Second),
            };

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER CLASSES

        private class ResultTypeRule
        {
            #region MEMBER FIELDS
            
            public enum ResultTypePriority
            {
                First = 1,
                Second,
                Last
            }

            #endregion


            #region MEMBER PROPERTIES

            public int ResultTypeId
            {
                get;
                private set;
            }

            public string ResultTypeName
            {
                get;
                private set;
            }

            public ResultTypePriority Priority
            {
                get;
                private set;
            }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ResultTypeRule(string resultTypeName, ResultTypePriority priority)
            {
                ResultTypeName = resultTypeName;
                Priority = priority;
            }

            public void SetResultTypeId(int resultTypeId)
            {
                ResultTypeId = resultTypeId;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public static void Initialize()
        {
            var resultTypes = m_repository.ReadAllResultTypes();
            foreach(var resultType in resultTypes)
            {
                ResultTypeRule rule = m_rules.FirstOrDefault(x => x.ResultTypeName == resultType.Name);
                if(rule != null)
                    rule.SetResultTypeId(resultType.Id);
            }
        }

        public static IEnumerable<Result> SortResults(IEnumerable<Result> results)
        {
            return (from result in results
                   join rule in m_rules on result.ResultType equals rule.ResultTypeId
                   orderby rule.Priority ascending
                   select result)
                   .ToList();
        }

        public static string GetResultTypeNameFromId(int resultTypeId)
        {
            ResultTypeRule rule = m_rules.FirstOrDefault(x => x.ResultTypeId == resultTypeId);
            return (rule != null) ? rule.ResultTypeName : string.Empty;
        }

        #endregion


        #region Private Functionality
        #endregion

        #endregion
    }

}