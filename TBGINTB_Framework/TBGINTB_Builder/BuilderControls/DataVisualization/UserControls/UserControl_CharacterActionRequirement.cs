using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_CharacterActionRequirement : UserControl
    {
        #region MEMBER FIELDS

        ComboBox_Character m_comboBox_character;
        ComboBox_Action m_comboBox_action;

        #endregion


        #region MEMBER PROPERTIES

        public int? CharacterActionRequirementId { get; private set; }
        public int? CharacterActionRequirementCharacter { get; private set; }
        public int? CharacterActionRequirementAction { get; private set; }
        private int NounId { get; set; }
        private int ParagraphStateId { get; set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_comboBox_character,
                    m_comboBox_action
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_CharacterActionRequirement(int? characterActionRequirementId, int? characterActionRequirementCharacter, int? characterActionRequirementAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            CharacterActionRequirementId = characterActionRequirementId;
            CharacterActionRequirementCharacter = characterActionRequirementCharacter;
            CharacterActionRequirementAction = characterActionRequirementAction;
            NounId = nounId;
            ParagraphStateId = paragraphStateId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;

            GinTubBuilderManager.CharacterAdded += GinTubBuilderManager_CharacterAdded;
            GinTubBuilderManager.ActionAdded += GinTubBuilderManager_ActionAdded;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.CharacterActionRequirementModified += GinTubBuilderManager_CharacterActionRequirementModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.CharacterActionRequirementModified -= GinTubBuilderManager_CharacterActionRequirementModified;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            
            ////////
            // Id Grid
            Grid grid_id = new Grid();
            grid_id.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_id.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_id, 0, 0);

            ////////
            // Id
            TextBlock textBlock_id =
                new TextBlock()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = (CharacterActionRequirementId.HasValue) ? CharacterActionRequirementId.ToString() : "NewCharacterActionRequirement"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Character Grid
            Grid grid_character = new Grid();
            grid_character.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_character.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_character, 1, 0);

            ////////
            // Character
            m_comboBox_character = new ComboBox_Character();
            m_comboBox_character.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_character.SelectionChanged += ComboBox_Character_SelectionChanged;
            Label label_character = new Label() { Content = "Character:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_character.SetGridRowColumn(m_comboBox_character, 1, 0);
            grid_character.SetGridRowColumn(label_character, 0, 0);

            ////////
            // Action Grid
            Grid grid_action = new Grid();
            grid_action.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_action.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_action, 2, 0);

            ////////
            // Action
            m_comboBox_action = new ComboBox_Action(NounId, ParagraphStateId);
            m_comboBox_action.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_action.SelectionChanged += ComboBox_Action_SelectionChanged;
            Label label_action = new Label() { Content = "Action:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_action.SetGridRowColumn(m_comboBox_action, 1, 0);
            grid_action.SetGridRowColumn(label_action, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        void GinTubBuilderManager_CharacterActionRequirementModified(object sender, GinTubBuilderManager.CharacterActionRequirementModifiedEventArgs args)
        {
            if(CharacterActionRequirementId == args.Id)
            {
                SetCharacterActionRequirementCharacter(args.Character);
                SetCharacterActionRequirementAction(args.Action);
            }
        }

        void GinTubBuilderManager_CharacterAdded(object sender, GinTubBuilderManager.CharacterAddedEventArgs args)
        {
            ResetCharacterActionRequirementCharacter(args.Id);
        }
        
        void GinTubBuilderManager_ActionAdded(object sender, GinTubBuilderManager.ActionAddedEventArgs args)
        {
            if (NounId == args.Noun)
                ResetCharacterActionRequirementAction(args.Id);
        }

        private void SetCharacterActionRequirementCharacter(int characterActionRequirementCharacter)
        {
            ComboBox_Character.ComboBoxItem_Character item =
                m_comboBox_character.Items.OfType<ComboBox_Character.ComboBoxItem_Character>().
                SingleOrDefault(i => i.CharacterId == characterActionRequirementCharacter);
            if (item != null)
                m_comboBox_character.SelectedItem = item;
        }

        private void SetCharacterActionRequirementAction(int characterActionRequirementAction)
        {
            ComboBox_Action.ComboBoxItem_Action item = m_comboBox_action.Items.OfType<ComboBox_Action.ComboBoxItem_Action>().
                SingleOrDefault(i => i.ActionId == characterActionRequirementAction);
            if (item != null)
                m_comboBox_action.SelectedItem = item;
        }

        private void ResetCharacterActionRequirementCharacter(int characterActionRequirementCharacter)
        {
            ComboBox_Character.ComboBoxItem_Character item =
                m_comboBox_character.Items.OfType<ComboBox_Character.ComboBoxItem_Character>().
                SingleOrDefault(i => CharacterActionRequirementCharacter.HasValue && CharacterActionRequirementCharacter.Value == characterActionRequirementCharacter && i.CharacterId == characterActionRequirementCharacter);
            if (item != null)
                m_comboBox_character.SelectedItem = item;
        }

        private void ResetCharacterActionRequirementAction(int characterActionRequirementAction)
        {
            ComboBox_Action.ComboBoxItem_Action item = m_comboBox_action.Items.OfType<ComboBox_Action.ComboBoxItem_Action>().
                SingleOrDefault(i => CharacterActionRequirementAction.HasValue && CharacterActionRequirementAction.Value == characterActionRequirementAction && i.ActionId == characterActionRequirementAction);
            if (item != null)
                m_comboBox_action.SelectedItem = item;
        }

        void ComboBox_Character_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Character.ComboBoxItem_Character item;
            if (m_comboBox_character.SelectedItem != null && (item = m_comboBox_character.SelectedItem as ComboBox_Character.ComboBoxItem_Character) != null)
                CharacterActionRequirementCharacter = item.CharacterId;
        }

        void ComboBox_Action_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Action.ComboBoxItem_Action item;
            if (m_comboBox_action.SelectedItem != null && (item = m_comboBox_action.SelectedItem as ComboBox_Action.ComboBoxItem_Action) != null)
                CharacterActionRequirementAction = item.ActionId;
        }

        #endregion

        #endregion
    }
}
