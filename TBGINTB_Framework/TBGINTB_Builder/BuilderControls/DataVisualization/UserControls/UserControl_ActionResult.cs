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
    public class UserControl_ActionResult : UserControl
    {
        #region MEMBER FIELDS

        ComboBox_Result m_comboBox_result;
        ComboBox_Action m_comboBox_action;

        #endregion


        #region MEMBER PROPERTIES

        public int? ActionResultId { get; private set; }
        public int? ActionResultResult { get; private set; }
        public int? ActionResultAction { get; private set; }
        private int NounId { get; set; }
        private int ParagraphStateId { get; set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_comboBox_result,
                    m_comboBox_action
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ActionResult(int? actionResultId, int? actionResultResult, int? actionResultAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            ActionResultId = actionResultId;
            ActionResultResult = actionResultResult;
            ActionResultAction = actionResultAction;
            NounId = nounId;
            ParagraphStateId = paragraphStateId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;

            GinTubBuilderManager.ResultAdded += GinTubBuilderManager_ResultAdded;
            GinTubBuilderManager.ActionAdded += GinTubBuilderManager_ActionAdded;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ActionResultModified += GinTubBuilderManager_ActionResultModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ActionResultModified -= GinTubBuilderManager_ActionResultModified;
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
                    Text = (ActionResultId.HasValue) ? ActionResultId.ToString() : "NewActionResult"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Result Grid
            Grid grid_result = new Grid();
            grid_result.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_result.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_result, 1, 0);

            ////////
            // Result
            m_comboBox_result = new ComboBox_Result();
            m_comboBox_result.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_result.SelectionChanged += ComboBox_Result_SelectionChanged;
            Label label_result = new Label() { Content = "Result:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_result.SetGridRowColumn(m_comboBox_result, 1, 0);
            grid_result.SetGridRowColumn(label_result, 0, 0);

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

        void GinTubBuilderManager_ActionResultModified(object sender, GinTubBuilderManager.ActionResultModifiedEventArgs args)
        {
            if(ActionResultId == args.Id)
            {
                SetActionResultResult(args.Result);
                SetActionResultAction(args.Action);
            }
        }

        void GinTubBuilderManager_ResultAdded(object sender, GinTubBuilderManager.ResultAddedEventArgs args)
        {
            ResetActionResultResult(args.Id);
        }
        
        void GinTubBuilderManager_ActionAdded(object sender, GinTubBuilderManager.ActionAddedEventArgs args)
        {
            if (NounId == args.Noun)
                ResetActionResultAction(args.Id);
        }

        private void SetActionResultResult(int actionResultResult)
        {
            ComboBox_Result.ComboBoxItem_Result item =
                m_comboBox_result.Items.OfType<ComboBox_Result.ComboBoxItem_Result>().
                SingleOrDefault(i => i.ResultId == actionResultResult);
            if (item != null)
                m_comboBox_result.SelectedItem = item;
        }

        private void SetActionResultAction(int actionResultAction)
        {
            ComboBox_Action.ComboBoxItem_Action item = m_comboBox_action.Items.OfType<ComboBox_Action.ComboBoxItem_Action>().
                SingleOrDefault(i => i.ActionId == actionResultAction);
            if (item != null)
                m_comboBox_action.SelectedItem = item;
        }

        private void ResetActionResultResult(int actionResultResult)
        {
            ComboBox_Result.ComboBoxItem_Result item =
                m_comboBox_result.Items.OfType<ComboBox_Result.ComboBoxItem_Result>().
                SingleOrDefault(i => ActionResultResult.HasValue && ActionResultResult.Value == actionResultResult && i.ResultId == actionResultResult);
            if (item != null)
                m_comboBox_result.SelectedItem = item;
        }

        private void ResetActionResultAction(int actionResultAction)
        {
            ComboBox_Action.ComboBoxItem_Action item = m_comboBox_action.Items.OfType<ComboBox_Action.ComboBoxItem_Action>().
                SingleOrDefault(i => ActionResultAction.HasValue && ActionResultAction.Value == actionResultAction && i.ActionId == actionResultAction);
            if (item != null)
                m_comboBox_action.SelectedItem = item;
        }

        void ComboBox_Result_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Result.ComboBoxItem_Result item;
            if (m_comboBox_result.SelectedItem != null && (item = m_comboBox_result.SelectedItem as ComboBox_Result.ComboBoxItem_Result) != null)
                ActionResultResult = item.ResultId;
        }

        void ComboBox_Action_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Action.ComboBoxItem_Action item;
            if (m_comboBox_action.SelectedItem != null && (item = m_comboBox_action.SelectedItem as ComboBox_Action.ComboBoxItem_Action) != null)
                ActionResultAction = item.ActionId;
        }

        #endregion

        #endregion
    }
}
