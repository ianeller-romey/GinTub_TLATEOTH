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
    public class UserControl_Bordered_RoomState : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_Bordered_RoomState(int? roomStateId, int? roomStateState, TimeSpan? roomStateTime, int? locationId, int roomId, bool enableEditing)
        {
            CreateControls(roomStateId, roomStateState, roomStateTime, locationId, roomId, enableEditing);
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

        private void CreateControls(int? roomStateId, int? roomStateState, TimeSpan? roomStateTime, int? locationId, int roomId, bool enableEditing)
        {
            m_userControl_roomState = new UserControl_RoomState(roomStateId, roomStateState, roomStateTime, locationId, roomId, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_roomState };
            Content = border;
        }

        #endregion

        #endregion
    }
}
