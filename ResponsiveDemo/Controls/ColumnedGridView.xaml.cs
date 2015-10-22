using Windows.UI.Xaml;

namespace ResponsiveDemo.Controls
{
    using System;

    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// The columned grid view.
    /// </summary>
    public sealed partial class ColumnedGridView
    {
        private static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate",
            typeof(DataTemplate),
            typeof(ColumnedGridView),
            new PropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ItemTemplateProperty);
            }
            set
            {
                this.SetValue(ItemTemplateProperty, value);
            }
        }

        private static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            "SelectionMode",
            typeof(ListViewSelectionMode),
            typeof(ColumnedGridView),
            new PropertyMetadata(ListViewSelectionMode.Single));

        public ListViewSelectionMode SelectionMode
        {
            get
            {
                return (ListViewSelectionMode)this.GetValue(SelectionModeProperty);
            }
            set
            {
                this.SetValue(SelectionModeProperty, value);
            }
        }

        private static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource",
            typeof(object),
            typeof(ColumnedGridView),
            new PropertyMetadata(null));

        public object ItemsSource
        {
            get
            {
                return this.GetValue(ItemsSourceProperty);
            }
            set
            {
                this.SetValue(ItemsSourceProperty, value);
            }
        }

        private static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns",
            typeof(int),
            typeof(ColumnedGridView),
            new PropertyMetadata(1));

        public int Columns
        {
            get
            {
                return (int)this.GetValue(ColumnsProperty);
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(
                        string.Format("The value, Columns - {0}, must be greater than 0", value));
                this.SetValue(ColumnsProperty, value);
            }
        }

        public ColumnedGridView()
        {
            this.InitializeComponent();
        }

        private void OnWrapGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapGrid itemsWrapGrid = sender as WrapGrid;
            if (itemsWrapGrid != null)
            {
                itemsWrapGrid.ItemWidth = e.NewSize.Width / this.Columns;
            }
        }
    }
}