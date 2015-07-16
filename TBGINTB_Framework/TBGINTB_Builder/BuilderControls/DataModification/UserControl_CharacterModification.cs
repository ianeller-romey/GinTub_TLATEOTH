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
    public class UserControl_CharacterModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Character m_grid_character;

        #endregion


        #region MEMBER PROPERTIES

        public int? CharactereId { get { return m_grid_character.CharacterId; } }
        public string CharacterName { get { return m_grid_character.CharacterName; } }
        public string CharacterDescription { get { return m_grid_character.CharacterDescription; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_CharacterModification(int? characterId, string characterName, string characterDescription)
        {
            CreateControls(characterId, characterName, characterDescription);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_grid_character.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_grid_character.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? characterId, string characterName, string characterDescription)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyCharacter = new Button() { Content = "Modify Character" };
            button_modifyCharacter.Click += Button_UpdateCharacter_Click;
            grid_main.SetGridRowColumn(button_modifyCharacter, 0, 0);

            m_grid_character = new UserControl_Character(characterId, characterName, characterDescription, false);
            grid_main.SetGridRowColumn(m_grid_character, 1, 0);
            m_grid_character.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_UpdateCharacter_Click(object sender, RoutedEventArgs e)
        {
            Window_Character window =
                new Window_Character
                (
                    m_grid_character.CharacterId, 
                    m_grid_character.CharacterName,
                    m_grid_character.CharacterDescription,
                    (win) =>
                    {
                        Window_Character wWin = win as Window_Character;
                        if (wWin != null)
                            GinTubBuilderManager.UpdateCharacter(wWin.CharacterId.Value, wWin.CharacterName, wWin.CharacterDescription);
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
