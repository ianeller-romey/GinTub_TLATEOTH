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
    public class UserControl_Bordered_ActionResult : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_Bordered_ActionResult(int? actionResultId, int? actionResultResult, int? actionResultAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            CreateControls(actionResultId, actionResultResult, actionResultAction, nounId, paragraphStateId, enableEditing);
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

        private void CreateControls(int? actionResultId, int? actionResultResult, int? actionResultAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            m_userControl_actionResult = new UserControl_ActionResult(actionResultId, actionResultResult, actionResultAction, nounId, paragraphStateId, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_actionResult };
            Content = border;
        }

        #endregion

        #endregion
    }
}
