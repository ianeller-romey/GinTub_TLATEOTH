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
    public class UserControl_GameStateOnInitialLoadModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_GameStateOnInitialLoad m_userControl_GameStateOnInitialLoad;

        #endregion


        #region MEMBER PROPERTIES

        public int? GameStateOnInitialLoadArea { get { return m_userControl_GameStateOnInitialLoad.GameStateOnInitialLoadArea; } }
        public int? GameStateOnInitialLoadRoom { get { return m_userControl_GameStateOnInitialLoad.GameStateOnInitialLoadRoom; } }
        public TimeSpan? GameStateOnInitialLoadTime { get { return m_userControl_GameStateOnInitialLoad.GameStateOnInitialLoadTime; } } 

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_GameStateOnInitialLoadModification(int? gameStateOnInitialLoadArea, int? gameStateOnInitialLoadRoom, TimeSpan? gameStateOnInitialLoadTime)
        {
            CreateControls(gameStateOnInitialLoadArea, gameStateOnInitialLoadRoom, gameStateOnInitialLoadTime);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_GameStateOnInitialLoad.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_GameStateOnInitialLoad.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? gameStateOnInitialLoadArea, int? gameStateOnInitialLoadRoom, TimeSpan? gameStateOnInitialLoadTime)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyGameStateOnInitialLoad = new Button() { Content = "Modify GameStateOnInitialLoad" };
            button_modifyGameStateOnInitialLoad.Click += Button_UpdateGameStateOnInitialLoad_Click;
            grid_main.SetGridRowColumn(button_modifyGameStateOnInitialLoad, 0, 0);

            m_userControl_GameStateOnInitialLoad = new UserControl_GameStateOnInitialLoad(gameStateOnInitialLoadArea, gameStateOnInitialLoadRoom, gameStateOnInitialLoadTime, false);
            grid_main.SetGridRowColumn(m_userControl_GameStateOnInitialLoad, 1, 0);
            m_userControl_GameStateOnInitialLoad.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_UpdateGameStateOnInitialLoad_Click(object sender, RoutedEventArgs e)
        {
            Window_GameStateOnInitialLoad window =
                new Window_GameStateOnInitialLoad
                (
                    m_userControl_GameStateOnInitialLoad.GameStateOnInitialLoadArea, 
                    m_userControl_GameStateOnInitialLoad.GameStateOnInitialLoadRoom,
                    m_userControl_GameStateOnInitialLoad.GameStateOnInitialLoadTime,
                    (win) =>
                    {
                        Window_GameStateOnInitialLoad wWin = win as Window_GameStateOnInitialLoad;
                        if (wWin != null)
                            GinTubBuilderManager.UpdateGameStateOnInitialLoad(wWin.GameStateOnInitialLoadArea.Value, wWin.GameStateOnInitialLoadRoom.Value, wWin.GameStateOnInitialLoadTime.Value);
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
