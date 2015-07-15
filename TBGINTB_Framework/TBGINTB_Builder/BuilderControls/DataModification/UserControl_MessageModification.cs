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
    public class UserControl_MessageModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_MessageModification(int? messageId, string messageName, string messageText)
        {
            CreateControls(messageId, messageName, messageText);
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

        private void CreateControls(int? messageId, string messageName, string messageText)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyMessage = new Button() { Content = "Modify Message" };
            button_modifyMessage.Click += Button_ModifyMessage_Click;
            grid_main.SetGridRowColumn(button_modifyMessage, 0, 0);

            m_userControl_message = new UserControl_Message(messageId, messageName, messageText, false, true);
            grid_main.SetGridRowColumn(m_userControl_message, 1, 0);
            m_userControl_message.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_ModifyMessage_Click(object sender, RoutedEventArgs e)
        {
            Window_Message window =
                new Window_Message
                (
                    m_userControl_message.MessageId, 
                    m_userControl_message.MessageName,
                    m_userControl_message.MessageText,
                    (win) =>
                    {
                        Window_Message wWin = win as Window_Message;
                        if (wWin != null)
                            GinTubBuilderManager.ModifyMessage(wWin.MessageId.Value, wWin.MessageName, wWin.MessageText);
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
