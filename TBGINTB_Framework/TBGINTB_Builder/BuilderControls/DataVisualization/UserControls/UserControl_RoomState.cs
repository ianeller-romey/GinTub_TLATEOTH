﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_RoomState : UserControl_Gettable
    {
        #region MEMBER FIELDS

        TextBlock m_textBlock_state;
        ComboBox_Location m_comboBox_location;
        ComboBox
            m_comboBox_time_hour,
            m_comboBox_time_minute;

        #endregion


        #region MEMBER PROPERTIES

        public int? RoomStateId { get; private set; }
        public int? RoomStateState { get; private set; }
        public DateTime? RoomStateTime { get; private set; }
        public int? LocationId { get; private set; }
        public int RoomId { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_comboBox_location,
                    m_comboBox_time_hour,
                    m_comboBox_time_minute
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_RoomState(int? roomStateId, int? roomStateState, DateTime? roomStateTime, int? locationId, int roomId, bool enableEditing)
        {
            RoomStateId = roomStateId;
            RoomStateState = roomStateState;
            RoomStateTime = roomStateTime;
            LocationId = locationId;
            RoomId = roomId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
            if(!enableEditing)
                MouseLeftButtonDown += Grid_RoomStateData_MouseLeftButtonDown;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.RoomStateModified += GinTubBuilderManager_RoomStateModified;
            GinTubBuilderManager.RoomStateGet += GinTubBuilderManager_RoomStateGet;

            GinTubBuilderManager.LocationAdded += GinTubBuilderManager_LocationAdded;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.RoomStateModified -= GinTubBuilderManager_RoomStateModified;
            GinTubBuilderManager.RoomStateGet -= GinTubBuilderManager_RoomStateGet;

            GinTubBuilderManager.LocationAdded -= GinTubBuilderManager_LocationAdded;
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
            // State Grid
            Grid grid_state = new Grid();
            grid_state.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_state.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_state, 0, 0);

            ////////
            // State
            m_textBlock_state = new TextBlock() { VerticalAlignment = VerticalAlignment.Center };
            SetRoomStateState(RoomStateState);
            Label label_state = new Label() { Content = "State:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_state.SetGridRowColumn(m_textBlock_state, 0, 1);
            grid_state.SetGridRowColumn(label_state, 0, 0);

            ////////
            // Location
            StackPanel stackPanel_location =
                new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
            grid_main.SetGridRowColumn(stackPanel_location, 1, 0);

            Label label_location = new Label() { Content = "Location: ", FontWeight = FontWeights.Bold };
            stackPanel_location.Children.Add(label_location);

            m_comboBox_location = new ComboBox_Location();
            m_comboBox_location.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_location.SelectionChanged += ComboBox_Location_SelectionChanged;
            stackPanel_location.Children.Add(m_comboBox_location);

            ////////
            // Time
            Grid grid_time = new Grid() { HorizontalAlignment = HorizontalAlignment.Center };
            grid_time.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_time.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_time.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_time.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_time.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_time, 2, 0);

            Label label_time = new Label() { Content = "Time: ", FontWeight = FontWeights.Bold };
            Grid.SetColumnSpan(label_time, 3);
            grid_time.SetGridRowColumn(label_time, 0, 0);

            m_comboBox_time_hour = new ComboBox();
            for (int i = 0; i <= 24; ++i)
                m_comboBox_time_hour.Items.Add(string.Format("{0:00}", i));
            grid_time.SetGridRowColumn(m_comboBox_time_hour, 1, 0);

            Label label_colon = new Label() { Content = " : " };
            grid_time.SetGridRowColumn(label_colon, 1, 1);

            m_comboBox_time_minute = new ComboBox();
            for (int i = 0; i < 60; ++i)
                m_comboBox_time_minute.Items.Add(string.Format("{0:00}", i));
            grid_time.SetGridRowColumn(m_comboBox_time_minute, 1, 2);

            SetRoomStateTime(RoomStateTime);
            m_comboBox_time_hour.SelectionChanged += ComboBox_Time_SelectionChanged;
            m_comboBox_time_minute.SelectionChanged += ComboBox_Time_SelectionChanged;

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_RoomStateModified(object sender, GinTubBuilderManager.RoomStateModifiedEventArgs args)
        {
            if(RoomStateId == args.Id)
            {
                SetRoomStateState(args.State);
                SetRoomStateLocation(args.Location);
                SetRoomStateTime(args.Time);
                RoomId = args.Room;

                GinTubBuilderManager.LoadAllLocations();
            }
        }

        private void GinTubBuilderManager_RoomStateGet(object sender, GinTubBuilderManager.RoomStateGetEventArgs args)
        {
            SetGettableBackground(RoomStateId == args.Id);
        }

        private void GinTubBuilderManager_LocationAdded(object sender, GinTubBuilderManager.LocationAddedEventArgs args)
        {
            if (LocationId == args.Id)
                m_comboBox_location.SelectedItem = m_comboBox_location.Items.OfType<ComboBox_Location.ComboBoxItem_Location>().SingleOrDefault(i => i.LocationId == args.Id);
        }

        private void SetRoomStateLocation(int locationId)
        {
            LocationId = locationId;
            m_comboBox_location.SelectedItem = m_comboBox_location.Items.OfType<ComboBox_Location.ComboBoxItem_Location>().SingleOrDefault(l => l.LocationId == locationId);
        }

        private void SetRoomStateState(int? roomStateState)
        {
            m_textBlock_state.Text = (roomStateState.HasValue) ? roomStateState.ToString() : "NewState";
        }

        private void SetRoomStateTime(DateTime? roomStateTime)
        {
            RoomStateTime = roomStateTime;
            if (RoomStateTime != null)
            {
                m_comboBox_time_hour.SelectedItem = m_comboBox_time_hour.Items.OfType<string>().SingleOrDefault(h => int.Parse(h) == RoomStateTime.Value.Hour);
                m_comboBox_time_minute.SelectedItem = m_comboBox_time_minute.Items.OfType<string>().SingleOrDefault(m => int.Parse(m) == RoomStateTime.Value.Minute);
            }
            else
            {
                m_comboBox_time_hour.SelectedItem = null;
                m_comboBox_time_minute.SelectedItem = null;
            }
        }

        private void ComboBox_Location_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Location.ComboBoxItem_Location item;
            if (m_comboBox_location.SelectedItem != null && (item = m_comboBox_location.SelectedItem as ComboBox_Location.ComboBoxItem_Location) != null)
                LocationId = item.LocationId;
        }

        private void ComboBox_Time_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_comboBox_time_hour.SelectedItem != null && m_comboBox_time_minute.SelectedItem != null)
            {
                int
                    hour = int.Parse(m_comboBox_time_hour.SelectedItem.ToString()),
                    minute = int.Parse(m_comboBox_time_minute.SelectedItem.ToString());
                RoomStateTime = new DateTime(1988, 8, 13, hour, minute, 0);
            }
        }

        private void Grid_RoomStateData_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(RoomStateId.HasValue)
                GinTubBuilderManager.GetRoomState(RoomStateId.Value);
        }

        #endregion

        #endregion
    }
}
