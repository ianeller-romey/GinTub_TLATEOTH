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
    public class ComboBox_Noun : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxItem_newNoun = new ComboBoxItem() { Content = "New Noun ..." };

        #endregion


        #region MEMBER PROPERTIES

        public int ParagraphStateId { get; private set; }

        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_Noun : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int NounId { get; private set; }
            public string NounText { get; private set; }
            public int ParagraphStateId { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Noun(int nounId, string nounText, int paragraphStateId)
            {
                NounId = nounId;
                SetNounText(nounText);
                SetParagraphStateId(paragraphStateId);
            }

            public void SetNounText(string nounText)
            {
                NounText = nounText;
                Content = NounText;
            }

            public void SetParagraphStateId(int paragraphStateId)
            {
                ParagraphStateId = paragraphStateId;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_Noun(int paragraphStateId)
        {
            ParagraphStateId = paragraphStateId;

            Items.Add(c_comboBoxItem_newNoun);

            SelectionChanged += ComboBox_Noun_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.NounAdded += GinTubBuilderManager_NounAdded;
            GinTubBuilderManager.NounModified += GinTubBuilderManager_NounModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.NounAdded -= GinTubBuilderManager_NounAdded;
            GinTubBuilderManager.NounModified -= GinTubBuilderManager_NounModified;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_NounAdded(object sender, GinTubBuilderManager.NounAddedEventArgs args)
        {
            if (ParagraphStateId == args.ParagraphState)
            {
                if (!Items.OfType<ComboBoxItem_Noun>().Any(i => i.NounId == args.Id))
                    Items.Add(new ComboBoxItem_Noun(args.Id, args.Text, args.ParagraphState));
            }
        }

        private void GinTubBuilderManager_NounModified(object sender, GinTubBuilderManager.NounModifiedEventArgs args)
        {
            if (ParagraphStateId == args.ParagraphState)
            {
                ComboBoxItem_Noun item = Items.OfType<ComboBoxItem_Noun>().SingleOrDefault(i => i.NounId == args.Id);
                if (item != null)
                {
                    item.SetNounText(args.Text);
                    item.SetParagraphStateId(args.ParagraphState);
                }
            }
        }

        private void NewNounDialog()
        {
            Window_Noun window = 
                new Window_Noun
                (
                    null, 
                    null,
                    ParagraphStateId,
                    (win) =>
                    {
                        Window_Noun wWin = win as Window_Noun;
                        if (wWin != null)
                            GinTubBuilderManager.AddNoun(wWin.NounText, wWin.ParagraphStateId);
                    }
                );
            window.Show();
        }

        private void ComboBox_Noun_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if ((item = SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newNoun)
                    NewNounDialog();
            }
        }

        #endregion

        #endregion
    }
}
