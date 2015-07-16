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
    public class ComboBox_ParagraphStateNouns : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private static readonly char[] c_splitter = { ' ', '.', '?', '!', ',', ';', ':', '\'', '\"' };

        #endregion


        #region MEMBER PROPERTIES

        public int ParagraphStateId { get; private set; }

        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_PossibleNoun : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public string PossibleNounText { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_PossibleNoun(string possibleNounText)
            {
                SetPossibleNounText(possibleNounText);
            }

            public void SetPossibleNounText(string possibleNounText)
            {
                PossibleNounText = possibleNounText;
                Content = PossibleNounText;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_ParagraphStateNouns(int paragraphStateId)
        {
            ParagraphStateId = paragraphStateId;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ParagraphStateRead += GinTubBuilderManager_ParagraphStateEvent;
            GinTubBuilderManager.ParagraphStateUpdated += GinTubBuilderManager_ParagraphStateEvent;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ParagraphStateRead -= GinTubBuilderManager_ParagraphStateEvent;
            GinTubBuilderManager.ParagraphStateUpdated -= GinTubBuilderManager_ParagraphStateEvent;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_ParagraphStateEvent(object sender, GinTubBuilderManager.ParagraphStateEventArgs args)
        {
            if (ParagraphStateId == args.Id)
                ChangePossibleNounItems(args.Text);
        }

        private void ChangePossibleNounItems(string paragraphStateText)
        {
            string currentText = null;
            ComboBoxItem_PossibleNoun item = SelectedItem as ComboBoxItem_PossibleNoun;
            if(item != null)
                currentText = item.PossibleNounText;
            Items.Clear();
            foreach (string noun in paragraphStateText.Split(c_splitter, StringSplitOptions.RemoveEmptyEntries))
                Items.Add(new ComboBoxItem_PossibleNoun(noun));
            SelectedItem = Items.OfType<ComboBoxItem_PossibleNoun>().FirstOrDefault(i => i.PossibleNounText == currentText);
        }

        #endregion

        #endregion
    }
}
