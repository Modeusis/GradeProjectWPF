using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace TournamentsApplication.Utility
{
    public class DragWindowBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
        }

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var window = Window.GetWindow(this.AssociatedObject);
                if (window != null)
                {
                    window.DragMove(); 
                }
            }
        }
    }
}
