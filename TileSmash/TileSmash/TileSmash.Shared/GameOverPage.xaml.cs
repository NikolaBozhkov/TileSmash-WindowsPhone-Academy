using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using TileSmash.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TileSmash
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameOverPage : Page
    {
        public GameOverPage()
        {
            this.InitializeComponent();
            this.DataContext = new GameOverViewModel();
        }

        private void NavigateToGamePage(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamePage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((GameOverViewModel)this.DataContext).Score = e.Parameter.ToString();
        } 
    }
}
