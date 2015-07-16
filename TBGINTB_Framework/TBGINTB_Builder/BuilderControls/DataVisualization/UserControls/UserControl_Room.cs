using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_Room : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        TextBox m_textBox_name;
        TextBlock
            m_textBlock_x,
            m_textBlock_y,
            m_textBlock_z;

        #endregion


        #region MEMBER PROPERTIES

        public int? RoomId { get; private set; }
        public string RoomName { get; private set; }
        public int RoomX { get; private set; }
        public int RoomY { get; private set; }
        public int RoomZ { get; private set; }
        public int AreaId { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_name
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Room(int? roomId, string roomName, int roomX, int roomY, int roomZ, int areaId, bool enableEditing)
        {
            RoomId = roomId;
            RoomName = roomName;
            RoomX = roomX;
            RoomY = roomY;
            RoomZ = roomZ;
            AreaId = areaId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
            if (!enableEditing)
                MouseLeftButtonDown += UserControl_Room_MouseLeftButtonDown;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.RoomUpdated += GinTubBuilderManager_RoomUpdated;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.RoomUpdated -= GinTubBuilderManager_RoomUpdated;
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
                    Text = (RoomId.HasValue) ? RoomId.ToString() : "NewRoom"
                };
            Label label_roomId = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_roomId, 0, 0);

            ////////
            // XYZ Grid
            Grid grid_XYZ = new Grid();
            grid_XYZ.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_XYZ.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_XYZ.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_XYZ.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_XYZ.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_XYZ.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_XYZ, 1, 0);

            ////////
            // X
            m_textBlock_x = new TextBlock() { VerticalAlignment = VerticalAlignment.Center, Text = RoomX.ToString() };
            Label label_x = new Label() { Content = "X:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_XYZ.SetGridRowColumn(m_textBlock_x, 0, 1);
            grid_XYZ.SetGridRowColumn(label_x, 0, 0);

            ////////
            // Y
            m_textBlock_y = new TextBlock() { VerticalAlignment = VerticalAlignment.Center, Text = RoomY.ToString() };
            Label label_y = new Label() { Content = "Y:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_XYZ.SetGridRowColumn(m_textBlock_y, 0, 3);
            grid_XYZ.SetGridRowColumn(label_y, 0, 2);

            ////////
            // Z
            m_textBlock_z = new TextBlock() { VerticalAlignment = VerticalAlignment.Center, Text = RoomZ.ToString() };
            Label label_z = new Label() { Content = "Z:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_XYZ.SetGridRowColumn(m_textBlock_z, 0, 5);
            grid_XYZ.SetGridRowColumn(label_z, 0, 4);

            ////////
            // Name Grid
            Grid grid_name = new Grid();
            grid_name.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_name.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_name, 2, 0);

            ////////
            // Name
            m_textBox_name = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Text = RoomName };
            m_textBox_name.TextChanged += TextBox_Name_TextChanged;
            Label label_name = new Label() { Content = "Name: ", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_name.SetGridRowColumn(m_textBox_name, 0, 1);
            grid_name.SetGridRowColumn(label_name, 0, 0);

            ////////
            // Fin
            Content = grid_main;

        }

        private void GinTubBuilderManager_RoomUpdated(object sender, GinTubBuilderManager.RoomUpdatedEventArgs args)
        {
            if (args.Id == RoomId)
            {
                SetRoomX(args.X);
                SetRoomY(args.Y);
                SetRoomZ(args.Z);
                SetRoomName(args.Name);
                AreaId = args.Area;
            }
        }

        private void SetRoomX(int roomX)
        {
            RoomX = roomX;
            m_textBlock_x.Text = RoomX.ToString();
        }

        private void SetRoomY(int roomY)
        {
            RoomY = roomY;
            m_textBlock_y.Text = RoomY.ToString();
        }

        private void SetRoomZ(int roomZ)
        {
            RoomZ = roomZ;
            m_textBlock_z.Text = RoomZ.ToString();
        }

        private void SetRoomName(string roomName)
        {
            m_textBox_name.Text = roomName;
            if (!m_textBox_name.IsEnabled)
                TextBox_Name_TextChanged(m_textBox_name, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void TextBox_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_name)
                RoomName = m_textBox_name.Text;
        }

        private void UserControl_Room_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (RoomId.HasValue)
                GinTubBuilderManager.SelectRoom(RoomId.Value);
        }

        #endregion

        #endregion
    }
}
