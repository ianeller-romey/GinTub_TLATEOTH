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

        private StackPanel m_stackPanel_groupedParagraphRoomStates;

        #endregion


        #region MEMBER PROPERTIES

        public int RoomId { get; private set; }
        public int ParagraphId { get; private set; }
        public IEnumerable<int> RoomStates
        {
            get
            {
                return m_stackPanel_groupedParagraphRoomStates.Children.OfType<UserControl_GroupedParagraphRoomStates>()
                    .Select(x => x.RoomStates)
                    .Aggregate((x, y) => x.Concat(y));
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
            m_stackPanel_groupedParagraphRoomStates = new StackPanel() { Orientation = Orientation.Vertical };

            Content = m_stackPanel_groupedParagraphRoomStates;
        }

        private void GinTubBuilderManager_ParagraphRoomStateRead(object sender, GinTubBuilderManager.ParagraphRoomStateReadEventArgs args)
        {
            // Is this the first time we've seen this state?
            // If so, add a group
            UserControl_GroupedParagraphRoomStates userControl = 
                m_stackPanel_groupedParagraphRoomStates.Children.OfType<UserControl_GroupedParagraphRoomStates>().FirstOrDefault(x => x.RoomStateState == args.RoomStateState);
            if (ParagraphId == args.Paragraph && userControl == null)
                userControl = AddGroupedParagraphRoomState(args.RoomStateState);

            if(!userControl.RoomStates.Contains(args.RoomStateState))
                userControl.AddParagraphRoomState(args.Id, args.Paragraph, args.Paragraph, args.RoomState, args.RoomStateName, args.RoomStateTime, true);
        }

        private void GinTubBuilderManager_RoomStateRead(object sender, GinTubBuilderManager.RoomStateReadEventArgs args)
        {
            // Is this the first time we've seen this state?
            // If so, add a group
            UserControl_GroupedParagraphRoomStates userControl =
                m_stackPanel_groupedParagraphRoomStates.Children.OfType<UserControl_GroupedParagraphRoomStates>().FirstOrDefault(x => x.RoomStateState == args.State);
            if (RoomId == args.Room && userControl == null)
                userControl = AddGroupedParagraphRoomState(args.State);

            if(!userControl.RoomStates.Contains(args.Id))
                userControl.AddParagraphRoomState(null, null, ParagraphId, args.Id, args.Name, args.Time, true);
        }

        private UserControl_GroupedParagraphRoomStates AddGroupedParagraphRoomState(int roomStateState)
        {

            var userControl = new UserControl_GroupedParagraphRoomStates(RoomId, roomStateState, ParagraphId);
            var userControl_greater =
                    m_stackPanel_groupedParagraphRoomStates.Children.OfType<UserControl_GroupedParagraphRoomStates>()
                    .FirstOrDefault(x => x.RoomStateState > roomStateState);
            if (userControl_greater != null)
                m_stackPanel_groupedParagraphRoomStates.Children.Insert
                (
                    m_stackPanel_groupedParagraphRoomStates.Children.IndexOf(userControl_greater),
                    userControl
                );
            else
                m_stackPanel_groupedParagraphRoomStates.Children.Add(userControl);
            return userControl;
        }

        #endregion

        #endregion
    }
}
