namespace TileSmash.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Windows.UI;

    public class Util
    {
        private static Random randomInstance;
        private static Color[] colors;

        public static Random RandomInstance
        {
            get
            {
                if (randomInstance == null)
                {
                    randomInstance = new Random();
                }

                return randomInstance;
            }
        }

        public static Color[] Colors
        {
            get
            {
                if (colors == null)
                {
                    colors = new Color[] 
                    {
                        Color.FromArgb(255, 251, 160, 38), // Neon Carrot
                        Color.FromArgb(255, 225, 73, 56), // Well Read
                        Color.FromArgb(255, 65, 168, 95), // Chateau Green
                        Color.FromArgb(255, 41, 105, 176), // Denim
                        Color.FromArgb(255, 147, 89, 181) // Wisteria
                    };
                }

                return colors;
            }
        }

        public static Color GetRandomColor()
        {
            return Colors[RandomInstance.Next(0, Colors.Length)];
        }
    }
}
