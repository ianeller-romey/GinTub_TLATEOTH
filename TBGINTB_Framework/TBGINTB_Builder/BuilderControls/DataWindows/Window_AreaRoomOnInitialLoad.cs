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
    public class Window_AreaRoomOnInitialLoad : Window_TaskOnAccept
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

        public Window_AreaRoomOnInitialLoad(int? areaRoomOnInitialLoadArea, int? areaRoomOnInitialLoadRoom, TaskOnAccept task) :
            base("AreaRoomOnInitialLoad Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(areaRoomOnInitialLoadArea, areaRoomOnInitialLoadRoom);
            m_userControl_areaRoomOnInitialLoad.SetActiveAndRegisterForGinTubEvents(); // needed for possible areaRoomOnInitialLoads
            GinTubBuilderManager.LoadAllAreas();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? areaRoomOnInitialLoadArea, int? areaRoomOnInitialLoadRoom)
        {
            m_userControl_areaRoomOnInitialLoad = new UserControl_AreaRoomOnInitialLoad(areaRoomOnInitialLoadArea, areaRoomOnInitialLoadRoom, true);
            return m_userControl_areaRoomOnInitialLoad;
        }

        #endregion

        #endregion
    }
}
