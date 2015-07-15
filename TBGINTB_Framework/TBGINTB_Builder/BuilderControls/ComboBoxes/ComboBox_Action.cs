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
    public class ComboBox_Action : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxItem_newAction = new ComboBoxItem() { Content = "New Action ..." };

        #endregion


        #region MEMBER PROPERTIES

        public int NounId { get; private set; }
        public int ParagraphStateId { get; private set; }

        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_Action : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int ActionId { get; private set; }
            public int ActionVerbType { get; private set; }
            public int ActionNoun { get; private set; }
            public string ActionName { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Action(int actionId, int actionVerbType, int actionNoun, string actionName)
            {
                ActionId = actionId;
                SetActionVerbType(actionVerbType);
                SetActionNoun(actionNoun);
                SetActionName(actionName);
            }

            public void SetActionVerbType(int actionVerbType)
            {
                ActionVerbType = actionVerbType;
            }

            public void SetActionNoun(int actionNoun)
            {
                ActionNoun = actionNoun;
            }

            public void SetActionName(string actionName)
            {
                ActionName = actionName;
                Content = ActionName;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_Action(int nounId, int paragraphStateId)
        {
            NounId = nounId;
            ParagraphStateId = paragraphStateId;

            Items.Add(c_comboBoxItem_newAction);

            SelectionChanged += ComboBox_Action_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ActionAdded += GinTubBuilderManager_ActionAdded;
            GinTubBuilderManager.ActionModified += GinTubBuilderManager_ActionModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ActionAdded -= GinTubBuilderManager_ActionAdded;
            GinTubBuilderManager.ActionModified -= GinTubBuilderManager_ActionModified;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_ActionAdded(object sender, GinTubBuilderManager.ActionAddedEventArgs args)
        {
            if (NounId == args.Noun)
            {
                if (!Items.OfType<ComboBoxItem_Action>().Any(i => i.ActionId == args.Id))
                    Items.Add(new ComboBoxItem_Action(args.Id, args.Noun, args.VerbType, args.Name));
            }
        }

        private void GinTubBuilderManager_ActionModified(object sender, GinTubBuilderManager.ActionModifiedEventArgs args)
        {
            if (NounId == args.Noun)
            {
                ComboBoxItem_Action item = Items.OfType<ComboBoxItem_Action>().SingleOrDefault(i => i.ActionId == args.Id);
                if (item != null)
                {
                    item.SetActionVerbType(args.VerbType);
                    item.SetActionNoun(args.Noun);
                    item.SetActionName(args.Name);
                }
            }
        }

        private void NewActionDialog()
        {
            Window_Action window = 
                new Window_Action
                (
                    null, 
                    null, 
                    NounId, 
                    ParagraphStateId,
                    (win) =>
                    {
                        Window_Action wWin = win as Window_Action;
                        if (wWin != null)
                            GinTubBuilderManager.AddAction(wWin.ActionVerbType.Value, wWin.ActionNoun.Value);
                    }
                );
            window.Show();
        }

        private void ComboBox_Action_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if ((item = SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newAction)
                    NewActionDialog();
            }
        }

        #endregion

        #endregion
    }
}
