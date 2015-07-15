using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.HelperControls;


namespace TBGINTB_Builder.BuilderControls
{
    public class Window_Room : Window_TaskOnAccept
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

        public Window_Room(int? roomId, string roomName, int roomX, int roomY, int roomZ, int areaId, TaskOnAccept task) :
            base("Room Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(roomId, roomName, roomX, roomY, roomZ, areaId);
            m_userControl_room.SetActiveAndRegisterForGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? roomId, string roomName, int roomX, int roomY, int roomZ, int areaId)
        {
            m_userControl_room = new UserControl_Room(roomId, roomName, roomX, roomY, roomZ, areaId, true);
            return m_userControl_room;
        }

        #endregion

        #endregion

    }
}
