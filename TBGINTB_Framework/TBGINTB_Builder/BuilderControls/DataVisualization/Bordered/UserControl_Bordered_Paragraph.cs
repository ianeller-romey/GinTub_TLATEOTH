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
    public class UserControl_Bordered_Paragraph : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Paragraph m_userControl_paragraph;

        #endregion


        #region MEMBER PROPERTIES

        public int? ParagraphId { get { return m_userControl_paragraph.ParagraphId; } }
        public int? ParagraphOrder { get { return m_userControl_paragraph.ParagraphOrder; } }
        public int RoomId { get { return m_userControl_paragraph.RoomId; } }
        public int? RoomStateId { get { return m_userControl_paragraph.RoomStateId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_Paragraph(int? paragraphId, int? paragraphOrder, int roomId, int? roomStateId, bool enableEditing)
        {
            CreateControls( paragraphId,  paragraphOrder, roomId,  roomStateId, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_paragraph.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_paragraph.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? paragraphId, int? paragraphOrder, int roomId, int? roomStateId, bool enableEditing)
        {
            m_userControl_paragraph = new UserControl_Paragraph(paragraphId, paragraphOrder, roomId, roomStateId, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_paragraph };
            Content = border;
        }

        #endregion

        #endregion
    }
}
