using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_EventActionRequirement : UserControl
    {
        #region MEMBER FIELDS

        ComboBox_Event m_comboBox_evnt;
        ComboBox_Action m_comboBox_action;

        #endregion


        #region MEMBER PROPERTIES

        public int? EventActionRequirementId { get; private set; }
        public int? EventActionRequirementEvent { get; private set; }
        public int? EventActionRequirementAction { get; private set; }
        private int NounId { get; set; }
        private int ParagraphStateId { get; set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_comboBox_evnt,
                    m_comboBox_action
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_EventActionRequirement(int? evntActionRequirementId, int? evntActionRequirementEvent, int? evntActionRequirementAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            EventActionRequirementId = evntActionRequirementId;
            EventActionRequirementEvent = evntActionRequirementEvent;
            EventActionRequirementAction = evntActionRequirementAction;
            NounId = nounId;
            ParagraphStateId = paragraphStateId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;

            GinTubBuilderManager.EventAdded += GinTubBuilderManager_EventAdded;
            GinTubBuilderManager.ActionAdded += GinTubBuilderManager_ActionAdded;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.EventActionRequirementModified += GinTubBuilderManager_EventActionRequirementModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.EventActionRequirementModified -= GinTubBuilderManager_EventActionRequirementModified;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            
            ////////
            // Id Grid
            Grid grid_id = new Grid();
            grid_id.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_id.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_id, 0, 0);

            ////////
            // Id
            TextBlock textBlock_id =
                new TextBlock()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = (EventActionRequirementId.HasValue) ? EventActionRequirementId.ToString() : "NewEventActionRequirement"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Event Grid
            Grid grid_evnt = new Grid();
            grid_evnt.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_evnt.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_evnt, 1, 0);

            ////////
            // Event
            m_comboBox_evnt = new ComboBox_Event();
            m_comboBox_evnt.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_evnt.SelectionChanged += ComboBox_Event_SelectionChanged;
            Label label_evnt = new Label() { Content = "Event:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_evnt.SetGridRowColumn(m_comboBox_evnt, 1, 0);
            grid_evnt.SetGridRowColumn(label_evnt, 0, 0);

            ////////
            // Action Grid
            Grid grid_action = new Grid();
            grid_action.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_action.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_action, 2, 0);

            ////////
            // Action
            m_comboBox_action = new ComboBox_Action(NounId, ParagraphStateId);
            m_comboBox_action.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_action.SelectionChanged += ComboBox_Action_SelectionChanged;
            Label label_action = new Label() { Content = "Action:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_action.SetGridRowColumn(m_comboBox_action, 1, 0);
            grid_action.SetGridRowColumn(label_action, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        void GinTubBuilderManager_EventActionRequirementModified(object sender, GinTubBuilderManager.EventActionRequirementModifiedEventArgs args)
        {
            if(EventActionRequirementId == args.Id)
            {
                SetEventActionRequirementEvent(args.Event);
                SetEventActionRequirementAction(args.Action);
            }
        }

        void GinTubBuilderManager_EventAdded(object sender, GinTubBuilderManager.EventAddedEventArgs args)
        {
            ResetEventActionRequirementEvent(args.Id);
        }
        
        void GinTubBuilderManager_ActionAdded(object sender, GinTubBuilderManager.ActionAddedEventArgs args)
        {
            if (NounId == args.Noun)
                ResetEventActionRequirementAction(args.Id);
        }

        private void SetEventActionRequirementEvent(int evntActionRequirementEvent)
        {
            ComboBox_Event.ComboBoxItem_Event item =
                m_comboBox_evnt.Items.OfType<ComboBox_Event.ComboBoxItem_Event>().
                SingleOrDefault(i => i.EventId == evntActionRequirementEvent);
            if (item != null)
                m_comboBox_evnt.SelectedItem = item;
        }

        private void SetEventActionRequirementAction(int evntActionRequirementAction)
        {
            ComboBox_Action.ComboBoxItem_Action item = m_comboBox_action.Items.OfType<ComboBox_Action.ComboBoxItem_Action>().
                SingleOrDefault(i => i.ActionId == evntActionRequirementAction);
            if (item != null)
                m_comboBox_action.SelectedItem = item;
        }

        private void ResetEventActionRequirementEvent(int evntActionRequirementEvent)
        {
            ComboBox_Event.ComboBoxItem_Event item =
                m_comboBox_evnt.Items.OfType<ComboBox_Event.ComboBoxItem_Event>().
                SingleOrDefault(i => EventActionRequirementEvent.HasValue && EventActionRequirementEvent.Value == evntActionRequirementEvent && i.EventId == evntActionRequirementEvent);
            if (item != null)
                m_comboBox_evnt.SelectedItem = item;
        }

        private void ResetEventActionRequirementAction(int evntActionRequirementAction)
        {
            ComboBox_Action.ComboBoxItem_Action item = m_comboBox_action.Items.OfType<ComboBox_Action.ComboBoxItem_Action>().
                SingleOrDefault(i => EventActionRequirementAction.HasValue && EventActionRequirementAction.Value == evntActionRequirementAction && i.ActionId == evntActionRequirementAction);
            if (item != null)
                m_comboBox_action.SelectedItem = item;
        }

        void ComboBox_Event_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Event.ComboBoxItem_Event item;
            if (m_comboBox_evnt.SelectedItem != null && (item = m_comboBox_evnt.SelectedItem as ComboBox_Event.ComboBoxItem_Event) != null)
                EventActionRequirementEvent = item.EventId;
        }

        void ComboBox_Action_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Action.ComboBoxItem_Action item;
            if (m_comboBox_action.SelectedItem != null && (item = m_comboBox_action.SelectedItem as ComboBox_Action.ComboBoxItem_Action) != null)
                EventActionRequirementAction = item.ActionId;
        }

        #endregion

        #endregion
    }
}
