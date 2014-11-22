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
    using Windows.UI.Xaml.Shapes;

    using TileSmash.Common;

    public class AppViewModel : ViewModelBase
    {
        public AppViewModel()
        {
            var randomColorBrush = new SolidColorBrush(Util.Colors[Util.RandomInstance.Next(0, Util.Colors.Length)]);
            this.GameModel = new GameViewModel(0, randomColorBrush);
        }

        public GameViewModel GameModel { get; set; }

        private BlockViewModel CreateBlock(int maximumBlocksOfColor)
        {
            var randomColorIndex = Util.RandomInstance.Next(0, Util.Colors.Length);
            while (this.GameModel.ColorBlockCounts[Util.Colors[randomColorIndex]] >= maximumBlocksOfColor)
            {
                randomColorIndex = Util.RandomInstance.Next(0, Util.Colors.Length);
            }

            var block = new BlockViewModel(Util.Colors[randomColorIndex]);

            ++this.GameModel.ColorBlockCounts[Util.Colors[randomColorIndex]];
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

                    gameGrid.Children.Add(block.UIElement);
                }
            }
        }
    }
}
