using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Builder.Commands
{
    public class EventToCommandBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommandBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommandBehavior), new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public string EventName { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                var eventInfo = AssociatedObject.GetType().GetEvent(EventName);
                if (eventInfo != null)
                {
                    var methodInfo = GetType().GetMethod("OnEvent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);
                    eventInfo.AddEventHandler(AssociatedObject, handler);
                }
            }
        }

        private void OnEvent(object sender, EventArgs e)
        {
            var multiBindingExpr = BindingOperations.GetMultiBindingExpression(this, CommandParameterProperty);
            if (multiBindingExpr != null)
            {
                multiBindingExpr.UpdateTarget();
            }

            object parameter = CommandParameter ?? e;
            if (Command != null && Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
            }
        }
    }
}
