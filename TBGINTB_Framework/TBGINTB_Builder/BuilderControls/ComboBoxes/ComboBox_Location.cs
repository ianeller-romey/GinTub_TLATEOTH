using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class ComboBox_Location : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxItem_newLocation = new ComboBoxItem() { Content = "New Location ..." };

        private static readonly List<string> c_validFileTypes = new List<string> { ".png", ".bmp", ".jpg" };

        #endregion


        #region MEMBER PROPERTIES

        public static List<string> ValidFileTypes { get { return c_validFileTypes; } }

        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_Location : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int LocationId { get; private set; }
            public string LocationName { get; private set; }
            public string LocationFile { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Location(int locationId, string locationName, string locationFile)
            {
                LocationId = locationId;
                SetLocationFile(locationFile);
                SetLocationName(locationName);
            }

            public void SetLocationName(string locationName)
            {
                LocationName = locationName;
                Content = LocationName;
            }

            public void SetLocationFile(string locationFile)
            {
                LocationFile = locationFile;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_Location()
        {
            Items.Add(c_comboBoxItem_newLocation);

            SelectionChanged += ComboBox_Location_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.LocationRead += GinTubBuilderManager_LocationRead;
            GinTubBuilderManager.LocationUpdated += GinTubBuilderManager_LocationUpdated;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.LocationRead -= GinTubBuilderManager_LocationRead;
            GinTubBuilderManager.LocationUpdated += GinTubBuilderManager_LocationUpdated;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_LocationRead(object sender, GinTubBuilderManager.LocationReadEventArgs args)
        {
            if (!Items.OfType<ComboBoxItem_Location>().Any(i => i.LocationId == args.Id))
                Items.Add(new ComboBoxItem_Location(args.Id, args.Name, args.LocationFile));
        }

        private void GinTubBuilderManager_LocationUpdated(object sender, GinTubBuilderManager.LocationUpdatedEventArgs args)
        {
            ComboBoxItem_Location item = Items.OfType<ComboBoxItem_Location>().SingleOrDefault(i => i.LocationId == args.Id);
            if(item != null)
            {
                item.SetLocationName(args.Name);
                item.SetLocationFile(args.LocationFile);
            }
        }

        private void NewLocationDialog()
        {
            Window_OpenFile window_openFile = new Window_OpenFile("Location File", string.Empty);
            window_openFile.Closed += (x, y) =>
                {
                    if (window_openFile.Accepted)
                    {
                        Window_TextEntry window_textEntry = new Window_TextEntry("Location Name", Path.GetFileNameWithoutExtension(window_openFile.FileName));
                        window_textEntry.Closed += (a, b) =>
                            {
                                if (window_textEntry.Accepted)
                                    GinTubBuilderManager.CreateLocation(window_textEntry.Text, window_openFile.FileName);
                            };
                        window_textEntry.Show();
                    }
                };
            window_openFile.Show();
        }

        private void ComboBox_Location_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if ((item = SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newLocation)
                    NewLocationDialog();
            }
        }

        #endregion

        #endregion
    }
}
