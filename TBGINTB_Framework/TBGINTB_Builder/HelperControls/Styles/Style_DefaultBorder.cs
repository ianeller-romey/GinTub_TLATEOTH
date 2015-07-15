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
    public class Style_DefaultBorder : Style
    {
        #region MEMBER FIELDS

        private static readonly Thickness
            s_thickness_borderThickness = new Thickness(1.5),
            s_thickness_margin = new Thickness(2.5),
            s_thickness_padding = new Thickness(5);
        private static readonly Brush s_border_borderBrush = Brushes.Black;
        private static readonly CornerRadius s_cornerRadius_cornerRadius = new CornerRadius(2.5);

        private static readonly Style_DefaultBorder s_style_defaultBorder = new Style_DefaultBorder();

        #endregion


        #region MEMBER PROPERTIES

        public Style_DefaultBorder()
        {
            Setters.Add(new Setter(Border.BorderThicknessProperty, s_thickness_borderThickness));
            Setters.Add(new Setter(Border.BorderBrushProperty, s_border_borderBrush));
            Setters.Add(new Setter(Border.CornerRadiusProperty, s_cornerRadius_cornerRadius));
            Setters.Add(new Setter(Control.MarginProperty, s_thickness_margin));
            Setters.Add(new Setter(Control.PaddingProperty, s_thickness_padding));
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality
        #endregion


        #region Private Functionality

        #endregion

        #endregion

    }
}
