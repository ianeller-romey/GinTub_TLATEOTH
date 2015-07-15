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
    public class ComboBox_ResultType : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxItem_newResultType = new ComboBoxItem() { Content = "New Result Type ..." };

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_ResultType : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int ResultTypeId { get; private set; }
            public string ResultTypeName { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_ResultType(int resultTypeId, string resultTypeName)
            {
                ResultTypeId = resultTypeId;
                SetResultTypeName(resultTypeName);
            }

            public void SetResultTypeName(string resultTypeName)
            {
                ResultTypeName = resultTypeName;
                Content = ResultTypeName;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_ResultType()
        {
            Items.Add(c_comboBoxItem_newResultType);

            SelectionChanged += ComboBox_ResultType_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ResultTypeAdded += GinTubBuilderManager_ResultTypeAdded;
            GinTubBuilderManager.ResultTypeModified += GinTubBuilderManager_ResultTypeModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ResultTypeAdded -= GinTubBuilderManager_ResultTypeAdded;
            GinTubBuilderManager.ResultTypeModified -= GinTubBuilderManager_ResultTypeModified;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_ResultTypeAdded(object sender, GinTubBuilderManager.ResultTypeAddedEventArgs args)
        {
            if (!Items.OfType<ComboBoxItem_ResultType>().Any(i => i.ResultTypeId == args.Id))
                Items.Add(new ComboBoxItem_ResultType(args.Id, args.Name));
        }

        private void GinTubBuilderManager_ResultTypeModified(object sender, GinTubBuilderManager.ResultTypeModifiedEventArgs args)
        {
            ComboBoxItem_ResultType item = Items.OfType<ComboBoxItem_ResultType>().SingleOrDefault(i => i.ResultTypeId == args.Id);
            if (item != null)
                item.SetResultTypeName(args.Name);
        }

        private void NewResultTypeDialog()
        {
            Window_TextEntry window = new Window_TextEntry("Result Type", "");
            window.Closed += (x, y) => { if (window.Accepted) GinTubBuilderManager.AddResultType(window.Text); };
            window.Show();
        }

        private void ComboBox_ResultType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if ((item = SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newResultType)
                    NewResultTypeDialog();
            }
        }

        #endregion

        #endregion
    }
}
