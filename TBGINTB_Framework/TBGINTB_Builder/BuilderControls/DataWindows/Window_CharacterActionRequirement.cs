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
    public class Window_CharacterActionRequirement : Window_TaskOnAccept
    {
        #region MEMBER FIELDS

        UserControl_CharacterActionRequirement m_userControl_characterActionRequirement;

        #endregion


        #region MEMBER PROPERTIES

        public int? CharacterActionRequirementId { get { return m_userControl_characterActionRequirement.CharacterActionRequirementId; } }
        public int? CharacterActionRequirementCharacter { get { return m_userControl_characterActionRequirement.CharacterActionRequirementCharacter; } }
        public int? CharacterActionRequirementAction { get { return m_userControl_characterActionRequirement.CharacterActionRequirementAction; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_CharacterActionRequirement(int? characterActionRequirementId, int? characterActionRequirementCharacter, int? characterActionRequirementAction, int nounId, int paragraphStateId, TaskOnAccept task) :
            base("Character Action Requirement Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(characterActionRequirementId, characterActionRequirementCharacter, characterActionRequirementAction, nounId, paragraphStateId);
            m_userControl_characterActionRequirement.SetActiveAndRegisterForGinTubEvents(); // needed for possible characters, actions
            GinTubBuilderManager.LoadAllCharacters();
            GinTubBuilderManager.LoadAllActionsForNoun(nounId);
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? characterActionRequirementId, int? characterActionRequirementCharacter, int? characterActionRequirementAction, int nounId, int paragraphStateId)
        {
            m_userControl_characterActionRequirement = new UserControl_CharacterActionRequirement(characterActionRequirementId, characterActionRequirementCharacter, characterActionRequirementAction, nounId, paragraphStateId, true);
            return m_userControl_characterActionRequirement;
        }

        #endregion

        #endregion
    }
}
