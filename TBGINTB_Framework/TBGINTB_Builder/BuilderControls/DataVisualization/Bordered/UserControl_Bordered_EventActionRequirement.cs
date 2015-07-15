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
    public class UserControl_Bordered_EventActionRequirement : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_EventActionRequirement m_userControl_evntActionRequirement;

        #endregion


        #region MEMBER PROPERTIES

        public int? EventActionRequirementId { get { return m_userControl_evntActionRequirement.EventActionRequirementId; } }
        public int? EventActionRequirementEvent { get { return m_userControl_evntActionRequirement.EventActionRequirementEvent; } }
        public int? EventActionRequirementAction { get { return m_userControl_evntActionRequirement.EventActionRequirementAction; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_EventActionRequirement(int? evntActionRequirementId, int? evntActionRequirementEvent, int? evntActionRequirementAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            CreateControls(evntActionRequirementId, evntActionRequirementEvent, evntActionRequirementAction, nounId, paragraphStateId, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_evntActionRequirement.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_evntActionRequirement.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? evntActionRequirementId, int? evntActionRequirementEvent, int? evntActionRequirementAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            m_userControl_evntActionRequirement = new UserControl_EventActionRequirement(evntActionRequirementId, evntActionRequirementEvent, evntActionRequirementAction, nounId, paragraphStateId, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_evntActionRequirement };
            Content = border;
        }

        #endregion

        #endregion
    }
}
