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
    public class UserControl_ParagraphRoomState : UserControl
    {
        #region MEMBER FIELDS

        UserControl_RoomStateNameAndTime m_userControl_roomStateName;
        CheckBox m_checkBox_paragraphState;

        #endregion


        #region MEMBER PROPERTIES

        public int? ParagraphRoomStateId { get; private set; }
        public int? ParagraphRoomStateParagraph { get; private set; }
        public int ParagraphRoomStateRoomState { get { return m_userControl_roomStateName.RoomStateId; } }
        public string ParagraphRoomStateRoomStateName { get { return m_userControl_roomStateName.RoomStateName; } }
        public TimeSpan ParagraphRoomStateRoomStateTime { get { return m_userControl_roomStateName.RoomStateTime; } }
        private int ParagraphRoomStateParagraphToCheck { get; set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_checkBox_paragraphState
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ParagraphRoomState
        (
            int? paragraphRoomStateId, 
            int? paragraphRoomStateParagraph, 
            int paragraphRoomStateParagraphToCheck, 
            int paragraphRoomStateRoomState, 
            string paragraphRoomStateRoomStateName,
            TimeSpan paragraphRoomStateRoomStateTime,
            bool enableEditing
        )
        {
            ParagraphRoomStateId = paragraphRoomStateId;
            ParagraphRoomStateParagraph = paragraphRoomStateParagraph;
            ParagraphRoomStateParagraphToCheck = paragraphRoomStateParagraphToCheck;

            CreateControls(paragraphRoomStateRoomState, paragraphRoomStateRoomStateName, paragraphRoomStateRoomStateTime);

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int paragraphRoomStateRoomState, string paragraphRoomStateRoomStateName, TimeSpan paragraphRoomStateRoomStateTime)
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
                    Text = (ParagraphRoomStateId.HasValue) ? ParagraphRoomStateId.ToString() : "NewParagraphRoomState"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // RoomState
            m_userControl_roomStateName = new UserControl_RoomStateNameAndTime(paragraphRoomStateRoomState, paragraphRoomStateRoomStateName, paragraphRoomStateRoomStateTime);
            m_userControl_roomStateName.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            grid_main.SetGridRowColumn(m_userControl_roomStateName, 1, 0);

            ////////
            // Paragraph Grid
            Grid grid_paragraph = new Grid();
            grid_paragraph.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_paragraph.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_paragraph, 2, 0);

            ////////
            // Paragraph
            m_checkBox_paragraphState = new CheckBox() { IsChecked = ParagraphRoomStateParagraph.HasValue, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            m_checkBox_paragraphState.Checked += CheckBox_Paragraph_Checked;
            Label label_paragraph = new Label() { Content = "Use Paragraph?", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_paragraph.SetGridRowColumn(m_checkBox_paragraphState, 0, 1);
            grid_paragraph.SetGridRowColumn(label_paragraph, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        private void CheckBox_Paragraph_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if(checkBox != null && checkBox == m_checkBox_paragraphState)
                ParagraphRoomStateParagraph = (m_checkBox_paragraphState.IsChecked.HasValue && m_checkBox_paragraphState.IsChecked.Value) ? (int?)ParagraphRoomStateParagraphToCheck : null;
        }

        #endregion

        #endregion
    }
}
