namespace TileSmash.ViewModels
{
    public class GameOverViewModel: ViewModelBase
    {
        private string score;

        public string Score
        {
            get
            {
                return string.Format("\n{0}", this.score);
            }

            set
            {
                this.score = value;
                this.NotifyPropertyChanged("Score");
            }
        }
    }
}
