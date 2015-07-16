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
    public class UserControl_ResultTypeJSONProperty : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        TextBox m_textBox_jsonProperty;
        ComboBox_JSONPropertyDataType m_comboBox_dataType;
        ComboBox_ResultType m_comboBox_resultType;

        #endregion


        #region MEMBER PROPERTIES

        public int? ResultTypeJSONPropertyId { get; private set; }
        public string ResultTypeJSONPropertyJSONProperty { get; private set; }
        public int? ResultTypeJSONPropertyDataType { get; private set; }
        public int ResultTypeId { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_jsonProperty,
                    m_comboBox_dataType,
                    m_comboBox_resultType
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ResultTypeJSONProperty
        (
            int? resultTypeJSONPropertyId, 
            string resultTypeJSONPropertyJSONProperty, 
            int? resultTypeJSONPropertyDataType,
            int resultTypeId, 
            bool enableEditing
        )
        {
            ResultTypeJSONPropertyId = resultTypeJSONPropertyId;
            ResultTypeJSONPropertyJSONProperty = resultTypeJSONPropertyJSONProperty;
            ResultTypeJSONPropertyDataType = resultTypeJSONPropertyDataType;
            ResultTypeId = resultTypeId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ResultTypeJSONPropertyUpdated += GinTubBuilderManager_ResultTypeJSONPropertyUpdated;

            GinTubBuilderManager.JSONPropertyDataTypeRead += GinTubBuilderManager_JSONPropertyDataTypeRead;

            GinTubBuilderManager.ResultTypeRead += GinTubBuilderManager_ResultTypeRead;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ResultTypeJSONPropertyUpdated -= GinTubBuilderManager_ResultTypeJSONPropertyUpdated;

            GinTubBuilderManager.JSONPropertyDataTypeRead -= GinTubBuilderManager_JSONPropertyDataTypeRead;

            GinTubBuilderManager.ResultTypeRead -= GinTubBuilderManager_ResultTypeRead;
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
                    Text = (ResultTypeJSONPropertyId.HasValue) ? ResultTypeJSONPropertyId.ToString() : "NewResultTypeJSONProperty"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Property Grid
            Grid grid_field = new Grid();
            grid_field.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_field.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_field, 1, 0);

            ////////
            // Property
            m_textBox_jsonProperty = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = ResultTypeJSONPropertyJSONProperty };
            m_textBox_jsonProperty.TextChanged += TextBox_JSONProperty_TextChanged;
            Label jsonProperty = new Label() { Content = "Field:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_field.SetGridRowColumn(m_textBox_jsonProperty, 1, 0);
            grid_field.SetGridRowColumn(jsonProperty, 0, 0);

            ////////
            // DataType Grid
            Grid grid_dataType = new Grid();
            grid_dataType.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_dataType.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_dataType, 2, 0);

            ////////
            // DataType
            m_comboBox_dataType = new ComboBox_JSONPropertyDataType();
            m_comboBox_dataType.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_dataType.SelectionChanged += ComboBox_DataType_SelectionChanged;
            SetDataType(ResultTypeJSONPropertyDataType);
            Label label_dataType = new Label() { Content = "Data Type:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_dataType.SetGridRowColumn(m_comboBox_dataType, 0, 1);
            grid_dataType.SetGridRowColumn(label_dataType, 0, 0);

            ////////
            // ResultType Grid
            Grid grid_resultType = new Grid();
            grid_resultType.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_resultType.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_resultType, 3, 0);

            ////////
            // ResultType
            m_comboBox_resultType = new ComboBox_ResultType();
            m_comboBox_resultType.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_resultType.SelectionChanged += ComboBox_ResultType_SelectionChanged;
            SetResultTypeId(ResultTypeId);
            Label label_resultType = new Label() { Content = "Result Type:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_resultType.SetGridRowColumn(m_comboBox_resultType, 0, 1);
            grid_resultType.SetGridRowColumn(label_resultType, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_ResultTypeJSONPropertyUpdated(object sender, GinTubBuilderManager.ResultTypeJSONPropertyUpdatedEventArgs args)
        {
            if (ResultTypeJSONPropertyId == args.Id)
            {
                SetResultTypeJSONPropertyJSONProperty(args.JSONProperty);
                SetResultTypeId(args.ResultType);
            }
        }

        private void GinTubBuilderManager_JSONPropertyDataTypeRead(object sender, GinTubBuilderManager.JSONPropertyDataTypeReadEventArgs args)
        {
            if (ResultTypeJSONPropertyDataType == args.Id)
                m_comboBox_dataType.SelectedItem = m_comboBox_dataType.Items.OfType<ComboBox_JSONPropertyDataType.ComboBoxItem_JSONPropertyDataType>().SingleOrDefault(r => r.JSONPropertyDataTypeId == ResultTypeJSONPropertyDataType);
        }

        private void GinTubBuilderManager_ResultTypeRead(object sender, GinTubBuilderManager.ResultTypeReadEventArgs args)
        {
            if (ResultTypeId == args.Id)
                m_comboBox_resultType.SelectedItem = m_comboBox_resultType.Items.OfType<ComboBox_ResultType.ComboBoxItem_ResultType>().SingleOrDefault(r => r.ResultTypeId == ResultTypeId);
        }

        private void SetResultTypeJSONPropertyJSONProperty(string resultTypeJSONPropertyJSONProperty)
        {
            m_textBox_jsonProperty.Text = resultTypeJSONPropertyJSONProperty;
            if (!m_textBox_jsonProperty.IsEnabled)
                TextBox_JSONProperty_TextChanged(m_textBox_jsonProperty, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void SetDataType(int? resultTypeJSONPropertyDataType)
        {
            ResultTypeJSONPropertyDataType = resultTypeJSONPropertyDataType;
            m_comboBox_dataType.SelectedItem = m_comboBox_dataType.Items.OfType<ComboBox_JSONPropertyDataType.ComboBoxItem_JSONPropertyDataType>().SingleOrDefault(r => r.JSONPropertyDataTypeId == ResultTypeJSONPropertyDataType);
        }

        private void SetResultTypeId(int resultTypeJSONPropertyTypeId)
        {
            ResultTypeId = resultTypeJSONPropertyTypeId;
            m_comboBox_resultType.SelectedItem = m_comboBox_resultType.Items.OfType<ComboBox_ResultType.ComboBoxItem_ResultType>().SingleOrDefault(r => r.ResultTypeId == ResultTypeId);
        }

        private void TextBox_JSONProperty_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_jsonProperty)
                ResultTypeJSONPropertyJSONProperty = m_textBox_jsonProperty.Text;
        }

        private void ComboBox_DataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_JSONPropertyDataType comboBox = sender as ComboBox_JSONPropertyDataType;
            ComboBox_JSONPropertyDataType.ComboBoxItem_JSONPropertyDataType comboBoxItem = null;
            if (comboBox != null && (comboBoxItem = comboBox.SelectedItem as ComboBox_JSONPropertyDataType.ComboBoxItem_JSONPropertyDataType) != null)
                ResultTypeJSONPropertyDataType = comboBoxItem.JSONPropertyDataTypeId;
        }

        private void ComboBox_ResultType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_ResultType comboBox = sender as ComboBox_ResultType;
            ComboBox_ResultType.ComboBoxItem_ResultType comboBoxItem = null;
            if (comboBox != null && (comboBoxItem = comboBox.SelectedItem as ComboBox_ResultType.ComboBoxItem_ResultType) != null)
                ResultTypeId = comboBoxItem.ResultTypeId;
        }

        #endregion

        #endregion
    }
}
