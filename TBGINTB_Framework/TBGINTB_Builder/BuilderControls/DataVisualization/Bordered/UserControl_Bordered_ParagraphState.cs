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
    public class UserControl_Bordered_ParagraphState : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_ParagraphState m_userControl_paragraphState;

        #endregion


        #region MEMBER PROPERTIES

        public int? ParagraphStateId { get { return m_userControl_paragraphState.ParagraphStateId; } }
        public string ParagraphStateText { get { return m_userControl_paragraphState.ParagraphStateText; } }
        public int? ParagraphStateState { get { return m_userControl_paragraphState.ParagraphStateState; } }
        public int ParagraphId { get { return m_userControl_paragraphState.ParagraphId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_ParagraphState(int? paragraphStateId, string paragraphStateText, int? paragraphStateState, int paragraphId, bool enableEditing, bool enableGetting)
        {
            CreateControls(paragraphStateId, paragraphStateText, paragraphStateState, paragraphId, enableEditing, enableGetting);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_paragraphState.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_paragraphState.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? paragraphStateId, string paragraphStateText, int? paragraphStateState, int paragraphId, bool enableEditing, bool enableGetting)
        {
            m_userControl_paragraphState = new UserControl_ParagraphState(paragraphStateId, paragraphStateText, paragraphStateState, paragraphId, enableEditing, enableGetting);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_paragraphState };
            Content = border;
        }

        #endregion

        #endregion
    }
}
