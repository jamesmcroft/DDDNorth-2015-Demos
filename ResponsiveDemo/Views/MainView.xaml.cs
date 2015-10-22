// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainView.xaml.cs" company="James Croft">
//   Copyright (c) 2015 James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the MainView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ResponsiveDemo.Views
{
    using System;
    using System.Collections.ObjectModel;

    using ResponsiveDemo.Models;

    using Windows.Devices.Geolocation;

    using WinRTXamlToolkit.Controls.DataVisualization.Charting;

    /// <summary>
    /// The main page.
    /// </summary>
    public sealed partial class MainView
    {
        public MainView()
        {
            this.InitializeComponent();

            this.Statistics = new ObservableCollection<Statistic>();
            this.AcquireStatistics();

            this.GraphStatistics = new ObservableCollection<GraphStatistic>();
            this.AcquireGraphStatistics();

            this.Map.Center = new Geopoint(new BasicGeoposition { Latitude = 54.904485, Longitude = -1.391271 });
        }

        private void AcquireGraphStatistics()
        {
            var rand = new Random();

            this.GraphStatistics.Clear();

            for (var i = 0; i < 45; i++)
            {
                var val = 0;

                if (i >= 0 && i < 5)
                {
                    val = rand.Next(65, 80);
                }
                else if (i >= 5 && i < 15)
                {
                    val = rand.Next(75, 100);
                }
                else if (i >= 15 && i < 20)
                {
                    val = rand.Next(90, 140);
                }
                else if (i >= 20 && i < 35)
                {
                    val = rand.Next(130, 180);
                }
                else if (i >= 35 && i < 45)
                {
                    val = rand.Next(80, 140);
                }

                this.GraphStatistics.Add(new GraphStatistic { Time = DateTime.Now.AddMinutes(i), Value = val });
            }

            this.ChartLineSeries.DependentRangeAxis = new LinearAxis
            {
                Minimum = 60,
                Maximum = 190,
                Orientation = AxisOrientation.Y,
                Interval = 20,
                ShowGridLines = true
            };
        }

        public ObservableCollection<Statistic> Statistics { get; }

        public ObservableCollection<GraphStatistic> GraphStatistics { get; }

        private void AcquireStatistics()
        {
            this.Statistics.Clear();

            this.Statistics.Add(new Statistic { Name = "Calories burned", Value = "541" });
            this.Statistics.Add(new Statistic { Name = "Average pace", Value = "10' 25\"" });
            this.Statistics.Add(new Statistic { Name = "Low HR", Value = "67" });
            this.Statistics.Add(new Statistic { Name = "Peak HR", Value = "191" });
            this.Statistics.Add(new Statistic { Name = "Ending HR", Value = "186" });
            this.Statistics.Add(new Statistic { Name = "Cardio benefit", Value = "Stenuous" });
        }
    }
}
