using System;
using System.Collections.Generic;
using MapsXamarin.Models;
using MapsXamarin.ViewModels;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace MapsXamarin.Views
{
    public partial class PositionView : ContentPage
    {

        public PositionView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<Position>(this, "ObtainedPosition", async (exc) => {
                await DisplayAlert("Atenção", "Obtida com sucesso!", "OK");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<Position>(this, "ObtainedPosition");
        }
    }
}
