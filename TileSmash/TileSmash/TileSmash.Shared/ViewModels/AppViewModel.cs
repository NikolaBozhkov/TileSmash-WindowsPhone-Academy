namespace TileSmash.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Windows.Foundation;
    using Windows.Storage;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Animation;
    using Windows.UI.Xaml.Shapes;

    using TileSmash.Common;

    public class AppViewModel : ViewModelBase
    {
        public const string BestKey = "Best";

        public AppViewModel()
        {
            var randomColorBrush = new SolidColorBrush(Util.GetRandomColor());

            int best;
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(BestKey))
            {
                best = (int)(ApplicationData.Current.LocalSettings.Values[BestKey]);
            }
            else
            {
                best = 0;            
            }

            this.GameModel = new GameViewModel(best, randomColorBrush);
        }

        public GameViewModel GameModel { get; set; }

        private BlockViewModel CreateBlock(int maximumBlocksOfColor)
        {
            var randomColor = Util.GetRandomColor();
            while (this.GameModel.ColorBlockCounts[randomColor] >= maximumBlocksOfColor)
            {
                randomColor = Util.GetRandomColor();
            }

            var block = new BlockViewModel(randomColor);

            ++this.GameModel.ColorBlockCounts[randomColor];
            return block;
        }

        public void InitializeGameGrid(Grid gameGrid)
        {
            var blocksCount = gameGrid.RowDefinitions.Count * gameGrid.ColumnDefinitions.Count;
            var maximumBlocksOfColor = blocksCount / Util.Colors.Length;

            for (int i = 0; i < gameGrid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < gameGrid.ColumnDefinitions.Count; j++)
                {
                    var block = CreateBlock(maximumBlocksOfColor);

                    Grid.SetRow(block.UIElement, i);
                    Grid.SetColumn(block.UIElement, j);

                    this.GameModel.Blocks.Add(block);
                    block.Destroyed += this.GameModel.HandleBlockDestroyed;
                    block.DestroyPowerUsed += this.GameModel.HandleDestroyPowerUsed;

                    gameGrid.Children.Add(block.UIElement);
                }
            }

            this.GameModel.ToggleBlocksTap(false);
        }
    }
}
