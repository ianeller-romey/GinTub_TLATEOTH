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
    public class Window_Noun : Window_TaskOnAccept
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

        public Window_Noun(int? nounId, string nounText, int paragraphStateId, TaskOnAccept task) :
            base("Noun Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(nounId, nounText, paragraphStateId);
            m_userControl_noun.SetActiveAndRegisterForGinTubEvents(); // needed for possible nouns
            GinTubBuilderManager.LoadParagraphStateNounPossibilities(paragraphStateId);
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? nounId, string nounText, int paragraphStateId)
        {
            m_userControl_noun = new UserControl_Noun(nounId, nounText, paragraphStateId, true);
            return m_userControl_noun;
        }

        #endregion

        #endregion
    }
}
