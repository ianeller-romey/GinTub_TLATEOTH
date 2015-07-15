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
    public class UserControl_Bordered_AreaRoomOnInitialLoad : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_Bordered_AreaRoomOnInitialLoad(int? areaRoomOnInitialLoadArea, int? areaRoomOnInitialLoadRoom, bool enableEditing)
        {
            CreateControls(areaRoomOnInitialLoadArea, areaRoomOnInitialLoadRoom, enableEditing);
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

        private void CreateControls(int? areaRoomOnInitialLoadId, int? areaRoomOnInitialLoadRoom, bool enableEditing)
        {
            m_userControl_areaRoomOnInitialLoad = new UserControl_AreaRoomOnInitialLoad(areaRoomOnInitialLoadId, areaRoomOnInitialLoadRoom, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_areaRoomOnInitialLoad };
            Content = border;
        }

        #endregion

        #endregion
    }
}
