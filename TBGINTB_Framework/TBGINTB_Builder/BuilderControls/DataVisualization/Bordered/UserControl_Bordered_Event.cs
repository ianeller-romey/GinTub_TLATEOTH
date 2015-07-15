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
    public class UserControl_Bordered_Event : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_Bordered_Event(int? evntId, string evntName, string evntDescription, bool enableEditing)
        {
            CreateControls(evntId, evntName, evntDescription, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_evnt.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_evnt.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? evntId, string evntName, string evntDescription, bool enableEditing)
        {
            m_userControl_evnt = new UserControl_Event(evntId, evntName, evntDescription, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_evnt };
            Content = border;
        }

        #endregion

        #endregion
    }
}
