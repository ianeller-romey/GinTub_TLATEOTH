using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class TabItem_Events : TabItem, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        Grid m_grid_main;
        ComboBox_Event m_comboBox_event;
        UserControl_EventModification m_grid_event;

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public TabItem_Events()
        {
            Header = "Events";
            Content = CreateControls();

            GinTubBuilderManager.LoadAllEvents();
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            if (m_grid_event != null)
                m_grid_event.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            if (m_grid_event != null)
                m_grid_event.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls()
        {
            m_grid_main = new Grid();
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });

            m_comboBox_event = new ComboBox_Event();
            m_comboBox_event.SetActiveAndRegisterForGinTubEvents();
            m_comboBox_event.SelectionChanged += ComboBox_Event_SelectionChanged;
            m_grid_main.SetGridRowColumn(m_comboBox_event, 0, 0);

            return m_grid_main;
        }

        private void ComboBox_Event_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Event comboBox = sender as ComboBox_Event;
            ComboBox_Event.ComboBoxItem_Event comboBoxItem;
            if (comboBox.SelectedItem != null && (comboBoxItem = comboBox.SelectedItem as ComboBox_Event.ComboBoxItem_Event) != null)
            {
                if (m_grid_event != null)
                    m_grid_main.Children.Remove(m_grid_event);
                m_grid_event = new UserControl_EventModification(comboBoxItem.EventId, comboBoxItem.EventName, comboBoxItem.EventDescription);
                m_grid_event.SetActiveAndRegisterForGinTubEvents();
                m_grid_main.SetGridRowColumn(m_grid_event, 1, 0);
            }
        }

        #endregion

        #endregion

    }
}
