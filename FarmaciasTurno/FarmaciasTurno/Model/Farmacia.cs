using System;
using System.Collections.Generic;
using System.Text;

namespace FarmaciasTurno.Model
{
    public class Farmacia
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Telefono { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
