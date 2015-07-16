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
    public class Window_Action : Window_TaskOnAccept
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

        public Window_Action(int? actionId, int? actionVerbType, int? actionNoun, int paragraphStateId, TaskOnAccept task) :
            base("Action Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(actionId, actionVerbType, actionNoun, paragraphStateId);
            m_userControl_action.SetActiveAndRegisterForGinTubEvents(); // needed for possible nouns
            GinTubBuilderManager.ReadAllVerbTypes();
            GinTubBuilderManager.ReadAllNounsForParagraphState(paragraphStateId);
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? actionId, int? actionVerbType, int? actionNoun, int paragraphStateId)
        {
            m_userControl_action = new UserControl_Action(actionId, actionVerbType, actionNoun, paragraphStateId, true, false);
            return m_userControl_action;
        }

        #endregion

        #endregion
    }
}
