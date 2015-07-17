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
    public class UserControl_ParagraphRoomStates : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private StackPanel m_stackPanel_paragraphRoomStates;

        #endregion


        #region MEMBER PROPERTIES

        public int RoomId { get; private set; }
        public int ParagraphId { get; private set; }
        public IEnumerable<int> RoomStates
        {
            get
            {
                return m_stackPanel_paragraphRoomStates.Children.OfType<UserControl_ParagraphRoomState>()
                    .Where(x => x.ParagraphRoomStateParagraph != null)
                    .Select(x => x.ParagraphRoomStateRoomState)
                    .ToList();
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ParagraphRoomStates(int roomId, int paragraphId)
        {
            RoomId = roomId;
            ParagraphId = paragraphId;

            CreateControls();
        }
    
        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ParagraphRoomStateRead += GinTubBuilderManager_ParagraphRoomStateRead;

            GinTubBuilderManager.RoomStateRead += GinTubBuilderManager_RoomStateRead;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ParagraphRoomStateRead -= GinTubBuilderManager_ParagraphRoomStateRead;

            GinTubBuilderManager.RoomStateRead -= GinTubBuilderManager_RoomStateRead;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            m_stackPanel_paragraphRoomStates = new StackPanel() { Orientation = Orientation.Vertical };

            Content = m_stackPanel_paragraphRoomStates;
        }

        private void GinTubBuilderManager_ParagraphRoomStateRead(object sender, GinTubBuilderManager.ParagraphRoomStateReadEventArgs args)
        {
            if (ParagraphId == args.Paragraph && !m_stackPanel_paragraphRoomStates.Children.OfType<UserControl_ParagraphRoomState>().Any(i => i.ParagraphRoomStateId == args.Id))
            {
                UserControl_ParagraphRoomState grid = new UserControl_ParagraphRoomState(args.Id, args.Paragraph, args.Paragraph, args.RoomState, args.RoomStateName, true);
                grid.SetActiveAndRegisterForGinTubEvents();
                m_stackPanel_paragraphRoomStates.Children.Add(grid);
            }
        }

        //private void GinTubBuilderManager_RoomStateRead(object sender, GinTubBuilderManager.RoomStateReadEventArgs args)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #endregion
    }
}
