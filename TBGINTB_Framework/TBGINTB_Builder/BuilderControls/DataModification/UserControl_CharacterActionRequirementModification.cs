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
    public class UserControl_CharacterActionRequirementModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_CharacterActionRequirement m_userControl_actionCharacterActionRequirement;

        #endregion


        #region MEMBER PROPERTIES

        public int? CharacterActionRequirementId { get { return m_userControl_actionCharacterActionRequirement.CharacterActionRequirementId; } }
        public int? CharacterActionRequirementCharacter { get { return m_userControl_actionCharacterActionRequirement.CharacterActionRequirementCharacter; } }
        public int? CharacterActionRequirementAction { get { return m_userControl_actionCharacterActionRequirement.CharacterActionRequirementAction; } }
        private int NounId { get; set; }
        private int ParagraphStateId { get; set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_CharacterActionRequirementModification(int? actionCharacterActionRequirementId, int characterActionRequirementCharacter, int characterActionRequirementAction, int nounId, int paragraphStateId)
        {
            NounId = nounId;
            ParagraphStateId = paragraphStateId;
            CreateControls(actionCharacterActionRequirementId, characterActionRequirementCharacter, characterActionRequirementAction);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_actionCharacterActionRequirement.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_actionCharacterActionRequirement.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? actionCharacterActionRequirementId, int characterActionRequirementCharacter, int characterActionRequirementAction)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyCharacterActionRequirement = new Button() { Content = "Modify CharacterActionRequirement" };
            button_modifyCharacterActionRequirement.Click += Button_ModifyCharacterActionRequirement_Click;
            grid_main.SetGridRowColumn(button_modifyCharacterActionRequirement, 0, 0);

            m_userControl_actionCharacterActionRequirement = new UserControl_CharacterActionRequirement(actionCharacterActionRequirementId, characterActionRequirementCharacter, characterActionRequirementAction, NounId, ParagraphStateId, false);
            grid_main.SetGridRowColumn(m_userControl_actionCharacterActionRequirement, 1, 0);
            m_userControl_actionCharacterActionRequirement.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_ModifyCharacterActionRequirement_Click(object sender, RoutedEventArgs e)
        {
            Window_CharacterActionRequirement window =
                new Window_CharacterActionRequirement
                (
                    m_userControl_actionCharacterActionRequirement.CharacterActionRequirementId,
                    m_userControl_actionCharacterActionRequirement.CharacterActionRequirementCharacter,
                    m_userControl_actionCharacterActionRequirement.CharacterActionRequirementAction,
                    NounId,
                    ParagraphStateId,
                    (win) =>
                    {
                        Window_CharacterActionRequirement wWin = win as Window_CharacterActionRequirement;
                        if (wWin != null)
                            GinTubBuilderManager.ModifyCharacterActionRequirement
                            (
                                wWin.CharacterActionRequirementId.Value,
                                wWin.CharacterActionRequirementCharacter.Value,
                                wWin.CharacterActionRequirementAction.Value
                            );
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
