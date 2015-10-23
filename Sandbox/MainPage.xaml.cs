// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="James Croft">
//   Copyright (c) 2015 James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the MainPage type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sandbox
{
    using Sandbox.ViewModels;

    /// <summary>
    /// The main page.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public void OnLaunchedEvent(string arguments)
        {
            var vm = this.DataContext as MainPageViewModel;
            vm?.OnLaunchedEvent(arguments);
        }
    }
}
