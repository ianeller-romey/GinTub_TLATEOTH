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
    public class UserControl_Bordered_Room : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Room m_userControl_room;

        #endregion


        #region MEMBER PROPERTIES

        public int? RoomId { get { return m_userControl_room.RoomId; } }
        public string RoomName { get { return m_userControl_room.RoomName; } }
        public int RoomX { get { return m_userControl_room.RoomX; } }
        public int RoomY { get { return m_userControl_room.RoomY; } }
        public int RoomZ { get { return m_userControl_room.RoomZ; } }
        public int AreaId { get { return m_userControl_room.AreaId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_Room(int? roomId, string roomName, int roomX, int roomY, int roomZ, int areaId, bool enableEditing)
        {
            CreateControls(roomId, roomName, roomX, roomY, roomZ, areaId, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_room.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_room.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? roomId, string roomName, int roomX, int roomY, int roomZ, int areaId, bool enableEditing)
        {
            m_userControl_room = new UserControl_Room(roomId, roomName, roomX, roomY, roomZ, areaId, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_room };
            Content = border;
        }

        #endregion

        #endregion
    }
}
