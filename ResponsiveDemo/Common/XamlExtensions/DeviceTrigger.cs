// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeviceTrigger.cs" company="James Croft">
//   Copyright (c) 2015 James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the DeviceTrigger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ResponsiveDemo.Common.XamlExtensions
{
    using System;

    using Windows.System.Profile;
    using Windows.UI.Xaml;

    using ResponsiveDemo.Common.Enums;

    public class DeviceTrigger : StateTriggerBase
    {
        private static readonly string CurrentDevice;

        static DeviceTrigger()
        {
            CurrentDevice = AnalyticsInfo.VersionInfo.DeviceFamily;
        }

        private static readonly DependencyProperty DeviceTypeProperty = DependencyProperty.Register(
            "DeviceType",
            typeof(DeviceType),
            typeof(DeviceTrigger),
            new PropertyMetadata(DeviceType.None, OnDeviceTypeChanged));

        private bool _isActive;

        private static void OnDeviceTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var trigger = (DeviceTrigger)obj;
            var newVal = (DeviceType)args.NewValue;

            switch (CurrentDevice)
            {
                case "Windows.Desktop":
                    trigger.IsActive = newVal == DeviceType.Desktop;
                    break;
                case "Windows.Mobile":
                    trigger.IsActive = newVal == DeviceType.Mobile;
                    break;
                case "Windows.Team":
                    trigger.IsActive = newVal == DeviceType.SurfaceHub;
                    break;
                case "Windows.IoT":
                    trigger.IsActive = newVal == DeviceType.IoT;
                    break;
                case "Windows.Xbox":
                    trigger.IsActive = newVal == DeviceType.Xbox;
                    break;
                default:
                    trigger.IsActive = newVal == DeviceType.None;
                    break;
            }
        }

        public DeviceType DeviceType
        {
            get
            {
                return (DeviceType)this.GetValue(DeviceTypeProperty);
            }
            set
            {
                this.SetValue(DeviceTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the trigger is currently active.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return this._isActive;
            }
            private set
            {
                if (this._isActive == value)
                {
                    return;
                }

                this._isActive = value;
                base.SetActive(value); // Sets the trigger as active causing the UI to update

                this.IsActiveChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the IsActive property changes.
        /// </summary>
        public event EventHandler IsActiveChanged;
    }
}