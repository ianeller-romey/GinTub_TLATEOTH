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
    public class UserControl_MessageChoiceResult : UserControl
    {
        #region MEMBER FIELDS

        ComboBox_Result m_comboBox_result;
        ComboBox_MessageChoice m_comboBox_messageChoice;

        #endregion


        #region MEMBER PROPERTIES

        public int? MessageChoiceResultId { get; private set; }
        public int? MessageChoiceResultResult { get; private set; }
        public int? MessageChoiceResultMessageChoice { get; private set; }
        private int MessageId { get; set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_comboBox_result,
                    m_comboBox_messageChoice
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_MessageChoiceResult(int? messageChoiceResultId, int? messageChoiceResultResult, int? messageChoiceResultMessageChoice, int messageId, bool enableEditing)
        {
            MessageChoiceResultId = messageChoiceResultId;
            MessageChoiceResultResult = messageChoiceResultResult;
            MessageChoiceResultMessageChoice = messageChoiceResultMessageChoice;
            MessageId = messageId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;

            GinTubBuilderManager.ResultAdded += GinTubBuilderManager_ResultAdded;
            GinTubBuilderManager.MessageChoiceAdded += GinTubBuilderManager_MessageChoiceAdded;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.MessageChoiceResultModified += GinTubBuilderManager_MessageChoiceResultModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.MessageChoiceResultModified -= GinTubBuilderManager_MessageChoiceResultModified;
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
                    Text = (MessageChoiceResultId.HasValue) ? MessageChoiceResultId.ToString() : "NewMessageChoiceResult"
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
            // MessageChoice Grid
            Grid grid_messageChoice = new Grid();
            grid_messageChoice.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_messageChoice.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_messageChoice, 2, 0);

            ////////
            // MessageChoice
            m_comboBox_messageChoice = new ComboBox_MessageChoice(MessageId);
            m_comboBox_messageChoice.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_messageChoice.SelectionChanged += ComboBox_MessageChoice_SelectionChanged;
            Label label_messageChoice = new Label() { Content = "MessageChoice:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_messageChoice.SetGridRowColumn(m_comboBox_messageChoice, 1, 0);
            grid_messageChoice.SetGridRowColumn(label_messageChoice, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        void GinTubBuilderManager_MessageChoiceResultModified(object sender, GinTubBuilderManager.MessageChoiceResultModifiedEventArgs args)
        {
            if(MessageChoiceResultId == args.Id)
            {
                SetMessageChoiceResultResult(args.Result);
                SetMessageChoiceResultMessageChoice(args.MessageChoice);
            }
        }

        void GinTubBuilderManager_ResultAdded(object sender, GinTubBuilderManager.ResultAddedEventArgs args)
        {
            ResetMessageChoiceResultResult(args.Id);
        }
        
        void GinTubBuilderManager_MessageChoiceAdded(object sender, GinTubBuilderManager.MessageChoiceAddedEventArgs args)
        {
            if (MessageId == args.Message)
                ResetMessageChoiceResultMessageChoice(args.Id);
        }

        private void SetMessageChoiceResultResult(int messageChoiceResultResult)
        {
            ComboBox_Result.ComboBoxItem_Result item =
                m_comboBox_result.Items.OfType<ComboBox_Result.ComboBoxItem_Result>().
                SingleOrDefault(i => i.ResultId == messageChoiceResultResult);
            if (item != null)
                m_comboBox_result.SelectedItem = item;
        }

        private void SetMessageChoiceResultMessageChoice(int messageChoiceResultMessageChoice)
        {
            ComboBox_MessageChoice.ComboBoxItem_MessageChoice item = m_comboBox_messageChoice.Items.OfType<ComboBox_MessageChoice.ComboBoxItem_MessageChoice>().
                SingleOrDefault(i => i.MessageChoiceId == messageChoiceResultMessageChoice);
            if (item != null)
                m_comboBox_messageChoice.SelectedItem = item;
        }

        private void ResetMessageChoiceResultResult(int messageChoiceResultResult)
        {
            ComboBox_Result.ComboBoxItem_Result item =
                m_comboBox_result.Items.OfType<ComboBox_Result.ComboBoxItem_Result>().
                SingleOrDefault(i => MessageChoiceResultResult.HasValue && MessageChoiceResultResult.Value == messageChoiceResultResult && i.ResultId == messageChoiceResultResult);
            if (item != null)
                m_comboBox_result.SelectedItem = item;
        }

        private void ResetMessageChoiceResultMessageChoice(int messageChoiceResultMessageChoice)
        {
            ComboBox_MessageChoice.ComboBoxItem_MessageChoice item = m_comboBox_messageChoice.Items.OfType<ComboBox_MessageChoice.ComboBoxItem_MessageChoice>().
                SingleOrDefault(i => MessageChoiceResultMessageChoice.HasValue && MessageChoiceResultMessageChoice.Value == messageChoiceResultMessageChoice && i.MessageChoiceId == messageChoiceResultMessageChoice);
            if (item != null)
                m_comboBox_messageChoice.SelectedItem = item;
        }

        void ComboBox_Result_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Result.ComboBoxItem_Result item;
            if (m_comboBox_result.SelectedItem != null && (item = m_comboBox_result.SelectedItem as ComboBox_Result.ComboBoxItem_Result) != null)
                MessageChoiceResultResult = item.ResultId;
        }

        void ComboBox_MessageChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_MessageChoice.ComboBoxItem_MessageChoice item;
            if (m_comboBox_messageChoice.SelectedItem != null && (item = m_comboBox_messageChoice.SelectedItem as ComboBox_MessageChoice.ComboBoxItem_MessageChoice) != null)
                MessageChoiceResultMessageChoice = item.MessageChoiceId;
        }

        #endregion

        #endregion
    }
}
