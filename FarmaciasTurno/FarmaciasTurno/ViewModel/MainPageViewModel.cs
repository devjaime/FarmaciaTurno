using FarmaciasTurno.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using FarmaciasTurno.View;
using System.Collections.ObjectModel;
using System.Collections;
using Xamarin.Essentials;
using System.Threading;
using FarmaciasTurno.Behaviors;

namespace FarmaciasTurno.ViewModel
{

    public class MainPageViewModel : BaseViewModel
    {
        //definimos nuestro modelo recuerda agregar using FramaciasTurno.Model
        public FarmaciasData Data { get; set; }
        //definiremos un comando para llenar nuestro control recuerda agregar la referencia a using system.windows.input
        public ICommand BuscarFarmacias { get; set; }

        public int Kilometros = 18;
        private List<Farmacia> _items;

        public List<Farmacia> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        int accuracy = (int)GeolocationAccuracy.Default;
        public int Accuracy
        {
            get => accuracy;
            set => SetProperty(ref accuracy, value);
        }
        string currentLocation;
        public string CurrentLocation
        {
            get => currentLocation;
            set => SetProperty(ref currentLocation, value);
        }
        CancellationTokenSource cts;
        //luego de lo anterior crearemos un constructor para inicializar el comando de busqueda
        public MainPageViewModel()
        {


            //recuerda importar el espacio de nombres Xamarin.Forms
            //ejecutaremos una tarea asincrona
            BuscarFarmacias = new Command(async () =>
            {
                await GetData("http://www.mocky.io/v2/5cbbf6fb320000980580d7b3"); //la idea de este metodo es recibir el servicio rest que traera la data de forma asincrona de FarmaciasData

            });
            OnGetCurrentLocation();
            SensorSpeed speed = SensorSpeed.UI;
            Accelerometer.Start(speed);
        }

        //importar using System.Threading.Tasks;
        public async Task GetData(string url)
        {
            //using System.Net.Http; es de .net standard

            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var response =
                await client.GetAsync(client.BaseAddress);
            response.EnsureSuccessStatusCode();
            var jsonResult = await response.Content.ReadAsStringAsync();
            FarmaciasData result = JsonConvert.DeserializeObject<FarmaciasData>(jsonResult);

            List<Farmacia> UnoItems = new List<Farmacia>();
            foreach (var _result in result.Data)
            {
                string value_lat;
                string value_lng;
                double number_lat;
                double number_lng;
                value_lat = _result.local_lat;
                value_lng = _result.local_lng;
               
                UnoItems.Add(
                        new Farmacia
                        {
                            Id = Convert.ToInt16(_result.local_id),
                            Name = _result.local_nombre,
                            Description = _result.local_direccion,
                            Latitude = Double.TryParse(value_lat, out number_lat) ? number_lat : -32.9849921792696,
                            Longitude = Double.TryParse(value_lng, out number_lng) ? number_lng : -71.2757177058683,
                            Telefono = _result.local_telefono
                        }
                    );

            }
            Items = UnoItems;

        }


        async void OnGetCurrentLocation()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                var request = new GeolocationRequest((GeolocationAccuracy)Accuracy);
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                CurrentLocation = FormatLocation(location);
            }
            catch (Exception ex)
            {
                CurrentLocation = FormatLocation(null, ex);
            }
            finally
            {
                cts.Dispose();
                cts = null;
            }
            IsBusy = false;
        }
        string notAvailable = "no disponible";
        string FormatLocation(Location location, Exception ex = null)
        {
            if (location == null)
            {
                return $"No se puede detectar la ubicación. Excepción: {ex?.Message ?? string.Empty}";
            }

          
            return
                $"Latitud: {location.Latitude}\n" +
                $"Longitud: {location.Longitude}\n";
           
            //$"Exactitud: {location.Accuracy}\n"; //+
            //$"Altitud: {(location.Altitude.HasValue ? location.Altitude.Value.ToString() : notAvailable)}\n" +
            //$"Titulo: {(location.Course.HasValue ? location.Course.Value.ToString() : notAvailable)}\n" +
            //$"Velocidad: {(location.Speed.HasValue ? location.Speed.Value.ToString() : notAvailable)}\n" +
            //$"Fecha (UTC): {location.Timestamp:d}\n" +
            //$"Hora (UTC): {location.Timestamp:T}" +
            //$"Moking Provider: {location.IsFromMockProvider}";
        }

        public override void OnAppearing()
        {
            //Accelerometer.ReadingChanged += OnReadingChanged;
            Accelerometer.ShakeDetected += Accelerometer_OnShaked;

            base.OnAppearing();
        }

        void OnStop()
        {
            //IsActive = false;
            Accelerometer.Stop();
        }


        void Accelerometer_OnShaked(object sender, EventArgs e)
        {
            
            Kilometros = Kilometros - 2;
            
            BuscarFarmacias = new Command(async () =>
            {
                await GetData("http://www.mocky.io/v2/5cbbf6fb320000980580d7b3"); //la idea de este metodo es recibir el servicio rest que traera la data de forma asincrona de FarmaciasData

            });

        }
     

        public override void OnDisappearing()
        {
            OnStop();
            //Accelerometer.ReadingChanged -= OnReadingChanged;
            Accelerometer.ShakeDetected -= Accelerometer_OnShaked;
            base.OnDisappearing();
        }

       
    }
}
