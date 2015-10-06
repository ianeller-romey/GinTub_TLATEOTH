using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_Location : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private static readonly Uri s_uri_imageNotFound = new Uri("/TBGINTB_Builder;component/Images/image_not_found.jpg", UriKind.Relative);

        ComboBox_Location m_comboBox_location;
        Image m_image_locationFile;

        string m_absolutePath = string.Empty;

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Location(bool enableEditing)
        {
            CreateControls();

            m_image_locationFile.AllowDrop = enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.LocationUpdated += GinTubBuilderManager_LocationUpdatedOrSelect;
            GinTubBuilderManager.LocationSelect += GinTubBuilderManager_LocationUpdatedOrSelect;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.LocationUpdated -= GinTubBuilderManager_LocationUpdatedOrSelect;
            GinTubBuilderManager.LocationSelect -= GinTubBuilderManager_LocationUpdatedOrSelect;
        }
        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });

            m_comboBox_location = new ComboBox_Location();
            m_comboBox_location.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_location.SelectionChanged += ComboBox_Location_SelectionChanged;
            grid_main.SetGridRowColumn(m_comboBox_location, 0, 0);

            m_image_locationFile = new Image();
            m_image_locationFile.Drop += Image_LocationFile_Drop;
            grid_main.SetGridRowColumn(m_image_locationFile, 1, 0);

            Content = grid_main;
        }

        void GinTubBuilderManager_LocationUpdatedOrSelect(object sender, GinTubBuilderManager.LocationEventArgs args)
        {
            ComboBox_Location.ComboBoxItem_Location comboBoxItem = null;
            if ((comboBoxItem = m_comboBox_location.SelectedItem as ComboBox_Location.ComboBoxItem_Location) != null && comboBoxItem.LocationId == args.Id)
                SetLocationFile(args.LocationFile);
        }

        private void SetLocationFile(string locationFile)
        {
            Uri uri = null;
            if (TryCreateRelativeHttpUri(locationFile))
            {
                m_image_locationFile.Source = LoadImageFromHttp(Path.Combine(m_absolutePath, locationFile));
            }
            else
            {
                if (File.Exists(locationFile))
                    uri = new Uri(locationFile);
                else
                    uri = s_uri_imageNotFound;
                m_image_locationFile.Source = new BitmapImage(uri);
            }
        }

        private bool TryCreateRelativeHttpUri(string locationFile)
        {
            Uri uri = null;
            // can we create a relative uri from the filename?
            if(Uri.TryCreate(locationFile, UriKind.Relative, out uri))
            {
                var cancelled = false;
                // if so
                while(string.IsNullOrWhiteSpace(m_absolutePath) && !cancelled)
                {
                    var window_absolutePath = new Window_TextEntry("Absolute Path", "");
                    window_absolutePath.ShowDialog();
                    cancelled = !window_absolutePath.Accepted;
                    // if the user entered an absolute path
                    if(!cancelled)
                    {
                        var absolutePath = window_absolutePath.Text;
                        var absoluteLocationFile = Path.Combine(absolutePath, locationFile);
                        // and the combination of the absolute path and the relative filename is valid
                        if(Uri.TryCreate(absoluteLocationFile, UriKind.Absolute, out uri) && uri.Scheme == Uri.UriSchemeHttp)
                        {
                            m_absolutePath = absolutePath;
                        }
                    }
                }
            }
            // if we can't create a relative uri from the filename, this is not an image we need to load via http
            else
            {
                return false;
            }
            
            return !string.IsNullOrWhiteSpace(m_absolutePath);
        }

        private BitmapImage LoadImageFromHttp(string httpFile)
        {
            // cheesed from http://stackoverflow.com/questions/3148163/wpf-image-urisource-and-data-binding-using-http-url
            var image = new BitmapImage();
            const int BytesToRead = 100;

            WebRequest request = WebRequest.Create(new Uri(httpFile, UriKind.Absolute));
            request.Timeout = -1;
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            BinaryReader reader = new BinaryReader(responseStream);
            MemoryStream memoryStream = new MemoryStream();

            byte[] bytebuffer = new byte[BytesToRead];
            int bytesRead = reader.Read(bytebuffer, 0, BytesToRead);

            while (bytesRead > 0)
            {
                memoryStream.Write(bytebuffer, 0, bytesRead);
                bytesRead = reader.Read(bytebuffer, 0, BytesToRead);
            }

            image.BeginInit();
            memoryStream.Seek(0, SeekOrigin.Begin);

            image.StreamSource = memoryStream;
            image.EndInit();

            return image;
        }

        private void ComboBox_Location_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Location comboBox = sender as ComboBox_Location;
            ComboBox_Location.ComboBoxItem_Location comboBoxItem = null;
            if (comboBox != null && (comboBoxItem = comboBox.SelectedItem as ComboBox_Location.ComboBoxItem_Location) != null)
                GinTubBuilderManager.SelectLocation(comboBoxItem.LocationId);
            else
                SetLocationFile(null);
        }

        private void Image_LocationFile_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string file = files.First();
                if (ComboBox_Location.ValidFileTypes.Contains(Path.GetExtension(file)))
                {
                    ComboBox_Location.ComboBoxItem_Location comboBoxItem = null;
                    if ((comboBoxItem = m_comboBox_location.SelectedItem as ComboBox_Location.ComboBoxItem_Location) != null)
                        GinTubBuilderManager.UpdateLocation(comboBoxItem.LocationId, null, file);
                }
            }
        }

        #endregion

        #endregion
    }
}
