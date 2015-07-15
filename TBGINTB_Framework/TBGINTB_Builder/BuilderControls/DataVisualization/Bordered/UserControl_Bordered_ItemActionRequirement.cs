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
    public class UserControl_Bordered_ItemActionRequirement : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_ItemActionRequirement m_userControl_itemActionRequirement;

        #endregion


        #region MEMBER PROPERTIES

        public int? ItemActionRequirementId { get { return m_userControl_itemActionRequirement.ItemActionRequirementId; } }
        public int? ItemActionRequirementItem { get { return m_userControl_itemActionRequirement.ItemActionRequirementItem; } }
        public int? ItemActionRequirementAction { get { return m_userControl_itemActionRequirement.ItemActionRequirementAction; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_ItemActionRequirement(int? itemActionRequirementId, int? itemActionRequirementItem, int? itemActionRequirementAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            CreateControls(itemActionRequirementId, itemActionRequirementItem, itemActionRequirementAction, nounId, paragraphStateId, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_itemActionRequirement.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_itemActionRequirement.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? itemActionRequirementId, int? itemActionRequirementItem, int? itemActionRequirementAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            m_userControl_itemActionRequirement = new UserControl_ItemActionRequirement(itemActionRequirementId, itemActionRequirementItem, itemActionRequirementAction, nounId, paragraphStateId, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_itemActionRequirement };
            Content = border;
        }

        #endregion

        #endregion
    }
}
