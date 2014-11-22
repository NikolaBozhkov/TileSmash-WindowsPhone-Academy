namespace TileSmash.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;

    using TileSmash.Common;
    using System.Threading;
    using System.Diagnostics;

    public class GameViewModel: ViewModelBase
    {
        public const int MaxStonesInField = 10;
        public const int SecondsToDestroy = 7;
        public const int StreakBonusPoints = 5;

        private int score;
        private int best;
        private TimeSpan timeLeft;
        private int stones;
        private SolidColorBrush currentColor;

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

            this.RunTimer();
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
                this.NotifyPropertyChanged("TimeLeftFormatted");
            }
        }

        public string TimeLeftFormatted
        {
            get
            {
                return string.Format("{0:s\\.ff}", this.TimeLeft);
            }
        }

        public SolidColorBrush CurrentColor
        {
            get
            {
                return this.currentColor;
            }

            set
            {
                this.currentColor = value;
                this.NotifyPropertyChanged("CurrentColor");
            }
        }

        public IDictionary<Color, int> ColorBlockCounts { get; private set; }

        public IList<BlockViewModel> Blocks { get; private set; }

        private void RunTimer()
        {
            var sw = new Stopwatch();
            sw.Start();

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += (sender, e) =>
            {
                var currentLeftMillisec = -1 * (sw.ElapsedMilliseconds - (SecondsToDestroy * 1000));
                this.TimeLeft = TimeSpan.FromMilliseconds(currentLeftMillisec);
                if (this.TimeLeft.TotalMilliseconds <= 0)
                {
                    sw.Restart();

                    var randomColor = Util.GetRandomColor();
                    while (this.ColorBlockCounts[randomColor] <= 0)
                    {
                        randomColor = Util.GetRandomColor();
                    }

                    this.CurrentColor = new SolidColorBrush(randomColor);
                }
            };

            timer.Start();
        }

        public void HandleBlockDestroyed(object sender, EventArgs e)
        {
            var block = (BlockViewModel)sender;
            var blockBrush = (SolidColorBrush)block.UIElement.Background;
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