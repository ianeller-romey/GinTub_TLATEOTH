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
    public class Window_MessageChoice : Window_TaskOnAccept
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

        public Window_MessageChoice(int? messageChoiceId, string messageChoiceName, string messageChoiceText, int messageId, TaskOnAccept task) :
            base("Message Choice Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(messageChoiceId, messageChoiceName, messageChoiceText, messageId);
            m_userControl_messageChoice.SetActiveAndRegisterForGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? messageChoiceId, string messageChoiceName, string messageChoiceText, int messageId)
        {
            m_userControl_messageChoice = new UserControl_MessageChoice(messageChoiceId, messageChoiceName, messageChoiceText, messageId, true, false);
            return m_userControl_messageChoice;
        }

        #endregion

        #endregion
    }
}
