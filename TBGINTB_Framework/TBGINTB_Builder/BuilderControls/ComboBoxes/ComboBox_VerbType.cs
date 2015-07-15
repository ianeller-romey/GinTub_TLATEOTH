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
    public class ComboBox_VerbType : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxItem_newVerbType = new ComboBoxItem() { Content = "New Verb Type ..." };

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_VerbType : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int VerbTypeId { get; private set; }
            public string VerbTypeName { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_VerbType(int verbTypeId, string verbTypeName)
            {
                VerbTypeId = verbTypeId;
                SetVerbTypeName(verbTypeName);
            }

            public void SetVerbTypeName(string verbTypeName)
            {
                VerbTypeName = verbTypeName;
                Content = VerbTypeName;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_VerbType()
        {
            Items.Add(c_comboBoxItem_newVerbType);

            SelectionChanged += ComboBox_VerbType_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.VerbTypeAdded += GinTubBuilderManager_VerbTypeAdded;
            GinTubBuilderManager.VerbTypeModified += GinTubBuilderManager_VerbTypeModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.VerbTypeAdded -= GinTubBuilderManager_VerbTypeAdded;
            GinTubBuilderManager.VerbTypeModified -= GinTubBuilderManager_VerbTypeModified;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_VerbTypeAdded(object sender, GinTubBuilderManager.VerbTypeAddedEventArgs args)
        {
            if (!Items.OfType<ComboBoxItem_VerbType>().Any(i => i.VerbTypeId == args.Id))
                Items.Add(new ComboBoxItem_VerbType(args.Id, args.Name));
        }

        private void GinTubBuilderManager_VerbTypeModified(object sender, GinTubBuilderManager.VerbTypeModifiedEventArgs args)
        {
            ComboBoxItem_VerbType item = Items.OfType<ComboBoxItem_VerbType>().SingleOrDefault(i => i.VerbTypeId == args.Id);
            if (item != null)
                item.SetVerbTypeName(args.Name);
        }

        private void NewVerbTypeDialog()
        {
            Window_TextEntry window = new Window_TextEntry("Verb Type", "");
            window.Closed += (x, y) => { if (window.Accepted) GinTubBuilderManager.AddVerbType(window.Text); };
            window.Show();
        }

        private void ComboBox_VerbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if ((item = SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newVerbType)
                    NewVerbTypeDialog();
            }
        }

        #endregion

        #endregion
    }
}
