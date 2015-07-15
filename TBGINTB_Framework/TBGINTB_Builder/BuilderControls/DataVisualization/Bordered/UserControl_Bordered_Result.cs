using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_Bordered_Result : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_Bordered_Result(int? resultId, string resultName, string resultJSONData, int resultTypeId, bool enableEditing)
        {
            CreateControls(resultId, resultName, resultJSONData, resultTypeId, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_result.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_result.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? resultId, string resultName, string resultJSONData, int resultTypeId, bool enableEditing)
        {
            m_userControl_result = new UserControl_Result(resultId, resultName, resultJSONData, resultTypeId, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_result };
            Content = border;
        }

        #endregion

        #endregion
    }
}
