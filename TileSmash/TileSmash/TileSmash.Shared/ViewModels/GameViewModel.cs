namespace TileSmash.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Windows.Storage;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Controls;

    using TileSmash.Common;

    public class GameViewModel: ViewModelBase
    {
        public static int cyclesSinceLastDestroyPowerUsed;
        public static int best;

        public const int MaxStonesInField = 10;
        public const int SecondsToDestroy = 5;
        public const int StreakBonusPoints = 5;
        public const int DestroyPowerAvailableAtCycle = 3;

        private int score;
        private int streakCount = 0;
        private TimeSpan timeLeft;
        private int stones;
        private SolidColorBrush currentColor;

        public GameViewModel(int best, SolidColorBrush currentColor)
        {
            cyclesSinceLastDestroyPowerUsed = DestroyPowerAvailableAtCycle;
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
                this.Best = value > this.Best ? value : this.Best;
                this.NotifyPropertyChanged("Score");
            }
        }

        public int Best
        {
            get
            {
                return best;
            }

            set
            {
                best = value;
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

        public void StartGame()
        {
            this.ToggleBlocksTap(true);
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
                    var oldStones = this.Stones;
                    this.TurnFailedBlocksToStones();

                    if (oldStones != this.Stones)
                    {
                        this.streakCount = 0;
                    }
                    else
                    {
                        this.Score += StreakBonusPoints * this.streakCount;
                        ++this.streakCount;
                    }

                    if (this.Stones == MaxStonesInField)
                    {
                        this.EndGame();
                        sw.Stop();
                        timer.Stop();
                        this.TimeLeft = TimeSpan.FromSeconds(0);
                        return;
                    }

                    sw.Restart();
                    this.ChangeCurrentColor();
                    ++cyclesSinceLastDestroyPowerUsed;
                }
            };

            timer.Start();
        }

        private void EndGame()
        {
            this.ToggleBlocksTap(false);
            ApplicationData.Current.LocalSettings.Values[AppViewModel.BestKey] = best;

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) =>
            {
                var frame = Window.Current.Content as Frame;
                if (frame != null)
                {
                    frame.Navigate(typeof(GameOverPage), this.Score);
                }

                timer.Stop();
            };

            timer.Start();
        }

        private void ChangeCurrentColor()
        {
            var randomColor = Util.GetRandomColor();
            while (this.ColorBlockCounts[randomColor] <= 0)
            {
                randomColor = Util.GetRandomColor();
            }

            this.CurrentColor = new SolidColorBrush(randomColor);
        }

        private void TurnFailedBlocksToStones()
        {
            var blocksOfCurrentColor = this.Blocks.Where(b => !b.IsStone && b.Color == this.CurrentColor.Color);
            var newStones = this.Stones;
            foreach (BlockViewModel block in blocksOfCurrentColor)
            {
                --this.ColorBlockCounts[block.Color];
                block.TurnToStone();
                ++newStones;
            }

            this.Stones = newStones > MaxStonesInField ? MaxStonesInField : newStones;
        }

        public void HandleBlockDestroyed(object sender, EventArgs e)
        {
            var block = (BlockViewModel)sender;
            if (block.IsStone)
            {
                return;
            }

            --this.ColorBlockCounts[block.Color];

            if (this.CurrentColor.Color == block.Color)
            {
                this.Score += BlockViewModel.Points; 
            }

            block.ResetBlockUIElement();
            ++this.ColorBlockCounts[block.Color];
        }

        public void HandleDestroyPowerUsed(object sender, EventArgs e)
        {
            cyclesSinceLastDestroyPowerUsed = 0;
            var block = (BlockViewModel)sender;
            var blocksOfColor = this.Blocks.Where(b => !b.IsStone && b.Color == block.Color).ToList();
            foreach (var blockOfColor in blocksOfColor)
            {
                blockOfColor.Destroy();
            }
        }

        public void ToggleBlocksTap(bool enabled)
        {
            foreach (var block in this.Blocks)
            {
                block.UIElement.IsTapEnabled = enabled;
            }
        }
    }
}