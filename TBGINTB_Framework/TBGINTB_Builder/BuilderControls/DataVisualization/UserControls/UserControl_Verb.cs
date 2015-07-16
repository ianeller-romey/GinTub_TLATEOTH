using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_Verb : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        TextBox m_textBox_name;
        ComboBox_VerbType m_comboBox_verbType;

        #endregion


        #region MEMBER PROPERTIES

        public int? VerbId { get; private set; }
        public string VerbName { get; private set; }
        public int VerbTypeId { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_name,
                    m_comboBox_verbType
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Verb(int? verbId, string verbName, int verbTypeId, bool enableEditing)
        {
            VerbId = verbId;
            VerbName = verbName;
            VerbTypeId = verbTypeId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.VerbUpdated += GinTubBuilderManager_VerbUpdated;

            GinTubBuilderManager.VerbTypeRead += GinTubBuilderManager_VerbTypeRead;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.VerbUpdated -= GinTubBuilderManager_VerbUpdated;

            GinTubBuilderManager.VerbTypeRead -= GinTubBuilderManager_VerbTypeRead;
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
                    Text = (VerbId.HasValue) ? VerbId.ToString() : "NewVerb"
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
            m_textBox_name = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = VerbName };
            m_textBox_name.TextChanged += TextBox_Name_TextChanged;
            Label label_name = new Label() { Content = "Name:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_name.SetGridRowColumn(m_textBox_name, 1, 0);
            grid_name.SetGridRowColumn(label_name, 0, 0);

            ////////
            // VerbTypeId Grid
            Grid grid_verbType = new Grid();
            grid_verbType.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_verbType.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_verbType, 2, 0);

            ////////
            // VerbTypeId
            m_comboBox_verbType = new ComboBox_VerbType();
            m_comboBox_verbType.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_verbType.SelectionChanged += ComboBox_VerbType_SelectionChanged;
            SetVerbTypeId(VerbTypeId);
            Label label_verbType = new Label() { Content = "Verb Type:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_verbType.SetGridRowColumn(m_comboBox_verbType, 0, 1);
            grid_verbType.SetGridRowColumn(label_verbType, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        void GinTubBuilderManager_VerbUpdated(object sender, GinTubBuilderManager.VerbUpdatedEventArgs args)
        {
            if (VerbId == args.Id)
            {
                SetVerbName(args.Name);
                SetVerbTypeId(args.VerbType);
            }
        }
        

        void GinTubBuilderManager_VerbTypeRead(object sender, GinTubBuilderManager.VerbTypeReadEventArgs args)
        {
            if (VerbTypeId == args.Id)
                m_comboBox_verbType.SelectedItem = m_comboBox_verbType.Items.OfType<ComboBox_VerbType.ComboBoxItem_VerbType>().SingleOrDefault(v => v.VerbTypeId == VerbTypeId);
        }

        private void SetVerbName(string verbName)
        {
            m_textBox_name.Text = verbName;
            if (!m_textBox_name.IsEnabled)
                TextBox_Name_TextChanged(m_textBox_name, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void SetVerbTypeId(int verbTypeId)
        {
            VerbTypeId = verbTypeId;
            m_comboBox_verbType.SelectedItem = m_comboBox_verbType.Items.OfType<ComboBox_VerbType.ComboBoxItem_VerbType>().SingleOrDefault(v => v.VerbTypeId == VerbTypeId);
        }

        void TextBox_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_name)
                VerbName = m_textBox_name.Text;
        }

        private void ComboBox_VerbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_VerbType comboBox = sender as ComboBox_VerbType;
            ComboBox_VerbType.ComboBoxItem_VerbType comboBoxItem = null;
            if (comboBox != null && (comboBoxItem = comboBox.SelectedItem as ComboBox_VerbType.ComboBoxItem_VerbType) != null)
                VerbTypeId = comboBoxItem.VerbTypeId;
        }

        #endregion

        #endregion
    }
}
