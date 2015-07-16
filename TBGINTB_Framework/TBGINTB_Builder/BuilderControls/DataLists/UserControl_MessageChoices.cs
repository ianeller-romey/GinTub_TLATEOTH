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
    public class UserControl_MessageChoices : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly Button c_button_newMessageChoice = new Button() { Content = "New Message Choice ..." };

        private StackPanel m_stackPanel_messageChoices;

        #endregion


        #region MEMBER PROPERTIES

        public int MessageId { get; set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_MessageChoices(int messageId)
        {
            MessageId = messageId;

            CreateControls();

            c_button_newMessageChoice.Click += Button_NewMessageChoice_Click;
        }
    
        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.MessageChoiceRead += GinTubBuilderManager_MessageChoiceRead;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.MessageChoiceRead -= GinTubBuilderManager_MessageChoiceRead;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });

            grid_main.SetGridRowColumn(c_button_newMessageChoice, 0, 0);

            m_stackPanel_messageChoices = new StackPanel() { Orientation = Orientation.Vertical };
            grid_main.SetGridRowColumn(m_stackPanel_messageChoices, 1, 0);

            Content = grid_main;
        }

        private void GinTubBuilderManager_MessageChoiceRead(object sender, GinTubBuilderManager.MessageChoiceReadEventArgs args)
        {
            if (MessageId == args.Message && !m_stackPanel_messageChoices.Children.OfType<UserControl_MessageChoiceModification>().Any(i => i.MessageChoiceId == args.Id))
            {
                UserControl_MessageChoiceModification grid = new UserControl_MessageChoiceModification(args.Id, args.Name, args.Text, args.Message);
                grid.SetActiveAndRegisterForGinTubEvents();
                m_stackPanel_messageChoices.Children.Add(grid);
                GinTubBuilderManager.ReadAllMessages();
            }
        }

        private void NewMessageChoiceDialog()
        {
            Window_MessageChoice window = 
                new Window_MessageChoice
                (
                    null, 
                    null, 
                    null,
                    MessageId,
                    (win) =>
                    {
                        Window_MessageChoice wWin = win as Window_MessageChoice;
                        if (wWin != null)
                            GinTubBuilderManager.CreateMessageChoice
                            (
                                wWin.MessageChoiceName,
                                wWin.MessageChoiceText,
                                wWin.MessageId
                            );
                    }
                );
            window.Show();
        }
        
        private void Button_NewMessageChoice_Click(object sender, RoutedEventArgs e)
        {
            Button item = null;
            if ((item = sender as Button) != null && item == c_button_newMessageChoice)
                NewMessageChoiceDialog();
        }

        #endregion

        #endregion
    }
}
