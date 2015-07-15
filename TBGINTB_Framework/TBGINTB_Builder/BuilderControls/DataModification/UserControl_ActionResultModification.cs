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
    public class UserControl_ActionResultModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_ActionResult m_userControl_actionResult;

        #endregion


        #region MEMBER PROPERTIES

        public int? ActionResultId { get { return m_userControl_actionResult.ActionResultId; } }
        public int? ActionResultResult { get { return m_userControl_actionResult.ActionResultResult; } }
        public int? ActionResultAction { get { return m_userControl_actionResult.ActionResultAction; } }
        private int NounId { get; set; }
        private int ParagraphStateId { get; set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ActionResultModification(int? actionResultId, int actionResultResult, int actionResultAction, int nounId, int paragraphStateId)
        {
            NounId = nounId;
            ParagraphStateId = paragraphStateId;
            CreateControls(actionResultId, actionResultResult, actionResultAction);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_actionResult.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_actionResult.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? actionResultId, int actionResultResult, int actionResultAction)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyActionResult = new Button() { Content = "Modify ActionResult" };
            button_modifyActionResult.Click += Button_ModifyActionResult_Click;
            grid_main.SetGridRowColumn(button_modifyActionResult, 0, 0);

            m_userControl_actionResult = new UserControl_ActionResult(actionResultId, actionResultResult, actionResultAction, NounId, ParagraphStateId, false);
            Border border_actionResult = new Border() { Style = new Style_DefaultBorder() };
            border_actionResult.Child = m_userControl_actionResult;
            grid_main.SetGridRowColumn(border_actionResult, 1, 0);
            m_userControl_actionResult.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_ModifyActionResult_Click(object sender, RoutedEventArgs e)
        {
            Window_ActionResult window =
                new Window_ActionResult
                (
                    m_userControl_actionResult.ActionResultId,
                    m_userControl_actionResult.ActionResultResult,
                    m_userControl_actionResult.ActionResultAction,
                    NounId,
                    ParagraphStateId,
                    (win) =>
                    {
                        Window_ActionResult wWin = win as Window_ActionResult;
                        if (wWin != null)
                            GinTubBuilderManager.ModifyActionResult
                            (
                                wWin.ActionResultId.Value,
                                wWin.ActionResultResult.Value,
                                wWin.ActionResultAction.Value
                            );
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
