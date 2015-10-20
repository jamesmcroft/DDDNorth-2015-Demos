// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SplitViewNavigationListView.cs" company="James Croft">
//   Copyright (c) 2015 James Croft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ResponsiveDemo.Controls
{
    using System;

    using Windows.System;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Animation;

    /// <summary>
    /// A custom ListView design for SplitView navigation.
    /// </summary>
    public class SplitViewNavigationListView : ListView
    {
        private SplitView _parent;

        public SplitViewNavigationListView()
        {
            this.SelectionMode = ListViewSelectionMode.Single;
            this.IsItemClickEnabled = true;
            this.ItemClick += this.OnItemClicked;

            this.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object o, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this);
            while (parent != null && !(parent is SplitView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                this._parent = parent as SplitView;

                this._parent.RegisterPropertyChangedCallback(
                    SplitView.IsPaneOpenProperty,
                    (sender, args) =>
                        {
                            this.OnPaneToggled();
                        });

                this.OnPaneToggled();
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            for (var i = 0; i < this.ItemContainerTransitions.Count; i++)
            {
                if (this.ItemContainerTransitions[i] is EntranceThemeTransition)
                {
                    this.ItemContainerTransitions.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Selects an item within the list view.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void SetSelectedItem(ListViewItem item)
        {
            var index = -1;
            if (item != null)
            {
                index = this.IndexFromContainer(item);
            }

            for (var i = 0; i < this.Items.Count; i++)
            {
                var lvi = (ListViewItem)this.ContainerFromIndex(i);
                if (lvi != null)
                {
                    if (i != index)
                    {
                        lvi.IsSelected = false;
                    }
                    else if (i == index)
                    {
                        lvi.IsSelected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Event for when an item has been selected.
        /// </summary>
        public event EventHandler<ListViewItem> ItemInvoked;

        /// <summary>
        /// Called on keyboard key pressed down.
        /// </summary>
        /// <param name="e">
        /// The event args.
        /// </param>
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            var focusedItem = FocusManager.GetFocusedElement();

            switch (e.Key)
            {
                case VirtualKey.Up:
                    TryMoveFocus(FocusNavigationDirection.Up);
                    e.Handled = true;
                    break;

                case VirtualKey.Down:
                    TryMoveFocus(FocusNavigationDirection.Down);
                    e.Handled = true;
                    break;

                case VirtualKey.Tab:
                    var shiftKeyState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift);
                    var shiftKeyDown = (shiftKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;

                    var lvi = focusedItem as ListViewItem;
                    if (lvi != null)
                    {
                        bool isLastItem = this.IndexFromContainer(lvi) == this.Items.Count - 1;
                        bool isFirstItem = this.IndexFromContainer(lvi) == 0;

                        if (!shiftKeyDown)
                        {
                            TryMoveFocus(
                                isLastItem ? FocusNavigationDirection.Next : FocusNavigationDirection.Down);
                        }
                        else
                        {
                            TryMoveFocus(
                                isFirstItem ? FocusNavigationDirection.Previous : FocusNavigationDirection.Up);
                        }
                    }
                    else if (focusedItem is Control)
                    {
                        TryMoveFocus(!shiftKeyDown ? FocusNavigationDirection.Down : FocusNavigationDirection.Up);
                    }

                    e.Handled = true;
                    break;

                case VirtualKey.Space:
                case VirtualKey.Enter:
                    this.InvokeItem(focusedItem);
                    e.Handled = true;
                    break;

                default:
                    base.OnKeyDown(e);
                    break;
            }
        }

        private static void TryMoveFocus(FocusNavigationDirection direction)
        {
            if (direction == FocusNavigationDirection.Next || direction == FocusNavigationDirection.Previous)
            {
                FocusManager.TryMoveFocus(direction);
            }
            else
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                control?.Focus(FocusState.Programmatic);
            }
        }

        private void OnItemClicked(object sender, ItemClickEventArgs e)
        {
            var item = this.ContainerFromItem(e.ClickedItem);
            this.InvokeItem(item);
        }

        private void InvokeItem(object focusedItem)
        {
            this.SetSelectedItem(focusedItem as ListViewItem);
            this.ItemInvoked(this, focusedItem as ListViewItem);

            if (this._parent.IsPaneOpen && (
                this._parent.DisplayMode == SplitViewDisplayMode.CompactOverlay ||
                this._parent.DisplayMode == SplitViewDisplayMode.Overlay))
            {
                this._parent.IsPaneOpen = false;
                var lvi = focusedItem as ListViewItem;
                lvi?.Focus(FocusState.Programmatic);
            }
        }

        private void OnPaneToggled()
        {
            if (this.ItemsPanelRoot != null)
            {
                if (this._parent.IsPaneOpen)
                {
                    this.ItemsPanelRoot.ClearValue(FrameworkElement.WidthProperty);
                    this.ItemsPanelRoot.ClearValue(FrameworkElement.HorizontalAlignmentProperty);
                }
                else if (this._parent.DisplayMode == SplitViewDisplayMode.CompactInline
                         || this._parent.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                {
                    this.ItemsPanelRoot.SetValue(FrameworkElement.WidthProperty, this._parent.CompactPaneLength);
                    this.ItemsPanelRoot.SetValue(
                        FrameworkElement.HorizontalAlignmentProperty,
                        HorizontalAlignment.Left);
                }
            }
        }
    }
}