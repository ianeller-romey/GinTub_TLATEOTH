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
    public class Window_RoomState : Window_TaskOnAccept
    {
        #region MEMBER FIELDS

        UserControl_RoomState m_userControl_roomState;

        #endregion


        #region MEMBER PROPERTIES

        public int? RoomStateId { get { return m_userControl_roomState.RoomStateId; } }
        public int? RoomStateState { get { return m_userControl_roomState.RoomStateState; } }
        public DateTime? RoomStateTime { get { return m_userControl_roomState.RoomStateTime; } }
        public int? LocationId { get { return m_userControl_roomState.LocationId; } }
        public int RoomId { get { return m_userControl_roomState.RoomId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_RoomState(int? roomStateId, int? roomStateState, DateTime? roomStateTime, int? locationId, int roomId, TaskOnAccept task) :
            base("Room State Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(roomStateId, roomStateState, roomStateTime, locationId, roomId);
            m_userControl_roomState.SetActiveAndRegisterForGinTubEvents(); // need for loading Location
            GinTubBuilderManager.LoadAllLocations();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? roomStateId, int? roomStateState, DateTime? roomStateTime, int? locationId, int roomId)
        {
            m_userControl_roomState = new UserControl_RoomState(roomStateId, roomStateState, roomStateTime, locationId, roomId, true);
            return m_userControl_roomState;
        }

        #endregion

        #endregion
    }
}
