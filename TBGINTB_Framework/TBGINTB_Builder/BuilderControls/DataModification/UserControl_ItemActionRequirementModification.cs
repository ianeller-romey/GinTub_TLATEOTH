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
    public class UserControl_ItemActionRequirementModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_ItemActionRequirement m_userControl_actionItemActionRequirement;

        #endregion


        #region MEMBER PROPERTIES

        public int? ItemActionRequirementId { get { return m_userControl_actionItemActionRequirement.ItemActionRequirementId; } }
        public int? ItemActionRequirementItem { get { return m_userControl_actionItemActionRequirement.ItemActionRequirementItem; } }
        public int? ItemActionRequirementAction { get { return m_userControl_actionItemActionRequirement.ItemActionRequirementAction; } }
        private int NounId { get; set; }
        private int ParagraphStateId { get; set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ItemActionRequirementModification(int? actionItemActionRequirementId, int itemActionRequirementItem, int itemActionRequirementAction, int nounId, int paragraphStateId)
        {
            NounId = nounId;
            ParagraphStateId = paragraphStateId;
            CreateControls(actionItemActionRequirementId, itemActionRequirementItem, itemActionRequirementAction);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_actionItemActionRequirement.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_actionItemActionRequirement.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? actionItemActionRequirementId, int itemActionRequirementItem, int itemActionRequirementAction)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyItemActionRequirement = new Button() { Content = "Modify ItemActionRequirement" };
            button_modifyItemActionRequirement.Click += Button_UpdateItemActionRequirement_Click;
            grid_main.SetGridRowColumn(button_modifyItemActionRequirement, 0, 0);

            m_userControl_actionItemActionRequirement = new UserControl_ItemActionRequirement(actionItemActionRequirementId, itemActionRequirementItem, itemActionRequirementAction, NounId, ParagraphStateId, false);
            grid_main.SetGridRowColumn(m_userControl_actionItemActionRequirement, 1, 0);
            m_userControl_actionItemActionRequirement.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_UpdateItemActionRequirement_Click(object sender, RoutedEventArgs e)
        {
            Window_ItemActionRequirement window =
                new Window_ItemActionRequirement
                (
                    m_userControl_actionItemActionRequirement.ItemActionRequirementId,
                    m_userControl_actionItemActionRequirement.ItemActionRequirementItem,
                    m_userControl_actionItemActionRequirement.ItemActionRequirementAction,
                    NounId,
                    ParagraphStateId,
                    (win) =>
                    {
                        Window_ItemActionRequirement wWin = win as Window_ItemActionRequirement;
                        if (wWin != null)
                            GinTubBuilderManager.UpdateItemActionRequirement
                            (
                                wWin.ItemActionRequirementId.Value,
                                wWin.ItemActionRequirementItem.Value,
                                wWin.ItemActionRequirementAction.Value
                            );
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
