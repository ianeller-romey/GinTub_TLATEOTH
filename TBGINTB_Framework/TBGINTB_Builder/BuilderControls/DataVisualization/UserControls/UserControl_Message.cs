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
    public class UserControl_Message : UserControl_Gettable, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        TextBox
            m_textBox_name,
            m_textBox_text;

        #endregion


        #region MEMBER PROPERTIES

        public int? MessageId { get; private set; }
        public string MessageName { get; private set; }
        public string MessageText { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_name,
                    m_textBox_text
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Message(int? messageId, string messageName, string messageText, bool enableEditing, bool enableGetting)
        {
            MessageId = messageId;
            MessageName = messageName;
            MessageText = messageText;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;

            if (enableGetting)
                MouseLeftButtonDown += Grid_MessageData_MouseLeftButtonDown;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.MessageModified += GinTubBuilderManager_MessageModified;
            GinTubBuilderManager.MessageGet += GinTubBuilderManager_MessageGet;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.MessageModified -= GinTubBuilderManager_MessageModified;
            GinTubBuilderManager.MessageGet -= GinTubBuilderManager_MessageGet;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
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
                    Text = (MessageId.HasValue) ? MessageId.ToString() : "NewMessage"
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
            m_textBox_name = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = MessageName };
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
            m_textBox_text = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = MessageText };
            m_textBox_text.TextChanged += TextBox_Text_TextChanged;
            Label label_text = new Label() { Content = "Text:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_text.SetGridRowColumn(m_textBox_text, 0, 1);
            grid_text.SetGridRowColumn(label_text, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_MessageModified(object sender, GinTubBuilderManager.MessageModifiedEventArgs args)
        {
            if (MessageId == args.Id)
            {
                SetMessageName(args.Name);
                SetMessageText(args.Text);
            }
        }

        private void GinTubBuilderManager_MessageGet(object sender, GinTubBuilderManager.MessageGetEventArgs args)
        {
            SetGettableBackground(MessageId == args.Id);
        }

        private void SetMessageName(string messageName)
        {
            m_textBox_name.Text = messageName;
            if (!m_textBox_name.IsEnabled)
                TextBox_Name_TextChanged(m_textBox_name, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void SetMessageText(string messageText)
        {
            m_textBox_text.Text = messageText;
            if (!m_textBox_text.IsEnabled)
                TextBox_Text_TextChanged(m_textBox_text, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void TextBox_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_name)
                MessageName = m_textBox_name.Text;
        }

        private void TextBox_Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_text)
                MessageText = m_textBox_text.Text;
        }

        private void Grid_MessageData_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MessageId.HasValue)
                GinTubBuilderManager.GetMessage(MessageId.Value);
        }

        #endregion

        #endregion
    }
}
