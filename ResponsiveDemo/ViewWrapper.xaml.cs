// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewWrapper.xaml.cs" company="James Croft">
//   Copyright (c) 2015 James Croft. All rights reserved.
// </copyright>
// <summary>
//   A simple wrapper for the application which takes advantage of the SplitView control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ResponsiveDemo
{
    using System.Collections.Generic;
    using System.Linq;

    using ResponsiveDemo.Common;
    using ResponsiveDemo.Controls;
    using ResponsiveDemo.Views;

    using Windows.Foundation;
    using Windows.Foundation.Metadata;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Automation;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// A simple wrapper for the application which takes advantage of the SplitView control.
    /// </summary>
    public sealed partial class ViewWrapper
    {
        private readonly List<SplitViewMenuItem> _navigationList =
            new List<SplitViewMenuItem>(
                new[]
                    {
                        new SplitViewMenuItem { Symbol = Symbol.Home, Label = "Home", PageSource = typeof(MainView) },
                        new SplitViewMenuItem
                            {
                                Symbol = Symbol.Contact,
                                Label = "Profile",
                                PageSource = typeof(ProfileView)
                            },
                        new SplitViewMenuItem
                            {
                                Symbol = Symbol.Setting,
                                Label = "Settings",
                                PageSource = typeof(SettingsView)
                            }
                    });

        public static ViewWrapper Current;

        public ViewWrapper()
        {
            this.InitializeComponent();

            this.Loaded += this.OnLoaded;

            this.SplitViewMenu.RegisterPropertyChangedCallback(
                SplitView.DisplayModeProperty,
                (s, a) =>
                    {
                        this.CheckTogglePaneButtonSizeChanged();
                    });

            SystemNavigationManager.GetForCurrentView().BackRequested += this.OnSystemNavigationBackRequested;

            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                this.BackButton.Visibility = Visibility.Collapsed;
            }

            this.SplitViewMenuList.ItemsSource = this._navigationList;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Current = this;
            this.HamburgerButton.Focus(FocusState.Programmatic);
        }

        public Frame ContentFrame => this.PageFrame;

        private void OnSystemNavigationBackRequested(object sender, BackRequestedEventArgs e)
        {
            bool handled = e.Handled;
            this.OnBackRequested(ref handled);
            e.Handled = handled;
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            bool ignored = false;
            this.OnBackRequested(ref ignored);
        }

        private void OnBackRequested(ref bool handled)
        {
            if (this.ContentFrame == null) return;

            if (this.ContentFrame.CanGoBack && !handled)
            {
                handled = true;
                this.ContentFrame.GoBack();
            }
        }

        /// <summary>
        /// Called when a SplitView menu item is invoked.
        /// </summary>
        private void OnSplitViewMenuItemInvoked(object sender, ListViewItem listViewItem)
        {
            var menuItem = (SplitViewMenuItem)((SplitViewNavigationListView)sender).ItemFromContainer(listViewItem);

            if (menuItem?.PageSource != null && menuItem.PageSource != this.ContentFrame.CurrentSourcePageType)
            {
                this.ContentFrame.Navigate(menuItem.PageSource, menuItem.Arguments);
            }
        }

        /// <summary>
        /// Ensures the nav menu reflects reality when navigation is triggered outside of
        /// the nav menu buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFrameNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                var item =
                    (from p in this._navigationList where p.PageSource == e.SourcePageType select p).SingleOrDefault();
                if (item == null && this.ContentFrame.BackStackDepth > 0)
                {
                    foreach (var entry in this.ContentFrame.BackStack.Reverse())
                    {
                        item =
                            (from p in this._navigationList where p.PageSource == entry.SourcePageType select p)
                                .SingleOrDefault();
                        if (item != null) break;
                    }
                }

                var container = (ListViewItem)this.SplitViewMenuList.ContainerFromItem(item);

                if (container != null) container.IsTabStop = false;
                this.SplitViewMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            }
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            var page = e.Content as Page;
            if (page != null && e.Content != null)
            {
                page.Loaded += this.OnPageLoaded;
            }
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            ((Page)sender).Focus(FocusState.Programmatic);
            ((Page)sender).Loaded -= this.OnPageLoaded;
            this.CheckTogglePaneButtonSizeChanged();
        }

        public Rect TogglePaneButtonRect { get; private set; }

        /// <summary>
        /// An event to notify listeners when the hamburger button may occlude other content in the app.
        /// </summary>
        public event TypedEventHandler<ViewWrapper, Rect> TogglePaneButtonRectChanged;

        private void OnHamburgerButtonUnchecked(object sender, RoutedEventArgs e)
        {
            this.CheckTogglePaneButtonSizeChanged();
        }

        private void CheckTogglePaneButtonSizeChanged()
        {
            if (this.SplitViewMenu.DisplayMode == SplitViewDisplayMode.Inline
                || this.SplitViewMenu.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                var transform = this.HamburgerButton.TransformToVisual(this);
                var rect =
                    transform.TransformBounds(
                        new Rect(0, 0, this.HamburgerButton.ActualWidth, this.HamburgerButton.ActualHeight));
                this.TogglePaneButtonRect = rect;
            }
            else
            {
                this.TogglePaneButtonRect = new Rect();
            }

            var handler = this.TogglePaneButtonRectChanged;
            handler?.DynamicInvoke(this, this.TogglePaneButtonRect);
        }

        private void OnSplitViewMenuContainerContentChanging(
            ListViewBase sender,
            ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item is SplitViewMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((SplitViewMenuItem)args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }
    }
}