using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class Window_GameStateOnInitialLoad : Window_TaskOnAccept
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

        public Window_GameStateOnInitialLoad(int? gameStateOnInitialLoadArea, int? gameStateOnInitialLoadRoom, TimeSpan? gameStateOnInitialLoadTime, TaskOnAccept task) :
            base("GameStateOnInitialLoad Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(gameStateOnInitialLoadArea, gameStateOnInitialLoadRoom, gameStateOnInitialLoadTime);
            m_userControl_GameStateOnInitialLoad.SetActiveAndRegisterForGinTubEvents(); // needed for possible GameStateOnInitialLoads
            GinTubBuilderManager.ReadAllAreas();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? gameStateOnInitialLoadArea, int? gameStateOnInitialLoadRoom, TimeSpan? gameStateOnInitialLoadTime)
        {
            m_userControl_GameStateOnInitialLoad = new UserControl_GameStateOnInitialLoad(gameStateOnInitialLoadArea, gameStateOnInitialLoadRoom, gameStateOnInitialLoadTime, true);
            return m_userControl_GameStateOnInitialLoad;
        }

        #endregion

        #endregion
    }
}
