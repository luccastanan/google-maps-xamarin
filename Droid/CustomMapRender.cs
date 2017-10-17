using System;
using System.Collections.Generic;
using Android.Gms.Maps.Model;
using MapsXamarin;
using MapsXamarin.Droid;
using MapsXamarin.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MapsXamarin.Droid
{
    public class CustomMapRenderer : MapRenderer
    {
        List<CustomCircle> lstCircle;
        bool isDrawn;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                lstCircle = formsMap.Circles;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("VisibleRegion") && !isDrawn)
            {
                foreach (var circle in lstCircle)
                {
                    var circleOptions = new CircleOptions();
                    circleOptions.InvokeCenter(new LatLng(circle.Position.Latitude, circle.Position.Longitude));
                    circleOptions.InvokeRadius(circle.Radius);
                    circleOptions.InvokeFillColor(0X66FF0000);
                    circleOptions.InvokeStrokeColor(0X66FF0000);
                    circleOptions.InvokeStrokeWidth(0);

                    NativeMap.AddCircle(circleOptions);
                }
                isDrawn = true;
            }
        }
    }
}

