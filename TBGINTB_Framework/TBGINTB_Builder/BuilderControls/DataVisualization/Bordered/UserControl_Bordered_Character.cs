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
    public class UserControl_Bordered_Character : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Character m_userControl_character;

        #endregion


        #region MEMBER PROPERTIES

        public int? CharacterId { get { return m_userControl_character.CharacterId; } }
        public string CharacterName { get { return m_userControl_character.CharacterName; } }
        public string CharacterDescription { get { return m_userControl_character.CharacterDescription; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_Character(int? characterId, string characterName, string characterDescription, bool enableEditing)
        {
            CreateControls(characterId, characterName, characterDescription, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_character.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_character.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? characterId, string characterName, string characterDescription, bool enableEditing)
        {
            m_userControl_character = new UserControl_Character(characterId, characterName, characterDescription, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_character };
            Content = border;
        }

        #endregion

        #endregion
    }
}
