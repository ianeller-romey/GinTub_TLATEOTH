using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class Window_MessageTree : Window, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        Canvas m_canvas_messageTree;

        #endregion


        #region MEMBER PROPERTIES

        public int MessageId { get; private set; }

        #endregion


        #region MEMBER CLASSES
        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_MessageTree(int messageId, string messageName, string messageText)
        {
            MessageId = messageId;

            Title = "Message Tree";

            CreateControls();
            AddMessageTreeMessage(messageId, messageName, messageText, null);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.MessageTreeMessageRead += GinTubBuilderManager_MessageTreeMessageRead;
            GinTubBuilderManager.MessageTreeMessageChoiceRead += GinTubBuilderManager_MessageTreeMessageChoiceRead;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.MessageTreeMessageRead -= GinTubBuilderManager_MessageTreeMessageRead;
            GinTubBuilderManager.MessageTreeMessageChoiceRead -= GinTubBuilderManager_MessageTreeMessageChoiceRead;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            m_canvas_messageTree = new Canvas();

            Content = m_canvas_messageTree;
        }

        private void GinTubBuilderManager_MessageTreeMessageRead(object sender, GinTubBuilderManager.MessageTreeMessageReadEventArgs args)
        {
            var userControl_messageTreeMessage = m_canvas_messageTree.Children.OfType<UserControl_Bordered_MessageTreeMessage>().FirstOrDefault(x => x.MessageId == args.Id && x.MessageParentMessageChoiceId == args.ParentMessageChoice);
            if (userControl_messageTreeMessage == null)
            {
                userControl_messageTreeMessage = AddMessageTreeMessage(args.Id, args.Name, args.Text, args.ParentMessageChoice);
                GinTubBuilderManager.ReadMessageTreeForMessage(args.Id, args.ParentMessageChoice);
            }
        }

        private void GinTubBuilderManager_MessageTreeMessageChoiceRead(object sender, GinTubBuilderManager.MessageTreeMessageChoiceReadEventArgs args)
        {
            var userControl_messageTreeMessageChoice = m_canvas_messageTree.Children.OfType<UserControl_Bordered_MessageTreeMessageChoice>().FirstOrDefault(x => x.MessageChoiceId == args.Id);
            if (userControl_messageTreeMessageChoice == null)
                userControl_messageTreeMessageChoice = AddMessageTreeMessageChoice(args.Id, args.Name, args.Text, args.ParentMessage);
            GinTubBuilderManager.ReadMessageTreeForMessageChoice(args.Id);
        }

        private UserControl_Bordered_MessageTreeMessage AddMessageTreeMessage(int messageId, string messageName, string messageText, int? messageParentMessageChoiceId)
        {
            var userControl = new UserControl_Bordered_MessageTreeMessage(messageId, messageName, messageText, messageParentMessageChoiceId, false, false);
            userControl = SetInitialPosition(AssignMovementHandlers(userControl)) as UserControl_Bordered_MessageTreeMessage;
            m_canvas_messageTree.Children.Add(userControl);
            return userControl;
        }

        private UserControl_Bordered_MessageTreeMessageChoice AddMessageTreeMessageChoice(int messageChoiceId, string messageChoiceName, string messageChoiceText, int messageChoiceParentMessageId)
        {
            var userControl = new UserControl_Bordered_MessageTreeMessageChoice(messageChoiceId, messageChoiceName, messageChoiceText, messageChoiceParentMessageId, false, false);
            userControl = SetInitialPosition(AssignMovementHandlers(userControl)) as UserControl_Bordered_MessageTreeMessageChoice;
            m_canvas_messageTree.Children.Add(userControl);
            return userControl;
        }

        private void AttachMessageToMessageChoice(UserControl_Bordered_MessageTreeMessage userControl_messageTreeMessage)
        {
            var userControl_messageTreeMessageChoice = m_canvas_messageTree.Children.OfType<UserControl_Bordered_MessageTreeMessageChoice>().FirstOrDefault(x => x.MessageChoiceId == userControl_messageTreeMessage.MessageParentMessageChoiceId);
            if(userControl_messageTreeMessageChoice != null)
            {

            }
        }

        private FrameworkElement AssignMovementHandlers(FrameworkElement control)
        {
            // cheating using closures so we don't have to inherit from our _Bordered_ classes to make them movable
            Func<FrameworkElement, FrameworkElement> createAndAssignMovementHandlers = new Func<FrameworkElement, FrameworkElement>(
                (x) =>
                {
                    bool isDragging = false;
                    Point clickPosition = new Point(0.0, 0.0);

                    MouseButtonEventHandler Control_MouseLeftButtonDown = (sender, e) =>
                        {
                            isDragging = true;
                            var draggableControl = sender as UserControl;
                            clickPosition = e.GetPosition(this);
                            draggableControl.CaptureMouse();
                        },
                        Control_MouseLeftButtonUp = (sender, e) =>
                        {
                            isDragging = false;
                            var draggable = sender as UserControl;
                            draggable.ReleaseMouseCapture();
                        };
                    MouseEventHandler Control_MouseMove = (sender, e) =>
                        {
                            var draggableControl = sender as UserControl;

                            if (isDragging && draggableControl != null)
                            {
                                Point currentPosition = e.GetPosition(this.Parent as UIElement);

                                var transform = draggableControl.RenderTransform as TranslateTransform;
                                if (transform == null)
                                {
                                    transform = new TranslateTransform();
                                    draggableControl.RenderTransform = transform;
                                }

                                transform.X = currentPosition.X - clickPosition.X;
                                transform.Y = currentPosition.Y - clickPosition.Y;
                            }
                        };

                    x.MouseLeftButtonDown += Control_MouseLeftButtonDown;
                    x.MouseLeftButtonUp += Control_MouseLeftButtonUp;
                    x.MouseMove += Control_MouseMove;

                    return x;
                });

            return createAndAssignMovementHandlers(control);
        }

        private FrameworkElement SetInitialPosition(FrameworkElement control)
        {
            FrameworkElement lastControl = m_canvas_messageTree.Children.OfType<FrameworkElement>().LastOrDefault();
            double top = 0.0,
                   left = 0.0;
            if(lastControl != null)
            {
                top = Canvas.GetTop(lastControl) + lastControl.ActualHeight;
                left = Canvas.GetLeft(lastControl);
            }
            Canvas.SetTop(control, top);
            Canvas.SetLeft(control, left);

            return control;
        }

        #endregion

        #endregion
    }
}
