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
    public class UserControl_GameStateOnInitialLoad : UserControl
    {
        #region MEMBER FIELDS

        ComboBox_Area m_comboBox_area;
        GroupBox m_groupBox_room;
        ComboBox_Room m_comboBox_room;

        #endregion


        #region MEMBER PROPERTIES

        public int? GameStateOnInitialLoadArea { get; private set; }
        public int? GameStateOnInitialLoadRoom { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_comboBox_area,
                    m_groupBox_room
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_GameStateOnInitialLoad(int? gameStateOnInitialLoadArea, int? gameStateOnInitialLoadRoom, bool enableEditing)
        {
            GameStateOnInitialLoadArea = gameStateOnInitialLoadArea;
            GameStateOnInitialLoadRoom = gameStateOnInitialLoadRoom;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.GameStateOnInitialLoadUpdated += GinTubBuilderManager_GameStateOnInitialLoadUpdated;

            GinTubBuilderManager.AreaRead += GinTubBuilderManager_AreaRead;

            GinTubBuilderManager.RoomRead += GinTubBuilderManager_RoomRead;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.GameStateOnInitialLoadUpdated -= GinTubBuilderManager_GameStateOnInitialLoadUpdated;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            ////////
            // Area Grid
            Grid grid_area = new Grid();
            grid_area.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_area.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_area, 0, 0);

            ////////
            // Area
            m_comboBox_area = new ComboBox_Area();
            m_comboBox_area.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_area.SelectionChanged += ComboBox_Area_SelectionChanged;
            Label label_area = new Label() { Content = "Area:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_area.SetGridRowColumn(m_comboBox_area, 1, 0);
            grid_area.SetGridRowColumn(label_area, 0, 0);

            ////////
            // Room
            m_groupBox_room = new GroupBox() { Header = "Room:" };
            grid_main.SetGridRowColumn(m_groupBox_room, 1, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_GameStateOnInitialLoadUpdated(object sender, GinTubBuilderManager.GameStateOnInitialLoadUpdatedEventArgs args)
        {
            SetGameStateOnInitialLoadArea(args.Area.Value);
            SetGameStateOnInitialLoadRoom(args.Room.Value);
        }

        private void GinTubBuilderManager_AreaRead(object sender, GinTubBuilderManager.AreaReadEventArgs args)
        {
            ResetGameStateOnInitialLoadArea(args.Id);
        }

        void GinTubBuilderManager_RoomRead(object sender, GinTubBuilderManager.RoomReadEventArgs args)
        {
            if (GameStateOnInitialLoadArea == args.Area)
                ResetGameStateOnInitialLoadRoom(args.Id);
        }

        private void SetGameStateOnInitialLoadArea(int GameStateOnInitialLoadArea)
        {
            ComboBox_Area.ComboBoxItem_Area item =
                m_comboBox_area.Items.OfType<ComboBox_Area.ComboBoxItem_Area>().
                SingleOrDefault(i => i.AreaId == GameStateOnInitialLoadArea);
            if (item != null)
                m_comboBox_area.SelectedItem = item;
        }

        private void SetGameStateOnInitialLoadRoom(int GameStateOnInitialLoadRoom)
        {
            if (m_comboBox_room != null)
            {
                ComboBox_Room.ComboBoxItem_Room item = m_comboBox_room.Items.OfType<ComboBox_Room.ComboBoxItem_Room>().
                    SingleOrDefault(i => i.RoomId == GameStateOnInitialLoadRoom);
                if (item != null)
                    m_comboBox_room.SelectedItem = item;
            }
        }

        private void ResetGameStateOnInitialLoadArea(int gameStateOnInitialLoadArea)
        {
            ComboBox_Area.ComboBoxItem_Area item =
                m_comboBox_area.Items.OfType<ComboBox_Area.ComboBoxItem_Area>().
                SingleOrDefault(i => GameStateOnInitialLoadArea.HasValue && GameStateOnInitialLoadArea.Value == GameStateOnInitialLoadArea && i.AreaId == GameStateOnInitialLoadArea);
            if (item != null)
                m_comboBox_area.SelectedItem = item;
        }

        private void ResetGameStateOnInitialLoadRoom(int gameStateOnInitialLoadRoom)
        {
            if (m_comboBox_room != null)
            {
                ComboBox_Room.ComboBoxItem_Room item = m_comboBox_room.Items.OfType<ComboBox_Room.ComboBoxItem_Room>().
                    SingleOrDefault(i => GameStateOnInitialLoadRoom.HasValue && GameStateOnInitialLoadRoom.Value == GameStateOnInitialLoadRoom && i.RoomId == GameStateOnInitialLoadRoom);
                if (item != null)
                    m_comboBox_room.SelectedItem = item;
            }
        }

        private void ComboBox_Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Area.ComboBoxItem_Area item;
            if (m_comboBox_area.SelectedItem != null && (item = m_comboBox_area.SelectedItem as ComboBox_Area.ComboBoxItem_Area) != null)
            {
                // kind of a hack here
                // since this Control is created before the m_comboBox_room Control,
                // and since we want the m_comboBox_room Control to handle the RoomRead event first,
                // we'll remove the handler here, and added back after the m_comboBox_room has been created
                GinTubBuilderManager.RoomRead -= GinTubBuilderManager_RoomRead;

                GameStateOnInitialLoadArea = item.AreaId;

                if (m_groupBox_room.Content != null)
                    m_groupBox_room.Content = null;
                m_comboBox_room = new ComboBox_Room(GameStateOnInitialLoadArea.Value);
                m_comboBox_room.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
                m_comboBox_room.SelectionChanged += ComboBox_Room_SelectionChanged;
                m_groupBox_room.Content = m_comboBox_room;

                GinTubBuilderManager.RoomRead += GinTubBuilderManager_RoomRead;

                GinTubBuilderManager.ReadAllRoomsInArea(GameStateOnInitialLoadArea.Value);
            }
        }

        private void ComboBox_Room_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Room.ComboBoxItem_Room item;
            if (m_comboBox_room.SelectedItem != null && (item = m_comboBox_room.SelectedItem as ComboBox_Room.ComboBoxItem_Room) != null)
                GameStateOnInitialLoadRoom = item.RoomId;
        }

        #endregion

        #endregion
    }
}
