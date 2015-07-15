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
    public class ComboBox_Area : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxItem_newArea = new ComboBoxItem() { Content = "New Area ..." };

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_Area : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int AreaId { get; private set; }
            public string AreaName { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Area(int areaId, string areaName)
            {
                AreaId = areaId;
                SetAreaName(areaName);
            }

            public void SetAreaName(string areaName)
            {
                AreaName = areaName;
                Content = AreaName;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_Area()
        {
            Items.Add(c_comboBoxItem_newArea);

            SelectionChanged += ComboBox_Area_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.AreaAdded += GinTubBuilderManager_AreaAdded;
            GinTubBuilderManager.AreaModified += GinTubBuilderManager_AreaModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.AreaAdded -= GinTubBuilderManager_AreaAdded;
            GinTubBuilderManager.AreaModified -= GinTubBuilderManager_AreaModified;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_AreaAdded(object sender, GinTubBuilderManager.AreaAddedEventArgs args)
        {
            if (!Items.OfType<ComboBoxItem_Area>().Any(i => i.AreaId == args.Id))
                Items.Add(new ComboBoxItem_Area(args.Id, args.Name));
        }

        private void GinTubBuilderManager_AreaModified(object sender, GinTubBuilderManager.AreaModifiedEventArgs args)
        {
            ComboBoxItem_Area item = Items.OfType<ComboBoxItem_Area>().SingleOrDefault(i => i.AreaId == args.Id);
            if (item != null)
                item.SetAreaName(args.Name);
        }

        private void NewAreaDialog()
        {
            Window_Area window = 
                new Window_Area
                (
                    null, 
                    null,
                    (win) =>
                    {
                        Window_Area wWin = win as Window_Area;
                        if (wWin != null)
                            GinTubBuilderManager.AddArea(wWin.AreaName);
                    }
                );
            window.Show();
        }

        private void ComboBox_Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if ((item = SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newArea)
                    NewAreaDialog();
            }
        }

        #endregion

        #endregion
    }
}
