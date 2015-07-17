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
    public class UserControl_RoomStateNameAndTime : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        TextBlock 
            m_textBlock_name;

        #endregion


        #region MEMBER PROPERTIES

        public int RoomStateId { get; private set; }
        public string RoomStateName { get; private set; }
        public TimeSpan RoomStateTime { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_RoomStateNameAndTime(int roomStateId, string roomStateName, TimeSpan roomStateTime)
        {
            CreateControls();

            RoomStateId = roomStateId;
            SetRoomStateName(roomStateName);
            SetRoomStateTime(roomStateTime);
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

            ////////
            // Name Grid
            Grid grid_name = new Grid();
            grid_name.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_name.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_name, 0, 0);

            ////////
            // Name
            m_textBlock_name = new TextBlock() { VerticalAlignment = VerticalAlignment.Center };
            Label label_name = new Label() { Content = "RoomState Name:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_name.SetGridRowColumn(m_textBlock_name, 0, 1);
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
                SetRoomStateTime(args.Time);
            }
        }

        private void SetRoomStateName(string roomStateName)
        {
            RoomStateName = roomStateName;
            m_textBlock_name.Text = RoomStateName;
        }

        private void SetRoomStateTime(TimeSpan roomStateTime)
        {
            RoomStateTime = roomStateTime;
        }
        
        #endregion

        #endregion
    }
}
