// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageBase.cs" company="James Croft">
//   Copyright (c) 2015 James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the PageBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sandbox.Common
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public abstract class PageBase : Page
    {
        protected PageBase()
        {
            this.Loaded += this.OnPageLoaded;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as PageBaseViewModel;
            vm?.OnPageLoaded();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = this.DataContext as PageBaseViewModel;
            vm?.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var vm = this.DataContext as PageBaseViewModel;
            vm?.OnNavigatedFrom(e);
        }
    }
}