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
    public class Window_Character : Window_TaskOnAccept
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

        public Window_Character(int? characterId, string characterName, string characterDescription, TaskOnAccept task) :
            base("Character Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(characterId, characterName, characterDescription);
            m_userControl_character.SetActiveAndRegisterForGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? characterId, string characterName, string characterDescription)
        {
            m_userControl_character = new UserControl_Character(characterId, characterName, characterDescription, true);
            return m_userControl_character;
        }

        #endregion

        #endregion
    }
}
