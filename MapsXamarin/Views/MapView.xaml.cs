using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MapsXamarin.Models;
using MapsXamarin.ViewModels;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapsXamarin.Views
{
    public partial class MapView : ContentPage
    {
        public const string MsgCurrentLocation = "CurrentLocation";
        public const string MsgOutsideFence = "OutsideFence";
        MapViewModel vm;
        //Models.Position position;
        public MapView()
        {
            InitializeComponent();
            CustomMap.Circles.Add(CreateCircle(new Models.Position(-23.3038824, -51.1597113), 100));
            CustomMap.Circles.Add(CreateCircle(new Models.Position(-23.305024, -51.157232), 100));
            CustomMap.Circles.Add(CreateCircle(new Models.Position(-23.306123, -51.158420), 100));

            vm = new MapViewModel(this, CustomMap.Circles);
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<MapView, Models.Position>(this, MsgCurrentLocation, (sender, position) =>
            {
                RefreshPosition(position);
            }, null);

            MessagingCenter.Subscribe<MapView>(this, MsgOutsideFence, (sender) =>
            {
                DisplayAlert("Atenção", "Pet fora da cerca", "Ok");
            }, null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<MapView>(this, MsgCurrentLocation);
            MessagingCenter.Unsubscribe<MapView>(this, MsgOutsideFence);
        }

        void RefreshPosition(Models.Position position)
        {
            CustomMap.Pins.Clear();
            CustomMap.Pins.Add(CreatePin(position, "SENAI Londrina - STI", "Rua São Vicent, 168"));
            MoveToRegion(position);
        }

        private Pin CreatePin(Models.Position position, string label = null, string address = null) =>
            new Pin
            {
                Type = PinType.Place,
                Position = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude),
                Label = label,
                Address = address
            };

        private CustomCircle CreateCircle(Models.Position position, int radius) =>
            new CustomCircle
            {
                Position = position,
                Radius = radius
            };

        private void MoveToRegion(Models.Position position)
        {
            CustomMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.2)));
        }
    }
}
