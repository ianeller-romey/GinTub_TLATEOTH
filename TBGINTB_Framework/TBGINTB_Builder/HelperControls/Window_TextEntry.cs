using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace TBGINTB_Builder.HelperControls
{
    public class Window_TextEntry : Window_AcceptCancel
    {
        #region MEMBER FIELDS
        #endregion


        #region MEMBER PROPERTIES

        public string Text { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_TextEntry(string title, string defaultText)
        {
            Title = title;
            Height = 100;
            Width = 300;
            Content = CreateContent(defaultText);
        }

        #endregion


        #region Private Functionality

        public UIElement CreateContent(string defaultText)
        {
            TextBox textBox_textEntry = new TextBox();
            textBox_textEntry.TextChanged += (sender, args) => { Text = textBox_textEntry.Text; };
            textBox_textEntry.Text = defaultText;
            return textBox_textEntry;
        }

        #endregion

        #endregion
    }
}
