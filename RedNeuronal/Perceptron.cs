using System;
using System.Collections.Generic;
using System.Text;

namespace RedNeuronal
{
    [Serializable]
    class Perceptron
    {
        readonly List<Capa> capas;

        public Perceptron(int[] neuronasPorCapa)
        {
            capas = new List<Capa>();
            Random r = new Random();

            for (int i = 0; i < neuronasPorCapa.Length; i++)
            {
                capas.Add(new Capa(neuronasPorCapa[i], i == 0 ? neuronasPorCapa[i] : neuronasPorCapa[i - 1], r));
            }
        }
        public double[] Activar(double[] entradas)
        {
            double[] salidas = new double[0];
            for (int i = 1; i < capas.Count; i++)
            {
                salidas = capas[i].Activar(entradas);
                entradas = salidas;
            }
            return salidas;
        }
        double ErrorIndividual(double[] salidaReal, double[] salidaDeseada)
        {
            double err = 0;
            for (int i = 0; i < salidaReal.Length; i++)
            {
                err += Math.Pow(salidaReal[i] - salidaDeseada[i], 2);
            }
            return err;
        }
        double ErrorGeneral(List<double[]> entradas, List<double[]> salidaDeseada)
        {
            double err = 0;
            for (int i = 0; i < entradas.Count; i++)
            {
                err += ErrorIndividual(Activar(entradas[i]), salidaDeseada[i]);
            }
            return err;
        }
        public bool Entrenar(List<double[]> salidas, List<double[]> salidaDeseada, double alpha, double errorMaximo, int iteracinesMaximas)
        {
            double err = 99999;
            var timer = 0;
            while (err > errorMaximo)
            {
                iteracinesMaximas--;
                if (iteracinesMaximas <= 0)
                {
                    Console.WriteLine("---------------------Minimo local-------------------------");
                    return false;
                }

                if (Console.KeyAvailable)
                {
                    Console.WriteLine("---------------------Boton de escape-------------------------");
                    return true;
                }

                AplicarPropagacionDeVuelta(salidas, salidaDeseada, alpha);
                err = ErrorGeneral(salidas, salidaDeseada);

                timer--;
                if (timer <= 0)
                {
                    timer = 1000;
                    Console.Clear();
                    Console.WriteLine("Entrenando la Red, este proceso puede tardar varios minutos");
                    Console.WriteLine($"Erro actual: {err}, Error esprado: {errorMaximo} Cuando los valores sean cercanos estara Lista para usar.");
                    Console.WriteLine($"Iteraciones restantes: {iteracinesMaximas}");
                }
                
                
                //Console.WriteLine( (err * 100) / errorMaximo );
            }
            return true;
        }

        List<double[]> sigmas;
        List<double[,]> deltas;

        void SetSigmas(double[] salidaDeseada)
        {
            sigmas = new List<double[]>();
            for (int i = 0; i < capas.Count; i++)
            {
                sigmas.Add(new double[capas[i].numeroDeNeuronas]);
            }
            for (int i = capas.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < capas[i].numeroDeNeuronas; j++)
                {
                    if (i == capas.Count - 1)
                    {
                        double y = capas[i].neuronas[j].ultimaActivacion;
                        sigmas[i][j] = (Neurona.Sigmoid(y) - salidaDeseada[j]) * Neurona.SigmoidDerivada(y);
                    }
                    else
                    {
                        double sum = 0;
                        for (int k = 0; k < capas[i + 1].numeroDeNeuronas; k++)
                        {
                            sum += capas[i + 1].neuronas[k].pesos[j] * sigmas[i + 1][k];
                        }
                        sigmas[i][j] = Neurona.SigmoidDerivada(capas[i].neuronas[j].ultimaActivacion) * sum;
                    }
                }
            }
        }
        void SetDeltas()
        {
            deltas = new List<double[,]>();
            for (int i = 0; i < capas.Count; i++)
            {
                deltas.Add(new double[capas[i].numeroDeNeuronas, capas[i].neuronas[0].pesos.Length]);
            }
        }
        void AgregarDelta()
        {
            for (int i = 1; i < capas.Count; i++)
            {
                for (int j = 0; j < capas[i].numeroDeNeuronas; j++)
                {
                    for (int k = 0; k < capas[i].neuronas[j].pesos.Length; k++)
                    {
                        deltas[i][j, k] += sigmas[i][j] * Neurona.Sigmoid(capas[i - 1].neuronas[k].ultimaActivacion);
                    }
                }
            }
        }
        void ActualizarTendencia(double alpha)
        {
            for (int i = 0; i < capas.Count; i++)
            {
                for (int j = 0; j < capas[i].numeroDeNeuronas; j++)
                {
                    capas[i].neuronas[j].tendencia -= alpha * sigmas[i][j];
                }
            }
        }
        void ActualizarPesos(double alpha)
        {
            for (int i = 0; i < capas.Count; i++)
            {
                for (int j = 0; j < capas[i].numeroDeNeuronas; j++)
                {
                    for (int k = 0; k < capas[i].neuronas[j].pesos.Length; k++)
                    {
                        capas[i].neuronas[j].pesos[k] -= alpha * deltas[i][j, k];
                    }
                }
            }
        }
        void AplicarPropagacionDeVuelta(List<double[]> entradas, List<double[]> salidaDeseada, double alpha)
        {
            SetDeltas();
            for (int i = 0; i < entradas.Count; i++)
            {
                Activar(entradas[i]);
                SetSigmas(salidaDeseada[i]);
                ActualizarTendencia(alpha);
                AgregarDelta();
            }
            ActualizarPesos(alpha);
        }
    }
}
