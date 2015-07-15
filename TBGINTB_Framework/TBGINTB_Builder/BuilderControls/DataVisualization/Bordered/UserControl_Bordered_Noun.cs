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
    public class UserControl_Bordered_Noun : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Noun m_userControl_noun;

        #endregion


        #region MEMBER PROPERTIES

        public int? NounId { get { return m_userControl_noun.NounId; } }
        public string NounText { get { return m_userControl_noun.NounText; } }
        public int ParagraphStateId { get { return m_userControl_noun.ParagraphStateId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_Noun(int? nounId, string nounText, int paragraphStateId, bool enableEditing)
        {
            CreateControls(nounId, nounText, paragraphStateId, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_noun.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_noun.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? nounId, string nounText, int paragraphStateId, bool enableEditing)
        {
            m_userControl_noun = new UserControl_Noun(nounId, nounText, paragraphStateId, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_noun };
            Content = border;
        }

        #endregion

        #endregion
    }
}
