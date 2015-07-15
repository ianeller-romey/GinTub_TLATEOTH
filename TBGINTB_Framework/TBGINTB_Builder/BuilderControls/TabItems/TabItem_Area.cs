﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class TabItem_Area : TabItem, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private Grid_RoomsOnFloor m_grid_roomsOnFloor;
        private UserControl_RoomAndStates m_grid_roomAndState;
        private UserControl_ParagraphsAndStates m_grid_paragraphsAndStates;
        private UserControl_NounsAndActions m_grid_nounsAndActions;
        private UserControl_ActionResults m_grid_actionResults;
        private UserControl_ActionRequirements m_grid_actionRequirements;
        private Grid 
            m_grid_main,
            m_grid_sub;
        private ComboBox 
            m_comboBox_areas,
            m_comboBox_z;
        private readonly ComboBoxItem
            c_comboBoxItem_newArea = new ComboBoxItem() { Content = "New Area ..." },
            c_comboBoxItem_newFloorAbove = new ComboBoxItem() { Content = "^", FontFamily = new FontFamily("Lucida Sans Typewriter"), FontWeight = FontWeights.Bold },
            c_comboBoxItem_newFloorBelow = new ComboBoxItem() { Content = "v", FontFamily = new FontFamily("Lucida Sans Typewriter"), FontWeight = FontWeights.Bold };

        #endregion


        #region MEMBER PROPERTIES

        public int AreaId { get; private set; }

        #endregion


        #region MEMBER CLASSES

        public class ComboBoxItem_Area : ComboBoxItem
        {
            #region MEMBER FIELDS

            private TextBox_AreaName m_textBox_areaName;

            #endregion


            #region MEMBER PROPERTIES

            public int AreaId { get; private set; }
            public string AreaName { get { return m_textBox_areaName.Text; } }

            #endregion


            #region MEMBER CLASSES

            private class TextBox_AreaName : TextBox
            {
                #region MEMBER METHODS

                #region Public Functionality

                public TextBox_AreaName(string text)
                {
                    Margin = new Thickness() { Top = 0.0, Right = 5.0, Bottom = 0.0, Left = 2.0 };
                    Text = text;
                    List<UIElement> removedItems = new List<UIElement>();
                    List<UIElement> addedItems = new List<UIElement>();
                    addedItems.Add(this);
                }

                #endregion

                #endregion
            }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Area(int id, string name)
            {
                AreaId = id;
                m_textBox_areaName = new TextBox_AreaName(name);
                Content = m_textBox_areaName;

                m_textBox_areaName.TextChanged += TextBox_AreaName_TextChanged;
                m_textBox_areaName.GotFocus += (x, y) => { RaiseEvent(new RoutedEventArgs(ComboBoxItem.SelectedEvent, this)); };
            }

            public void SetAreaName(string name)
            {
                if (m_textBox_areaName.Text != name)
                    m_textBox_areaName.Text = name;
            }

            #endregion


            #region Private Functionality

            void TextBox_AreaName_TextChanged(object sender, TextChangedEventArgs e)
            {
                GinTubBuilderManager.ModifyArea(AreaId, m_textBox_areaName.Text);
            }

            #endregion

            #endregion
        }
        
        private class ComboBoxItem_Z : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int Z { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Z(int z)
            {
                Z = z;
                Content = Z;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public TabItem_Area()
        {
            Header = "Areas";
            Content = CreateControls();
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.AreaAdded += GinTubBuilderManager_AreaAdded;
            GinTubBuilderManager.AreaModified += GinTubBuilderManager_AreaModified;
            GinTubBuilderManager.AreaGet += GinTubBuilderManager_AreaGet;

            GinTubBuilderManager.RoomGet += GinTubBuilderManager_RoomGet;

            GinTubBuilderManager.RoomStateGet += GinTubBuilderManager_RoomStateGet;

            GinTubBuilderManager.ParagraphGet += GinTubBuilderManager_ParagraphGet;

            GinTubBuilderManager.ParagraphStateGet += GinTubBuilderManager_ParagraphStateGet;

            GinTubBuilderManager.NounGet += GinTubBuilderManager_NounGet;

            GinTubBuilderManager.ActionGet += GinTubBuilderManager_ActionGet;

            if (m_grid_roomsOnFloor != null)
                m_grid_roomsOnFloor.SetActiveAndRegisterForGinTubEvents();
            if(m_grid_roomAndState != null)
                m_grid_roomAndState.SetActiveAndRegisterForGinTubEvents();

            GinTubBuilderManager.LoadAllAreas();
            m_comboBox_areas.SelectionChanged += ComboBox_Area_SelectionChanged;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.AreaAdded -= GinTubBuilderManager_AreaAdded;
            GinTubBuilderManager.AreaModified -= GinTubBuilderManager_AreaModified;
            GinTubBuilderManager.AreaGet -= GinTubBuilderManager_AreaGet;

            GinTubBuilderManager.RoomGet -= GinTubBuilderManager_RoomGet;

            GinTubBuilderManager.RoomStateGet -= GinTubBuilderManager_RoomStateGet;

            GinTubBuilderManager.ParagraphGet -= GinTubBuilderManager_ParagraphGet;

            GinTubBuilderManager.ParagraphStateGet -= GinTubBuilderManager_ParagraphStateGet;

            GinTubBuilderManager.NounGet -= GinTubBuilderManager_NounGet;

            GinTubBuilderManager.ActionGet -= GinTubBuilderManager_ActionGet;

            if(m_grid_roomsOnFloor != null)
                m_grid_roomsOnFloor.SetInactiveAndUnregisterFromGinTubEvents();
            if(m_grid_roomAndState != null)
                m_grid_roomAndState.SetInactiveAndUnregisterFromGinTubEvents();

            m_comboBox_areas.SelectionChanged -= ComboBox_Area_SelectionChanged;
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls()
        {
            m_grid_main = new Grid();
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });

            ////////
            // Area
            Label label_area = new Label() { Content = "Area:\t", FontWeight = FontWeights.Bold };

            m_comboBox_areas = new ComboBox();
            m_comboBox_areas.Items.Add(c_comboBoxItem_newArea);
            m_comboBox_areas.SelectedItem = null;

            StackPanel stackPanel_area = new StackPanel() { Orientation = Orientation.Horizontal };
            stackPanel_area.Children.Add(label_area);
            stackPanel_area.Children.Add(m_comboBox_areas);
            m_grid_main.SetGridRowColumn(stackPanel_area, 0, 0);

            ////////
            // Floor
            Label label_z = new Label() { Content = "Floor:\t", FontWeight = FontWeights.Bold };

            m_comboBox_z = new ComboBox();
            m_comboBox_z.SelectionChanged += ComboBox_Z_SelectionChanged;

            StackPanel stackPanel_z = new StackPanel() { Orientation = Orientation.Horizontal };
            stackPanel_z.Children.Add(label_z);
            stackPanel_z.Children.Add(m_comboBox_z);
            m_grid_main.SetGridRowColumn(stackPanel_z, 1, 0);

            ////////
            // Rooms Grid
            m_grid_sub = new Grid();
            m_grid_sub.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200.0, GridUnitType.Pixel) });
            m_grid_sub.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5.0, GridUnitType.Pixel) });
            m_grid_sub.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(140.0, GridUnitType.Pixel) });
            m_grid_sub.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5.0, GridUnitType.Pixel) });
            m_grid_sub.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100.0, GridUnitType.Star) });
            m_grid_sub.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5.0, GridUnitType.Pixel) });
            m_grid_sub.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100.0, GridUnitType.Star) });
            m_grid_sub.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5.0, GridUnitType.Pixel) });
            m_grid_sub.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100.0, GridUnitType.Star) });
            m_grid_sub.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5.0, GridUnitType.Pixel) });
            m_grid_sub.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100.0, GridUnitType.Star) });
            m_grid_main.SetGridRowColumn(m_grid_sub, 2, 0);

            Rectangle rectangle_roomsOnFloor = new Rectangle() { Fill = Brushes.Lavender };
            m_grid_sub.SetGridRowColumn(rectangle_roomsOnFloor, 0, 0);

            GridSplitter gridSplitter = new GridSplitter() { Width = 5, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, Background = Brushes.Black };
            m_grid_sub.SetGridRowColumn(gridSplitter, 0, 1);
            GridSplitter gridSplitter2 = new GridSplitter() { Width = 5, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, Background = Brushes.Black };
            m_grid_sub.SetGridRowColumn(gridSplitter2, 0, 3);
            GridSplitter gridSplitter3 = new GridSplitter() { Width = 5, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, Background = Brushes.Black };
            m_grid_sub.SetGridRowColumn(gridSplitter3, 0, 5);
            GridSplitter gridSplitter4 = new GridSplitter() { Width = 5, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, Background = Brushes.Black };
            m_grid_sub.SetGridRowColumn(gridSplitter4, 0, 7);
            GridSplitter gridSplitter5 = new GridSplitter() { Width = 5, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, Background = Brushes.Black };
            m_grid_sub.SetGridRowColumn(gridSplitter5, 0, 9);

            return m_grid_main;
        }

        private void GinTubBuilderManager_AreaAdded(object sender, GinTubBuilderManager.AreaAddedEventArgs args)
        {
            AddedArea(args.Id, args.Name);
        }

        private void GinTubBuilderManager_AreaModified(object sender, GinTubBuilderManager.AreaModifiedEventArgs args)
        {
            ModifiedArea(args.Id, args.Name);
        }

        private void GinTubBuilderManager_AreaGet(object sender, GinTubBuilderManager.AreaGetEventArgs args)
        {
            LoadArea(args.Id, args.Name, args.MaxX, args.MinX, args.MaxY, args.MinY, args.MinZ, args.MaxZ);
        }

        private void GinTubBuilderManager_RoomGet(object sender, GinTubBuilderManager.RoomGetEventArgs args)
        {
            LoadRoom(args.Id, args.Name, args.X, args.Y, args.Z);
        }

        void GinTubBuilderManager_RoomStateGet(object sender, GinTubBuilderManager.RoomStateGetEventArgs args)
        {
            LoadRoomState(args.Room, args.Id);
        }

        void GinTubBuilderManager_ParagraphGet(object sender, GinTubBuilderManager.ParagraphGetEventArgs args)
        {
            UnloadParagraphState();
            GinTubBuilderManager.LoadAllParagraphStatesForParagraph(args.Id);
        }

        void GinTubBuilderManager_ParagraphStateGet(object sender, GinTubBuilderManager.ParagraphStateGetEventArgs args)
        {
            LoadParagraphState(args.Id);
        }

        void GinTubBuilderManager_NounGet(object sender, GinTubBuilderManager.NounGetEventArgs args)
        {
            UnloadAction();
            GinTubBuilderManager.LoadAllActionsForNoun(args.Id);
        }

        void GinTubBuilderManager_ActionGet(object sender, GinTubBuilderManager.ActionGetEventArgs args)
        {
            LoadAction(args.Id);
        }

        private void AddingArea()
        {
            Window_TextEntry window = new Window_TextEntry("Area Name", "");
            window.Closed += (x, y) => { if (window.Accepted) GinTubBuilderManager.AddArea(window.Text); };
            window.Show();
        }

        private void AddedArea(int id, string name)
        {
            if (m_comboBox_areas.Items.OfType<ComboBoxItem_Area>().Any(a => a.AreaId == id))
                return;

            object prevItem = m_comboBox_areas.SelectedItem;
            ComboBoxItem_Area aItem = new ComboBoxItem_Area(id, name);
            aItem.Selected += ComboBoxItem_Area_Selected;
            m_comboBox_areas.Items.Add(aItem);
            m_comboBox_areas.SelectedItem = (prevItem == c_comboBoxItem_newArea) ? aItem : prevItem;
        }

        private void ModifiedArea(int id, string name)
        {
            ComboBoxItem_Area aItem = m_comboBox_areas.Items.OfType<ComboBoxItem_Area>().SingleOrDefault(i => i.AreaId == id);
            if (aItem != null)
                aItem.SetAreaName(name);
        }

        private void LoadArea(int id, string name, int maxX, int minX, int maxY, int minY, int minZ, int maxZ)
        {
            UnloadArea();

            AreaId = id;

            m_comboBox_z.Items.Add(c_comboBoxItem_newFloorAbove);
            m_comboBox_z.Items.Add(c_comboBoxItem_newFloorBelow);
            if(minZ < 0)
            {
                for (int z = minZ; z < 0; ++z)
                    AddFloorBelow();
                for (int z = 0; z <= maxZ; ++z)
                    AddFloorAbove();
            }
            else
            {
                for (int z = minZ; z <= maxZ; ++z)
                    AddFloorAbove();
            }
            m_comboBox_z.SelectedItem = m_comboBox_z.Items[m_comboBox_z.Items.Count - 2];

            m_grid_roomsOnFloor = new Grid_RoomsOnFloor(maxX, maxY, minZ, AreaId);
            m_grid_sub.SetGridRowColumn(m_grid_roomsOnFloor, 0, 0);
            m_grid_roomsOnFloor.SetActiveAndRegisterForGinTubEvents();
        }

        private void LoadRoom(int roomId, string roomName, int roomX, int roomY, int roomZ)
        {
            UnloadRoom();

            m_grid_roomAndState = new UserControl_RoomAndStates(roomId, roomName, roomX, roomY, roomZ, AreaId);
            m_grid_sub.SetGridRowColumn(m_grid_roomAndState, 0, 2);
            m_grid_roomAndState.SetActiveAndRegisterForGinTubEvents();
            GinTubBuilderManager.LoadAllRoomStatesForRoom(roomId);

            m_grid_paragraphsAndStates = new UserControl_ParagraphsAndStates(roomId, null);
            m_grid_sub.SetGridRowColumn(m_grid_paragraphsAndStates, 0, 4);
            m_grid_paragraphsAndStates.SetActiveAndRegisterForGinTubEvents();
            GinTubBuilderManager.LoadAllParagraphsForRoomAndRoomState(roomId, null);
        }

        private void LoadRoomState(int roomId, int roomStateId)
        {
            UnloadRoomState();

            m_grid_paragraphsAndStates = new UserControl_ParagraphsAndStates(roomId, roomStateId);
            m_grid_sub.SetGridRowColumn(m_grid_paragraphsAndStates, 0, 4);
            m_grid_paragraphsAndStates.SetActiveAndRegisterForGinTubEvents();
            GinTubBuilderManager.LoadAllParagraphsForRoomAndRoomState(roomId, roomStateId);
        }

        private void LoadParagraphState(int paragraphStateId)
        {
            UnloadParagraphState();

            m_grid_nounsAndActions = new UserControl_NounsAndActions(paragraphStateId);
            m_grid_sub.SetGridRowColumn(m_grid_nounsAndActions, 0, 6);
            m_grid_nounsAndActions.SetActiveAndRegisterForGinTubEvents();
            GinTubBuilderManager.LoadAllNounsForParagraphState(paragraphStateId);
        }

        private void LoadAction(int actionId)
        {
            UnloadAction();

            m_grid_actionResults = 
                new UserControl_ActionResults
                (
                    actionId,
                    m_grid_nounsAndActions.SelectedNounId.Value,
                    m_grid_paragraphsAndStates.SelectedParagraphId.Value
                );
            m_grid_sub.SetGridRowColumn(m_grid_actionResults, 0, 8);
            m_grid_actionResults.SetActiveAndRegisterForGinTubEvents();
            GinTubBuilderManager.LoadAllActionResultsForAction(actionId);

            m_grid_actionRequirements =
                new UserControl_ActionRequirements
                (
                    actionId,
                    m_grid_nounsAndActions.SelectedNounId.Value,
                    m_grid_paragraphsAndStates.SelectedParagraphId.Value
                );
            m_grid_sub.SetGridRowColumn(m_grid_actionRequirements, 0, 10);
            m_grid_actionRequirements.SetActiveAndRegisterForGinTubEvents();
            GinTubBuilderManager.LoadAllItemActionRequirementsForAction(actionId);
            GinTubBuilderManager.LoadAllEventActionRequirementsForAction(actionId);
            GinTubBuilderManager.LoadAllCharacterActionRequirementsForAction(actionId);
            GinTubBuilderManager.LoadAllActionsForNoun(m_grid_nounsAndActions.SelectedNounId.Value);
        }

        private void UnloadArea()
        {
            if(m_grid_roomsOnFloor != null)
                m_grid_sub.Children.Remove(m_grid_roomsOnFloor);
            m_grid_roomsOnFloor = null;

            m_comboBox_z.Items.Clear();

            UnloadRoom();
        }

        private void UnloadRoom()
        {
            if (m_grid_roomAndState != null)
                m_grid_sub.Children.Remove(m_grid_roomAndState);
            UnloadRoomState();
        }

        private void UnloadRoomState()
        {
            if (m_grid_paragraphsAndStates != null)
                m_grid_sub.Children.Remove(m_grid_paragraphsAndStates);
            UnloadParagraphState();
        }

        private void UnloadParagraphState()
        {
            UnloadAction();

            if (m_grid_nounsAndActions != null)
                m_grid_sub.Children.Remove(m_grid_nounsAndActions);
        }

        private void UnloadAction()
        {
            if (m_grid_actionResults != null)
                m_grid_sub.Children.Remove(m_grid_actionResults);
            if (m_grid_actionRequirements != null)
                m_grid_sub.Children.Remove(m_grid_actionRequirements);
        }

        private void AddFloorAbove()
        {
            ComboBoxItem_Z item = m_comboBox_z.Items[1] as ComboBoxItem_Z;
            int newFloor = (item == null) ? 0 : item.Z + 1;
            item = new ComboBoxItem_Z(newFloor);
            m_comboBox_z.Items.Insert(1, item);
            m_comboBox_z.SelectedItem = item;
        }

        private void AddFloorBelow()
        {
            ComboBoxItem_Z item = m_comboBox_z.Items[m_comboBox_z.Items.Count - 2] as ComboBoxItem_Z;
            int newFloor = (item == null) ? -1 : item.Z - 1;
            item = new ComboBoxItem_Z(newFloor);
            m_comboBox_z.Items.Insert(m_comboBox_z.Items.Count - 1, item);
            m_comboBox_z.SelectedItem = item;
        }

        private void ComboBoxItem_Area_Selected(object sender, RoutedEventArgs e)
        {
            ComboBoxItem_Area aItem = sender as ComboBoxItem_Area;
            if (aItem != null)
                m_comboBox_areas.SelectedItem = aItem;
        }

        private void ComboBox_Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if (sender == m_comboBox_areas && (item = m_comboBox_areas.SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newArea)
                    AddingArea();
                else
                {
                    ComboBoxItem_Area aItem = item as ComboBoxItem_Area;
                    if(aItem != null)
                        GinTubBuilderManager.GetArea(aItem.AreaId);
                }
            }
        }

        private void ComboBox_Z_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if(sender == m_comboBox_z && (item = m_comboBox_z.SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newFloorAbove)
                    AddFloorAbove();
                else if (item == c_comboBoxItem_newFloorBelow)
                    AddFloorBelow();
                else
                {
                    ComboBoxItem_Z zItem = item as ComboBoxItem_Z;
                    if (item != null && m_grid_roomsOnFloor != null)
                    {
                        UnloadRoom();
                        m_grid_roomsOnFloor.SetFloor(zItem.Z);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
