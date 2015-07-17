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
    public class UserControl_RoomStateName : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        TextBlock 
            m_textBlock_id,
            m_textBlock_name;

        #endregion


        #region MEMBER PROPERTIES

        public int RoomStateId { get; private set; }
        public string RoomStateName { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_RoomStateName(int roomStateId, string roomStateName)
        {
            CreateControls();

            SetRoomStateId(roomStateId);
            SetRoomStateName(roomStateName);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.RoomStateUpdated += GinTubBuilderManager_RoomStateUpdated;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.RoomStateUpdated -= GinTubBuilderManager_RoomStateUpdated;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
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
            TextBlock textBlock_id = new TextBlock() { VerticalAlignment = VerticalAlignment.Center };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Name Grid
            Grid grid_name = new Grid();
            grid_name.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_name.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_name, 1, 0);

            ////////
            // Name
            m_textBlock_name = new TextBlock() { VerticalAlignment = VerticalAlignment.Center };
            Label label_name = new Label() { Content = "Name:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_name.SetGridRowColumn(m_textBlock_name, 1, 0);
            grid_name.SetGridRowColumn(label_name, 0, 0);
            
            ////////
            // Fin
            Content = grid_main;
        }

        void GinTubBuilderManager_RoomStateUpdated(object sender, GinTubBuilderManager.RoomStateUpdatedEventArgs args)
        {
            if (RoomStateId == args.Id)
            {
                SetRoomStateName(args.Name);
            }
        }

        private void SetRoomStateId(int roomStateId)
        {
            RoomStateId = roomStateId;
            m_textBlock_id.Text = RoomStateId.ToString();
        }

        private void SetRoomStateName(string roomStateName)
        {
            RoomStateName = roomStateName;
            m_textBlock_name.Text = RoomStateName;
        }
        
        #endregion

        #endregion
    }
}
