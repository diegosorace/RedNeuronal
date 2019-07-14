using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace RedNeuronal
{
    public class Red
    {
        readonly CapaEntrada _capaEntrada;
        readonly CapaSalida _capaSalida;
        readonly CapaOculta _capaOculta;

        readonly bool _cargarRedNeuronal;
        readonly bool _guardarRedNeuronal = true;

        readonly string _direccionDatosEntrada;
        readonly string _direccionDatosSalida;
        readonly string _direccionRedNeuronal;

        readonly double _alpha;
        readonly double _errorMax;
        readonly long _iteracionesDeEntrenamiento;

        public Red(RedConfig redConfig)
        {
            _capaEntrada = redConfig.capaEntrada;
            _capaOculta = redConfig.capaOculta;
            _capaSalida = redConfig.capaSalida;
            _cargarRedNeuronal = redConfig.cargarRedNeuronal;
            _guardarRedNeuronal = redConfig.guardarRedNeuronal;
            _direccionDatosEntrada = redConfig.direccionDatosEntrada;
            _direccionDatosSalida = redConfig.direccionDatosSalida;
            _direccionRedNeuronal = redConfig.direccionRedNeuronal;
            _alpha = redConfig.alpha;
            _errorMax = redConfig.errorMax;
            _iteracionesDeEntrenamiento = redConfig.iteracionesDeEntrenamiento;
        }
        void LeerDatos()
        {
            string datos = File.ReadAllText(_direccionDatosEntrada).Replace("\r", "");

            string[] filas = datos.Split(Environment.NewLine.ToCharArray());
            for (int i = 0; i < filas.Length; i++)
            {
                string[] datosFila = filas[i].Split(';');

                double[] _entradas = new double[_capaEntrada.cantidadEntradas];
                double[] _salidas = new double[_capaSalida.cantidadSalidas];

                for (int j = 0; j < datosFila.Length; j++)
                {
                    if (j < _capaEntrada.cantidadEntradas)
                    {
                        _entradas[j] = NormalizarEntrada(double.Parse(datosFila[j]), _capaEntrada.valorMinimoEntrada, _capaEntrada.valorMaximoEntrada);
                    }
                    else
                    {
                        _salidas[j - _capaEntrada.cantidadEntradas] = NormalizarEntrada(double.Parse(datosFila[j]), _capaSalida.valorMinimoSalida, _capaSalida.valorMaximoSalida);
                    }
                }

                _capaEntrada.entradas.Add(_entradas);
                _capaSalida.salidas.Add(_salidas);
            }
        }
        double NormalizarEntrada(double valor, double min, double max)
        {
            return (valor - min) / (max - min);
        }
        double NormalizarSalida(double valor, double min, double max)
        {
            return valor * (max - min) + min;
        }
        void EntradaSalida(Perceptron p)
        {
            File.WriteAllText(_direccionDatosSalida, "");

            while (true)
            {
                double[] val = new double[_capaEntrada.cantidadEntradas];
                for (int i = 0; i < _capaEntrada.cantidadEntradas; i++)
                {
                    Console.WriteLine($"Ingrese un valor {i}: ");
                    var result = double.Parse(Console.ReadLine());
                    File.AppendAllText(_direccionDatosSalida, $"{result};");
                    val[i] = NormalizarEntrada(result, _capaEntrada.valorMinimoEntrada, _capaEntrada.valorMaximoEntrada);
                }

                double[] sal = p.Activar(val);
                for (int i = 0; i < _capaSalida.cantidadSalidas; i++)
                {
                    var result = NormalizarSalida(sal[i], _capaSalida.valorMinimoSalida, _capaSalida.valorMaximoSalida);
                    File.AppendAllText(_direccionDatosSalida, $"{result};");
                    Console.Write($"Respuesta {i}: {result}");
                }

                File.AppendAllText(_direccionDatosSalida, $"\n");

                Console.WriteLine("");
            }
        }
        public void Ejecutar()
        {
            Perceptron p;

            if (!_cargarRedNeuronal)
            {
                LeerDatos();
                List<int> capas = new List<int>();

                capas.Add(_capaEntrada.entradas.First().Length);
                for (int i = 0; i < _capaOculta.cantidadCapas; i++)
                {
                    capas.Add(_capaOculta.cantidadNeuronasPorCapa);
                }
                capas.Add(_capaSalida.salidas.First().Length);

                p = new Perceptron(capas.ToArray());

                while (!p.Entrenar(_capaEntrada.entradas, _capaSalida.salidas, _alpha, _errorMax, _iteracionesDeEntrenamiento))
                {
                    p = new Perceptron(capas.ToArray());
                }

                if (_guardarRedNeuronal)
                {
                    FileStream fs = new FileStream(_direccionRedNeuronal, FileMode.Create);
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        formatter.Serialize(fs, p);
                    }
                    catch (SerializationException e)
                    {
                        Console.WriteLine($"Fallo la serializacion: {e.Message}");
                        throw;
                    }
                    finally
                    {
                        fs.Close();
                    }
                }
            }
            else
            {
                FileStream fs = new FileStream(_direccionRedNeuronal, FileMode.Open);
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    p = (Perceptron)bf.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine($"Fallo la deserealizacion: {e.Message}");
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
            EntradaSalida(p);
        }
    }
}
