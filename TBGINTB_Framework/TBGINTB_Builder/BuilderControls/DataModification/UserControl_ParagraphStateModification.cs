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
    public class UserControl_ParagraphStateModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_ParagraphStateModification(int? paragraphStateId, string paragraphStateText, int? paragraphStateState, int paragraphId)
        {
           CreateControls(paragraphStateId, paragraphStateText, paragraphStateState, paragraphId);
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

        private void CreateControls(int? paragraphStateId, string paragraphStateText, int? paragraphStateState, int paragraphId)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyParagraphState = new Button() { Content = "Modify Paragraph State" };
            button_modifyParagraphState.Click += Button_ModifyParagraphState_Click;
            grid_main.SetGridRowColumn(button_modifyParagraphState, 0, 0);

            m_userControl_paragraphState = new UserControl_ParagraphState(paragraphStateId, paragraphStateText, paragraphStateState, paragraphId, false, true);
            Border border_paragraphState = new Border() { Style = new Style_DefaultBorder() };
            border_paragraphState.Child = m_userControl_paragraphState;
            grid_main.SetGridRowColumn(border_paragraphState, 1, 0);
            m_userControl_paragraphState.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_ModifyParagraphState_Click(object sender, RoutedEventArgs e)
        {
            Window_ParagraphState window = 
                new Window_ParagraphState
                (
                    m_userControl_paragraphState.ParagraphStateId,
                    m_userControl_paragraphState.ParagraphStateText,
                    m_userControl_paragraphState.ParagraphStateState,
                    m_userControl_paragraphState.ParagraphId,
                    (win) =>
                    {
                        Window_ParagraphState wWin = win as Window_ParagraphState;
                        if (wWin != null)
                            GinTubBuilderManager.ModifyParagraphState
                            (
                                wWin.ParagraphStateId.Value,
                                wWin.ParagraphStateText,
                                wWin.ParagraphStateState.Value,
                                wWin.ParagraphId
                            );
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
