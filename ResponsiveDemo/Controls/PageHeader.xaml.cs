// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageHeader.xaml.cs" company="James Croft">
//   Copyright (c) 2015 James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the PageHeader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ResponsiveDemo.Controls
{
    using Windows.Foundation;
    using Windows.UI.Xaml;

    /// <summary>
    /// The page header.
    /// </summary>
    public sealed partial class PageHeader
    {
        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
            "HeaderContent",
            typeof(UIElement),
            typeof(PageHeader),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        public PageHeader()
        {
            this.InitializeComponent();

            this.Loaded += this.OnLoaded;
        }

        public UIElement HeaderContent
        {
            get
            {
                return (UIElement)this.GetValue(HeaderContentProperty);
            }
            set
            {
                this.SetValue(HeaderContentProperty, value);
            }
        }

        private void OnLoaded(object o, RoutedEventArgs e)
        {
            if (ViewWrapper.Current != null)
            {
                ViewWrapper.Current.TogglePaneButtonRectChanged += this.OnViewWrapperTogglePaneButtonRectChanged;
                this.TitleBar.Margin = new Thickness(ViewWrapper.Current.TogglePaneButtonRect.Right, 0, 0, 0);
            }
        }

        private void OnViewWrapperTogglePaneButtonRectChanged(ViewWrapper sender, Rect e)
        {
            this.TitleBar.Margin = new Thickness(e.Right, 0, 0, 0);
        }
    }
}
