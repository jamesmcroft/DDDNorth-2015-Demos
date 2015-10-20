// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="James Croft">
//   Copyright (c) 2015 James Croft. All rights reserved.
// </copyright>
// <summary>
//   Provides application-specific behavior to supplement the default Application class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ResponsiveDemo
{
    using System;

    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var wrapper = Window.Current.Content as ViewWrapper;
            if (wrapper == null)
            {
                wrapper = new ViewWrapper { Language = Windows.Globalization.ApplicationLanguages.Languages[0] };
                wrapper.ContentFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // ToDo: Load state from previously suspended application
                }
            }

            Window.Current.Content = wrapper;

            if (wrapper.ContentFrame.Content == null)
            {
                wrapper.ContentFrame.Navigate(
                    typeof(Views.MainView),
                    e.Arguments,
                    new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private static void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // ToDo - Any suspension code here.

            deferral.Complete();
        }
    }
}
