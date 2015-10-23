// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageBaseViewModel.cs" company="James Croft">
//   Copyright (c) 2015 James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the PageBaseViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sandbox.Common
{
    using GalaSoft.MvvmLight;

    using Windows.UI.Xaml.Navigation;

    public abstract class PageBaseViewModel : ViewModelBase
    {
        public abstract void OnNavigatedTo(NavigationEventArgs args);

        public abstract void OnNavigatedFrom(NavigationEventArgs args);

        public virtual void OnPageLoaded()
        {
        }
    }
}
