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
    public class UserControl_Bordered_Action : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Action m_userControl_action;

        #endregion


        #region MEMBER PROPERTIES

        public int? ActionId { get { return m_userControl_action.ActionId; } }
        public int? ActionVerbType { get { return m_userControl_action.ActionVerbType; } }
        public int? ActionNoun { get { return m_userControl_action.ActionNoun; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_Action(int? actionId, int? actionVerbType, int? actionNoun, int paragraphStateId, bool enableEditing, bool enableSelectting)
        {
            CreateControls(actionId, actionVerbType, actionNoun, paragraphStateId, enableEditing, enableSelectting);
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

        private void CreateControls(int? actionId, int? actionVerbType, int? actionNoun, int paragraphStateId, bool enableEditing, bool enableSelectting)
        {
            m_userControl_action = new UserControl_Action(actionId, actionVerbType, actionNoun, paragraphStateId, enableEditing, enableSelectting);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_action };
            Content = border;
        }

        #endregion

        #endregion
    }
}
