using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace TBGINTB_Builder.HelperControls
{
    public class UserControl_Selecttable : UserControl
    {
        #region MEMBER FIELDS

        static readonly Brush s_brush_getBackground = Brushes.LightGreen;

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER METHODS

        #region Public Functionality
        #endregion


        #region Protected Functionality

        public void SetSelecttableBackground(bool isSelect)
        {
            Background = (isSelect) ? s_brush_getBackground : null;
        }

        #endregion

        #endregion

    }
}
