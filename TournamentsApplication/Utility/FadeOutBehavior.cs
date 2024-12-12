using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Xaml.Behaviors;

namespace TournamentsApplication.Utility
{
    public class FadeOutBehavior : Behavior<Border>
    {
        public static readonly DependencyProperty StatusOpacityProperty =
            DependencyProperty.Register("StatusOpacity", typeof(double), typeof(FadeOutBehavior), new PropertyMetadata(1.0, OnStatusOpacityChanged));

        public double StatusOpacity
        {
            get => (double)GetValue(StatusOpacityProperty);
            set => SetValue(StatusOpacityProperty, value);
        }
        private static void OnStatusOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (FadeOutBehavior)d;
            if (behavior.StatusOpacity == 1)
            {
                behavior.StartFadeInAndOutAnimations();
                StatusService.Instance.ClearStatus();
            }
        }

        private void StartFadeInAndOutAnimations()
        {
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0.2,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };

            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                BeginTime = TimeSpan.FromSeconds(4),
            };

            var moveAnimation = new DoubleAnimation
            {
                From = -150,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.25),
            };

            var moveOutAnimation = new DoubleAnimation
            {
                From = 0,
                To = -100,
                Duration = TimeSpan.FromSeconds(0.5),
                BeginTime = TimeSpan.FromSeconds(4),
            };

            var storyboard = new Storyboard();

            Storyboard.SetTarget(fadeInAnimation, AssociatedObject);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(moveAnimation, AssociatedObject);
            Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            Storyboard.SetTarget(fadeOutAnimation, AssociatedObject);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(moveOutAnimation, AssociatedObject);
            Storyboard.SetTargetProperty(moveOutAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            AssociatedObject.RenderTransform = new TranslateTransform();

            storyboard.Children.Add(fadeInAnimation);
            storyboard.Children.Add(moveAnimation);
            storyboard.Children.Add(fadeOutAnimation);
            storyboard.Children.Add(moveOutAnimation);

            storyboard.Begin();
        }
    }
}
