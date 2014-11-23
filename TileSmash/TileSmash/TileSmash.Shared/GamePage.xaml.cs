namespace TileSmash
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;

    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Navigation;

    using TileSmash.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        public GamePage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            this.IsTapEnabled = true;
            this.Tapped += this.OnPageTapped;
            this.TapToStartTextBlock.Visibility = Visibility.Visible;

            foreach (var child in this.GameGrid.Children.ToList())
            {
                if (child != this.TapToStartTextBlock)
                {
                    this.GameGrid.Children.Remove(child);
                }
            }

            var appViewModel = new AppViewModel();
            appViewModel.InitializeGameGrid(this.GameGrid);
            this.DataContext = appViewModel;
        }

        private void OnPageTapped(object sender, TappedRoutedEventArgs e)
        {
            this.IsTapEnabled = false;
            this.Tapped -= OnPageTapped;
            ((AppViewModel)this.DataContext).GameModel.StartGame();
            this.TapToStartTextBlock.Visibility = Visibility.Collapsed;
        }
    }
}
