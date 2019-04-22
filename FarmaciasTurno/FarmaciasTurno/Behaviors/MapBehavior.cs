using FarmaciasTurno.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FarmaciasTurno.Behaviors
{
    public class MapBehavior : BindableBehavior<Xamarin.Forms.Maps.Map>
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<Farmacia>), typeof(MapBehavior), null, BindingMode.Default, propertyChanged: ItemsSourceChanged);

        public IEnumerable<Farmacia> ItemsSource
        {
            get => (IEnumerable<Farmacia>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        int accuracy = (int)GeolocationAccuracy.Default;
        public int Accuracy
        {
            get => accuracy;
            set => SetProperty(ref accuracy, value);
        }

        private void SetProperty(ref int accuracy, int value)
        {
            throw new NotImplementedException();
        }

        string currentLocation;
        public string CurrentLocation
        {
            get => currentLocation;
            set => SetProperty(ref currentLocation, value);
        }

        private void SetProperty(ref string currentLocation, string value)
        {
            throw new NotImplementedException();
        }

        CancellationTokenSource cts;

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is MapBehavior behavior)) return;
            behavior.AddPins();
        }

        async void OnGetCurrentLocation(int Kilometros)
        {
            
            try
            {
                var request = new GeolocationRequest((GeolocationAccuracy)Accuracy);
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                 FormatLocation(location, Kilometros);
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                cts.Dispose();
                cts = null;
            }
           
        }
        string notAvailable = "no disponible";
        private void FormatLocation(Location location, int Kilometros)
        {
           
            if (location != null)
            {
                var map = AssociatedObject;
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(Kilometros)));
            }
          
        }


       

        public void inicializaposicion(int Kilometros)
        {
            var map = AssociatedObject;
            OnGetCurrentLocation(Kilometros);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(-33.5952287, -70.5903447), Distance.FromKilometers(Kilometros)));
        }
        private void AddPins()
        {
            var Kilometros = 20;
            inicializaposicion(Kilometros);
            var map = AssociatedObject;
            for (int i = map.Pins.Count - 1; i >= 0; i--)
            {
                map.Pins[i].Clicked -= PinOnClicked;
                map.Pins.RemoveAt(i);
            }

            var pins = ItemsSource.Select(x =>
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(x.Latitude, x.Longitude),
                    Label = x.Name,
                    Address = x.Description,

                };

                pin.Clicked += PinOnClicked;
                return pin;
            }).ToArray();
            foreach (var pin in pins)
                map.Pins.Add(pin);
        }

        private void PinOnClicked(object sender, EventArgs eventArgs)
        {
            var pin = sender as Pin;
            if (pin == null) return;
            var viewModel = ItemsSource.FirstOrDefault(x => x.Name == pin.Label);
            if (viewModel == null) return;
            //viewModel.Command.Execute(null); // TODO We are going to implement this later ;)
        }
    }
}
