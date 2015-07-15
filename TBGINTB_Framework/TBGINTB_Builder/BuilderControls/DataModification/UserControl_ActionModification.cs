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
    public class UserControl_ActionModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Action m_userControl_action;

        #endregion


        #region MEMBER PROPERTIES

        public int? ActionId { get { return m_userControl_action.ActionId; } }
        public int? ActionVerbType { get { return m_userControl_action.ActionVerbType; } }
        public int? ActionNoun { get { return m_userControl_action.ActionNoun; } }
        private int ParagraphStateId { get; set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ActionModification(int? actionId, int? actionVerbType, int? actionNoun, int paragraphStateId)
        {
            ParagraphStateId = paragraphStateId;
            CreateControls(actionId, actionVerbType, actionNoun);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_action.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_action.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? actionId, int? actionVerbType, int? actionNoun)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyAction = new Button() { Content = "Modify Action" };
            button_modifyAction.Click += Button_ModifyAction_Click;
            grid_main.SetGridRowColumn(button_modifyAction, 0, 0);

            m_userControl_action = new UserControl_Action(actionId, actionVerbType, actionNoun, ParagraphStateId, false, true);
            Border border_action = new Border() { Style = new Style_DefaultBorder() };
            border_action.Child = m_userControl_action;
            grid_main.SetGridRowColumn(border_action, 1, 0);
            m_userControl_action.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_ModifyAction_Click(object sender, RoutedEventArgs e)
        {
            Window_Action window =
                new Window_Action
                (
                    m_userControl_action.ActionId, 
                    m_userControl_action.ActionVerbType, 
                    m_userControl_action.ActionNoun,
                    ParagraphStateId,
                    (win) =>
                    {
                        Window_Action wWin = win as Window_Action;
                        if (wWin != null)
                            GinTubBuilderManager.ModifyAction(wWin.ActionId.Value, wWin.ActionVerbType.Value, wWin.ActionNoun.Value);
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
