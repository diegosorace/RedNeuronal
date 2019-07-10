using System;
using System.Collections.Generic;
using System.Text;

namespace RedNeuronal
{
    [Serializable]
    class Neurona
    {
        public double[] pesos;
        public double ultimaActivacion;
        public double tendencia;

        public Neurona(int numeroEntradas, Random r)
        {
            tendencia = 10 * r.NextDouble() - 5;
            pesos = new double[numeroEntradas];
            for (int i = 0; i < numeroEntradas; i++)
            {
                pesos[i] = 10 * r.NextDouble() - 5;
            }
        }
        public double Activar(double[] entradas)
        {
            double activacion = tendencia;

            for (int i = 0; i < pesos.Length; i++)
            {
                activacion += pesos[i] * entradas[i];
            }

            ultimaActivacion = activacion;
            return Sigmoid(activacion);
        }
        public static double Sigmoid(double entrada)
        {
            return 1 / (1 + Math.Exp(-entrada));
        }
        public static double SigmoidDerivada(double entrada)
        {
            double y = Sigmoid(entrada);
            return y * (1 - y);
        }
    }
}
