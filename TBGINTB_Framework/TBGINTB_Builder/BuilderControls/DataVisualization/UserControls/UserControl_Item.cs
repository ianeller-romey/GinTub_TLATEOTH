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
    public class UserControl_Item : UserControl
    {
        #region MEMBER FIELDS

        TextBox
            m_textBox_name,
            m_textBox_description;

        #endregion


        #region MEMBER PROPERTIES

        public int? ItemId { get; private set; }
        public string ItemName { get; private set; }
        public string ItemDescription { get; private set; }

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

        public UserControl_Item(int? itemId, string itemName, string itemDescription, bool enableEditing)
        {
            ItemId = itemId;
            ItemName = itemName;
            ItemDescription = itemDescription;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ItemUpdated += GinTubBuilderManager_ItemUpdated;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ItemUpdated -= GinTubBuilderManager_ItemUpdated;
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
                    Text = (ItemId.HasValue) ? ItemId.ToString() : "NewItem"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Item Grid
            Grid grid_name = new Grid();
            grid_name.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_name.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_name, 1, 0);

            ////////
            // Item
            m_textBox_name = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = ItemName };
            m_textBox_name.TextChanged += TextBox_Name_TextChanged;
            Label label_name = new Label() { Content = "Item:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
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
            m_textBox_description = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = ItemDescription };
            m_textBox_description.TextChanged += TextBox_Description_TextChanged;
            Label label_description = new Label() { Content = "Description:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_description.SetGridRowColumn(m_textBox_description, 0, 1);
            grid_description.SetGridRowColumn(label_description, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        void GinTubBuilderManager_ItemUpdated(object sender, GinTubBuilderManager.ItemUpdatedEventArgs args)
        {
            if (ItemId == args.Id)
            {
                SetItemName(args.Name);
                SetItemDescription(args.Description);
            }
        }

        private void SetItemName(string itemName)
        {
            m_textBox_name.Text = itemName;
            if (!m_textBox_name.IsEnabled)
                TextBox_Name_TextChanged(m_textBox_name, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void SetItemDescription(string itemDescription)
        {
            m_textBox_description.Text = itemDescription;
            if (!m_textBox_description.IsEnabled)
                TextBox_Description_TextChanged(m_textBox_description, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        void TextBox_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_name)
                ItemName = m_textBox_name.Text;
        }

        void TextBox_Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_description)
                ItemDescription = m_textBox_description.Text;
        }

        #endregion

        #endregion
    }
}
