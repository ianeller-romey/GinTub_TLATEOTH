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
    public class UserControl_Bordered_CharacterActionRequirement : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_Bordered_CharacterActionRequirement(int? characterActionRequirementId, int? characterActionRequirementCharacter, int? characterActionRequirementAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            CreateControls(characterActionRequirementId, characterActionRequirementCharacter, characterActionRequirementAction, nounId, paragraphStateId, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_characterActionRequirement.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_characterActionRequirement.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? characterActionRequirementId, int? characterActionRequirementCharacter, int? characterActionRequirementAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            m_userControl_characterActionRequirement = new UserControl_CharacterActionRequirement(characterActionRequirementId, characterActionRequirementCharacter, characterActionRequirementAction, nounId, paragraphStateId, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_characterActionRequirement };
            Content = border;
        }

        #endregion

        #endregion
    }
}
