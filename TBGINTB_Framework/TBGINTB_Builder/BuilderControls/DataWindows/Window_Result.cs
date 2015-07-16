using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class Window_Result : Window_TaskOnAccept
    {
        #region MEMBER FIELDS

        UserControl_Result m_userControl_result;

        #endregion


        #region MEMBER PROPERTIES

        public int? ResultId { get { return m_userControl_result.ResultId; } }
        public string ResultName { get { return m_userControl_result.ResultName; } }
        public string ResultJSONData { get { return m_userControl_result.ResultJSONData; } }
        public int ResultTypeId { get { return m_userControl_result.ResultTypeId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_Result(int? resultId, string resultName, string resultJSONData, int resultTypeId, TaskOnAccept task) :
            base("Result Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(resultId, resultName, resultJSONData, resultTypeId);
            m_userControl_result.SetActiveAndRegisterForGinTubEvents(); // needed for possible nouns
            GinTubBuilderManager.ReadAllResultTypes();
            GinTubBuilderManager.ReadAllResultTypeJSONPropertiesForResultType(resultTypeId);
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? resultId, string resultName, string resultJSONData, int resultTypeId)
        {
            m_userControl_result = new UserControl_Result(resultId, resultName, resultJSONData, resultTypeId, true);
            return m_userControl_result;
        }

        #endregion

        #endregion
    }
}
