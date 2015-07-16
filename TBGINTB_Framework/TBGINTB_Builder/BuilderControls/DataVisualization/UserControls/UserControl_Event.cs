using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_Event : UserControl
    {
        #region MEMBER FIELDS

        TextBox
            m_textBox_name,
            m_textBox_description;

        #endregion


        #region MEMBER PROPERTIES

        public int? EventId { get; private set; }
        public string EventName { get; private set; }
        public string EventDescription { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_name,
                    m_textBox_description
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Event(int? evntId, string evntName, string evntDescription, bool enableEditing)
        {
            EventId = evntId;
            EventName = evntName;
            EventDescription = evntDescription;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.EventUpdated += GinTubBuilderManager_EventUpdated;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.EventUpdated -= GinTubBuilderManager_EventUpdated;
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
                    Text = (EventId.HasValue) ? EventId.ToString() : "NewEvent"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Event Grid
            Grid grid_name = new Grid();
            grid_name.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_name.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_name, 1, 0);

            ////////
            // Event
            m_textBox_name = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = EventName };
            m_textBox_name.TextChanged += TextBox_Name_TextChanged;
            Label label_name = new Label() { Content = "Event:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_name.SetGridRowColumn(m_textBox_name, 1, 0);
            grid_name.SetGridRowColumn(label_name, 0, 0);

            ////////
            // Description Grid
            Grid grid_description = new Grid();
            grid_description.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_description.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_description, 2, 0);

            ////////
            // Description
            m_textBox_description = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = EventDescription };
            m_textBox_description.TextChanged += TextBox_Description_TextChanged;
            Label label_description = new Label() { Content = "Description:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_description.SetGridRowColumn(m_textBox_description, 0, 1);
            grid_description.SetGridRowColumn(label_description, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        void GinTubBuilderManager_EventUpdated(object sender, GinTubBuilderManager.EventUpdatedEventArgs args)
        {
            if (EventId == args.Id)
            {
                SetEventName(args.Name);
                SetEventDescription(args.Description);
            }
        }

        private void SetEventName(string evntName)
        {
            m_textBox_name.Text = evntName;
            if (!m_textBox_name.IsEnabled)
                TextBox_Name_TextChanged(m_textBox_name, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void SetEventDescription(string evntDescription)
        {
            m_textBox_description.Text = evntDescription;
            if (!m_textBox_description.IsEnabled)
                TextBox_Description_TextChanged(m_textBox_description, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        void TextBox_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_name)
                EventName = m_textBox_name.Text;
        }

        void TextBox_Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_description)
                EventDescription = m_textBox_description.Text;
        }

        #endregion

        #endregion
    }
}
