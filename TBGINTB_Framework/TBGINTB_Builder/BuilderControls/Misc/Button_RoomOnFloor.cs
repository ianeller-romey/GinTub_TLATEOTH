using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class Button_RoomOnFloor : Button, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS
        #endregion


        #region MEMBER PROPERTIES

        public int RoomId { get; protected set; }
        public string RoomName { get; protected set; }
        public int RoomX { get; protected set; }
        public int RoomY { get; protected set; }
        public int RoomZ { get; protected set; }
        public int AreaId { get; protected set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Button_RoomOnFloor(int roomX, int roomY, int roomZ, int areaId)
        {
            AreaId = areaId;
            RoomX = roomX;
            RoomY = roomY;
            RoomZ = roomZ;

            FontSize = 9.0;

            SetActiveAndRegisterForGinTubEvents();

            HasNoRoom();
        }

        public Button_RoomOnFloor(int roomId, string name, int x, int y, int z, int areaId)
        {
            RoomX = x;
            RoomY = y;
            RoomZ = z;
            AreaId = areaId;

            FontSize = 9.0;

            HasRoom(roomId, name);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.RoomAdded += GinTubBuilderManager_RoomAdded;
            GinTubBuilderManager.RoomModified += GinTubBuilderManager_RoomModified;
            GinTubBuilderManager.RoomGet += GinTubBuilderManager_RoomGet;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.RoomAdded -= GinTubBuilderManager_RoomAdded;
            GinTubBuilderManager.RoomModified -= GinTubBuilderManager_RoomModified;
            GinTubBuilderManager.RoomGet -= GinTubBuilderManager_RoomGet;
        }

        public void SetFloor(int z)
        {
            RoomZ = z;
        }

        public void HasRoom(int roomId, string roomName)
        {
            RoomId = roomId;
            RoomName = roomName;

            Content = RoomName;
            ToolTip = RoomName;
            Background = Brushes.LightBlue;

            RemoveClickHandlers();
            Click += Button_UpdateRoom_Click;
        }

        public void HasNoRoom()
        {
            RoomId = -1;
            RoomName = string.Empty;

            Content = RoomName;
            ToolTip = null;
            Background = Brushes.LightGray;

            RemoveClickHandlers();
            Click += Button_CreateRoom_Click;
        }

        #endregion


        #region Private Functionality

        private void Button_CreateRoom_Click(object sender, RoutedEventArgs e)
        {
            Window_Room window = 
                new Window_Room
                (
                    null, 
                    string.Empty, 
                    RoomX, 
                    RoomY, 
                    RoomZ,
                    AreaId,
                    (win) =>
                    {
                        Window_Room wWin = win as Window_Room;
                        if (wWin != null)
                            GinTubBuilderManager.AddRoom(wWin.RoomName, wWin.RoomX, wWin.RoomY, wWin.RoomZ, wWin.AreaId);
                    }
                );
            window.Show();
        }

        private void Button_UpdateRoom_Click(object sender, RoutedEventArgs e)
        {
            GinTubBuilderManager.GetRoom(RoomId);
        }

        private void RemoveClickHandlers()
        {
            // Remove everything, in case we accidentally call "HasRoom" or "HasNoRoom" twice
            Click -= Button_CreateRoom_Click;
            Click -= Button_UpdateRoom_Click;
        }

        private void GinTubBuilderManager_RoomAdded(object sender, GinTubBuilderManager.RoomAddedEventArgs args)
        {
            if (args.Area == AreaId && args.X == RoomX && args.Y == RoomY && args.Z == RoomZ)
                HasRoom(args.Id, args.Name);
        }

        private void GinTubBuilderManager_RoomModified(object sender, GinTubBuilderManager.RoomModifiedEventArgs args)
        {
            if (args.Area == AreaId && args.X == RoomX && args.Y == RoomY && args.Z == RoomZ)
                HasRoom(args.Id, args.Name);
        }

        private void GinTubBuilderManager_RoomGet(object sender, GinTubBuilderManager.RoomGetEventArgs args)
        {
            Background = 
                (args.Area == AreaId && args.X == RoomX && args.Y == RoomY && args.Z == RoomZ)
                ? Brushes.LightGreen
                : null;
        }

        #endregion

        #endregion

    }
}
