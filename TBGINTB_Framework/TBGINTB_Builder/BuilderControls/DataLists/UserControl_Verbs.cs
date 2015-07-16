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
    public class UserControl_Verbs : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        ItemsControl m_itemsControl_results;
        private readonly Button c_button_newVerb = new Button() { Content = "New Verb ..." };

        #endregion


        #region MEMBER PROPERTIES

        public int VerbTypeId { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Verbs(int resultTypeId)
        {
            VerbTypeId = resultTypeId;

            CreateControls();

            c_button_newVerb.Click += Button_NewVerb_Click;

            IsEnabled = false;
            IsEnabledChanged += UserControl_Verb_IsEnabledChanged;
            IsEnabled = true;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.VerbRead += GinTubBuilderManager_VerbRead;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.VerbRead -= GinTubBuilderManager_VerbRead;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            m_itemsControl_results = new ItemsControl();

            ScrollViewer scrollViewer_results = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Visible };
            scrollViewer_results.Content = m_itemsControl_results;

            StackPanel stackPanel_main = new StackPanel() { Orientation = Orientation.Vertical };
            stackPanel_main.Children.Add(c_button_newVerb);
            stackPanel_main.Children.Add(scrollViewer_results);

            Content = stackPanel_main;
        }

        private void GinTubBuilderManager_VerbRead(object sender, GinTubBuilderManager.VerbReadEventArgs args)
        {
            if (args.VerbType == VerbTypeId && !m_itemsControl_results.Items.OfType<UserControl_VerbModification>().Any(i => i.VerbId == args.Id))
            {
                UserControl_VerbModification grid = new UserControl_VerbModification(args.Id, args.Name, args.VerbType);
                grid.SetActiveAndRegisterForGinTubEvents();
                m_itemsControl_results.Items.Add(grid);
                GinTubBuilderManager.ReadAllVerbTypes();
            }
        }

        private void NewVerbDialog()
        {
            Window_Verb window = 
                new Window_Verb
                (
                    null, 
                    null,
                    VerbTypeId,
                    (win) =>
                    {
                        Window_Verb wWin = win as Window_Verb;
                        if (wWin != null)
                            GinTubBuilderManager.CreateVerb(wWin.VerbName, wWin.VerbTypeId);
                    }
                );
            window.Show();
        }


        private void Button_NewVerb_Click(object sender, RoutedEventArgs e)
        {
            Button item = null;
            if ((item = sender as Button) != null && item == c_button_newVerb)
                NewVerbDialog();
        }

        private void UserControl_Verb_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserControl_Verbs control = sender as UserControl_Verbs;
            if (sender != null && sender == this && e.Property == ItemsControl.IsEnabledProperty)
            {
                if ((bool)e.OldValue == true && (bool)e.NewValue == false)
                    c_button_newVerb.Visibility = System.Windows.Visibility.Collapsed;
                else if ((bool)e.OldValue == false && (bool)e.NewValue == true)
                    c_button_newVerb.Visibility = System.Windows.Visibility.Visible;
            }
        }

        #endregion

        #endregion
    }
}
