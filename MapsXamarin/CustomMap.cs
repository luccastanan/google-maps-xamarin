using System;
using System.Collections.Generic;
using MapsXamarin.Models;
using Xamarin.Forms.Maps;

namespace MapsXamarin
{
    public class CustomMap : Map
    {   
        public List<CustomCircle> Circles { get; set; }

        public CustomMap(){
            Circles = new List<CustomCircle>();
        }
    }
}
