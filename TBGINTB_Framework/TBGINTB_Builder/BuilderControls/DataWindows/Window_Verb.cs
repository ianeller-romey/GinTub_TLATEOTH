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
    public class Window_Verb : Window_TaskOnAccept
    {
        #region MEMBER FIELDS

        UserControl_Verb m_userControl_verb;

        #endregion


        #region MEMBER PROPERTIES

        public int? VerbId { get { return m_userControl_verb.VerbId; } }
        public string VerbName { get { return m_userControl_verb.VerbName; } }
        public int VerbTypeId { get { return m_userControl_verb.VerbTypeId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_Verb(int? verbId, string verbName, int verbTypeId, TaskOnAccept task) :
            base("Verb Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(verbId, verbName, verbTypeId);
            m_userControl_verb.SetActiveAndRegisterForGinTubEvents(); // needed for possible nouns
            GinTubBuilderManager.LoadAllVerbTypes();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? verbId, string verbName, int verbTypeId)
        {
            m_userControl_verb = new UserControl_Verb(verbId, verbName, verbTypeId, true);
            return m_userControl_verb;
        }

        #endregion

        #endregion
    }
}
