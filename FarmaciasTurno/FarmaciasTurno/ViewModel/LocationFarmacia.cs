using System.ComponentModel;
using Xamarin.Forms.Maps;

namespace FarmaciasTurno
{
    public class LocationFarmacia : INotifyPropertyChanged
    {
        Position _position;

        public string Address { get;}
        public string Description { get;}

        public Position Position
        {
            get => _position;
            set
            {
                if (!_position.Equals(value))
                {
                    _position = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Position)));
                }
            }
        }

        public LocationFarmacia(string address, string description, Position position)
        {
            Address = address;
            Description = description;
            Position = position;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}