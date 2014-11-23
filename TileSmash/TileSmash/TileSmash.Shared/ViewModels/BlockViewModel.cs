namespace TileSmash.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Windows.Foundation;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Animation;
    using Windows.UI.Xaml.Media.Imaging;

    using TileSmash.Common;

    public class BlockViewModel
    {
        public event EventHandler Destroyed;
        public event EventHandler DestroyPowerUsed;

        public const int Points = 1;
        public const int StartHealth = 2;
        public const double BlockMargin = 4.5;
        public const double BlockCornerRadius = 4;
        public const double RotateAngle = 10;
        public const int AnimationDurationInMilliseconds = 20;  

        private ImageBrush stoneBrush;

        public BlockViewModel(Color color)
        {
            this.stoneBrush = new ImageBrush();
            var uri = new Uri("ms-appx:Images/stone_block.png", UriKind.Absolute);
            this.stoneBrush.ImageSource = new BitmapImage(uri);

            this.Health = StartHealth;
            this.IsStone = false;
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
            this.UIElement.IsDoubleTapEnabled = false;
            this.UIElement.Tapped += (sender, e) =>
            {
                if (this.IsStone)
                {
                    return;
                }

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

            this.UIElement.Holding += OnHoldingDestroyAllOfColor;
        }

        void OnHoldingDestroyAllOfColor(object sender, HoldingRoutedEventArgs e)
        {
            if (GameViewModel.cyclesSinceLastDestroyPowerUsed >= GameViewModel.DestroyPowerAvailableAtCycle)
            {
                this.DestroyPowerUsed(this, null);
            }
        }

        public int Health { get; private set; }

        public bool IsStone { get; private set; }

        public Border UIElement { get; private set; }

        public Color Color
        {
            get
            {
                return ((SolidColorBrush)this.UIElement.Background).Color;
            }
        }

        private void TakeDamage()
        {
            var opacityForHealthPoint = 1.0 / StartHealth;
            this.UIElement.Opacity -= opacityForHealthPoint;
            this.Health--;

            if (this.Health == 0)
            {
                this.Destroy();
            }
        }

        public void Destroy()
        {
            this.Health = StartHealth;

            if (this.Destroyed != null)
            {
                this.Destroyed(this, null);
            }
        }

        public void ResetBlockUIElement()
        {
            this.UIElement.Opacity = 1;
            var newColor = Util.GetRandomColor();
            while (newColor == this.Color)
            {
                newColor = Util.GetRandomColor();
            }

            this.UIElement.Background = new SolidColorBrush(newColor);
        }

        public void TurnToStone()
        {
            this.IsStone = true;
            this.UIElement.Opacity = 1;
            this.UIElement.Background = this.stoneBrush;
        }
    }
}
