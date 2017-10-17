using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MapsXamarin.Models;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace MapsXamarin.ViewModels
{
    public class PositionViewModel : BaseViewModel
    {
        public Position Position { get; set; }

        public PositionViewModel()
        {
            Position = new Position();
            GetPositionCommand = new Command(async () =>
            {
                Position position = await GetPosition();
                MessagingCenter.Send(position, "ObtainedPosition");
            });
        }
        
        public double Latitude
        {
            get => Position.Latitude;

            set
            {
                Position.Latitude = value;
                OnPropertyChanged();
            }
        }

        public double Longitude
        {
            get => Position.Longitude;

            set
            {
                Position.Longitude = value;
                OnPropertyChanged();
            }
        }

        public async Task<Position> GetPosition()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            Latitude = position.Latitude;
            Longitude = position.Longitude;
            return new Position(Latitude, Longitude);

        }
        public ICommand GetPositionCommand { get; set; }
    }
}
