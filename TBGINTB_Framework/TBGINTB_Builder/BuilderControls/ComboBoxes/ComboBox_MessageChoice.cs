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
    public class ComboBox_MessageChoice : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxItem_newMessageChoice = new ComboBoxItem() { Content = "New Message Choice ..." };

        #endregion


        #region MEMBER PROPERTIES

        public int MessageId { get; private set; }

        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_MessageChoice : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int MessageChoiceId { get; private set; }
            public string MessageChoiceName { get; private set; }
            public string MessageChoiceText { get; private set; }
            public int MessageId { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_MessageChoice(int messageChoiceId, string messageChoiceName, string messageChoiceText, int messageId)
            {
                MessageChoiceId = messageChoiceId;
                SetMessageChoiceName(messageChoiceName);
                SetMessageChoiceText(messageChoiceText);
                SetMessageId(messageId);
            }

            public void SetMessageChoiceName(string messageChoiceName)
            {
                MessageChoiceName = messageChoiceName;
                Content = MessageChoiceName;
            }

            public void SetMessageChoiceText(string messageChoiceNoun)
            {
                MessageChoiceText = messageChoiceNoun;
            }

            public void SetMessageId(int messageId)
            {
                MessageId = messageId;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_MessageChoice(int messageId)
        {
            MessageId = messageId;

            Items.Add(c_comboBoxItem_newMessageChoice);

            SelectionChanged += ComboBox_MessageChoice_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.MessageChoiceAdded += GinTubBuilderManager_MessageChoiceAdded;
            GinTubBuilderManager.MessageChoiceModified += GinTubBuilderManager_MessageChoiceModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.MessageChoiceAdded -= GinTubBuilderManager_MessageChoiceAdded;
            GinTubBuilderManager.MessageChoiceModified -= GinTubBuilderManager_MessageChoiceModified;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_MessageChoiceAdded(object sender, GinTubBuilderManager.MessageChoiceAddedEventArgs args)
        {
            if (MessageId == args.Message)
            {
                if (!Items.OfType<ComboBoxItem_MessageChoice>().Any(i => i.MessageChoiceId == args.Id))
                    Items.Add(new ComboBoxItem_MessageChoice(args.Id, args.Name, args.Text, args.Message));
            }
        }

        private void GinTubBuilderManager_MessageChoiceModified(object sender, GinTubBuilderManager.MessageChoiceModifiedEventArgs args)
        {
            if (MessageId == args.Message)
            {
                ComboBoxItem_MessageChoice item = Items.OfType<ComboBoxItem_MessageChoice>().SingleOrDefault(i => i.MessageChoiceId == args.Id);
                if (item != null)
                {
                    item.SetMessageChoiceName(args.Name);
                    item.SetMessageChoiceText(args.Text);
                    item.SetMessageId(args.Message);
                }
            }
        }

        private void NewMessageChoiceDialog()
        {
            Window_MessageChoice window = 
                new Window_MessageChoice
                (
                    null, 
                    null, 
                    null,
                    MessageId,
                    (win) =>
                    {
                        Window_MessageChoice wWin = win as Window_MessageChoice;
                        if (wWin != null)
                            GinTubBuilderManager.AddMessageChoice
                            (
                                wWin.MessageChoiceName,
                                wWin.MessageChoiceText,
                                wWin.MessageId
                            );
                    }
                );
            window.Show();
        }

        private void ComboBox_MessageChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if ((item = SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newMessageChoice)
                    NewMessageChoiceDialog();
            }
        }

        #endregion

        #endregion
    }
}
