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
    public class UserControl_EventActionRequirementModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_EventActionRequirement m_userControl_actionEventActionRequirement;

        #endregion


        #region MEMBER PROPERTIES

        public int? EventActionRequirementId { get { return m_userControl_actionEventActionRequirement.EventActionRequirementId; } }
        public int? EventActionRequirementEvent { get { return m_userControl_actionEventActionRequirement.EventActionRequirementEvent; } }
        public int? EventActionRequirementAction { get { return m_userControl_actionEventActionRequirement.EventActionRequirementAction; } }
        private int NounId { get; set; }
        private int ParagraphStateId { get; set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_EventActionRequirementModification(int? actionEventActionRequirementId, int evntActionRequirementEvent, int evntActionRequirementAction, int nounId, int paragraphStateId)
        {
            NounId = nounId;
            ParagraphStateId = paragraphStateId;
            CreateControls(actionEventActionRequirementId, evntActionRequirementEvent, evntActionRequirementAction);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_actionEventActionRequirement.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_actionEventActionRequirement.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? actionEventActionRequirementId, int evntActionRequirementEvent, int evntActionRequirementAction)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyEventActionRequirement = new Button() { Content = "Modify EventActionRequirement" };
            button_modifyEventActionRequirement.Click += Button_ModifyEventActionRequirement_Click;
            grid_main.SetGridRowColumn(button_modifyEventActionRequirement, 0, 0);

            m_userControl_actionEventActionRequirement = new UserControl_EventActionRequirement(actionEventActionRequirementId, evntActionRequirementEvent, evntActionRequirementAction, NounId, ParagraphStateId, false);
            grid_main.SetGridRowColumn(m_userControl_actionEventActionRequirement, 1, 0);
            m_userControl_actionEventActionRequirement.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_ModifyEventActionRequirement_Click(object sender, RoutedEventArgs e)
        {
            Window_EventActionRequirement window =
                new Window_EventActionRequirement
                (
                    m_userControl_actionEventActionRequirement.EventActionRequirementId,
                    m_userControl_actionEventActionRequirement.EventActionRequirementEvent,
                    m_userControl_actionEventActionRequirement.EventActionRequirementAction,
                    NounId,
                    ParagraphStateId,
                    (win) =>
                    {
                        Window_EventActionRequirement wWin = win as Window_EventActionRequirement;
                        if (wWin != null)
                            GinTubBuilderManager.ModifyEventActionRequirement
                            (
                                wWin.EventActionRequirementId.Value,
                                wWin.EventActionRequirementEvent.Value,
                                wWin.EventActionRequirementAction.Value
                            );
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
