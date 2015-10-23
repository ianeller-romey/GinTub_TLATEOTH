using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TBGINTB_Builder.BuilderControls;
using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder
{
    public partial class MainWindow : Window
    {
        #region MEMBER FIELDS

        Menu m_menu_main;
        MenuItem
            m_menuItem_file,
            m_menuItem_loadFromDatabase,
            m_menuItem_exportToXml,
            m_menuItem_importFromXml,
            m_menuItem_setup,
            m_menuItem_GameStateOnInitialLoad;
        Grid m_grid_main;
        TabControl m_tabControl_controls;

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public MainWindow()
        {
            InitializeComponent();
            Content = CreateControls();
            CreateGinTubManager();
            Width = 1000;
            Height = 500;

            GinTubBuilderManager.GameStateOnInitialLoadRead += GinTubBuilderManager_GameStateOnInitialLoadRead;
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls()
        {
            m_menu_main = new Menu();

            ////////
            // File
            m_menuItem_loadFromDatabase = new MenuItem() { Header = "Load DB" };
            m_menuItem_loadFromDatabase.Click += MenuItem_LoadFromDatabase_Click;

            m_menuItem_exportToXml = new MenuItem() { Header = "Export to Xml" };
            m_menuItem_exportToXml.Click += MenuItem_ExportToXml_Click;

            m_menuItem_importFromXml = new MenuItem() { Header = "Import From Xml" };
            m_menuItem_importFromXml.Click += MenuItem_ImportFromXml_Click;

            m_menuItem_file = new MenuItem() { Header = "File" };
            m_menuItem_file.Items.Add(m_menuItem_loadFromDatabase);

            m_menu_main.Items.Add(m_menuItem_file);

            ////////
            // Setup
            m_menuItem_GameStateOnInitialLoad = new MenuItem() { Header = "Area/Room On Initial Load" };
            RoutedEventHandler menuItem_GameStateOnInitialLoad_clickHandler = 
                new RoutedEventHandler((x, y) =>
                {
                    if (m_menuItem_GameStateOnInitialLoad.IsSubmenuOpen)
                        m_menuItem_setup.IsSubmenuOpen = false;
                });
            m_menuItem_GameStateOnInitialLoad.AddHandler(Button.ClickEvent, menuItem_GameStateOnInitialLoad_clickHandler);
            m_menuItem_GameStateOnInitialLoad.AddHandler(MenuItem.ClickEvent, menuItem_GameStateOnInitialLoad_clickHandler);
                

            m_menuItem_setup = new MenuItem() { Header = "Setup" };
            m_menuItem_setup.Items.Add(m_menuItem_GameStateOnInitialLoad);

            ////////
            // Toolbar
            DockPanel dockPanel_main = new DockPanel();
            dockPanel_main.Children.Add(m_menu_main);
            DockPanel.SetDock(m_menu_main, Dock.Top);

            ////////
            // grid
            m_grid_main = new Grid();
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            m_grid_main.SetGridRowColumn(dockPanel_main, 0, 0);

            return m_grid_main;
        }

        private void CreateControlsAfterLoading()
        {
            ////////
            // tab control
            m_tabControl_controls = new TabControl();
            m_tabControl_controls.Items.Add(new TabItem_Area());
            m_tabControl_controls.Items.Add(new TabItem_Locations());
            m_tabControl_controls.Items.Add(new TabItem_Verbs());
            m_tabControl_controls.Items.Add(new TabItem_Results());
            m_tabControl_controls.Items.Add(new TabItem_Items());
            m_tabControl_controls.Items.Add(new TabItem_Events());
            m_tabControl_controls.Items.Add(new TabItem_Characters());
            m_tabControl_controls.Items.Add(new TabItem_Messages());

            ////////
            // grid
            m_grid_main.SetGridRowColumn(m_tabControl_controls, 1, 0);
        }

        private void CreateGinTubManager()
        {
            GinTubBuilderManager.Initialize();
            JSONPropertyManager.Initialize();
        }

        private void GinTubBuilderManager_GameStateOnInitialLoadRead(object sender, GinTubBuilderManager.GameStateOnInitialLoadReadEventArgs args)
        {
            if (m_menuItem_GameStateOnInitialLoad.Items.Count == 0)
            {
                UserControl_GameStateOnInitialLoadModification control = new UserControl_GameStateOnInitialLoadModification(args.Area, args.Room);
                control.SetActiveAndRegisterForGinTubEvents();
                m_menuItem_GameStateOnInitialLoad.Items.Add(control);
                GinTubBuilderManager.ReadAllAreas();
            }
            GinTubBuilderManager.GameStateOnInitialLoadRead -= GinTubBuilderManager_GameStateOnInitialLoadRead;
        }

        private void MenuItem_LoadFromDatabase_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if(menuItem != null)
            {
                CreateControlsAfterLoading();
                CreateGinTubManager();
                m_tabControl_controls.SelectionChanged += TabControl_Controls_SelectionChanged;
                m_tabControl_controls.SelectedItem = m_tabControl_controls.Items.OfType<TabItem>().First();

                m_menuItem_file.Items.Remove(m_menuItem_loadFromDatabase);
                m_menuItem_file.Items.Add(m_menuItem_exportToXml);
                m_menuItem_file.Items.Add(m_menuItem_importFromXml);

                m_menu_main.Items.Add(m_menuItem_setup);
                GinTubBuilderManager.ReadGameStateOnInitialLoad();
            }
        }

        private void MenuItem_ExportToXml_Click(object sender, RoutedEventArgs e)
        {
            Window_SelectFile window = new Window_SelectFile("Export to ...", string.Empty);
            window.ShowDialog();
            if (window.Accepted)
                GinTubBuilderManager.ExportToXml(window.FileName);
        }

        private void MenuItem_ImportFromXml_Click(object sender, RoutedEventArgs e)
        {
            Window_Notification window_notification = new Window_Notification("Notice", "Importing from an .xml file will OVERWRITE all records currently in the database.\r\nThis action CANNOT be undone.\r\nContinue?");
            window_notification.ShowDialog();
            if(window_notification.Accepted)
            {
                string backupFile = null;
                window_notification = new Window_Notification("Notice", "Would you like to backup your database before the import?");
                window_notification.ShowDialog();
                if(window_notification.Accepted)
                {
                    Window_SelectFile window_selectFile = new Window_SelectFile("Backup to ...", string.Empty);
                    window_selectFile.ShowDialog();
                    if (window_selectFile.Accepted)
                        backupFile = window_selectFile.FileName;
                }
                Window_OpenFile window_openFile = new Window_OpenFile("Import from ...", string.Empty);
                window_openFile.ShowDialog();
                if (window_openFile.Accepted)
                    GinTubBuilderManager.ImportFromXml(window_openFile.FileName, backupFile);
            }
        }

        private void TabControl_Controls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != m_tabControl_controls)
                return;

            foreach (var i in m_tabControl_controls.Items.OfType<IRegisterGinTubEventsOnlyWhenActive>())
                i.SetInactiveAndUnregisterFromGinTubEvents();
            var item = (m_tabControl_controls.SelectedItem as IRegisterGinTubEventsOnlyWhenActive);
            if (item != null)
                item.SetActiveAndRegisterForGinTubEvents();
        }

        #endregion

        #endregion
    }
}
