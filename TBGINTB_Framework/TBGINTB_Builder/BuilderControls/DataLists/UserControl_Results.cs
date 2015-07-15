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
    public class UserControl_Results : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        ItemsControl m_itemsControl_results;
        private readonly Button c_button_newResult = new Button() { Content = "New Result ..." };

        #endregion


        #region MEMBER PROPERTIES

        public int ResultTypeId { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Results(int resultTypeId)
        {
            ResultTypeId = resultTypeId;

            CreateControls();

            c_button_newResult.Click += Button_NewResult_Click;

            IsEnabled = false;
            IsEnabledChanged += UserControl_Result_IsEnabledChanged;
            IsEnabled = true;
        }
    
        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ResultAdded += GinTubBuilderManager_ResultAdded;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ResultAdded -= GinTubBuilderManager_ResultAdded;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            m_itemsControl_results = new ItemsControl();

            ScrollViewer scrollViewer_results = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Visible };
            scrollViewer_results.Content = m_itemsControl_results;

            StackPanel stackPanel_main = new StackPanel() { Orientation = Orientation.Vertical };
            stackPanel_main.Children.Add(c_button_newResult);
            stackPanel_main.Children.Add(scrollViewer_results);

            Content = stackPanel_main;
        }

        private void GinTubBuilderManager_ResultAdded(object sender, GinTubBuilderManager.ResultAddedEventArgs args)
        {
            if (args.ResultType == ResultTypeId && !m_itemsControl_results.Items.OfType<UserControl_ResultModification>().Any(i => i.ResultId == args.Id))
            {
                UserControl_ResultModification grid = new UserControl_ResultModification(args.Id, args.Name, args.JSONData, args.ResultType);
                grid.SetActiveAndRegisterForGinTubEvents();
                m_itemsControl_results.Items.Add(grid);
                GinTubBuilderManager.LoadAllResultTypes();
                GinTubBuilderManager.LoadAllResultTypeJSONPropertiesForResultType(args.ResultType);
            }
        }

        private void NewResultDialog()
        {
            Window_Result window = 
                new Window_Result
                (
                    null, 
                    null, 
                    null,
                    ResultTypeId,
                    (win) =>
                    {
                        Window_Result wWin = win as Window_Result;
                        if (wWin != null)
                            GinTubBuilderManager.AddResult(wWin.ResultName, wWin.ResultJSONData, wWin.ResultTypeId);
                    }
                );
            window.Show();
        }
        

        private void Button_NewResult_Click(object sender, RoutedEventArgs e)
        {
            Button item = null;
            if ((item = sender as Button) != null && item == c_button_newResult)
                NewResultDialog();
        }

        private void UserControl_Result_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserControl_Results control = sender as UserControl_Results;
            if(sender != null && sender == this && e.Property == ItemsControl.IsEnabledProperty)
            {
                if ((bool)e.OldValue == true && (bool)e.NewValue == false)
                    c_button_newResult.Visibility = System.Windows.Visibility.Collapsed;
                else if ((bool)e.OldValue == false && (bool)e.NewValue == true)
                    c_button_newResult.Visibility = System.Windows.Visibility.Visible;
            }
        }

        #endregion

        #endregion
    }
}
