namespace TileSmash.ParseClasses
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Parse;

    [ParseClassName("PlayerScore")]
    public class PlayerScore : ParseObject
    {
        [ParseFieldName("name")]
        public string Name
        {
            get 
            { 
                return GetProperty<string>(); 
            }
            set 
            { 
                SetProperty<string>(value); 
            }
        }

        [ParseFieldName("bestScore")]
        public int BestScore
        {
            get
            {
                return GetProperty<int>();
            }
            set
            {
                SetProperty<int>(value);
            }
        }
    }
}
