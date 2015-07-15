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
    public class UserControl_ParagraphsAndStates : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        Grid
            m_grid_main,
            m_grid_sub;
        StackPanel m_stackPanel_paragraphs;
        Button 
            m_button_modifyParagraph,
            m_button_addParagraphState;
        GridSplitter m_gridSplitter_paragraphStates;
        StackPanel m_stackPanel_paragraphStates;

        #endregion


        #region MEMBER PROPERTIES

        public int RoomId { get; private set; }
        public int? RoomStateId { get; private set; }

        public int? SelectedParagraphId { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ParagraphsAndStates(int roomId, int? roomStateId)
        {
            RoomId = roomId;
            RoomStateId = roomStateId;

            CreateControls();
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ParagraphAdded += GinTubBuilderManager_ParagraphAdded;
            GinTubBuilderManager.ParagraphStateAdded += GinTubBuilderManager_ParagraphStateAdded;

            foreach (var block in m_stackPanel_paragraphs.Children.OfType<UserControl_Paragraph>())
                block.SetActiveAndRegisterForGinTubEvents();
            if(m_stackPanel_paragraphStates != null)
                foreach (var grid in m_stackPanel_paragraphStates.Children.OfType<UserControl_ParagraphStateModification>())
                    grid.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ParagraphAdded -= GinTubBuilderManager_ParagraphAdded;
            GinTubBuilderManager.ParagraphStateAdded -= GinTubBuilderManager_ParagraphStateAdded;

            foreach (var block in m_stackPanel_paragraphs.Children.OfType<UserControl_Paragraph>())
                block.SetInactiveAndUnregisterFromGinTubEvents();
            if (m_stackPanel_paragraphStates != null)
                foreach (var grid in m_stackPanel_paragraphStates.Children.OfType<UserControl_ParagraphStateModification>())
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
            // Add paragraph
            Button button_addParagraph = new Button() { Content = "New Paragraph ..." };
            button_addParagraph.Click += Button_AddParagraph_Click;
            m_grid_main.SetGridRowColumn(button_addParagraph, 0, 0);

            ////////
            // Paragraphs
            m_button_modifyParagraph = new Button() { Content = "Modify Paragraph", IsEnabled = false };
            m_button_modifyParagraph.Click += Button_ModifyParagraph_Click;
            m_grid_main.SetGridRowColumn(m_button_modifyParagraph, 1, 0);

            m_stackPanel_paragraphs = new StackPanel() { Orientation = Orientation.Vertical };
            ScrollViewer scrollViewer_paragraphs =
                new ScrollViewer()
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Visible
                };
            scrollViewer_paragraphs.Content = m_stackPanel_paragraphs;
            m_grid_main.SetGridRowColumn(scrollViewer_paragraphs, 2, 0);

            ////////
            // Fin
            Content = m_grid_main;
        }

        private void GinTubBuilderManager_ParagraphAdded(object sender, GinTubBuilderManager.ParagraphAddedEventArgs args)
        {
            if (!m_stackPanel_paragraphs.Children.OfType<UserControl_Bordered_ParagraphWithPreview>().Any(t => t.ParagraphId == args.Id))
            {
                UserControl_Bordered_ParagraphWithPreview border = new UserControl_Bordered_ParagraphWithPreview(args.Id, args.Order, args.Room, args.RoomState, false);
                border.MouseLeftButtonDown += UserControl_ParagraphWithPreview_MouseLeftButtonDown;
                border.SetActiveAndRegisterForGinTubEvents();
                m_stackPanel_paragraphs.Children.Add(border);

                GinTubBuilderManager.LoadParagraphStateForParagraphPreview(0, args.Id);
            }
        }

        private void GinTubBuilderManager_ParagraphStateAdded(object sender, GinTubBuilderManager.ParagraphStateAddedEventArgs args)
        {
            if (SelectedParagraphId == args.Paragraph && !m_stackPanel_paragraphStates.Children.OfType<UserControl_ParagraphStateModification>().Any(t => t.ParagraphStateId == args.Id))
            {
                UserControl_ParagraphStateModification grid = new UserControl_ParagraphStateModification(args.Id, args.Text, args.State, args.Paragraph);
                m_stackPanel_paragraphStates.Children.Add(grid);
            }
        }

        private void ShowParagraphStateControls()
        {
            if (m_grid_main.RowDefinitions.Count == 3)
            {
                m_grid_main.RowDefinitions[2].Height = new GridLength(25.0, GridUnitType.Star);
                m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(5.0, GridUnitType.Pixel) });
                m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(75.0, GridUnitType.Star) });
            }
            if(m_gridSplitter_paragraphStates == null)
            {
                m_gridSplitter_paragraphStates = 
                    new GridSplitter() 
                    { 
                        Height = 5, 
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        VerticalAlignment = System.Windows.VerticalAlignment.Stretch, 
                        Background = Brushes.Black 
                    };
                m_grid_main.SetGridRowColumn(m_gridSplitter_paragraphStates, 3, 0);
            }
            if(m_grid_sub == null)
            {
                m_grid_sub = new Grid();
                m_grid_sub.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                m_grid_sub.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
                m_grid_main.SetGridRowColumn(m_grid_sub, 4, 0);
            }
            if (m_button_addParagraphState == null)
            {
                m_button_addParagraphState = new Button() { Content = "New Paragraph Text State ...", IsEnabled = false };
                m_button_addParagraphState.Click += Button_AddParagraphState_Click;
                m_grid_sub.SetGridRowColumn(m_button_addParagraphState, 0, 0);
            }
            if (m_stackPanel_paragraphStates == null)
            {
                m_stackPanel_paragraphStates = new StackPanel() { Orientation = Orientation.Vertical };
                ScrollViewer scrollViewer_paragraphStates =
                    new ScrollViewer()
                    {
                        VerticalScrollBarVisibility = ScrollBarVisibility.Visible
                    };
                scrollViewer_paragraphStates.Content = m_stackPanel_paragraphStates;
                m_grid_sub.SetGridRowColumn(scrollViewer_paragraphStates, 1, 0);
            }
        }

        private void Button_AddParagraph_Click(object sender, RoutedEventArgs e)
        {
            Window_Paragraph window = 
                new Window_Paragraph
                (
                    null, 
                    null, 
                    RoomId,
                    RoomStateId,
                    (win) =>
                    {
                        Window_Paragraph wWin = win as Window_Paragraph;
                        if (wWin != null)
                            GinTubBuilderManager.AddParagraph(wWin.ParagraphOrder.Value, wWin.RoomId, wWin.RoomStateId);
                    }
                );
            window.Show();
        }

        private void Button_ModifyParagraph_Click(object sender, RoutedEventArgs e)
        {
            UserControl_Bordered_ParagraphWithPreview userControl = 
                m_stackPanel_paragraphs.Children.OfType<UserControl_Bordered_ParagraphWithPreview>().Single(g => g.ParagraphId == SelectedParagraphId);
            Window_Paragraph window = 
                new Window_Paragraph
                (
                    userControl.ParagraphId, 
                    userControl.ParagraphOrder, 
                    userControl.RoomId,
                    userControl.RoomStateId,
                    (win) =>
                    {
                        Window_Paragraph wWin = win as Window_Paragraph;
                        if (wWin != null)
                            GinTubBuilderManager.ModifyParagraph(wWin.ParagraphId.Value, wWin.ParagraphOrder.Value, wWin.RoomId, wWin.RoomStateId);
                    }
                );
            window.Show();
        }

        private void Button_AddParagraphState_Click(object sender, RoutedEventArgs e)
        {
            Window_ParagraphState window = 
                new Window_ParagraphState
                (
                    null, 
                    null, 
                    null,
                    SelectedParagraphId.Value,
                    (win) =>
                    {
                        Window_ParagraphState wWin = win as Window_ParagraphState;
                        if (wWin != null)
                            GinTubBuilderManager.AddParagraphState(wWin.ParagraphStateText, wWin.ParagraphId);
                    }
                );
            window.Show();
        }

        private void UserControl_ParagraphWithPreview_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UserControl_Bordered_ParagraphWithPreview border = sender as UserControl_Bordered_ParagraphWithPreview;
            if (border != null)
            {
                if (!SelectedParagraphId.HasValue)
                    ShowParagraphStateControls();
                m_stackPanel_paragraphStates.Children.Clear();

                SelectedParagraphId = border.ParagraphId.Value;
                GinTubBuilderManager.GetParagraph(SelectedParagraphId.Value);
                GinTubBuilderManager.GetParagraphStateForParagraphPreview(0, SelectedParagraphId.Value);

                m_button_modifyParagraph.IsEnabled = true;
                m_button_addParagraphState.IsEnabled = true;
            }
        }

        #endregion

        #endregion

    }
}
