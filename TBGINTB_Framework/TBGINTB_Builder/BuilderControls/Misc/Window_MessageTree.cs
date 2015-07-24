using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class Window_MessageTree : Window, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        Canvas m_canvas_messageTree;

        #endregion


        #region MEMBER PROPERTIES

        public int MessageId { get; private set; }

        #endregion


        #region MEMBER CLASSES

        private class Tree_Line : Line
        {

        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_MessageTree(int messageId)
        {
            MessageId = messageId;

            Title = "Message Tree";

            CreateControls();
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.MessageTreeParagraphStateSelect += GinTubBuilderManager_MessageTreeParagraphStateSelect;
            GinTubBuilderManager.MessageTreeNounSelect += GinTubBuilderManager_MessageTreeNounSelect;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.MessageTreeParagraphStateSelect -= GinTubBuilderManager_MessageTreeParagraphStateSelect;
            GinTubBuilderManager.MessageTreeNounSelect -= GinTubBuilderManager_MessageTreeNounSelect;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            ////////
            // TextBlock
            m_textBlock_roomPreview = new TextBlock() { TextWrapping = TextWrapping.Wrap, Text = string.Empty };
            m_textBlock_roomPreview.MouseLeftButtonDown += TextBlock_MessageTree_MouseLeftButtonDown;
            m_textBlock_roomPreview.Visibility = System.Windows.Visibility.Collapsed;
            grid_main.SetGridRowColumn(m_textBlock_roomPreview, 0, 0);

            ////////
            // TextBox
            m_textBox_roomPreview = new TextBox() { TextWrapping = TextWrapping.Wrap };
            grid_main.SetGridRowColumn(m_textBox_roomPreview, 0, 0);

            ////////
            // Button
            Button button_loadPreview = new Button() { Content = "Load Preview" };
            button_loadPreview.Click += Button_LoadPreview_Click;
            grid_main.SetGridRowColumn(button_loadPreview, 1, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_MessageTreeParagraphStateSelect(object sender, GinTubBuilderManager.MessageTreeParagraphStateSelectEventArgs args)
        {
            CreateParagraphState(args.Text, args.Nouns.Select(n => n.Text));
        }

        private void GinTubBuilderManager_MessageTreeNounSelect(object sender, GinTubBuilderManager.MessageTreeNounSelectEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void CreateParagraphState(string paragraphStateText, IEnumerable<string> nounsText)
        {
            List<string> nounsTextOrdered = nounsText.OrderBy(n => paragraphStateText.IndexOf(n)).ToList();
            List<Run> runs = new List<Run>();
            for (int i = 0, j = nounsTextOrdered.Count; i < j; ++i)
            {
                string nounText = nounsTextOrdered[i];
                string subText = paragraphStateText.Substring(0, paragraphStateText.IndexOf(nounText));
                runs.Add(new Run(subText));
                runs.Add(new Run_Noun(nounText));
                paragraphStateText = paragraphStateText.Replace(subText, string.Empty);
                paragraphStateText = paragraphStateText.Replace(nounText, string.Empty);
            }
            runs.Add(new Run(paragraphStateText));
            runs.Add(new Run(" "));

            foreach (var run in runs)
                m_textBlock_roomPreview.Inlines.Add(run);

            ViewTextBlock();
        }

        private void ViewTextBlock()
        {
            if (!m_textBlock_roomPreview.IsVisible)
            {
                m_textBlock_roomPreview.Visibility = System.Windows.Visibility.Visible;
                m_textBox_roomPreview.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void ViewTextBox()
        {
            if (!m_textBox_roomPreview.IsVisible)
            {
                m_textBlock_roomPreview.Visibility = System.Windows.Visibility.Collapsed;
                m_textBox_roomPreview.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void TextBlock_MessageTree_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(sender is TextBlock)
            {
                m_textBox_roomPreview.Text = m_textBlock_roomPreview.Inlines.OfType<Run>().Select(r => r.Text).Aggregate((x,y) => string.Format("{0}{1}", x, y));
                ViewTextBox();
            }
        }

        private void Button_LoadPreview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                m_textBlock_roomPreview.Inlines.Clear();
                m_textBlock_roomPreview.Text = string.Empty;

                GinTubBuilderManager.SelectMessageTree(RoomId);
            }
        }

        #endregion

        #endregion
    }
}
