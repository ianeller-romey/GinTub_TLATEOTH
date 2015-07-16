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
    public class UserControl_MessageChoiceModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_MessageChoiceModification(int? messageChoiceId, string messageChoiceName, string messageChoiceText, int messageId)
        {
            CreateControls(messageChoiceId, messageChoiceName, messageChoiceText, messageId);
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

        private void CreateControls(int? messageChoiceId, string messageChoiceName, string messageChoiceText, int messageId)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyMessageChoice = new Button() { Content = "Modify MessageChoice" };
            button_modifyMessageChoice.Click += Button_UpdateMessageChoice_Click;
            grid_main.SetGridRowColumn(button_modifyMessageChoice, 0, 0);

            m_userControl_messageChoice = new UserControl_MessageChoice(messageChoiceId, messageChoiceName, messageChoiceText, messageId, false, true);
            grid_main.SetGridRowColumn(m_userControl_messageChoice, 1, 0);
            m_userControl_messageChoice.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_UpdateMessageChoice_Click(object sender, RoutedEventArgs e)
        {
            Window_MessageChoice window =
                new Window_MessageChoice
                (
                    m_userControl_messageChoice.MessageChoiceId,
                    m_userControl_messageChoice.MessageChoiceName, 
                    m_userControl_messageChoice.MessageChoiceText,
                    m_userControl_messageChoice.MessageId,
                    (win) =>
                    {
                        Window_MessageChoice wWin = win as Window_MessageChoice;
                        if (wWin != null)
                            GinTubBuilderManager.UpdateMessageChoice
                            (
                                wWin.MessageChoiceId.Value,
                                wWin.MessageChoiceName,
                                wWin.MessageChoiceText,
                                wWin.MessageId
                            );
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
