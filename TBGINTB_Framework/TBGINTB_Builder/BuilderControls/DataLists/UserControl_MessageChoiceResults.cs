using System;
using System.Collections.Generic;
using System.IO;
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
    public class UserControl_MessageChoiceResults : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly Button c_button_newMessageChoiceResult = new Button() { Content = "New Message Choice Result ..." };

        private StackPanel m_stackPanel_messageChoiceResults;

        #endregion


        #region MEMBER PROPERTIES

        public int MessageChoiceId { get; set; }
        public int MessageId { get; set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_MessageChoiceResults(int messageChoiceId, int messageId)
        {
            MessageChoiceId = messageChoiceId;
            MessageId = messageId;

            CreateControls();

            c_button_newMessageChoiceResult.Click += Button_NewMessageChoiceResult_Click;
        }
    
        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.MessageChoiceResultRead += GinTubBuilderManager_MessageChoiceResultRead;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.MessageChoiceResultRead -= GinTubBuilderManager_MessageChoiceResultRead;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid userControl_main = new Grid();
            userControl_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            userControl_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });

            userControl_main.SetGridRowColumn(c_button_newMessageChoiceResult, 0, 0);

            m_stackPanel_messageChoiceResults = new StackPanel() { Orientation = Orientation.Vertical };
            userControl_main.SetGridRowColumn(m_stackPanel_messageChoiceResults, 1, 0);

            Content = userControl_main;
        }

        private void GinTubBuilderManager_MessageChoiceResultRead(object sender, GinTubBuilderManager.MessageChoiceResultReadEventArgs args)
        {
            if (MessageChoiceId == args.MessageChoice && !m_stackPanel_messageChoiceResults.Children.OfType<UserControl_MessageChoiceResultModification>().Any(i => i.MessageChoiceResultId == args.Id))
            {
                UserControl_MessageChoiceResultModification grid = new UserControl_MessageChoiceResultModification(args.Id, args.Result, args.MessageChoice, MessageId);
                grid.SetActiveAndRegisterForGinTubEvents();
                m_stackPanel_messageChoiceResults.Children.Add(grid);
                GinTubBuilderManager.ReadAllResultsForMessageChoiceResultType(args.MessageChoice);
                GinTubBuilderManager.ReadAllMessageChoicesForMessage(MessageId);
            }
        }

        private void NewMessageChoiceResultDialog()
        {
            Window_ResultType window_resultType =
                new Window_ResultType
                (
                    null,
                    null,
                    (win) =>
                    {
                        Window_ResultType wWin = win as Window_ResultType;
                        if (wWin != null)
                        {
                            Window_MessageChoiceResult window_messageChoiceResult =
                                new Window_MessageChoiceResult
                                (
                                    null,
                                    null,
                                    MessageChoiceId,
                                    MessageId,
                                    wWin.ResultTypeId.Value,
                                    (wwWin) =>
                                    {
                                        Window_MessageChoiceResult wwwWin = wwWin as Window_MessageChoiceResult;
                                        if (wwwWin != null)
                                            GinTubBuilderManager.CreateMessageChoiceResult
                                            (
                                                wwwWin.MessageChoiceResultResult.Value,
                                                wwwWin.MessageChoiceResultMessageChoice.Value
                                            );
                                    }
                                );
                            window_messageChoiceResult.Show();
                        }
                    }
                );
            window_resultType.Show();
        }
        
        private void Button_NewMessageChoiceResult_Click(object sender, RoutedEventArgs e)
        {
            Button item = null;
            if ((item = sender as Button) != null && item == c_button_newMessageChoiceResult)
                NewMessageChoiceResultDialog();
        }

        #endregion

        #endregion
    }
}
