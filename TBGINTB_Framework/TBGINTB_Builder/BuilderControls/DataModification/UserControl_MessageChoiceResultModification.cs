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
    public class UserControl_MessageChoiceResultModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_MessageChoiceResult m_userControl_messageChoiceMessageChoiceResult;

        #endregion


        #region MEMBER PROPERTIES

        public int? MessageChoiceResultId { get { return m_userControl_messageChoiceMessageChoiceResult.MessageChoiceResultId; } }
        public int? MessageChoiceResultResult { get { return m_userControl_messageChoiceMessageChoiceResult.MessageChoiceResultResult; } }
        public int? MessageChoiceResultMessageChoice { get { return m_userControl_messageChoiceMessageChoiceResult.MessageChoiceResultMessageChoice; } }
        private int MessageId { get; set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_MessageChoiceResultModification(int? messageChoiceMessageChoiceResultId, int messageChoiceResultResult, int messageChoiceResultMessageChoice, int messageId)
        {
            MessageId = messageId;
            CreateControls(messageChoiceMessageChoiceResultId, messageChoiceResultResult, messageChoiceResultMessageChoice);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_messageChoiceMessageChoiceResult.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_messageChoiceMessageChoiceResult.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? messageChoiceMessageChoiceResultId, int messageChoiceResultResult, int messageChoiceResultMessageChoice)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyMessageChoiceResult = new Button() { Content = "Modify MessageChoiceResult" };
            button_modifyMessageChoiceResult.Click += Button_ModifyMessageChoiceResult_Click;
            grid_main.SetGridRowColumn(button_modifyMessageChoiceResult, 0, 0);

            m_userControl_messageChoiceMessageChoiceResult = new UserControl_MessageChoiceResult(messageChoiceMessageChoiceResultId, messageChoiceResultResult, messageChoiceResultMessageChoice, MessageId, false);
            grid_main.SetGridRowColumn(m_userControl_messageChoiceMessageChoiceResult, 1, 0);
            m_userControl_messageChoiceMessageChoiceResult.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_ModifyMessageChoiceResult_Click(object sender, RoutedEventArgs e)
        {
            Window_MessageChoiceResult window =
                new Window_MessageChoiceResult
                (
                    m_userControl_messageChoiceMessageChoiceResult.MessageChoiceResultId,
                    m_userControl_messageChoiceMessageChoiceResult.MessageChoiceResultResult,
                    m_userControl_messageChoiceMessageChoiceResult.MessageChoiceResultMessageChoice,
                    MessageId,
                    (win) =>
                    {
                        Window_MessageChoiceResult wWin = win as Window_MessageChoiceResult;
                        if (wWin != null)
                            GinTubBuilderManager.ModifyMessageChoiceResult
                            (
                                wWin.MessageChoiceResultId.Value,
                                wWin.MessageChoiceResultResult.Value,
                                wWin.MessageChoiceResultMessageChoice.Value
                            );
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
