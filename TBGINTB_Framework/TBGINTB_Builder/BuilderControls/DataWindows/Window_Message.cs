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
    public class Window_Message : Window_TaskOnAccept
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

        public Window_Message(int? messageId, string messageName, string messageText, TaskOnAccept task) :
            base("Message Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(messageId, messageName, messageText);
            m_userControl_message.SetActiveAndRegisterForGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? messageId, string messageName, string messageText)
        {
            m_userControl_message = new UserControl_Message(messageId, messageName, messageText, true, false);
            return m_userControl_message;
        }

        #endregion

        #endregion
    }
}
