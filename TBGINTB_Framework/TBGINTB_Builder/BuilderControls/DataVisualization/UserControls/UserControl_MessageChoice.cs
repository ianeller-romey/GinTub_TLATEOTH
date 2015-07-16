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
    public class UserControl_MessageChoice : UserControl_Selecttable, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        TextBox
            m_textBox_name,
            m_textBox_text;
        ComboBox_Message m_comboBox_message;

        #endregion


        #region MEMBER PROPERTIES

        public int? MessageChoiceId { get; private set; }
        public string MessageChoiceName { get; private set; }
        public string MessageChoiceText { get; private set; }
        public int MessageId { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_name,
                    m_textBox_text,
                    m_comboBox_message
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_MessageChoice(int? messageChoiceId, string messageChoiceName, string messageChoiceText, int messageId, bool enableEditing, bool enableSelectting)
        {
            MessageChoiceId = messageChoiceId;
            MessageChoiceName = messageChoiceName;
            MessageChoiceText = messageChoiceText;
            MessageId = messageId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;

            if (enableSelectting)
                MouseLeftButtonDown += Grid_MessageData_MouseLeftButtonDown;

            GinTubBuilderManager.MessageRead += GinTubBuilderManager_MessageRead;

            GinTubBuilderManager.ReadAllMessages();
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.MessageChoiceUpdated += GinTubBuilderManager_MessageChoiceUpdated;
            GinTubBuilderManager.MessageChoiceSelect += GinTubBuilderManager_MessageChoiceSelect;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.MessageChoiceUpdated -= GinTubBuilderManager_MessageChoiceUpdated;
            GinTubBuilderManager.MessageChoiceSelect -= GinTubBuilderManager_MessageChoiceSelect;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            ////////
            // Id Grid
            Grid grid_id = new Grid();
            grid_id.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_id.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_id, 0, 0);

            ////////
            // Id
            TextBlock textBlock_id =
                new TextBlock()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = (MessageChoiceId.HasValue) ? MessageChoiceId.ToString() : "NewMessageChoice"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Name Grid
            Grid grid_name = new Grid();
            grid_name.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_name.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_name, 1, 0);

            ////////
            // Name
            m_textBox_name = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = MessageChoiceName };
            m_textBox_name.TextChanged += TextBox_Name_TextChanged;
            Label label_name = new Label() { Content = "Name:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_name.SetGridRowColumn(m_textBox_name, 1, 0);
            grid_name.SetGridRowColumn(label_name, 0, 0);

            ////////
            // Text Grid
            Grid grid_text = new Grid();
            grid_text.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_text.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_text, 2, 0);

            ////////
            // Text
            m_textBox_text = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = MessageChoiceText };
            m_textBox_text.TextChanged += TextBox_Text_TextChanged;
            Label label_text = new Label() { Content = "Text:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_text.SetGridRowColumn(m_textBox_text, 0, 1);
            grid_text.SetGridRowColumn(label_text, 0, 0);

            ////////
            // Message Grid
            Grid grid_message = new Grid();
            grid_message.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_message.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_message, 3, 0);

            ////////
            // Message
            m_comboBox_message = new ComboBox_Message();
            m_comboBox_message.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_message.SelectionChanged += ComboBox_Message_SelectionChanged;
            Label label_message = new Label() { Content = "Message:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_message.SetGridRowColumn(m_comboBox_message, 1, 0);
            grid_message.SetGridRowColumn(label_message, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_MessageRead(object sender, GinTubBuilderManager.MessageReadEventArgs args)
        {
            ResetMessageChoiceMessage(args.Id);
        }

        private void GinTubBuilderManager_MessageChoiceUpdated(object sender, GinTubBuilderManager.MessageChoiceUpdatedEventArgs args)
        {
            if (MessageChoiceId == args.Id)
            {
                SetMessageChoiceName(args.Name);
                SetMessageChoiceText(args.Text);
                SetMessageChoiceMessage(args.Message);
            }
        }

        private void GinTubBuilderManager_MessageChoiceSelect(object sender, GinTubBuilderManager.MessageChoiceSelectEventArgs args)
        {
            SetSelecttableBackground(MessageChoiceId == args.Id);
        }

        private void SetMessageChoiceName(string messageChoiceName)
        {
            m_textBox_name.Text = messageChoiceName;
            if (!m_textBox_name.IsEnabled)
                TextBox_Name_TextChanged(m_textBox_name, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void SetMessageChoiceText(string messageChoiceText)
        {
            m_textBox_text.Text = messageChoiceText;
            if (!m_textBox_text.IsEnabled)
                TextBox_Text_TextChanged(m_textBox_text, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void SetMessageChoiceMessage(int messageChoiceMessage)
        {
            ComboBox_Message.ComboBoxItem_Message item = m_comboBox_message.Items.OfType<ComboBox_Message.ComboBoxItem_Message>().
                SingleOrDefault(i => i.MessageId == messageChoiceMessage);
            if (item != null)
                m_comboBox_message.SelectedItem = item;
        }

        private void ResetMessageChoiceMessage(int messageChoiceMessage)
        {
            ComboBox_Message.ComboBoxItem_Message item = m_comboBox_message.Items.OfType<ComboBox_Message.ComboBoxItem_Message>().
                SingleOrDefault(i => MessageId == messageChoiceMessage && i.MessageId == messageChoiceMessage);
            if (item != null)
                m_comboBox_message.SelectedItem = item;
        }

        void TextBox_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_name)
                MessageChoiceName = m_textBox_name.Text;
        }

        void TextBox_Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_text)
                MessageChoiceText = m_textBox_text.Text;
        }

        void ComboBox_Message_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Message.ComboBoxItem_Message item;
            if (m_comboBox_message.SelectedItem != null && (item = m_comboBox_message.SelectedItem as ComboBox_Message.ComboBoxItem_Message) != null)
                MessageId = item.MessageId;
        }

        private void Grid_MessageData_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MessageChoiceId.HasValue)
                GinTubBuilderManager.SelectMessageChoice(MessageChoiceId.Value);
        }

        #endregion

        #endregion
    }
}
