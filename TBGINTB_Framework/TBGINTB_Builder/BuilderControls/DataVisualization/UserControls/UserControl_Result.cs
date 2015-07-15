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
    public class UserControl_Result : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        TextBox m_textBox_name;
        GroupBox m_groupBox_jsonProperties;
        StackPanel m_stackPanel_jsonProperties;
        ComboBox_ResultType m_comboBox_resultType;

        #endregion


        #region MEMBER PROPERTIES

        public int? ResultId { get; private set; }
        public string ResultName { get; private set; }
        public string ResultJSONData
        {
            get
            {
                return
                    string.Format
                    (
                        "{{{0}}}",
                        m_stackPanel_jsonProperties.Children.OfType<GroupBox_JSONPropertyValueEditor>().
                        Select(g => 
                                string.Format
                                (
                                    "\"{0}\":{1}", 
                                    g.JSONPropertyName, 
                                    JSONPropertyManager.FormatJSONPropertyStringValueFromDataTypeId
                                    (
                                        g.JSONPropertyValue,
                                        g.JSONPropertyDataTypeId
                                    )
                                )
                            ).
                        Aggregate((x, y) => string.Format("{0}, {1}", x, y))
                    );
            }
        }
        public int ResultTypeId { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_name,
                    m_groupBox_jsonProperties,
                    m_comboBox_resultType
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Result(int? resultId, string resultName, string resultJSONData, int resultTypeId, bool enableEditing)
        {
            ResultId = resultId;
            ResultName = resultName;
            ResultTypeId = resultTypeId;

            CreateControls();

            SetResultJSONData(resultJSONData);

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ResultModified += GinTubBuilderManager_ResultModified;

            GinTubBuilderManager.ResultTypeAdded += GinTubBuilderManager_ResultTypeAdded;
            GinTubBuilderManager.ResultTypeJSONPropertyAdded += GinTubBuilderManager_ResultTypeJSONPropertyAdded;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ResultModified -= GinTubBuilderManager_ResultModified;

            GinTubBuilderManager.ResultTypeAdded -= GinTubBuilderManager_ResultTypeAdded;
            GinTubBuilderManager.ResultTypeJSONPropertyAdded -= GinTubBuilderManager_ResultTypeJSONPropertyAdded;
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
                    Text = (ResultId.HasValue) ? ResultId.ToString() : "NewResult"
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
            m_textBox_name = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = ResultName };
            m_textBox_name.TextChanged += TextBox_Name_TextChanged;
            Label label_name = new Label() { Content = "Name:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_name.SetGridRowColumn(m_textBox_name, 1, 0);
            grid_name.SetGridRowColumn(label_name, 0, 0);

            ////////
            // JSONData Grid
            Grid grid_jsonData = new Grid();
            grid_jsonData.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_jsonData.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_jsonData, 2, 0);

            ////////
            // JSONData
            m_groupBox_jsonProperties = new GroupBox();
            Label label_jsonData = new Label() { Content = "JSONData:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_jsonData.SetGridRowColumn(m_groupBox_jsonProperties, 1, 0);
            grid_jsonData.SetGridRowColumn(label_jsonData, 0, 0);

            m_stackPanel_jsonProperties = new StackPanel() { Orientation = Orientation.Vertical };
            m_groupBox_jsonProperties.Content = m_stackPanel_jsonProperties;

            ////////
            // ResultTypeId Grid
            Grid grid_resultType = new Grid();
            grid_resultType.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_resultType.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_resultType, 3, 0);

            ////////
            // ResultTypeId
            m_comboBox_resultType = new ComboBox_ResultType() { IsEnabled = false };
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

        void GinTubBuilderManager_ResultModified(object sender, GinTubBuilderManager.ResultModifiedEventArgs args)
        {
            if (ResultId == args.Id)
            {
                SetResultName(args.Name);
                SetResultJSONData(args.JSONData);
                SetResultTypeId(args.ResultType);
            }
        }
        

        void GinTubBuilderManager_ResultTypeAdded(object sender, GinTubBuilderManager.ResultTypeAddedEventArgs args)
        {
            if (ResultTypeId == args.Id)
                m_comboBox_resultType.SelectedItem = m_comboBox_resultType.Items.OfType<ComboBox_ResultType.ComboBoxItem_ResultType>().SingleOrDefault(v => v.ResultTypeId == ResultTypeId);
        }

        void GinTubBuilderManager_ResultTypeJSONPropertyAdded(object sender, GinTubBuilderManager.ResultTypeJSONPropertyAddedEventArgs args)
        {

            if( ResultTypeId == args.ResultType &&
                m_stackPanel_jsonProperties != null && 
                !m_stackPanel_jsonProperties.Children.OfType<GroupBox_JSONPropertyValueEditor>().Any(g => g.JSONPropertyName == args.JSONProperty))
            {
                AddJSONProperty(args.JSONProperty, "", args.DataType);
            }
        }

        private void SetResultName(string resultName)
        {
            m_textBox_name.Text = resultName;
            if (!m_textBox_name.IsEnabled)
                TextBox_Name_TextChanged(m_textBox_name, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void SetResultJSONData(string resultJSONData)
        {
            if (!string.IsNullOrEmpty(resultJSONData))
            {
                m_groupBox_jsonProperties.Content = null;
                m_stackPanel_jsonProperties = new StackPanel() { Orientation = Orientation.Vertical };

                foreach(var property in JSONPropertyManager.ParseJSONIntoJSONProperties(resultJSONData))
                    AddJSONProperty
                        (
                            property.Name,
                            property.Value.ToString(),
                            property.DataTypeId
                        );

                m_groupBox_jsonProperties.Content = m_stackPanel_jsonProperties;
            }
        }

        private void AddJSONProperty(string jsonPropertyName, string jsonPropertyValue, int jsonPropertyDataTypeId)
        {
            m_stackPanel_jsonProperties.Children.Add(new GroupBox_JSONPropertyValueEditor(jsonPropertyName, jsonPropertyValue, jsonPropertyDataTypeId));
        }

        private void SetResultTypeId(int resultTypeId)
        {
            ResultTypeId = resultTypeId;
            m_comboBox_resultType.SelectedItem = m_comboBox_resultType.Items.OfType<ComboBox_ResultType.ComboBoxItem_ResultType>().SingleOrDefault(v => v.ResultTypeId == ResultTypeId);
        }

        void TextBox_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_name)
                ResultName = m_textBox_name.Text;
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
