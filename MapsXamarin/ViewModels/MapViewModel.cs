using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MapsXamarin.Models;
using MapsXamarin.Views;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace MapsXamarin.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public MapViewModel(MapView mapview, List<CustomCircle> circles)
        {
            GetLocationCommand = new Command(async () =>
            {
                var position = await GetPosition();
                MessagingCenter.Send<Views.MapView, Models.Position>(mapview, MapView.MsgCurrentLocation, position);

                bool outside = true;
                foreach (var circle in circles)
                {
                    double distance = Distance(position.Latitude, position.Longitude, circle.Position.Latitude, circle.Position.Longitude, 'M');
                    if (distance < circle.Radius)
                    {
                        outside = false;
                        break;
                    }
                }

                if (outside)
                    MessagingCenter.Send<Views.MapView>(mapview, MapView.MsgOutsideFence);
            });
        }
        public async Task<Position> GetPosition()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            return new Position(position.Latitude, position.Longitude);

        }
        public ICommand GetLocationCommand { get; set; }

        private double Distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2)) + Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) * Math.Cos(Deg2rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K' || unit == 'M')
            {
                dist = dist * 1.609344;
                if (unit == 'M')
                    dist *= 1000;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return dist;
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double Deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double Rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
