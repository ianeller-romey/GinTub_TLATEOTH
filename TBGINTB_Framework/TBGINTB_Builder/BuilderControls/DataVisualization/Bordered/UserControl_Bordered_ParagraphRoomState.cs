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
    public class UserControl_Bordered_ParagraphRoomState : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_ParagraphRoomState m_userControl_paragraphRoomState;

        #endregion


        #region MEMBER PROPERTIES

        public int? ParagraphRoomStateId { get { return m_userControl_paragraphRoomState.ParagraphRoomStateId; } }
        public int ParagraphRoomStateRoomState { get { return m_userControl_paragraphRoomState.ParagraphRoomStateRoomState; } }
        public string ParagraphRoomStateRoomStateName { get { return m_userControl_paragraphRoomState.ParagraphRoomStateRoomStateName; } }
        public TimeSpan ParagraphRoomStateRoomStateTime { get { return m_userControl_paragraphRoomState.ParagraphRoomStateRoomStateTime; } }
        public int? ParagraphRoomStateParagraph { get { return m_userControl_paragraphRoomState.ParagraphRoomStateParagraph; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_ParagraphRoomState
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
            CreateControls
            (
                paragraphRoomStateId,
                paragraphRoomStateParagraph,
                paragraphRoomStateParagraphToCheck,
                paragraphRoomStateRoomState,
                paragraphRoomStateRoomStateName,
                paragraphRoomStateRoomStateTime,
                enableEditing
            );
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_paragraphRoomState.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_paragraphRoomState.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls
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
            m_userControl_paragraphRoomState =
                new UserControl_ParagraphRoomState
                (
                    paragraphRoomStateId,
                    paragraphRoomStateParagraph,
                    paragraphRoomStateParagraphToCheck,
                    paragraphRoomStateRoomState,
                    paragraphRoomStateRoomStateName,
                    paragraphRoomStateRoomStateTime,
                    enableEditing
                );
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_paragraphRoomState };
            Content = border;
        }

        #endregion

        #endregion
    }
}
