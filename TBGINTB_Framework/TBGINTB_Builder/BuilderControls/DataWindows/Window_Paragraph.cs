﻿using System;
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
    public class Window_Paragraph : Window_TaskOnAccept
    {
        #region MEMBER FIELDS

        UserControl_Paragraph m_userControl_paragraph;

        #endregion


        #region MEMBER PROPERTIES

        public int? ParagraphId { get { return m_userControl_paragraph.ParagraphId; } }
        public int? ParagraphOrder { get { return m_userControl_paragraph.ParagraphOrder; } }
        public int RoomId { get { return m_userControl_paragraph.RoomId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_Paragraph(int? paragraphId, int? paragraphOrder, int roomId, TaskOnAccept task) :
            base("Paragraph Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(paragraphId, paragraphOrder, roomId);
            m_userControl_paragraph.SetActiveAndRegisterForGinTubEvents();
            GinTubBuilderManager.ReadAllLocations();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? paragraphId, int? paragraphOrder, int roomId)
        {
            m_userControl_paragraph = new UserControl_Paragraph(paragraphId, paragraphOrder, roomId, true);
            return m_userControl_paragraph;
        }

        #endregion

        #endregion
    }
}
