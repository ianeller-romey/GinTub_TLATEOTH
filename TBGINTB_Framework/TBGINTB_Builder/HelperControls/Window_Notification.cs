using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace TBGINTB_Builder.HelperControls
{
    public class Window_Notification : Window_AcceptCancel
    {
        #region MEMBER FIELDS
        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_Notification(string title, string text)
        {
            Title = title;
            Height = 100;
            Width = 300;
            Content = CreateContent(text);
        }

        #endregion


        #region Private Functionality

        public UIElement CreateContent(string text)
        {
            TextBlock textBlock_textEntry = new TextBlock();
            textBlock_textEntry.Text = text;
            return textBlock_textEntry;
        }

        #endregion

        #endregion
    }
}
