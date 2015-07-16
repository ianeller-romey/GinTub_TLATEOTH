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
    public class UserControl_VerbModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_VerbModification(int? verbId, string verbName, int verbTypeId)
        {
            CreateControls(verbId, verbName, verbTypeId);
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

        private void CreateControls(int? verbId, string verbName, int verbTypeId)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyVerb = new Button() { Content = "Modify Verb" };
            button_modifyVerb.Click += Button_UpdateVerb_Click;
            grid_main.SetGridRowColumn(button_modifyVerb, 0, 0);

            m_userControl_verb = new UserControl_Verb(verbId, verbName, verbTypeId, false);
            grid_main.SetGridRowColumn(m_userControl_verb, 1, 0);
            m_userControl_verb.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_UpdateVerb_Click(object sender, RoutedEventArgs e)
        {
            Window_Verb window =
                new Window_Verb
                (
                    m_userControl_verb.VerbId,
                    m_userControl_verb.VerbName,
                    m_userControl_verb.VerbTypeId,
                    (win) =>
                    {
                        Window_Verb wWin = win as Window_Verb;
                        if (wWin != null)
                            GinTubBuilderManager.UpdateVerb(wWin.VerbId.Value, wWin.VerbName, wWin.VerbTypeId);
                    }
                );
            window.Show();                
        }

        #endregion

        #endregion
    }
}
