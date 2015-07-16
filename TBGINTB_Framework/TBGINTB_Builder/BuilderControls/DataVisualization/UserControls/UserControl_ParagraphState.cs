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
    public class UserControl_ParagraphState : UserControl_Selecttable
    {
        #region MEMBER FIELDS

        TextBox m_textBox_text;
        TextBlock m_textBlock_state;

        #endregion


        #region MEMBER PROPERTIES

        public int? ParagraphStateId { get; private set; }
        public string ParagraphStateText { get; private set; }
        public int? ParagraphStateState { get; private set; }
        public int ParagraphId { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_text,
                    m_textBlock_state
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ParagraphState(int? paragraphStateId, string paragraphStateText, int? paragraphStateState, int paragraphId, bool enableEditing, bool enableSelectting)
        {
            ParagraphStateId = paragraphStateId;
            ParagraphStateText = paragraphStateText;
            ParagraphStateState = paragraphStateState;
            ParagraphId = paragraphId;
            
            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;

            if (enableSelectting)
                MouseLeftButtonDown += Grid_ParagraphStateData_MouseLeftButtonDown;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ParagraphStateUpdated += GinTubBuilderManager_ParagraphStateUpdated;
            GinTubBuilderManager.ParagraphStateSelect += GinTubBuilderManager_ParagraphStateSelect;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ParagraphStateUpdated -= GinTubBuilderManager_ParagraphStateUpdated;
            GinTubBuilderManager.ParagraphStateSelect -= GinTubBuilderManager_ParagraphStateSelect;
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
                    Text = (ParagraphStateId.HasValue) ? ParagraphStateId.ToString() : "NewParagraphState"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Text Grid
            Grid grid_text = new Grid();
            grid_text.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_text.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_text, 1, 0);

            ////////
            // Text
            m_textBox_text = 
                new TextBox() 
                { 
                    VerticalAlignment = VerticalAlignment.Center, 
                    TextWrapping = System.Windows.TextWrapping.Wrap, 
                    Text = ParagraphStateText 
                };
            m_textBox_text.TextChanged += TextBox_Text_TextChanged;
            Label label_text = new Label() { Content = "Text:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_text.SetGridRowColumn(m_textBox_text, 1, 0);
            grid_text.SetGridRowColumn(label_text, 0, 0);
            
            ////////
            // State Grid
            Grid grid_state = new Grid();
            grid_state.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_state.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_state, 2, 0);
            
            ////////
            // State
            m_textBlock_state = new TextBlock() { VerticalAlignment = VerticalAlignment.Center };
            SetParagraphStateState(ParagraphStateState);
            Label label_state = new Label() { Content = "State:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_state.SetGridRowColumn(m_textBlock_state, 0, 1);
            grid_state.SetGridRowColumn(label_state, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_ParagraphStateUpdated(object sender, GinTubBuilderManager.ParagraphStateUpdatedEventArgs args)
        {
            if (ParagraphStateId == args.Id)
            {
                SetParagraphStateText(args.Text);
                SetParagraphStateState(args.State);
                ParagraphId = args.Paragraph;
            }
        }

        private void GinTubBuilderManager_ParagraphStateSelect(object sender, GinTubBuilderManager.ParagraphStateSelectEventArgs args)
        {
            SetSelecttableBackground(ParagraphStateId == args.Id);
        }

        private void SetParagraphStateState(int? paragraphStateState)
        {
            m_textBlock_state.Text = (paragraphStateState.HasValue) ? paragraphStateState.ToString() : "NewState";
        }

        private void SetParagraphStateText(string paragraphStateText)
        {
            m_textBox_text.Text = paragraphStateText;
            if (!m_textBox_text.IsEnabled)
                TextBox_Text_TextChanged(m_textBox_text, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        void TextBox_Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_text)
                ParagraphStateText = m_textBox_text.Text;
        }

        private void Grid_ParagraphStateData_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ParagraphStateId.HasValue)
                GinTubBuilderManager.SelectParagraphState(ParagraphStateId.Value);
        }

        #endregion

        #endregion
    }
}
