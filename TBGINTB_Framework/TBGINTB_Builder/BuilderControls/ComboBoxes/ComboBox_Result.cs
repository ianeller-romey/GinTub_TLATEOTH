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
    public class ComboBox_Result : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxItem_newResult = new ComboBoxItem() { Content = "New Result ..." };

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_Result : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int ResultId { get; private set; }
            public string ResultName { get; private set; }
            public int ResultTypeId { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Result(int resultId, string resultName, int resultTypeId)
            {
                ResultId = resultId;
                SetResultName(resultName);
                SetResultTypeId(resultTypeId);
            }

            public void SetResultName(string resultName)
            {
                ResultName = resultName;
                Content = ResultName;
            }

            public void SetResultTypeId(int resultTypeId)
            {
                ResultTypeId = resultTypeId;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_Result()
        {
            Items.Add(c_comboBoxItem_newResult);

            SelectionChanged += ComboBox_Result_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ResultRead += GinTubBuilderManager_ResultRead;
            GinTubBuilderManager.ResultUpdated += GinTubBuilderManager_ResultUpdated;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ResultRead -= GinTubBuilderManager_ResultRead;
            GinTubBuilderManager.ResultUpdated -= GinTubBuilderManager_ResultUpdated;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_ResultRead(object sender, GinTubBuilderManager.ResultReadEventArgs args)
        {
            if (!Items.OfType<ComboBoxItem_Result>().Any(i => i.ResultId == args.Id))
                Items.Add(new ComboBoxItem_Result(args.Id, args.Name, args.ResultType));
        }

        private void GinTubBuilderManager_ResultUpdated(object sender, GinTubBuilderManager.ResultUpdatedEventArgs args)
        {
            ComboBoxItem_Result item = Items.OfType<ComboBoxItem_Result>().SingleOrDefault(i => i.ResultId == args.Id);
            if (item != null)
            {
                item.SetResultName(args.Name);
                item.SetResultTypeId(args.ResultType);
            }
        }

        private void NewResultDialog()
        {
            Window_ResultType window_resultType = 
                new Window_ResultType
                (
                    null,
                    null,
                    (win) =>
                    {
                        Window_ResultType wWin = win as Window_ResultType;
                        if (wWin != null)
                        {
                            Window_Result window_result = 
                                new Window_Result
                                (
                                    null, 
                                    null, 
                                    null,
                                    wWin.ResultTypeId.Value,
                                    (wwWin) =>
                                    {
                                        Window_Result wwwWin = wwWin as Window_Result;
                                        if (wwwWin != null)
                                            GinTubBuilderManager.CreateResult(wwwWin.Name, wwwWin.ResultJSONData, wwwWin.ResultTypeId);
                                    }
                                );
                            window_result.Show();
                        }
                    }
                );
            window_resultType.Show();
        }

        private void ComboBox_Result_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if ((item = SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newResult)
                    NewResultDialog();
            }
        }

        #endregion

        #endregion
    }
}
