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
    public class UserControl_ResultType : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        Grid m_grid_main;
        ComboBox_ResultType m_comboBox_resultType;
        UserControl_ResultTypeJSONProperties m_userControl_resultTypeJSONProperties;
        UserControl_Results m_itemsControl_results;
        Grid m_grid_resultType;
        ScrollViewer
            m_scrollViewer_resultTypeJSONProperty,
            m_scrollViewer_results;

        private bool m_enableEditing;

        #endregion


        #region MEMBER PROPERTIES

        public int? SelectedResultTypeId { get; private set; }
        public string SelectedResultTypeName { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_comboBox_resultType,
                    m_userControl_resultTypeJSONProperties
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ResultType(bool enableEditing)
        {
            m_enableEditing = enableEditing;

            CreateControls();

            m_comboBox_resultType.IsEnabled = m_enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            //m_comboBox_resultType.SetActiveAndRegisterForGinTubEvents();
            //m_itemsControl_resultTypeJSONProperty.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            //m_comboBox_resultType.SetInactiveAndUnregisterFromGinTubEvents();
            //m_itemsControl_resultTypeJSONProperty.SetInactiveAndUnregisterFromGinTubEvents();
        }
        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            m_grid_main = new Grid();
            m_grid_main.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50.0, GridUnitType.Star) });
            m_grid_main.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50.0, GridUnitType.Star) });

            ////////
            // Result Type
            m_grid_resultType = new Grid();
            m_grid_resultType.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            m_grid_resultType.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            m_grid_main.SetGridRowColumn(m_grid_resultType, 0, 0);

            m_comboBox_resultType = 
                new ComboBox_ResultType() 
                { 
                    VerticalAlignment = System.Windows.VerticalAlignment.Top 
                };
            m_comboBox_resultType.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_resultType.SelectionChanged += ComboBox_ResultType_SelectionChanged;
            m_grid_resultType.SetGridRowColumn(m_comboBox_resultType, 0, 0);

            ////////
            // Fin
            Content = m_grid_main;
        }

        private void ComboBox_ResultType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_ResultType comboBox = sender as ComboBox_ResultType;
            ComboBox_ResultType.ComboBoxItem_ResultType comboBoxItem = null;
            if (comboBox != null && (comboBoxItem = comboBox.SelectedItem as ComboBox_ResultType.ComboBoxItem_ResultType) != null)
            {
                SelectedResultTypeId = comboBoxItem.ResultTypeId;
                SelectedResultTypeName = comboBoxItem.ResultTypeName;

                if (m_userControl_resultTypeJSONProperties != null)
                    m_grid_main.Children.Remove(m_scrollViewer_resultTypeJSONProperty); // remove the scrollviewer, because its child is the control we really want to remove
                if (m_itemsControl_results != null)
                    m_grid_main.Children.Remove(m_scrollViewer_results); // remove the scrollviewer, because its child is the control we really want to remove

                ////////
                // ResultTypeJSONProperties
                m_userControl_resultTypeJSONProperties = new UserControl_ResultTypeJSONProperties(SelectedResultTypeId.Value);
                m_userControl_resultTypeJSONProperties.IsEnabled = m_enableEditing;
                m_userControl_resultTypeJSONProperties.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are

                m_scrollViewer_resultTypeJSONProperty = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Visible };
                m_scrollViewer_resultTypeJSONProperty.Content = m_userControl_resultTypeJSONProperties;
                m_grid_resultType.SetGridRowColumn(m_scrollViewer_resultTypeJSONProperty, 1, 0);

                ////////
                // Results
                m_itemsControl_results = new UserControl_Results(SelectedResultTypeId.Value);
                m_itemsControl_results.IsEnabled = m_enableEditing;
                m_itemsControl_results.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are

                m_scrollViewer_results = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Visible };
                Grid.SetRowSpan(m_scrollViewer_results, 2);
                m_scrollViewer_results.Content = m_itemsControl_results;
                m_grid_main.SetGridRowColumn(m_scrollViewer_results, 0, 1);

                //
                GinTubBuilderManager.ReadAllResultTypeJSONPropertiesForResultType(SelectedResultTypeId.Value);
                GinTubBuilderManager.ReadAllResultsForResultType(SelectedResultTypeId.Value);
            }
            else if (comboBoxItem == null)
            {
                SelectedResultTypeId = null;
                SelectedResultTypeName = null;
            }
        }

        #endregion

        #endregion
    }
}
