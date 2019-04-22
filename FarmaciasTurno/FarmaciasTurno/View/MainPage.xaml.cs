using System;
using Xamarin.Forms.Maps;

namespace FarmaciasTurno.View
{
    public partial class MainPage : BasePage
    {
        public MainPage()
        {
            InitializeComponent();
            MapView.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(-33.5952287, -70.5903447), Distance.FromKilometers(20)));
        }
        public void PinMap()
        {
            
        }
        private void Street_OnClicked(object sender, EventArgs e)
        {
            MapView.MapType = MapType.Street;
        }


        private void Hybrid_OnClicked(object sender, EventArgs e)
        {
            MapView.MapType = MapType.Hybrid;
        }

        private void Satellite_OnClicked(object sender, EventArgs e)
        {
            MapView.MapType = MapType.Satellite;
        }
    }
}