using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class TabItem_Locations : TabItem, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Location m_grid_location;

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public TabItem_Locations()
        {
            Header = "Locations";
            Content = CreateControls();
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_grid_location.SetActiveAndRegisterForGinTubEvents();

            GinTubBuilderManager.ReadAllLocations();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_grid_location.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls()
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            m_grid_location = new UserControl_Location(true);
            grid_main.SetGridRowColumn(m_grid_location, 0, 0);

            Button button_automaticUpsert = new Button() { Content = "Automatic Upsert" };
            button_automaticUpsert.Click += (x, y) =>
            {
                AutomaticUpsert();
            };
            grid_main.SetGridRowColumn(button_automaticUpsert, 1, 0);

            return grid_main;
        }

        private void AutomaticUpsert()
        {
            var window_directory = new Window_SelectDirectory("Select Directory", null);
            window_directory.ShowDialog();
            if (window_directory.Accepted)
            {
                var window_relativeUri = new Window_TextEntry("Relative URI", @"images\");
                window_relativeUri.ShowDialog();
                if (window_relativeUri.Accepted)
                {
                    GinTubBuilderManager.UpsertLocationAutomatically(window_directory.DirectoryName, window_relativeUri.Text);
                }
            }
        }

        #endregion

        #endregion

    }
}
