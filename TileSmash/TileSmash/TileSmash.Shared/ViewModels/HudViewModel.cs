namespace TileSmash.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class HudViewModel
    {
        public HudViewModel(int hiScore)
            : this(0, hiScore)
        {
        }

        public HudViewModel(int score, int hiScore)
        {
            this.Score = score;
            this.HiScore = hiScore;
        }

        public int Score { get; set; }
        public int HiScore { get; set; }
    }
}
