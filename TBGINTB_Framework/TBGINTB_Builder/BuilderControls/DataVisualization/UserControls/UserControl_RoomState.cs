using System;
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
    public class UserControl_RoomState : UserControl_Selecttable
    {
        #region MEMBER FIELDS

        TextBox m_textBox_state;
        ComboBox_Location m_comboBox_location;
        UserControl_TimeSpan m_userControl_timeSpan;

        #endregion


        #region MEMBER PROPERTIES

        public int? RoomStateId { get; private set; }
        public int? RoomStateState { get; private set; }
        public TimeSpan? RoomStateTime { get; private set; }
        public int? LocationId { get; private set; }
        public int RoomId { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_state,
                    m_comboBox_location,
                    m_userControl_timeSpan
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_RoomState(int? roomStateId, int? roomStateState, TimeSpan? roomStateTime, int? locationId, int roomId, bool enableEditing)
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
            GinTubBuilderManager.RoomStateUpdated += GinTubBuilderManager_RoomStateUpdated;
            GinTubBuilderManager.RoomStateSelect += GinTubBuilderManager_RoomStateSelect;

            GinTubBuilderManager.LocationRead += GinTubBuilderManager_LocationRead;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.RoomStateUpdated -= GinTubBuilderManager_RoomStateUpdated;
            GinTubBuilderManager.RoomStateSelect -= GinTubBuilderManager_RoomStateSelect;

            GinTubBuilderManager.LocationRead -= GinTubBuilderManager_LocationRead;
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
                    Text = (RoomStateId.HasValue) ? RoomStateId.ToString() : "NewRoomState"
                };
            Label label_roomId = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_roomId, 0, 0);
            
            ////////
            // State Grid
            Grid grid_state = new Grid();
            grid_state.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_state.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_state, 1, 0);

            ////////
            // State
            m_textBox_state = new TextBox() { VerticalAlignment = VerticalAlignment.Center };
            m_textBox_state.TextChanged += TextBox_State_TextChanged;
            SetRoomStateState(RoomStateState);
            Label label_state = new Label() { Content = "State:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_state.SetGridRowColumn(m_textBox_state, 0, 1);
            grid_state.SetGridRowColumn(label_state, 0, 0);

            ////////
            // Location
            StackPanel stackPanel_location =
                new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
            grid_main.SetGridRowColumn(stackPanel_location, 2, 0);

            Label label_location = new Label() { Content = "Location: ", FontWeight = FontWeights.Bold };
            stackPanel_location.Children.Add(label_location);

            m_comboBox_location = new ComboBox_Location();
            m_comboBox_location.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_location.SelectionChanged += ComboBox_Location_SelectionChanged;
            stackPanel_location.Children.Add(m_comboBox_location);

            ////////
            // Time
            m_userControl_timeSpan = new UserControl_TimeSpan(RoomStateTime);
            grid_main.SetGridRowColumn(m_userControl_timeSpan, 3, 0);
            m_userControl_timeSpan.TimeChangedEvent += UserControl_TimeSpan_TimeChanged;

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_RoomStateUpdated(object sender, GinTubBuilderManager.RoomStateUpdatedEventArgs args)
        {
            if(RoomStateId == args.Id)
            {
                SetRoomStateState(args.State);
                SetRoomStateLocation(args.Location);
                SetRoomStateTime(args.Time);
                RoomId = args.Room;

                GinTubBuilderManager.ReadAllLocations();
            }
        }

        private void GinTubBuilderManager_RoomStateSelect(object sender, GinTubBuilderManager.RoomStateSelectEventArgs args)
        {
            SetSelecttableBackground(RoomStateId == args.Id);
        }

        private void GinTubBuilderManager_LocationRead(object sender, GinTubBuilderManager.LocationReadEventArgs args)
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
            m_textBox_state.Text = (roomStateState.HasValue) ? roomStateState.ToString() : "";
        }

        private void SetRoomStateTime(TimeSpan? roomStateTime)
        {
            RoomStateTime = roomStateTime;
            m_userControl_timeSpan.SetTime(RoomStateTime);
        }

        void TextBox_State_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if(textBox == m_textBox_state)
            {
                int state;
                if (int.TryParse(m_textBox_state.Text, out state))
                    RoomStateState = state;
            }
        }

        private void ComboBox_Location_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBox_Location.ComboBoxItem_Location item;
            if (comboBox == m_comboBox_location && m_comboBox_location.SelectedItem != null && (item = m_comboBox_location.SelectedItem as ComboBox_Location.ComboBoxItem_Location) != null)
                LocationId = item.LocationId;
        }

        private void UserControl_TimeSpan_TimeChanged(TimeSpan time)
        {
            RoomStateTime = time;
        }

        private void Grid_RoomStateData_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(RoomStateId.HasValue)
                GinTubBuilderManager.SelectRoomState(RoomStateId.Value);
        }

        #endregion

        #endregion
    }
}
