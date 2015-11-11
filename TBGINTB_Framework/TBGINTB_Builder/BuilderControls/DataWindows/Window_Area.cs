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
    public class Window_Area : Window_TaskOnAccept
    {
        #region MEMBER FIELDS

        UserControl_Area m_userControl_area;

        #endregion


        #region MEMBER PROPERTIES

        public int? AreaId { get { return m_userControl_area.AreaId; } }
        public string AreaName { get { return m_userControl_area.AreaName; } }
        public bool AreaDisplayTime { get { return m_userControl_area.AreaDisplayTime; } }
        public int? AreaAudio { get { return m_userControl_area.AreaAudio; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_Area(int? areaId, string areaName, bool areaDisplayTime, int? areaAudio, TaskOnAccept task) :
            base("Area Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(areaId, areaName, areaDisplayTime, areaAudio);
            m_userControl_area.SetActiveAndRegisterForGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? areaId, string areaName, bool areaDisplayTime, int? areaAudio)
        {
            m_userControl_area = new UserControl_Area(areaId, areaName, areaDisplayTime, areaAudio, true);
            return m_userControl_area;
        }

        #endregion

        #endregion
    }
}
