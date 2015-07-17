using System;
using System.Collections.Generic;
using System.IO;
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
    public class UserControl_GroupedParagraphRoomStates : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private StackPanel m_stackPanel_paragraphRoomStates;

        #endregion


        #region MEMBER PROPERTIES

        public int RoomId { get; private set; }
        public int ParagraphId { get; private set; }
        public int RoomStateState { get; private set; }
        public IEnumerable<int> RoomStates
        {
            get
            {
                return m_stackPanel_paragraphRoomStates.Children.OfType<UserControl_Bordered_ParagraphRoomState>()
                    .Where(x => x.ParagraphRoomStateParagraph != null)
                    .Select(x => x.ParagraphRoomStateRoomState)
                    .ToList();
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_GroupedParagraphRoomStates(int roomId, int roomStateState, int paragraphId)
        {
            RoomId = roomId;
            RoomStateState = roomStateState;
            ParagraphId = paragraphId;

            CreateControls();
        }
    
        public void SetActiveAndRegisterForGinTubEvents()
        {
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
        }

        public void AddParagraphRoomState
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
            var userControl =
                new UserControl_Bordered_ParagraphRoomState
                (
                    paragraphRoomStateId,
                    paragraphRoomStateParagraph,
                    paragraphRoomStateParagraphToCheck,
                    paragraphRoomStateRoomState,
                    paragraphRoomStateRoomStateName,
                    paragraphRoomStateRoomStateTime,
                    enableEditing
                );
            var userControl_greater =
                    m_stackPanel_paragraphRoomStates.Children.OfType<UserControl_Bordered_ParagraphRoomState>()
                    .FirstOrDefault(x => x.ParagraphRoomStateRoomStateTime > userControl.ParagraphRoomStateRoomStateTime);
            if (userControl_greater != null)
                m_stackPanel_paragraphRoomStates.Children.Insert(m_stackPanel_paragraphRoomStates.Children.IndexOf(userControl_greater), userControl);
            else
                m_stackPanel_paragraphRoomStates.Children.Add(userControl);
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            m_stackPanel_paragraphRoomStates = new StackPanel() { Orientation = Orientation.Vertical };

            GroupBox groupBox_main = new GroupBox() { Header = string.Format("State: {0}", RoomStateState) };
            groupBox_main.Content = m_stackPanel_paragraphRoomStates;

            Content = groupBox_main;
        }

        #endregion

        #endregion
    }
}
