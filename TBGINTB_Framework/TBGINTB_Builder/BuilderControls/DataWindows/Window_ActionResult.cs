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
    public class Window_ActionResult : Window_TaskOnAccept
    {
        #region MEMBER FIELDS

        UserControl_ActionResult m_userControl_actionResult;

        #endregion


        #region MEMBER PROPERTIES

        public int? ActionResultId { get { return m_userControl_actionResult.ActionResultId; } }
        public int? ActionResultResult { get { return m_userControl_actionResult.ActionResultResult; } }
        public int? ActionResultAction { get { return m_userControl_actionResult.ActionResultAction; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_ActionResult(int? actionResultId, int? actionResultResult, int? actionResultAction, int nounId, int paragraphStateId, int resultTypeId, TaskOnAccept task) :
            base("Action Result Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(actionResultId, actionResultResult, actionResultAction, nounId, paragraphStateId);
            m_userControl_actionResult.SetActiveAndRegisterForGinTubEvents(); // needed for possible results, actions
            GinTubBuilderManager.ReadAllResultsForResultType(resultTypeId);
            GinTubBuilderManager.ReadAllActionsForNoun(nounId);
        }

        public Window_ActionResult(int? actionResultId, int? actionResultResult, int? actionResultAction, int nounId, int paragraphStateId, TaskOnAccept task) :
            base("Action Result Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(actionResultId, actionResultResult, actionResultAction, nounId, paragraphStateId);
            m_userControl_actionResult.SetActiveAndRegisterForGinTubEvents(); // needed for possible results, actions
            if(actionResultAction.HasValue)
                GinTubBuilderManager.ReadAllResultsForActionResultType(actionResultAction.Value);
            GinTubBuilderManager.ReadAllActionsForNoun(nounId);
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? actionResultId, int? actionResultResult, int? actionResultAction, int nounId, int paragraphStateId)
        {
            m_userControl_actionResult = new UserControl_ActionResult(actionResultId, actionResultResult, actionResultAction, nounId, paragraphStateId, true);
            return m_userControl_actionResult;
        }

        #endregion

        #endregion
    }
}
