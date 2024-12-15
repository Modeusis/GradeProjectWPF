using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace TournamentsApplication.Utility
{
    public class PasswordBehavior : Behavior<PasswordBox>
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                "Password",
                typeof(string),
                typeof(PasswordBehavior),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPasswordPropertyChanged));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.Password != Password)
            {
                Password = AssociatedObject.Password;
            }
        }

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (PasswordBehavior)d;
            if (behavior.AssociatedObject != null && behavior.AssociatedObject.Password != (string)e.NewValue)
            {
                behavior.AssociatedObject.Password = (string)e.NewValue;
            }
        }
    }
}
