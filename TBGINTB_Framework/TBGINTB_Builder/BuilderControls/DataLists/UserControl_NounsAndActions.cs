using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_NounsAndActions : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        Grid
            m_grid_main,
            m_grid_sub;
        StackPanel m_stackPanel_nouns;
        Button 
            m_button_modifyNoun,
            m_button_addAction;
        GridSplitter m_gridSplitter_actions;
        StackPanel m_stackPanel_actions;

        #endregion


        #region MEMBER PROPERTIES

        public int ParagraphStateId { get; private set; }

        public int? SelectedNounId { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_NounsAndActions(int paragraphStateId)
        {
            ParagraphStateId = paragraphStateId;

            CreateControls();
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.NounRead += GinTubBuilderManager_NounRead;
            GinTubBuilderManager.ActionRead += GinTubBuilderManager_ActionRead;

            foreach (var block in m_stackPanel_nouns.Children.OfType<UserControl_Noun>())
                block.SetActiveAndRegisterForGinTubEvents();
            if(m_stackPanel_actions != null)
                foreach (var grid in m_stackPanel_actions.Children.OfType<UserControl_ActionModification>())
                    grid.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.NounRead -= GinTubBuilderManager_NounRead;
            GinTubBuilderManager.ActionRead -= GinTubBuilderManager_ActionRead;

            foreach (var block in m_stackPanel_nouns.Children.OfType<UserControl_Noun>())
                block.SetInactiveAndUnregisterFromGinTubEvents();
            if(m_stackPanel_actions != null)
                foreach (var grid in m_stackPanel_actions.Children.OfType<UserControl_ActionModification>())
                    grid.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            m_grid_main = new Grid();
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });

            ////////
            // Add Noun
            Button button_addNoun = new Button() { Content = "New Noun ..." };
            button_addNoun.Click += Button_CreateNoun_Click;
            m_grid_main.SetGridRowColumn(button_addNoun, 0, 0);

            ////////
            // Nouns
            m_button_modifyNoun = new Button() { Content = "Modify Noun", IsEnabled = false };
            m_button_modifyNoun.Click += Button_UpdateNoun_Click;
            m_grid_main.SetGridRowColumn(m_button_modifyNoun, 1, 0);

            m_stackPanel_nouns = new StackPanel() { Orientation = Orientation.Vertical };
            ScrollViewer scrollViewer_nouns =
                new ScrollViewer()
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Visible
                };
            scrollViewer_nouns.Content = m_stackPanel_nouns;
            m_grid_main.SetGridRowColumn(scrollViewer_nouns, 2, 0);

            ////////
            // Fin
            Content = m_grid_main;
        }

        private void GinTubBuilderManager_NounRead(object sender, GinTubBuilderManager.NounReadEventArgs args)
        {
            if (ParagraphStateId == args.ParagraphState && !m_stackPanel_nouns.Children.OfType<UserControl_Bordered_Noun>().Any(t => t.NounId == args.Id))
            {
                UserControl_Bordered_Noun border = new UserControl_Bordered_Noun(args.Id, args.Text, args.ParagraphState, false);
                border.MouseLeftButtonDown += UserControl_NounData_MouseLeftButtonDown;
                border.SetActiveAndRegisterForGinTubEvents();
                m_stackPanel_nouns.Children.Add(border);
                GinTubBuilderManager.ReadParagraphStateNounPossibilities(args.ParagraphState);
            }
        }

        private void GinTubBuilderManager_ActionRead(object sender, GinTubBuilderManager.ActionReadEventArgs args)
        {
            if (SelectedNounId == args.Noun && !m_stackPanel_actions.Children.OfType<UserControl_ActionModification>().Any(a => a.ActionId == args.Id))
            {
                UserControl_ActionModification grid = new UserControl_ActionModification(args.Id, args.VerbType, args.Noun, ParagraphStateId);
                m_stackPanel_actions.Children.Add(grid);
                GinTubBuilderManager.ReadAllVerbTypes();
                GinTubBuilderManager.ReadAllNounsForParagraphState(ParagraphStateId);
            }
        }

        private void ShowActionControls()
        {
            if (m_grid_main.RowDefinitions.Count == 3)
            {
                m_grid_main.RowDefinitions[2].Height = new GridLength(25.0, GridUnitType.Star);
                m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(5.0, GridUnitType.Pixel) });
                m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(75.0, GridUnitType.Star) });
            }
            if (m_gridSplitter_actions == null)
            {
                m_gridSplitter_actions =
                    new GridSplitter()
                    {
                        Height = 5,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                        Background = Brushes.Black
                    };
                m_grid_main.SetGridRowColumn(m_gridSplitter_actions, 3, 0);
            }
            if (m_grid_sub == null)
            {
                m_grid_sub = new Grid();
                m_grid_sub.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                m_grid_sub.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
                m_grid_main.SetGridRowColumn(m_grid_sub, 4, 0);
            }
            if (m_button_addAction == null)
            {
                m_button_addAction = new Button() { Content = "New Action ...", IsEnabled = false };
                m_button_addAction.Click += Button_CreateAction_Click;
                m_grid_sub.SetGridRowColumn(m_button_addAction, 0, 0);
            }
            if (m_stackPanel_actions == null)
            {
                m_stackPanel_actions = new StackPanel() { Orientation = Orientation.Vertical };
                ScrollViewer scrollViewer_actions =
                    new ScrollViewer()
                    {
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Visible
                    };
                scrollViewer_actions.Content = m_stackPanel_actions;
                m_grid_sub.SetGridRowColumn(scrollViewer_actions, 1, 0);
            }
        }

        private void Button_CreateNoun_Click(object sender, RoutedEventArgs e)
        {
            Window_Noun window = 
                new Window_Noun
                (
                    null, 
                    null,
                    ParagraphStateId,
                    (win) =>
                    {
                        Window_Noun wWin = win as Window_Noun;
                        if (wWin != null)
                            GinTubBuilderManager.CreateNoun(wWin.NounText, wWin.ParagraphStateId);
                    }
                );
            window.Show();
        }

        private void Button_UpdateNoun_Click(object sender, RoutedEventArgs e)
        {
            UserControl_Noun grid = m_stackPanel_nouns.Children.OfType<UserControl_Noun>().Single(g => g.NounId.Value == SelectedNounId);
            Window_Noun window = 
                new Window_Noun
                (
                    grid.NounId, 
                    grid.NounText,
                    grid.ParagraphStateId,
                    (win) =>
                    {
                        Window_Noun wWin = win as Window_Noun;
                        if (wWin != null)
                            GinTubBuilderManager.UpdateNoun(wWin.NounId.Value, wWin.NounText, wWin.ParagraphStateId);
                    }
                );
            window.Show();
        }

        private void Button_CreateAction_Click(object sender, RoutedEventArgs e)
        {
            Window_Action window = 
                new Window_Action
                (
                    null, 
                    null, 
                    SelectedNounId,
                    ParagraphStateId,
                    (win) =>
                    {
                        Window_Action wWin = win as Window_Action;
                        if (wWin != null)
                            GinTubBuilderManager.CreateAction(wWin.ActionVerbType.Value, wWin.ActionNoun.Value);
                    }
                );
            window.Show();
        }

        private void UserControl_NounData_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UserControl_Bordered_Noun border = sender as UserControl_Bordered_Noun;
            if (border != null)
            {
                if (!SelectedNounId.HasValue)
                    ShowActionControls();
                m_stackPanel_actions.Children.Clear();

                SelectedNounId = border.NounId.Value;
                GinTubBuilderManager.SelectNoun(SelectedNounId.Value);

                m_button_modifyNoun.IsEnabled = true;
                m_button_addAction.IsEnabled = true;
            }
        }

        #endregion

        #endregion

    }
}
