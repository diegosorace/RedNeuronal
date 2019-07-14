using System;
using System.Collections.Generic;

namespace RedNeuronal
{
    [Serializable]
    class Capa
    {
        public List<Neurona> neuronas;
        public int numeroDeNeuronas;
        public double[] salidas;

        public Capa(int _numeroDeNeuronas, int numeroDeEntradas, Random r)
        {
            numeroDeNeuronas = _numeroDeNeuronas;
            neuronas = new List<Neurona>();
            for (int i = 0; i < numeroDeNeuronas; i++)
            {
                neuronas.Add(new Neurona(numeroDeEntradas, r));
            }
        }
        public double[] Activar(double[] inputs)
        {
            List<double> _salidas = new List<double>();
            for (int i = 0; i < numeroDeNeuronas; i++)
            {
                _salidas.Add(neuronas[i].Activar(inputs));
            }
            salidas = _salidas.ToArray();
            return _salidas.ToArray();
        }
    }
}
