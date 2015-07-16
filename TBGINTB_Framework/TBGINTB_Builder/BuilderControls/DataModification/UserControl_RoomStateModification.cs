using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_RoomStateModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_RoomState m_userControl_roomState;

        #endregion


        #region MEMBER PROPERTIES

        public int? RoomStateId { get { return m_userControl_roomState.RoomStateId; } }
        public int? RoomStateState { get { return m_userControl_roomState.RoomStateState; } }
        public TimeSpan? RoomStateTime { get { return m_userControl_roomState.RoomStateTime; } }
        public int? LocationId { get { return m_userControl_roomState.LocationId; } }
        public int RoomId { get { return m_userControl_roomState.RoomId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_RoomStateModification(int roomStateId, int roomStateState, TimeSpan? roomStateTime, int locationId, int roomId)
        {
            CreateControls(roomStateId, roomStateState, roomStateTime, locationId, roomId);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_roomState.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_roomState.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int roomStateId, int roomStateState, TimeSpan? roomStateTime, int locationId, int roomId)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyRoomState = new Button() { Content = "Modify Room State" };
            button_modifyRoomState.Click += Button_UpdateRoomState_Click;
            grid_main.SetGridRowColumn(button_modifyRoomState, 0, 0);

            m_userControl_roomState = new UserControl_RoomState(roomStateId, roomStateState, roomStateTime, locationId, roomId, false);
            Border border_roomState = new Border() { Style = new Style_DefaultBorder() };
            border_roomState.Child = m_userControl_roomState;
            grid_main.SetGridRowColumn(border_roomState, 1, 0);
            m_userControl_roomState.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_UpdateRoomState_Click(object sender, RoutedEventArgs e)
        {
            Window_RoomState window = 
                new Window_RoomState
                (
                    m_userControl_roomState.RoomStateId,
                    m_userControl_roomState.RoomStateState,
                    m_userControl_roomState.RoomStateTime,  
                    m_userControl_roomState.LocationId,
                    m_userControl_roomState.RoomId,
                    (win) =>
                    {
                        Window_RoomState wWin = win as Window_RoomState;
                        if (wWin != null)
                            GinTubBuilderManager.UpdateRoomState
                            (
                                wWin.RoomStateId.Value,
                                wWin.RoomStateState.Value,
                                wWin.RoomStateTime.Value,
                                wWin.LocationId.Value,
                                wWin.RoomId
                            );
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
