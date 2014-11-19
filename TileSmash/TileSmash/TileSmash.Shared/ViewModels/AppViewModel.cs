namespace TileSmash.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    public class AppViewModel: ViewModelBase
    {
        private static Random random = new Random();

        public AppViewModel()
        {
            this.Hud = new HudViewModel(0);
        }

        public HudViewModel Hud { get; set; }

        public void InitializeGameGrid(Grid gameGrid)
        {
            var colors = new Color[] 
            {
                Color.FromArgb(255, 251, 160, 38), // Neon Carrot
                Color.FromArgb(255, 225, 73, 56), // Well Read
                Color.FromArgb(255, 65, 168, 95), // Chateau Green
                Color.FromArgb(255, 41, 105, 176), // Denim
                Color.FromArgb(255, 250, 197, 28) // Turbo
            };

            for (int i = 0; i < gameGrid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < gameGrid.ColumnDefinitions.Count; j++)
                {
                    var border = new Border();
                    border.Background = new SolidColorBrush(colors[random.Next(0, colors.Length)]);
                    border.Margin = new Thickness(4.5);
                    border.CornerRadius = new CornerRadius(4);
                    border.BorderThickness = new Thickness(1);
                    border.BorderBrush = new SolidColorBrush(Colors.Black);
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);

                    gameGrid.Children.Add(border);
                }
            }
        }
    }
}
