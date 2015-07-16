using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class ComboBox_Event : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxEvent_newEvent = new ComboBoxItem() { Content = "New Event ..." };

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_Event : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int EventId { get; private set; }
            public string EventName { get; private set; }
            public string EventDescription { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Event(int evntId, string evntName, string evntDescription)
            {
                EventId = evntId;
                SetEventName(evntName);
                SetEventDescription(evntDescription);
            }

            public void SetEventName(string evntName)
            {
                EventName = evntName;
                Content = EventName;
            }

            public void SetEventDescription(string evntDescription)
            {
                EventDescription = evntDescription;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_Event()
        {
            Items.Add(c_comboBoxEvent_newEvent);

            SelectionChanged += ComboBox_Event_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.EventRead += GinTubBuilderManager_EventRead;
            GinTubBuilderManager.EventUpdated += GinTubBuilderManager_EventUpdated;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.EventRead -= GinTubBuilderManager_EventRead;
            GinTubBuilderManager.EventUpdated -= GinTubBuilderManager_EventUpdated;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_EventRead(object sender, GinTubBuilderManager.EventReadEventArgs args)
        {
            if (!Items.OfType<ComboBoxItem_Event>().Any(i => i.EventId == args.Id))
                Items.Add(new ComboBoxItem_Event(args.Id, args.Name, args.Description));
        }

        private void GinTubBuilderManager_EventUpdated(object sender, GinTubBuilderManager.EventUpdatedEventArgs args)
        {
            ComboBoxItem_Event evnt = Items.OfType<ComboBoxItem_Event>().SingleOrDefault(i => i.EventId == args.Id);
            if (evnt != null)
            {
                evnt.SetEventName(args.Name);
                evnt.SetEventDescription(args.Description);
            }
        }

        private void NewEventDialog()
        {
            Window_Event window = 
                new Window_Event
                (
                    null, 
                    null,
                    null,
                    (win) =>
                    {
                        Window_Event wWin = win as Window_Event;
                        if (wWin != null)
                            GinTubBuilderManager.CreateEvent(wWin.EventName, wWin.EventDescription);
                    }
                );
            window.Show();
        }

        private void ComboBox_Event_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem evnt = null;
            if ((evnt = SelectedItem as ComboBoxItem) != null)
            {
                if (evnt == c_comboBoxEvent_newEvent)
                    NewEventDialog();
            }
        }

        #endregion

        #endregion
    }
}
