using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class TabItem_Messages : TabItem, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        Grid m_grid_main,
             m_grid_selectedMessage;
        ComboBox_Message m_comboBox_message;
        UserControl_MessageModification m_grid_messageModification;
        UserControl_MessageChoices m_grid_messageChoices;
        UserControl_MessageChoiceResults m_grid_messageChoiceResults;

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public TabItem_Messages()
        {
            Header = "Messages";
            Content = CreateControls();

            GinTubBuilderManager.LoadAllMessages();
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            if (m_grid_messageModification != null)
                m_grid_messageModification.SetActiveAndRegisterForGinTubEvents();
            if (m_grid_messageChoices != null)
                m_grid_messageChoices.SetActiveAndRegisterForGinTubEvents();
            if (m_grid_messageChoiceResults != null)
                m_grid_messageChoiceResults.SetActiveAndRegisterForGinTubEvents();

            GinTubBuilderManager.MessageGet += GinTubBuilderManager_MessageGet;
            GinTubBuilderManager.MessageChoiceGet += GinTubBuilderManager_MessageChoiceGet;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            if (m_grid_messageModification != null)
                m_grid_messageModification.SetInactiveAndUnregisterFromGinTubEvents();
            if (m_grid_messageChoices != null)
                m_grid_messageChoices.SetInactiveAndUnregisterFromGinTubEvents();
            if (m_grid_messageChoiceResults != null)
                m_grid_messageChoiceResults.SetInactiveAndUnregisterFromGinTubEvents();

            GinTubBuilderManager.MessageGet -= GinTubBuilderManager_MessageGet;
            GinTubBuilderManager.MessageChoiceGet -= GinTubBuilderManager_MessageChoiceGet;
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls()
        {
            m_grid_main = new Grid();
            m_grid_main.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(33.0, GridUnitType.Star) });
            m_grid_main.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(33.0, GridUnitType.Star) });
            m_grid_main.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(33.0, GridUnitType.Star) });

            m_grid_selectedMessage = new Grid();
            m_grid_selectedMessage.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            m_grid_selectedMessage.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            m_grid_main.SetGridRowColumn(m_grid_selectedMessage, 0, 0);

            m_comboBox_message = new ComboBox_Message();
            m_comboBox_message.SetActiveAndRegisterForGinTubEvents();
            m_comboBox_message.SelectionChanged += ComboBox_Event_SelectionChanged;
            m_grid_selectedMessage.SetGridRowColumn(m_comboBox_message, 0, 0);

            return m_grid_main;
        }

        private void GinTubBuilderManager_MessageGet(object sender, GinTubBuilderManager.MessageGetEventArgs args)
        {
            LoadMessage(args.Id);
        }

        private void GinTubBuilderManager_MessageChoiceGet(object sender, GinTubBuilderManager.MessageChoiceGetEventArgs args)
        {
            LoadMessageChoice(args.Id, args.Message);
        }

        private void LoadMessage(int messageId)
        {
            UnloadMessage();

            m_grid_messageChoices = new UserControl_MessageChoices(messageId);
            m_grid_messageChoices.SetActiveAndRegisterForGinTubEvents();
            m_grid_main.SetGridRowColumn(m_grid_messageChoices, 0, 1);
            GinTubBuilderManager.LoadAllMessageChoicesForMessage(messageId);
        }

        private void LoadMessageChoice(int messageChoiceId, int messageId)
        {
            UnloadMessageChoice();

            m_grid_messageChoiceResults = new UserControl_MessageChoiceResults(messageChoiceId, messageId);
            m_grid_messageChoiceResults.SetActiveAndRegisterForGinTubEvents();
            m_grid_main.SetGridRowColumn(m_grid_messageChoiceResults, 0, 2);
            GinTubBuilderManager.LoadAllMessageChoiceResultsForMessageChoice(messageChoiceId);
        }

        private void UnloadMessage()
        {
            UnloadMessageChoice();

            if (m_grid_messageChoices != null)
                m_grid_main.Children.Remove(m_grid_messageChoices);
        }

        private void UnloadMessageChoice()
        {
            if (m_grid_messageChoiceResults != null)
                m_grid_main.Children.Remove(m_grid_messageChoiceResults);
        }

        private void ComboBox_Event_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Message comboBox = sender as ComboBox_Message;
            ComboBox_Message.ComboBoxItem_Message comboBoxItem;
            if (comboBox.SelectedItem != null && (comboBoxItem = comboBox.SelectedItem as ComboBox_Message.ComboBoxItem_Message) != null)
            {
                if (m_grid_messageModification != null)
                    m_grid_selectedMessage.Children.Remove(m_grid_messageModification);
                m_grid_messageModification = new UserControl_MessageModification(comboBoxItem.MessageId, comboBoxItem.MessageName, comboBoxItem.MessageText);
                m_grid_messageModification.SetActiveAndRegisterForGinTubEvents();
                m_grid_selectedMessage.SetGridRowColumn(m_grid_messageModification, 1, 0);
            }
        }

        #endregion

        #endregion

    }
}
