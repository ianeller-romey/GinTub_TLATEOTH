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
    public class Window_SelectDirectory : Window_AcceptCancel
    {
        #region MEMBER FIELDS

        TextBox m_textBox_directoryName;

        #endregion


        #region MEMBER PROPERTIES

        public string DirectoryName { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_SelectDirectory(string title, string directoryName)
        {
            Title = title;
            Height = 100;
            Width = 300;
            Content = CreateContent(directoryName);
        }

        #endregion


        #region Private Functionality

        public UIElement CreateContent(string directoryName)
        {
            Grid grid_main = new Grid();
            grid_main.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100.0, GridUnitType.Star) });
            grid_main.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            m_textBox_directoryName = new TextBox();
            m_textBox_directoryName.TextChanged += (sender, args) => { DirectoryName = m_textBox_directoryName.Text; };
            m_textBox_directoryName.Text = directoryName;
            grid_main.SetGridRowColumn(m_textBox_directoryName, 0, 0);

            Button button_openFile = new Button() { Content = "Select directory ..." };
            button_openFile.Click += (x, y) =>
                {
                    System.Windows.Forms.FolderBrowserDialog selectDirectoryDialog = 
                        new System.Windows.Forms.FolderBrowserDialog()
                        {
                            SelectedPath = directoryName
                        };
                    if (selectDirectoryDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        m_textBox_directoryName.Text = selectDirectoryDialog.SelectedPath;
                };
            grid_main.SetGridRowColumn(button_openFile, 0, 1);

            return grid_main;
        }

        #endregion

        #endregion
    }
}
