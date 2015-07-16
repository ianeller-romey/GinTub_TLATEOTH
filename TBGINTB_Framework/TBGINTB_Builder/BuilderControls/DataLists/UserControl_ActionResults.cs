using System;
using System.Collections.Generic;
using System.IO;
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
    public class UserControl_ActionResults : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly Button c_button_newActionResult = new Button() { Content = "New Action Result ..." };

        private StackPanel m_stackPanel_actionResults;

        #endregion


        #region MEMBER PROPERTIES

        public int ActionId { get; set; }
        public int NounId { get; set; }
        public int ParagraphStateId { get; set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ActionResults(int actionId, int nounId, int paragraphStateId)
        {
            ActionId = actionId;
            NounId = nounId;
            ParagraphStateId = paragraphStateId;

            CreateControls();

            c_button_newActionResult.Click += Button_NewActionResult_Click;
        }
    
        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ActionResultRead += GinTubBuilderManager_ActionResultRead;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ActionResultRead -= GinTubBuilderManager_ActionResultRead;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid userControl_main = new Grid();
            userControl_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            userControl_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });

            userControl_main.SetGridRowColumn(c_button_newActionResult, 0, 0);

            m_stackPanel_actionResults = new StackPanel() { Orientation = Orientation.Vertical };
            userControl_main.SetGridRowColumn(m_stackPanel_actionResults, 1, 0);

            Content = userControl_main;
        }

        private void GinTubBuilderManager_ActionResultRead(object sender, GinTubBuilderManager.ActionResultReadEventArgs args)
        {
            if (ActionId == args.Action && !m_stackPanel_actionResults.Children.OfType<UserControl_ActionResultModification>().Any(i => i.ActionResultId == args.Id))
            {
                UserControl_ActionResultModification grid = new UserControl_ActionResultModification(args.Id, args.Result, args.Action, NounId, ParagraphStateId);
                grid.SetActiveAndRegisterForGinTubEvents();
                m_stackPanel_actionResults.Children.Add(grid);
                GinTubBuilderManager.ReadAllResultsForActionResultType(args.Action);
                GinTubBuilderManager.ReadAllActionsForNoun(NounId);
            }
        }

        private void NewActionResultDialog()
        {
            Window_ResultType window_resultType = 
                new Window_ResultType
                (
                    null,
                    null,
                    (win) =>
                    {
                        Window_ResultType wWin = win as Window_ResultType;
                        if (wWin != null)
                        {
                            Window_ActionResult window_actionResult = 
                                new Window_ActionResult
                                (
                                    null, 
                                    null, 
                                    ActionId, 
                                    NounId, 
                                    ParagraphStateId,
                                    wWin.ResultTypeId.Value,
                                    (wwWin) =>
                                    {
                                        Window_ActionResult wwwWin = wwWin as Window_ActionResult;
                                        if (wwwWin != null)
                                            GinTubBuilderManager.CreateActionResult
                                            (
                                                wwwWin.ActionResultResult.Value,
                                                wwwWin.ActionResultAction.Value
                                            );
                                    }
                                );
                            window_actionResult.Show();
                        }
                    }
                );
            window_resultType.Show();
        }
        
        private void Button_NewActionResult_Click(object sender, RoutedEventArgs e)
        {
            Button item = null;
            if ((item = sender as Button) != null && item == c_button_newActionResult)
                NewActionResultDialog();
        }

        #endregion

        #endregion
    }
}
