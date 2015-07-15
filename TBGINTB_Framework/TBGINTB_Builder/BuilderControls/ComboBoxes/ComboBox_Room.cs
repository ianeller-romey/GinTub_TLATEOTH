using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class ComboBox_Room : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxItem_newRoom = new ComboBoxItem() { Content = "New Room ..." };

        #endregion


        #region MEMBER PROPERTIES

        public int AreaId { get; private set; }

        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_Room : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int RoomId { get; private set; }
            public string RoomName { get; private set; }
            public int RoomX { get; private set; }
            public int RoomY { get; private set; }
            public int RoomZ { get; private set; }
            public int AreaId { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Room(int roomId, string roomName, int roomX, int roomY, int roomZ, int areaId)
            {
                RoomId = roomId;
                SetRoomName(roomName);
                SetRoomX(roomX);
                SetRoomY(roomY);
                SetRoomZ(roomZ);
                SetAreaId(areaId);
            }

            public void SetRoomName(string roomName)
            {
                RoomName = roomName;
                Content = RoomName;
            }

            public void SetRoomX(int roomX)
            {
                RoomX = roomX;
            }

            public void SetRoomY(int roomY)
            {
                RoomY = roomY;
            }

            public void SetRoomZ(int roomZ)
            {
                RoomZ = roomZ;
            }

            public void SetAreaId(int areaId)
            {
                AreaId = areaId;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_Room(int areaId)
        {
            AreaId = areaId;

            Items.Add(c_comboBoxItem_newRoom);

            SelectionChanged += ComboBox_Room_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.RoomAdded += GinTubBuilderManager_RoomAdded;
            GinTubBuilderManager.RoomModified += GinTubBuilderManager_RoomModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.RoomAdded -= GinTubBuilderManager_RoomAdded;
            GinTubBuilderManager.RoomModified -= GinTubBuilderManager_RoomModified;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_RoomAdded(object sender, GinTubBuilderManager.RoomAddedEventArgs args)
        {
            if (AreaId == args.Area)
            {
                if (!Items.OfType<ComboBoxItem_Room>().Any(i => i.RoomId == args.Id))
                    Items.Add(new ComboBoxItem_Room(args.Id, args.Name, args.X, args.Y, args.Z, args.Area));
            }
        }

        private void GinTubBuilderManager_RoomModified(object sender, GinTubBuilderManager.RoomModifiedEventArgs args)
        {
            if (AreaId == args.Area)
            {
                ComboBoxItem_Room item = Items.OfType<ComboBoxItem_Room>().SingleOrDefault(i => i.RoomId == args.Id);
                if (item != null)
                {
                    item.SetRoomName(args.Name);
                    item.SetRoomX(args.X);
                    item.SetRoomY(args.Y);
                    item.SetRoomZ(args.Z);
                    item.SetAreaId(args.Area);
                }
            }
        }

        private void NewRoomDialog()
        {
            Window_Notification window = new Window_Notification("Room Data", "This feature is not currently available.");
            window.ShowDialog();
        }

        private void ComboBox_Room_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if ((item = SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newRoom)
                    NewRoomDialog();
            }
        }

        #endregion

        #endregion
    }
}
