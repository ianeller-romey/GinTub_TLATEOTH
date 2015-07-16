using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_Bordered_ParagraphWithPreview : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Paragraph m_userControl_paragraph;
        TextBlock m_textBlock_paragraphPreview;

        #endregion


        #region MEMBER PROPERTIES

        public int? ParagraphId { get { return m_userControl_paragraph.ParagraphId; } }
        public int? ParagraphOrder { get { return m_userControl_paragraph.ParagraphOrder; } }
        public int RoomId { get { return m_userControl_paragraph.RoomId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_ParagraphWithPreview(int? paragraphId, int? paragraphOrder, int roomId, bool enableEditing)
        {
            CreateControls(paragraphId,  paragraphOrder, roomId, enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ParagraphStateRead += GinTubBuilderManager_ParagraphStateEvent;
            GinTubBuilderManager.ParagraphStateUpdated += GinTubBuilderManager_ParagraphStateEvent;

            m_userControl_paragraph.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ParagraphStateRead += GinTubBuilderManager_ParagraphStateEvent;
            GinTubBuilderManager.ParagraphStateUpdated += GinTubBuilderManager_ParagraphStateEvent;

            m_userControl_paragraph.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? paragraphId, int? paragraphOrder, int roomId, bool enableEditing)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            m_userControl_paragraph = new UserControl_Paragraph(paragraphId, paragraphOrder, roomId, enableEditing);
            grid_main.SetGridRowColumn(m_userControl_paragraph, 0, 0);

            m_textBlock_paragraphPreview = 
                new TextBlock() 
                { 
                    Background = Brushes.DarkGray, 
                    Foreground = Brushes.FloralWhite,
                    TextWrapping = System.Windows.TextWrapping.Wrap,
                    Visibility = System.Windows.Visibility.Collapsed 
                };
            grid_main.SetGridRowColumn(m_textBlock_paragraphPreview, 1, 0);

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void GinTubBuilderManager_ParagraphStateEvent(object sender, GinTubBuilderManager.ParagraphStateEventArgs args)
        {
            if (args.Paragraph == ParagraphId && args.State == 0)
                SetParagraphPreview(args.Text);
        }

        private void SetParagraphPreview(string paragraphStateText)
        {
            m_textBlock_paragraphPreview.Visibility = System.Windows.Visibility.Visible;
            m_textBlock_paragraphPreview.Text = string.Format("{0} ...", paragraphStateText.Substring(0, Math.Max(Math.Min(75, paragraphStateText.Length - 5), 0)));
        }

        #endregion

        #endregion
    }
}
