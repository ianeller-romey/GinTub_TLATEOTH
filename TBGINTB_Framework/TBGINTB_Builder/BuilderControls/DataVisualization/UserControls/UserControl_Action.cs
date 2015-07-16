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
    public class UserControl_Action : UserControl_Selecttable
    {
        #region MEMBER FIELDS

        ComboBox_VerbType m_comboBox_verbType;
        ComboBox_Noun m_comboBox_noun;

        #endregion


        #region MEMBER PROPERTIES

        public int? ActionId { get; private set; }
        public int? ActionVerbType { get; private set; }
        public int? ActionNoun { get; private set; }
        private int ParagraphStateId { get; set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_comboBox_verbType,
                    m_comboBox_noun
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Action(int? actionId, int? actionVerbType, int? actionNoun, int paragraphStateId, bool enableEditing, bool enableSelectting)
        {
            ActionId = actionId;
            ActionVerbType = actionVerbType;
            ActionNoun = actionNoun;
            ParagraphStateId = paragraphStateId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
            if(enableSelectting)
                MouseLeftButtonDown += UserControl_Action_MouseLeftButtonDown;

            GinTubBuilderManager.VerbTypeRead += GinTubBuilderManager_VerbTypeRead;
            GinTubBuilderManager.NounRead += GinTubBuilderManager_NounRead;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ActionUpdated += GinTubBuilderManager_ActionUpdated;
            GinTubBuilderManager.ActionSelect += GinTubBuilderManager_ActionSelect;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ActionUpdated -= GinTubBuilderManager_ActionUpdated;
            GinTubBuilderManager.ActionSelect -= GinTubBuilderManager_ActionSelect;
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
                    Text = (ActionId.HasValue) ? ActionId.ToString() : "NewAction"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // VerbType Grid
            Grid grid_verbType = new Grid();
            grid_verbType.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_verbType.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_verbType, 1, 0);

            ////////
            // VerbType
            m_comboBox_verbType = new ComboBox_VerbType();
            m_comboBox_verbType.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_verbType.SelectionChanged += ComboBox_VerbType_SelectionChanged;
            Label label_verbType = new Label() { Content = "Verb Type:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_verbType.SetGridRowColumn(m_comboBox_verbType, 1, 0);
            grid_verbType.SetGridRowColumn(label_verbType, 0, 0);

            ////////
            // Noun Grid
            Grid grid_noun = new Grid();
            grid_noun.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_noun.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_noun, 2, 0);

            ////////
            // Noun
            m_comboBox_noun = new ComboBox_Noun(ParagraphStateId);
            m_comboBox_noun.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_noun.SelectionChanged += ComboBox_Noun_SelectionChanged;
            Label label_noun = new Label() { Content = "Noun:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_noun.SetGridRowColumn(m_comboBox_noun, 1, 0);
            grid_noun.SetGridRowColumn(label_noun, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_ActionUpdated(object sender, GinTubBuilderManager.ActionUpdatedEventArgs args)
        {
            if(ActionId == args.Id)
            {
                SetActionVerbType(args.VerbType);
                SetActionNoun(args.Noun);
            }
        }

        private void GinTubBuilderManager_ActionSelect(object sender, GinTubBuilderManager.ActionSelectEventArgs args)
        {
            SetSelecttableBackground(ActionId == args.Id);
        }

        private void GinTubBuilderManager_VerbTypeRead(object sender, GinTubBuilderManager.VerbTypeReadEventArgs args)
        {
            ResetActionVerbType(args.Id);
        }

        private void GinTubBuilderManager_NounRead(object sender, GinTubBuilderManager.NounReadEventArgs args)
        {
            if (ParagraphStateId == args.ParagraphState)
                ResetActionNoun(args.Id);
        }

        private void SetActionVerbType(int actionVerbType)
        {
            ComboBox_VerbType.ComboBoxItem_VerbType item =
                m_comboBox_verbType.Items.OfType<ComboBox_VerbType.ComboBoxItem_VerbType>().
                SingleOrDefault(i => i.VerbTypeId == actionVerbType);
            if (item != null)
                m_comboBox_verbType.SelectedItem = item;
        }

        private void SetActionNoun(int actionNoun)
        {
            ComboBox_Noun.ComboBoxItem_Noun item = m_comboBox_noun.Items.OfType<ComboBox_Noun.ComboBoxItem_Noun>().
                SingleOrDefault(i => i.NounId == actionNoun);
            if (item != null)
                m_comboBox_noun.SelectedItem = item;
        }

        private void ResetActionVerbType(int actionVerbType)
        {
            ComboBox_VerbType.ComboBoxItem_VerbType item =
                m_comboBox_verbType.Items.OfType<ComboBox_VerbType.ComboBoxItem_VerbType>().
                SingleOrDefault(i => ActionVerbType.HasValue && ActionVerbType.Value == actionVerbType && i.VerbTypeId == actionVerbType);
            if (item != null)
                m_comboBox_verbType.SelectedItem = item;
        }

        private void ResetActionNoun(int actionNoun)
        {
            ComboBox_Noun.ComboBoxItem_Noun item = m_comboBox_noun.Items.OfType<ComboBox_Noun.ComboBoxItem_Noun>().
                SingleOrDefault(i => ActionNoun.HasValue && ActionNoun.Value == actionNoun && i.NounId == actionNoun);
            if (item != null)
                m_comboBox_noun.SelectedItem = item;
        }

        private void ComboBox_VerbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_VerbType.ComboBoxItem_VerbType item;
            if (m_comboBox_verbType.SelectedItem != null && (item = m_comboBox_verbType.SelectedItem as ComboBox_VerbType.ComboBoxItem_VerbType) != null)
                ActionVerbType = item.VerbTypeId;
        }

        private void ComboBox_Noun_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Noun.ComboBoxItem_Noun item;
            if (m_comboBox_noun.SelectedItem != null && (item = m_comboBox_noun.SelectedItem as ComboBox_Noun.ComboBoxItem_Noun) != null)
                ActionNoun = item.NounId;
        }

        private void UserControl_Action_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ActionId.HasValue)
                GinTubBuilderManager.SelectAction(ActionId.Value);
        }

        #endregion

        #endregion
    }
}
