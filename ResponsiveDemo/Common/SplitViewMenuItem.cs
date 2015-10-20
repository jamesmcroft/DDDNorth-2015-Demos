// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SplitViewMenuItem.cs" company="James Croft">
//   Copyright (c) 2015 James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the SplitViewMenuItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ResponsiveDemo.Common
{
    using System;

    using Windows.UI.Xaml.Controls;

    public class SplitViewMenuItem
    {
        public string Label { get; set; }

        public Symbol Symbol { get; set; }

        public char SymbolAsChar => (char)this.Symbol;

        public Type PageSource { get; set; }

        public object Arguments { get; set; }
    }
}