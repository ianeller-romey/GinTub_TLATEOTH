using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class ComboBox_Character : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxCharacter_newCharacter = new ComboBoxItem() { Content = "New Character ..." };

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_Character : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int CharacterId { get; private set; }
            public string CharacterName { get; private set; }
            public string CharacterDescription { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Character(int characterId, string characterName, string characterDescription)
            {
                CharacterId = characterId;
                SetCharacterName(characterName);
                SetCharacterDescription(characterDescription);
            }

            public void SetCharacterName(string characterName)
            {
                CharacterName = characterName;
                Content = CharacterName;
            }

            public void SetCharacterDescription(string characterDescription)
            {
                CharacterDescription = characterDescription;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_Character()
        {
            Items.Add(c_comboBoxCharacter_newCharacter);

            SelectionChanged += ComboBox_Character_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.CharacterAdded += GinTubBuilderManager_CharacterAdded;
            GinTubBuilderManager.CharacterModified += GinTubBuilderManager_CharacterModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.CharacterAdded -= GinTubBuilderManager_CharacterAdded;
            GinTubBuilderManager.CharacterModified -= GinTubBuilderManager_CharacterModified;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_CharacterAdded(object sender, GinTubBuilderManager.CharacterAddedEventArgs args)
        {
            if (!Items.OfType<ComboBoxItem_Character>().Any(i => i.CharacterId == args.Id))
                Items.Add(new ComboBoxItem_Character(args.Id, args.Name, args.Description));
        }

        private void GinTubBuilderManager_CharacterModified(object sender, GinTubBuilderManager.CharacterModifiedEventArgs args)
        {
            ComboBoxItem_Character character = Items.OfType<ComboBoxItem_Character>().SingleOrDefault(i => i.CharacterId == args.Id);
            if (character != null)
            {
                character.SetCharacterName(args.Name);
                character.SetCharacterDescription(args.Description);
            }
        }

        private void NewCharacterDialog()
        {
            Window_Character window = 
                new Window_Character
                (
                    null, 
                    null, 
                    null,
                    (win) =>
                    {
                        Window_Character wWin = win as Window_Character;
                        if (wWin != null)
                            GinTubBuilderManager.AddCharacter(wWin.CharacterName, wWin.CharacterDescription);
                    }
                );
            window.Show();
        }

        private void ComboBox_Character_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem character = null;
            if ((character = SelectedItem as ComboBoxItem) != null)
            {
                if (character == c_comboBoxCharacter_newCharacter)
                    NewCharacterDialog();
            }
        }

        #endregion

        #endregion
    }
}
