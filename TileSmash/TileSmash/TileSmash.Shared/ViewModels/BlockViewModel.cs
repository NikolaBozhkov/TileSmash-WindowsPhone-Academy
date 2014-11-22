namespace TileSmash.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Windows.Foundation;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Animation;

    using TileSmash.Common;

    public class BlockViewModel
    {
        public event EventHandler Destroyed;

        public const int Points = 1;
        public const int StartHealth = 2;
        public const double BlockMargin = 4.5;
        public const double BlockCornerRadius = 4;
        public const double RotateAngle = 10;
        public const int AnimationDurationInMilliseconds = 20;

        public BlockViewModel(Color color)
        {
            this.Health = StartHealth;
            this.UIElement = new Border();

            this.UIElement.Background = new SolidColorBrush(color);
            this.UIElement.Margin = new Thickness(BlockMargin);
            this.UIElement.CornerRadius = new CornerRadius(BlockCornerRadius);
            this.UIElement.RenderTransformOrigin = new Point(0.5, 0.5);

            var animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(AnimationDurationInMilliseconds));
            animation.AutoReverse = true;

            var rotateTransform = new RotateTransform();
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, rotateTransform);
            Storyboard.SetTargetProperty(animation, "Angle");

            this.UIElement.RenderTransform = rotateTransform;
            this.UIElement.Tapped += (sender, e) =>
            {
                if (e.GetPosition(this.UIElement).X <= this.UIElement.ActualWidth / 2)
                {
                    animation.To = -RotateAngle;
                }
                else
                {
                    animation.To = RotateAngle;
                }

                storyboard.Begin();
                this.TakeDamage();
            };
        }

        public int Health { get; set; }

        public Border UIElement { get; private set; }

        private void TakeDamage()
        {
            var opacityForHealthPoint = 1.0 / StartHealth;
            this.UIElement.Opacity -= opacityForHealthPoint;
            this.Health--;

            if (this.Health == 0)
            {
                this.Health = StartHealth;

                if (this.Destroyed != null)
                {
                    this.Destroyed(this, null);
                }
            }
        }

        public void ResetBlockUIElement()
        {
            this.UIElement.Opacity = 1;
            var newColor = Util.Colors[Util.RandomInstance.Next(0, Util.Colors.Length)];
            this.UIElement.Background = new SolidColorBrush(newColor);
        }
    }
}
