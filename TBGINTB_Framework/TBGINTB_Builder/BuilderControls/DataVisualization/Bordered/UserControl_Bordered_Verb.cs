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
    public class UserControl_Bordered_Verb : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_Bordered_Verb(int? verbId, string verbName, int verbTypeId, bool enableEditing)
        {
            CreateControls(verbId, verbName, verbTypeId, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_verb.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_verb.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? verbId, string verbName, int verbTypeId, bool enableEditing)
        {
            m_userControl_verb = new UserControl_Verb(verbId, verbName, verbTypeId, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_verb };
            Content = border;
        }

        #endregion

        #endregion
    }
}
