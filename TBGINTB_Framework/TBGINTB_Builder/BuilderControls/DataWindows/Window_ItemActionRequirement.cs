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
    public class Window_ItemActionRequirement : Window_TaskOnAccept
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

        public Window_ItemActionRequirement(int? itemActionRequirementId, int? itemActionRequirementItem, int? itemActionRequirementAction, int nounId, int paragraphStateId, TaskOnAccept task) :
            base("Item Action Requirement Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(itemActionRequirementId, itemActionRequirementItem, itemActionRequirementAction, nounId, paragraphStateId);
            m_userControl_itemActionRequirement.SetActiveAndRegisterForGinTubEvents(); // needed for possible items, actions
            GinTubBuilderManager.LoadAllItems();
            GinTubBuilderManager.LoadAllActionsForNoun(nounId);
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? itemActionRequirementId, int? itemActionRequirementItem, int? itemActionRequirementAction, int nounId, int paragraphStateId)
        {
            m_userControl_itemActionRequirement = new UserControl_ItemActionRequirement(itemActionRequirementId, itemActionRequirementItem, itemActionRequirementAction, nounId, paragraphStateId, true);
            return m_userControl_itemActionRequirement;
        }

        #endregion

        #endregion
    }
}
