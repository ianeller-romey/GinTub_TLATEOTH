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

            ScrollViewer scrollViewer_messageTree =
                new ScrollViewer()
                {
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                    Content = m_canvas_messageTree
                };

            Content = scrollViewer_messageTree;
        }

        private void GinTubBuilderManager_MessageTreeMessageRead(object sender, GinTubBuilderManager.MessageTreeMessageReadEventArgs args)
        {
            var userControl_messageTreeMessage = m_canvas_messageTree.Children.OfType<UserControl_Bordered_MessageTreeMessage>().FirstOrDefault(x => x.MessageId == args.Id && x.MessageParentMessageChoiceId == args.ParentMessageChoice);
            if (userControl_messageTreeMessage == null)
            {
                userControl_messageTreeMessage = AddMessageTreeMessage(args.Id, args.Name, args.Text, args.ParentMessageChoice);

                AttachTreeControlToParent(userControl_messageTreeMessage, m_canvas_messageTree.Children.OfType<UserControl_Bordered_MessageTreeMessageChoice>().FirstOrDefault(x => x.MessageChoiceId == args.ParentMessageChoice));

                GinTubBuilderManager.ReadMessageTreeForMessage(args.Id, args.ParentMessageChoice);
            }
        }

        private void GinTubBuilderManager_MessageTreeMessageChoiceRead(object sender, GinTubBuilderManager.MessageTreeMessageChoiceReadEventArgs args)
        {
            var userControl_messageTreeMessageChoice = m_canvas_messageTree.Children.OfType<UserControl_Bordered_MessageTreeMessageChoice>().FirstOrDefault(x => x.MessageChoiceId == args.Id);
            if (userControl_messageTreeMessageChoice == null)
            {
                userControl_messageTreeMessageChoice = AddMessageTreeMessageChoice(args.Id, args.Name, args.Text, args.ParentMessage);

                AttachTreeControlToParent(userControl_messageTreeMessageChoice, m_canvas_messageTree.Children.OfType<UserControl_Bordered_MessageTreeMessage>().FirstOrDefault(x => x.MessageId == args.ParentMessage));

                GinTubBuilderManager.ReadMessageTreeForMessageChoice(args.Id);
            }
        }

        private UserControl_Bordered_MessageTreeMessage AddMessageTreeMessage(int messageId, string messageName, string messageText, int? messageParentMessageChoiceId)
        {
            var userControl = new UserControl_Bordered_MessageTreeMessage(messageId, messageName, messageText, messageParentMessageChoiceId, false, false);
            userControl = SetInitialPosition(AssignMovementHandlers(userControl)) as UserControl_Bordered_MessageTreeMessage;
            EnableMessageAppending(userControl);
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

        private void AttachTreeControlToParent(FrameworkElement control, FrameworkElement parent)
        {
            if(parent != null)
            {
                Line line = new Line() { StrokeThickness = 2.5, Stroke = Brushes.Black, SnapsToDevicePixels = true };
                line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                m_canvas_messageTree.Children.Add(line);

                parent.LayoutUpdated += (x, y) =>
                    {
                        line.Y1 = Canvas.GetTop(parent) + parent.ActualHeight;
                        line.X1 = Canvas.GetLeft(parent) + (parent.ActualWidth / 2);
                    };
                parent.UpdateLayout();

                control.LayoutUpdated += (x, y) =>
                {
                    line.Y2 = Canvas.GetTop(control);
                    line.X2 = Canvas.GetLeft(control) + (control.ActualWidth / 2);
                };
                control.UpdateLayout();
            }
        }

        private FrameworkElement AssignMovementHandlers(FrameworkElement control)
        {
            // cheating using closures so we don't have to inherit from our _Bordered_ classes to make them movable
            Func<FrameworkElement, FrameworkElement> createAndAssignMovementHandlers = new Func<FrameworkElement, FrameworkElement>(
                (x) =>
                {
                    bool isDragging = false;

                    MouseButtonEventHandler Control_MouseLeftButtonDown = (sender, e) =>
                        {
                            isDragging = true;
                            var draggableControl = sender as UserControl;
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
                                Point currentPosition = e.GetPosition(m_canvas_messageTree);

                                Canvas.SetTop(x, currentPosition.Y);
                                Canvas.SetLeft(x, currentPosition.X);
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
            FrameworkElement lastControl = m_canvas_messageTree.Children.OfType<UserControl>().LastOrDefault();
            double top = 0.0,
                   left = 0.0;
            if(lastControl != null)
            {
                lastControl.UpdateLayout();
                top = Canvas.GetTop(lastControl) + lastControl.ActualHeight + 50.0;
                left = Canvas.GetLeft(lastControl);
            }
            Canvas.SetTop(control, top);
            Canvas.SetLeft(control, left);
            control.UpdateLayout();

            return control;
        }

        private void EnableMessageAppending(UserControl_Bordered_MessageTreeMessage control)
        {
            MenuItem menuItem_messageAppending = new MenuItem() { Header = "Append Message" };
            menuItem_messageAppending.Click += (x, y) =>
                {
                    var window_messageChoice =
                        new Window_MessageChoice
                        (
                            null,
                            string.Format("{0} - XYZ Choice", control.MessageName),
                            "\"...\"",
                            MessageId,
                            (win) =>
                            {
                                Window_MessageChoice wWin = win as Window_MessageChoice;
                                if (wWin != null)
                                {
                                    var window_message =
                                        new Window_Message
                                        (
                                            null,
                                            string.Format("{0} NEXT", control.MessageName),
                                            "...",
                                            (wwWin) =>
                                            {
                                                Window_Message wwwWin = wwWin as Window_Message;
                                                if (wwwWin != null)
                                                {
                                                    // This is a preeeetty hack-y way to handle this, but you reap what you sow
                                                    int
                                                        newMessageId = -1,
                                                        newMessageChoiceId = -1,
                                                        newResultId = -1;
                                                    GinTubBuilderManager.MessageReadEventHandler messageReadHandler = new GinTubBuilderManager.MessageReadEventHandler((sender, args) =>
                                                        {
                                                            newMessageId = args.Id;
                                                        });
                                                    GinTubBuilderManager.MessageChoiceReadEventHandler messageChoiceReadHandler = new GinTubBuilderManager.MessageChoiceReadEventHandler((sender, args) =>
                                                        {
                                                            newMessageChoiceId = args.Id;
                                                        });
                                                    GinTubBuilderManager.ResultReadEventHandler resultReadHandler = new GinTubBuilderManager.ResultReadEventHandler((sender, args) =>
                                                        {
                                                            newResultId = args.Id;
                                                        });

                                                    GinTubBuilderManager.MessageRead += messageReadHandler;
                                                    GinTubBuilderManager.CreateMessage(wwwWin.MessageName, wwwWin.MessageText);
                                                    GinTubBuilderManager.MessageRead -= messageReadHandler;

                                                    GinTubBuilderManager.MessageChoiceRead += messageChoiceReadHandler;
                                                    GinTubBuilderManager.CreateMessageChoice(wWin.MessageChoiceName, wWin.MessageChoiceText, control.MessageId.Value);
                                                    GinTubBuilderManager.MessageChoiceRead -= messageChoiceReadHandler;

                                                    GinTubBuilderManager.ResultRead += resultReadHandler;
                                                    // TODO: less hardcoding here
                                                    GinTubBuilderManager.CreateResult
                                                    (
                                                        string.Format("Message - {0} NEXT", control.MessageName),
                                                        "{\"messageId\":" + newMessageId.ToString() + "}",
                                                        10 // MAGIC NUMBER
                                                    );
                                                    GinTubBuilderManager.ResultRead -= resultReadHandler;

                                                    GinTubBuilderManager.CreateMessageChoiceResult(newResultId, newMessageChoiceId);

                                                    GinTubBuilderManager.ReadMessageTreeForMessage(control.MessageId.Value, control.MessageParentMessageChoiceId);
                                                }
                                            }
                                        );
                                }
                            }
                        );
                    
                };

            ContextMenu contextMenu_messageAppending = new ContextMenu();
            contextMenu_messageAppending.Items.Add(menuItem_messageAppending);

            control.ContextMenu = contextMenu_messageAppending;
        }

        #endregion

        #endregion
    }
}
