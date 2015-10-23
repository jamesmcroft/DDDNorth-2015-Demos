// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" company="James Croft">
//   Copyright (c) 2015 James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the MainPageViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sandbox.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Windows.Media.Capture;
    using Windows.Storage;
    using Windows.UI.Xaml.Navigation;

    using GalaSoft.MvvmLight.Command;

    using Sandbox.Common;

    public class MainPageViewModel : PageBaseViewModel
    {
        private StorageFile _photo;

        private StorageFile _video;

        public MainPageViewModel()
        {
            this.LaunchCameraCommand = new RelayCommand(async () => await this.LaunchCamera());
            this.LaunchVideoCommand = new RelayCommand(async () => await this.LaunchVideo());
        }

        public override void OnNavigatedFrom(NavigationEventArgs args)
        {
        }

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                var launchEvent = e.Parameter.ToString();
                this.OnLaunchedEvent(launchEvent);
            }
        }

        private async Task LaunchVideo()
        {
            var camera = new CameraCaptureUI();
            camera.VideoSettings.MaxResolution = CameraCaptureUIMaxVideoResolution.StandardDefinition;

            var storage = await camera.CaptureFileAsync(CameraCaptureUIMode.Video);

            if (storage != null)
            {
                this.Video = storage;
            }
        }

        private async Task LaunchCamera()
        {
            var camera = new CameraCaptureUI();
            camera.PhotoSettings.AllowCropping = true;

            var storage = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (storage != null)
            {
                this.Photo = storage;
            }
        }

        public StorageFile Photo
        {
            get
            {
                return this._photo;
            }
            set
            {
                this.Set(() => this.Photo, ref this._photo, value);
                if (value != null)
                {
                    this.RaisePropertyChanged(() => this.PhotoLocation);
                }
            }
        }

        public StorageFile Video
        {
            get
            {
                return this._video;
            }
            set
            {
                this.Set(() => this.Video, ref this._video, value);
                if (value != null)
                {
                    this.RaisePropertyChanged(() => this.VideoLocation);
                }
            }
        }

        public Uri VideoLocation => this._video != null ? new Uri(this._video.Path, UriKind.Absolute) : null;

        public Uri PhotoLocation => this._photo != null ? new Uri(this._photo.Path, UriKind.Absolute) : null;

        public ICommand LaunchCameraCommand { get; }

        public ICommand LaunchVideoCommand { get; }

        public void OnLaunchedEvent(string arguments)
        {
            switch (arguments.ToLower())
            {
                case "photo":
                    this.LaunchCameraCommand.Execute(null);
                    break;
                case "video":
                    this.LaunchVideoCommand.Execute(null);
                    break;
            }
        }
    }
}