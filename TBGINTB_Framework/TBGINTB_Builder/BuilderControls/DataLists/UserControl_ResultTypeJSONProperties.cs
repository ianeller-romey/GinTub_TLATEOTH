using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_ResultTypeJSONProperties : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        ItemsControl m_itemsControl_results;
        private readonly Button c_button_newResultTypeJSONProperty = new Button() { Content = "New Result Type JSON Property ..." };

        #endregion


        #region MEMBER PROPERTIES

        public int ResultTypeId { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ResultTypeJSONProperties(int resultTypeId)
        {
            ResultTypeId = resultTypeId;

            CreateControls();

            c_button_newResultTypeJSONProperty.Click += Button_NewResultTypeJSONProperty_Click;

            IsEnabled = false;
            IsEnabledChanged += UserControl_ResultTypeJSONProperty_IsEnabledChanged;
            IsEnabled = true;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ResultTypeJSONPropertyRead += GinTubBuilderManager_ResultTypeJSONPropertyRead;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ResultTypeJSONPropertyRead -= GinTubBuilderManager_ResultTypeJSONPropertyRead;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            m_itemsControl_results = new ItemsControl();

            ScrollViewer scrollViewer_results = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Visible };
            scrollViewer_results.Content = m_itemsControl_results;

            StackPanel stackPanel_main = new StackPanel() { Orientation = Orientation.Vertical };
            stackPanel_main.Children.Add(c_button_newResultTypeJSONProperty);
            stackPanel_main.Children.Add(scrollViewer_results);

            Content = stackPanel_main;
        }

        private void GinTubBuilderManager_ResultTypeJSONPropertyRead(object sender, GinTubBuilderManager.ResultTypeJSONPropertyReadEventArgs args)
        {
            if (args.ResultType == ResultTypeId && !m_itemsControl_results.Items.OfType<UserControl_ResultTypeJSONPropertyModification>().Any(i => i.ResultTypeJSONPropertyId == args.Id))
            {
                UserControl_ResultTypeJSONPropertyModification grid = 
                    new UserControl_ResultTypeJSONPropertyModification
                    (
                        args.Id, 
                        args.JSONProperty, 
                        args.DataType,
                        args.ResultType
                    );
                grid.SetActiveAndRegisterForGinTubEvents();
                m_itemsControl_results.Items.Add(grid);
                GinTubBuilderManager.ReadAllJSONPropertyDataTypes();
                GinTubBuilderManager.ReadAllResultTypes();
            }
        }

        private void NewResultTypeJSONPropertyDialog()
        {
            Window_ResultTypeJSONProperty window = 
                new Window_ResultTypeJSONProperty
                (
                    null, 
                    null,
                    null,
                    ResultTypeId,
                    (win) =>
                    {
                        Window_ResultTypeJSONProperty wWin = win as Window_ResultTypeJSONProperty;
                        if (wWin != null)
                            GinTubBuilderManager.CreateResultTypeJSONProperty
                            (
                                wWin.ResultTypeJSONPropertyJSONProperty, 
                                wWin.ResultTypeJSONPropertyDataType.Value, 
                                wWin.ResultTypeId
                            );
                    }
                );
            window.Show();
        }


        private void Button_NewResultTypeJSONProperty_Click(object sender, RoutedEventArgs e)
        {
            Button item = null;
            if ((item = sender as Button) != null && item == c_button_newResultTypeJSONProperty)
                NewResultTypeJSONPropertyDialog();
        }

        private void UserControl_ResultTypeJSONProperty_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserControl_ResultTypeJSONProperties control = sender as UserControl_ResultTypeJSONProperties;
            if (sender != null && sender == this && e.Property == ItemsControl.IsEnabledProperty)
            {
                if ((bool)e.OldValue == true && (bool)e.NewValue == false)
                    c_button_newResultTypeJSONProperty.Visibility = System.Windows.Visibility.Collapsed;
                else if ((bool)e.OldValue == false && (bool)e.NewValue == true)
                    c_button_newResultTypeJSONProperty.Visibility = System.Windows.Visibility.Visible;
            }
        }

        #endregion

        #endregion
    }
}
