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
    public class ComboBox_Message : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxMessage_newMessage = new ComboBoxItem() { Content = "New Message ..." };

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_Message : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int MessageId { get; private set; }
            public string MessageName { get; private set; }
            public string MessageText { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Message(int messageId, string messageName, string messageText)
            {
                MessageId = messageId;
                SetMessageName(messageName);
                SetMessageText(messageText);
            }

            public void SetMessageName(string messageName)
            {
                MessageName = messageName;
                Content = MessageName;
            }

            public void SetMessageText(string messageText)
            {
                MessageText = messageText;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_Message()
        {
            Items.Add(c_comboBoxMessage_newMessage);

            SelectionChanged += ComboBox_Message_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.MessageRead += GinTubBuilderManager_MessageRead;
            GinTubBuilderManager.MessageUpdated += GinTubBuilderManager_MessageUpdated;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.MessageRead -= GinTubBuilderManager_MessageRead;
            GinTubBuilderManager.MessageUpdated -= GinTubBuilderManager_MessageUpdated;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_MessageRead(object sender, GinTubBuilderManager.MessageReadEventArgs args)
        {
            if (!Items.OfType<ComboBoxItem_Message>().Any(i => i.MessageId == args.Id))
                Items.Add(new ComboBoxItem_Message(args.Id, args.Name, args.Text));
        }

        private void GinTubBuilderManager_MessageUpdated(object sender, GinTubBuilderManager.MessageUpdatedEventArgs args)
        {
            ComboBoxItem_Message message = Items.OfType<ComboBoxItem_Message>().SingleOrDefault(i => i.MessageId == args.Id);
            if (message != null)
            {
                message.SetMessageName(args.Name);
                message.SetMessageText(args.Text);
            }
        }

        private void NewMessageDialog()
        {
            Window_Message window = 
                new Window_Message
                (
                    null, 
                    null,
                    null,
                    (win) =>
                    {
                        Window_Message wWin = win as Window_Message;
                        if (wWin != null)
                            GinTubBuilderManager.CreateMessage(wWin.MessageName, wWin.MessageText);
                    }
                );
            window.Show();
        }

        private void ComboBox_Message_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem message = null;
            if ((message = SelectedItem as ComboBoxItem) != null)
            {
                if (message == c_comboBoxMessage_newMessage)
                    NewMessageDialog();
            }
        }

        #endregion

        #endregion
    }
}
