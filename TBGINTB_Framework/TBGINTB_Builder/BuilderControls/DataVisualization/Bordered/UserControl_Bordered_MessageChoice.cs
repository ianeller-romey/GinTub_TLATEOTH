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
    public class UserControl_Bordered_MessageChoice : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_MessageChoice m_userControl_messageChoice;

        #endregion


        #region MEMBER PROPERTIES

        public int? MessageChoiceId { get { return m_userControl_messageChoice.MessageChoiceId; } }
        public string MessageChoiceName { get { return m_userControl_messageChoice.MessageChoiceName; } }
        public string MessageChoiceText { get { return m_userControl_messageChoice.MessageChoiceText; } }
        public int MessageId { get { return m_userControl_messageChoice.MessageId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_MessageChoice(int? messageChoiceId, string messageChoiceName, string messageChoiceText, int messageId, bool enableEditing, bool enableSelectting)
        {
            CreateControls(messageChoiceId, messageChoiceName, messageChoiceText, messageId, enableEditing, enableSelectting);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_messageChoice.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_messageChoice.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? messageChoiceId, string messageChoiceName, string messageChoiceText, int messageId, bool enableEditing, bool enableSelectting)
        {
            m_userControl_messageChoice = new UserControl_MessageChoice(messageChoiceId, messageChoiceName, messageChoiceText, messageId, enableEditing, enableSelectting);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_messageChoice };
            Content = border;
        }

        #endregion

        #endregion
    }
}
