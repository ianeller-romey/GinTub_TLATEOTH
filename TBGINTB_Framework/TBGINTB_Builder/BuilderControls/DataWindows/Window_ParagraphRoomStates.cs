using System;
using System.Collections.Generic;
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
    public class Window_ParagraphRoomStates : Window_TaskOnAccept
    {
        #region MEMBER FIELDS

        UserControl_ParagraphRoomStates m_userControl_paragraphRoomStates;

        #endregion


        #region MEMBER PROPERTIES

        public int ParagraphId { get { return m_userControl_paragraphRoomStates.ParagraphId; } }
        public IEnumerable<int> RoomStates { get { return m_userControl_paragraphRoomStates.RoomStates; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_ParagraphRoomStates(int roomId, int paragraphId, TaskOnAccept task) :
            base("Paragraph RoomState Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(roomId, paragraphId);
            m_userControl_paragraphRoomStates.SetActiveAndRegisterForGinTubEvents(); // needed for possible results, actions
            GinTubBuilderManager.ReadAllParagraphRoomStatesForParagraph(paragraphId);
            GinTubBuilderManager.ReadAllRoomStatesForRoom(roomId);
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int roomId, int paragraphId)
        {
            m_userControl_paragraphRoomStates = new UserControl_ParagraphRoomStates(roomId, paragraphId);
            return m_userControl_paragraphRoomStates;
        }

        #endregion

        #endregion
    }
}
