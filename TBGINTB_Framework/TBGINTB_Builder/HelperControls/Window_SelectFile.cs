using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.Extensions;


namespace TBGINTB_Builder.HelperControls
{
    public class Window_SelectFile : Window_AcceptCancel
    {
        #region MEMBER FIELDS

        TextBox m_textBox_fileName;

        #endregion


        #region MEMBER PROPERTIES

        public string FileName { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_SelectFile(string title, string fileName)
        {
            Title = title;
            Height = 100;
            Width = 300;
            Content = CreateContent(fileName);
        }

        #endregion


        #region Private Functionality

        public UIElement CreateContent(string fileName)
        {
            Grid grid_main = new Grid();
            grid_main.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100.0, GridUnitType.Star) });
            grid_main.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            m_textBox_fileName = new TextBox();
            m_textBox_fileName.TextChanged += (sender, args) => { FileName = m_textBox_fileName.Text; };
            m_textBox_fileName.Text = fileName;
            grid_main.SetGridRowColumn(m_textBox_fileName, 0, 0);

            Button button_openFile = new Button() { Content = "Select file ..." };
            button_openFile.Click += (x, y) =>
                {
                    System.Windows.Forms.OpenFileDialog openFileDialog =
                        new System.Windows.Forms.OpenFileDialog()
                        {
                            CheckFileExists = false,
                            CheckPathExists = true
                        };
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        m_textBox_fileName.Text = openFileDialog.FileName;
                };
            grid_main.SetGridRowColumn(button_openFile, 0, 1);

            return grid_main;
        }

        #endregion

        #endregion
    }
}
