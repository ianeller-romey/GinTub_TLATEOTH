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
    public class Window_Event : Window_TaskOnAccept
    {
        #region MEMBER FIELDS

        UserControl_Event m_userControl_evnt;

        #endregion


        #region MEMBER PROPERTIES

        public int? EventId { get { return m_userControl_evnt.EventId; } }
        public string EventName { get { return m_userControl_evnt.EventName; } }
        public string EventDescription { get { return m_userControl_evnt.EventDescription; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_Event(int? evntId, string evntName, string evntDescription, TaskOnAccept task) :
            base("Event Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(evntId, evntName, evntDescription);
            m_userControl_evnt.SetActiveAndRegisterForGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? evntId, string evntName, string evntDescription)
        {
            m_userControl_evnt = new UserControl_Event(evntId, evntName, evntDescription, true);
            return m_userControl_evnt;
        }

        #endregion

        #endregion
    }
}
