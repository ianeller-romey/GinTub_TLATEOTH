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
    public class UserControl_Bordered_Message : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Message m_userControl_message;

        #endregion


        #region MEMBER PROPERTIES

        public int? MessageId { get { return m_userControl_message.MessageId; } }
        public string MessageName { get { return m_userControl_message.MessageName; } }
        public string MessageText { get { return m_userControl_message.MessageText; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_Message(int? messageId, string messageName, string messageText, bool enableEditing, bool enableGetting)
        {
            CreateControls(messageId, messageName, messageText, enableEditing, enableGetting);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_message.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_message.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? messageId, string messageName, string messageText, bool enableEditing, bool enableGetting)
        {
            m_userControl_message = new UserControl_Message(messageId, messageName, messageText, enableEditing, enableGetting);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_message };
            Content = border;
        }

        #endregion

        #endregion
    }
}
