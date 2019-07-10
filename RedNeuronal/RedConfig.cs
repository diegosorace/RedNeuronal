using System;
using System.Collections.Generic;
using System.Text;

namespace RedNeuronal
{
    public class RedConfig
    {
        public CapaEntrada capaEntrada;
        public CapaSalida capaSalida;
        public CapaOculta capaOculta;

        public bool cargarRedNeuronal;
        public bool guardarRedNeuronal;

        public string direccionDatosEntrada;
        public string direccionDatosSalida;
        public string direccionRedNeuronal;

        public double alpha;
        public double errorMax;
        public int iteracionesDeEntrenamiento;
    }
}
