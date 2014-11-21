namespace TileSmash.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Windows.UI.Xaml.Media;

    public class GameViewModel
    {
        public const int MaxStonesInField = 10;
        public const int SecondsToDestroy = 5;

        private int[] colorBlockCounts;

        public GameViewModel(int best, SolidColorBrush currentColor)
        {
            this.Best = best;
            this.CurrentColor = currentColor;
            this.Score = 0;
            this.Stones = 0;
            this.TimeLeft = new TimeSpan(0, 0, SecondsToDestroy);
        }

        public int MaxStones
        {
            get
            {
                return MaxStonesInField;
            }
        }

        public int Stones { get; set; }

        public int Score { get; set; }

        public int Best { get; set; }

        public TimeSpan TimeLeft { get; set; }

        public string TimeLeftFormatted
        {
            get
            {
                return string.Format("{0:s\\.ff}", this.TimeLeft);
            }
        }

        public SolidColorBrush CurrentColor { get; set; }
    }
}
