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
    public class UserControl_Bordered_MessageChoiceResult : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_MessageChoiceResult m_userControl_messageChoiceResult;

        #endregion


        #region MEMBER PROPERTIES

        public int? MessageChoiceResultId { get { return m_userControl_messageChoiceResult.MessageChoiceResultId; } }
        public int? MessageChoiceResultResult { get { return m_userControl_messageChoiceResult.MessageChoiceResultResult; } }
        public int? MessageChoiceResultMessageChoice { get { return m_userControl_messageChoiceResult.MessageChoiceResultMessageChoice; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_MessageChoiceResult(int? messageChoiceResultId, int? messageChoiceResultResult, int? messageChoiceResultMessageChoice, int messageId, bool enableEditing)
        {
            CreateControls(messageChoiceResultId, messageChoiceResultResult, messageChoiceResultMessageChoice, messageId, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_messageChoiceResult.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_messageChoiceResult.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? messageChoiceResultId, int? messageChoiceResultResult, int? messageChoiceResultMessageChoice, int messageId, bool enableEditing)
        {
            m_userControl_messageChoiceResult = new UserControl_MessageChoiceResult(messageChoiceResultId, messageChoiceResultResult, messageChoiceResultMessageChoice, messageId, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_messageChoiceResult };
            Content = border;
        }

        #endregion

        #endregion
    }
}
