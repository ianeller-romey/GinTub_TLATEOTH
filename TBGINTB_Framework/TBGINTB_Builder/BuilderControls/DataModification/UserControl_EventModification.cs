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
    public class UserControl_EventModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_EventModification(int? evntId, string evntName, string evntDescription)
        {
            CreateControls(evntId, evntName, evntDescription);
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

        private void CreateControls(int? evntId, string evntName, string evntDescription)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyEvent = new Button() { Content = "Modify Event" };
            button_modifyEvent.Click += Button_UpdateEvent_Click;
            grid_main.SetGridRowColumn(button_modifyEvent, 0, 0);

            m_userControl_evnt = new UserControl_Event(evntId, evntName, evntDescription, false);
            grid_main.SetGridRowColumn(m_userControl_evnt, 1, 0);
            m_userControl_evnt.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_UpdateEvent_Click(object sender, RoutedEventArgs e)
        {
            Window_Event window =
                new Window_Event
                (
                    m_userControl_evnt.EventId, 
                    m_userControl_evnt.EventName,
                    m_userControl_evnt.EventDescription,
                    (win) =>
                    {
                        Window_Event wWin = win as Window_Event;
                        if (wWin != null)
                            GinTubBuilderManager.UpdateEvent(wWin.EventId.Value, wWin.EventName, wWin.EventDescription);
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
