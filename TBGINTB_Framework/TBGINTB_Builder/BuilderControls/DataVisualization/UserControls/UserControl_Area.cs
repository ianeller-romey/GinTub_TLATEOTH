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
    public class UserControl_Area : UserControl
    {
        #region MEMBER FIELDS

        TextBox m_textBox_name;

        #endregion


        #region MEMBER PROPERTIES

        public int? AreaId { get; private set; }
        public string AreaName { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_name
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Area(int? areaId, string areaName, bool enableEditing)
        {
            AreaId = areaId;
            AreaName = areaName;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.AreaUpdated += GinTubBuilderManager_AreaUpdated;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.AreaUpdated -= GinTubBuilderManager_AreaUpdated;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
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
                    Text = (AreaId.HasValue) ? AreaId.ToString() : "NewArea"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Name Grid
            Grid grid_text = new Grid();
            grid_text.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_text.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_text, 1, 0);

            ////////
            // Name
            m_textBox_name = new TextBox();
            m_textBox_name.TextChanged += TextBox_Name_TextChanged;
            Label label_text = new Label() { Content = "Name:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_text.SetGridRowColumn(m_textBox_name, 1, 0);
            grid_text.SetGridRowColumn(label_text, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_AreaUpdated(object sender, GinTubBuilderManager.AreaUpdatedEventArgs args)
        {
            if (AreaId == args.Id)
                SetAreaName(args.Name);
        }

        private void SetAreaName(string areaName)
        {
            m_textBox_name.Text = areaName;
            if (!m_textBox_name.IsEnabled)
                TextBox_Name_TextChanged(m_textBox_name, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void TextBox_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_name)
                AreaName = m_textBox_name.Text;
        }

        #endregion

        #endregion
    }
}
