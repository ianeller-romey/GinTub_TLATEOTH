﻿using System;
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
    public class ComboBox_JSONPropertyDataType : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS
        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_JSONPropertyDataType : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int JSONPropertyDataTypeId { get; private set; }
            public string JSONPropertyDataTypeDataType { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_JSONPropertyDataType(int jsonPropertyDataTypeId, string jsonPropertyDataTypeDataType)
            {
                JSONPropertyDataTypeId = jsonPropertyDataTypeId;
                SetJSONPropertyDataTypeDataType(jsonPropertyDataTypeDataType);
            }

            public void SetJSONPropertyDataTypeDataType(string jsonPropertyDataTypeDataType)
            {
                JSONPropertyDataTypeDataType = jsonPropertyDataTypeDataType;
                Content = JSONPropertyDataTypeDataType;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_JSONPropertyDataType()
        {
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.JSONPropertyDataTypeAdded += GinTubBuilderManager_JSONPropertyDataTypeAdded;
            GinTubBuilderManager.JSONPropertyDataTypeModified += GinTubBuilderManager_JSONPropertyDataTypeModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.JSONPropertyDataTypeAdded -= GinTubBuilderManager_JSONPropertyDataTypeAdded;
            GinTubBuilderManager.JSONPropertyDataTypeModified -= GinTubBuilderManager_JSONPropertyDataTypeModified;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_JSONPropertyDataTypeAdded(object sender, GinTubBuilderManager.JSONPropertyDataTypeAddedEventArgs args)
        {
            if (!Items.OfType<ComboBoxItem_JSONPropertyDataType>().Any(i => i.JSONPropertyDataTypeId == args.Id))
                Items.Add(new ComboBoxItem_JSONPropertyDataType(args.Id, args.DataType));
        }

        private void GinTubBuilderManager_JSONPropertyDataTypeModified(object sender, GinTubBuilderManager.JSONPropertyDataTypeModifiedEventArgs args)
        {
            ComboBoxItem_JSONPropertyDataType item = Items.OfType<ComboBoxItem_JSONPropertyDataType>().SingleOrDefault(i => i.JSONPropertyDataTypeId == args.Id);
            if (item != null)
                item.SetJSONPropertyDataTypeDataType(args.DataType);
        }

        #endregion

        #endregion
    }
}
