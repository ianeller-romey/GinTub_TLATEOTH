using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class Window_RoomAuthoring : Window
    {
        #region MEMBER FIELDS

        TextBox m_textBox_roomAuthoring;

        Button m_button_generateParagraphs;

        static readonly Regex s_regex_sentences = new Regex(@"(\S.+?[.!?])(?=\s+|$)");

        #endregion


        #region MEMBER PROPERTIES

        public int RoomId { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_RoomAuthoring(int roomId, string roomName)
        {
            RoomId = roomId;

            Title = roomName;

            CreateControls();
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            ////////
            // TextBox
            m_textBox_roomAuthoring = new TextBox() { TextWrapping = TextWrapping.Wrap };
            grid_main.SetGridRowColumn(m_textBox_roomAuthoring, 0, 0);

            ////////
            // Button
            m_button_generateParagraphs = new Button() { Content = "Generate Paragraphs" };
            m_button_generateParagraphs.Click += Button_GenerateParagraphs_Click;
            grid_main.SetGridRowColumn(m_button_generateParagraphs, 1, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        void Button_GenerateParagraphs_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button && (sender as Button) == m_button_generateParagraphs)
            {
                GenerateParagraphs(m_textBox_roomAuthoring.Text);

                Close();
            }
        }

        private void GenerateParagraphs(string roomText)
        {
            List<string> sentences = s_regex_sentences.Matches(roomText).OfType<Match>().Select(x => x.Value).ToList();
            for (int i = 0, j = sentences.Count; i < j; ++i)
            {
                var paragraphState = 
                    new 
                    { 
                        Id = -1, 
                        State = -1, 
                        Paragraph = -1 
                    };
                GinTubBuilderManager.ParagraphStateReadEventHandler psreh = (x, y) =>
                    {
                        paragraphState = 
                            new 
                            { 
                                Id = y.Id, 
                                State = y.State, 
                                Paragraph = y.Paragraph 
                            };
                    };

                GinTubBuilderManager.ParagraphStateRead += psreh;
                GinTubBuilderManager.CreateParagraph(i, RoomId);
                GinTubBuilderManager.ParagraphStateRead -= psreh;

                // We update, not create, because the CreateParagraph stored procedure automagically creates a ParagraphState for us
                GinTubBuilderManager.UpdateParagraphState(paragraphState.Id, sentences[i], paragraphState.State, paragraphState.Paragraph);
            }
        }

        #endregion

        #endregion
    }
}
