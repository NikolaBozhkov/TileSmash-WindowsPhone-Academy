namespace TileSmash.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    using Windows.UI.Xaml.Media;

    using TileSmash.Common;
    using Windows.UI;

    public class GameViewModel: ViewModelBase
    {
        public const int MaxStonesInField = 10;
        public const int SecondsToDestroy = 5;
        public const int StreakBonusPoints = 5;

        private int score;
        private int best;
        private TimeSpan timeLeft;
        private int stones;

        public GameViewModel(int best, SolidColorBrush currentColor)
        {
            this.Best = best;
            this.CurrentColor = currentColor;
            this.Score = 0;
            this.Stones = 0;
            this.TimeLeft = TimeSpan.FromSeconds(SecondsToDestroy);
            this.Blocks = new List<BlockViewModel>();
            this.ColorBlockCounts = new Dictionary<Color, int>();
            foreach (var color in Util.Colors)
            {
                this.ColorBlockCounts[color] = 0;
            }
        }

        public int MaxStones
        {
            get
            {
                return MaxStonesInField;
            }
        }

        public int Stones
        {
            get
            {
                return this.stones;
            }

            set
            {
                this.stones = value;
                this.NotifyPropertyChanged("Stones");
            }
        }

        public int Score
        {
            get
            {
                return this.score;
            }

            set
            {
                this.score = value;
                this.NotifyPropertyChanged("Score");
            }
        }

        public int Best
        {
            get
            {
                return this.best;
            }

            set
            {
                this.best = value;
                this.NotifyPropertyChanged("Best");
            }
        }

        public TimeSpan TimeLeft
        {
            get
            {
                return this.timeLeft;
            }

            set
            {
                this.timeLeft = value;
                this.NotifyPropertyChanged("TimeLeft");
            }
        }

        public string TimeLeftFormatted
        {
            get
            {
                return string.Format("{0:s\\.ff}", this.TimeLeft);
            }
        }

        public SolidColorBrush CurrentColor { get; set; }

        public IDictionary<Color, int> ColorBlockCounts { get; private set; }

        public IList<BlockViewModel> Blocks { get; private set; }

        public void HandleBlockDestroyed(object sender, EventArgs e)
        {
            var block = (BlockViewModel)sender;
            var blockBrush = (SolidColorBrush)block.UIElement.Background;
            block.UIElement.Background = new SolidColorBrush(Util.Colors[0]);
            --this.ColorBlockCounts[blockBrush.Color];

            if (this.CurrentColor.Color == blockBrush.Color)
            {
                this.Score += BlockViewModel.Points;
                this.Best = this.Score > this.Best ? this.Score : this.Best;
            }

            block.ResetBlockUIElement();
            ++this.ColorBlockCounts[((SolidColorBrush)block.UIElement.Background).Color];
        }
    }
}