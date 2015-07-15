using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_RoomAndStates : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Bordered_Room m_grid_rooms;
        StackPanel m_stackPanel_roomStates;

        #endregion


        #region MEMBER PROPERTIES

        public int SelectedRoomId { get; private set; }
        public string SelectedRoomName { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_RoomAndStates(int roomId, string roomName, int roomX, int roomY, int roomZ, int areaId)
        {
            SelectedRoomId = roomId;
            SelectedRoomName = roomName;
            CreateControls(roomId, roomName, roomX, roomY, roomZ, areaId);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.RoomStateAdded += GinTubBuilderManager_RoomStateAdded;

            m_grid_rooms.SetActiveAndRegisterForGinTubEvents();
            foreach (var grid in m_stackPanel_roomStates.Children.OfType<UserControl_RoomStateModification>())
                grid.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.RoomStateAdded -= GinTubBuilderManager_RoomStateAdded;

            m_grid_rooms.SetInactiveAndUnregisterFromGinTubEvents();
            foreach (var grid in m_stackPanel_roomStates.Children.OfType<UserControl_RoomStateModification>())
                grid.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int roomId, string roomName, int roomX, int roomY, int roomZ, int areaId)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });

            ////////
            // Room
            Button button_roomPreview = new Button() { Content = "View Room Preview" };
            button_roomPreview.Click += Button_RoomPreview_Click;
            grid_main.SetGridRowColumn(button_roomPreview, 0, 0);

            Button button_modifyRoom = new Button() { Content = "Modify Room" };
            button_modifyRoom.Click += Button_ModifyRoom_Click;
            grid_main.SetGridRowColumn(button_modifyRoom, 1, 0);

            m_grid_rooms = new UserControl_Bordered_Room(roomId, roomName, roomX, roomY, roomZ, areaId, false);
            grid_main.SetGridRowColumn(m_grid_rooms, 2, 0);
            m_grid_rooms.SetActiveAndRegisterForGinTubEvents();

            ////////
            // RoomStates
            Button button_addRoomState = new Button() { Content = "New Room State ..." };
            button_addRoomState.Click += Button_AddRoomState_Click;
            grid_main.SetGridRowColumn(button_addRoomState, 3, 0);

            m_stackPanel_roomStates = new StackPanel() { Orientation = Orientation.Vertical };

            ScrollViewer scrollViewer_roomStates =
                new ScrollViewer()
                {
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Visible
                };
            scrollViewer_roomStates.Content = m_stackPanel_roomStates;
            grid_main.SetGridRowColumn(scrollViewer_roomStates, 4, 0);

            Content = grid_main;
        }

        private void AddRoomState(int roomStateId, int roomStateState, DateTime? roomStateTime, int locationId, int roomId)
        {
            UserControl_RoomStateModification grid = new UserControl_RoomStateModification(roomStateId, roomStateState, roomStateTime, locationId, roomId);
            m_stackPanel_roomStates.Children.Add(grid);
            GinTubBuilderManager.LoadAllLocations();
        }

        private void GinTubBuilderManager_RoomStateAdded(object sender, GinTubBuilderManager.RoomStateAddedEventArgs args)
        {
            AddRoomState(args.Id, args.State, args.Time, args.Location, args.Room);
        }

        void Button_RoomPreview_Click(object sender, RoutedEventArgs e)
        {
            Window_RoomPreview window = new Window_RoomPreview(SelectedRoomId, SelectedRoomName);
            window.SetActiveAndRegisterForGinTubEvents();
            window.Show();
            GinTubBuilderManager.GetRoomPreview(SelectedRoomId);
        }

        private void Button_ModifyRoom_Click(object sender, RoutedEventArgs e)
        {
            Window_Room window =
                new Window_Room
                (
                    m_grid_rooms.RoomId,
                    m_grid_rooms.RoomName,
                    m_grid_rooms.RoomX,
                    m_grid_rooms.RoomY,
                    m_grid_rooms.RoomZ,
                    m_grid_rooms.AreaId,
                    (win) =>
                    {
                        Window_Room wWin = win as Window_Room;
                        if (wWin != null)
                            GinTubBuilderManager.ModifyRoom
                            (
                                wWin.RoomId.Value,
                                wWin.RoomName,
                                wWin.RoomX,
                                wWin.RoomY,
                                wWin.RoomZ,
                                m_grid_rooms.AreaId
                            );
                    }
                );
            window.Show();
        }

        private void Button_AddRoomState_Click(object sender, RoutedEventArgs e)
        {
            Window_RoomState window =
                new Window_RoomState
                (
                    null, 
                    null, 
                    null, 
                    null,
                    m_grid_rooms.RoomId.Value,
                    (win) =>
                    {
                        Window_RoomState wWin = win as Window_RoomState;
                        if (wWin != null)
                            GinTubBuilderManager.AddRoomState(wWin.RoomStateTime, wWin.LocationId.Value, m_grid_rooms.RoomId.Value);
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
