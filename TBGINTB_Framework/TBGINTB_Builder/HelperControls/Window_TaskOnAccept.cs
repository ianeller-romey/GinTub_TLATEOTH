using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace TBGINTB_Builder.HelperControls
{
    public class Window_TaskOnAccept : Window_AcceptCancel
    {
        #region MEMBER FIELDS

        public delegate void TaskOnAccept(Window_TaskOnAccept win);

        private static TaskOnAccept s_defaultTask = (win) => {};

        #endregion


        #region MEMBER PROPERTIES

        public TaskOnAccept Task
        {
            get;
            private set;
        }

        public static TaskOnAccept DefaultTask
        {
            get { return s_defaultTask; }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_TaskOnAccept(string title, TaskOnAccept task)
        {
            Task = task;

            Title = title;
            Height = 100;
            Width = 300;

            Closed += Window_TaskOnAccept_Closed;
        }

        #endregion


        #region Private Functionality

        void Window_TaskOnAccept_Closed(object sender, EventArgs e)
        {
            if (Accepted)
                Task(this);
        }

        #endregion

        #endregion
    }
}
