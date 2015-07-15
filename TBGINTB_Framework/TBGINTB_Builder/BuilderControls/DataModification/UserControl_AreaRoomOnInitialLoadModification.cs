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
    public class UserControl_AreaRoomOnInitialLoadModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_AreaRoomOnInitialLoad m_userControl_areaRoomOnInitialLoad;

        #endregion


        #region MEMBER PROPERTIES

        public int? AreaRoomOnInitialLoadArea { get { return m_userControl_areaRoomOnInitialLoad.AreaRoomOnInitialLoadArea; } }
        public int? AreaRoomOnInitialLoadRoom { get { return m_userControl_areaRoomOnInitialLoad.AreaRoomOnInitialLoadRoom; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_AreaRoomOnInitialLoadModification(int? areaRoomOnInitialLoadArea, int? areaRoomOnInitialLoadRoom)
        {
            CreateControls(areaRoomOnInitialLoadArea, areaRoomOnInitialLoadRoom);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_areaRoomOnInitialLoad.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_areaRoomOnInitialLoad.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? areaRoomOnInitialLoadArea, int? areaRoomOnInitialLoadRoom)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyAreaRoomOnInitialLoad = new Button() { Content = "Modify AreaRoomOnInitialLoad" };
            button_modifyAreaRoomOnInitialLoad.Click += Button_ModifyAreaRoomOnInitialLoad_Click;
            grid_main.SetGridRowColumn(button_modifyAreaRoomOnInitialLoad, 0, 0);

            m_userControl_areaRoomOnInitialLoad = new UserControl_AreaRoomOnInitialLoad(areaRoomOnInitialLoadArea, areaRoomOnInitialLoadRoom, false);
            grid_main.SetGridRowColumn(m_userControl_areaRoomOnInitialLoad, 1, 0);
            m_userControl_areaRoomOnInitialLoad.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_ModifyAreaRoomOnInitialLoad_Click(object sender, RoutedEventArgs e)
        {
            Window_AreaRoomOnInitialLoad window =
                new Window_AreaRoomOnInitialLoad
                (
                    m_userControl_areaRoomOnInitialLoad.AreaRoomOnInitialLoadArea, 
                    m_userControl_areaRoomOnInitialLoad.AreaRoomOnInitialLoadRoom,
                    (win) =>
                    {
                        Window_AreaRoomOnInitialLoad wWin = win as Window_AreaRoomOnInitialLoad;
                        if (wWin != null)
                            GinTubBuilderManager.ModifyAreaRoomOnInitialLoad(wWin.AreaRoomOnInitialLoadArea.Value, wWin.AreaRoomOnInitialLoadRoom.Value);
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
