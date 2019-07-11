using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Json;

namespace RedNeuronal
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = JsonObject.Load(new System.IO.FileStream($"{Environment.CurrentDirectory}/config.json",System.IO.FileMode.Open));

            RedConfig redConfig = new RedConfig
            {
                capaEntrada = new CapaEntrada
                {
                    entradas = new List<double[]>(),
                    cantidadEntradas = int.Parse(configuration["capaEntrada"]["cantidadEntradas"].ToString(), CultureInfo.InvariantCulture),
                    valorMaximoEntrada = int.Parse(configuration["capaEntrada"]["valorMaximoEntrada"].ToString(), CultureInfo.InvariantCulture),
                    valorMinimoEntrada = int.Parse(configuration["capaEntrada"]["valorMinimoEntrada"].ToString(), CultureInfo.InvariantCulture)
                },
                capaSalida = new CapaSalida
                {
                    salidas = new List<double[]>(),
                    cantidadSalidas = int.Parse(configuration["capaSalida"]["cantidadSalidas"].ToString(), CultureInfo.InvariantCulture),
                    valorMaximoSalida = int.Parse(configuration["capaSalida"]["valorMaximoSalida"].ToString(), CultureInfo.InvariantCulture),
                    valorMinimoSalida = int.Parse(configuration["capaSalida"]["valorMinimoSalida"].ToString(), CultureInfo.InvariantCulture)
                },
                capaOculta = new CapaOculta
                {
                    cantidadCapas = int.Parse(configuration["capaOculta"]["cantidadCapas"].ToString(), CultureInfo.InvariantCulture),
                    cantidadNeuronasPorCapa = int.Parse(configuration["capaOculta"]["cantidadNeuronasPorCapa"].ToString(), CultureInfo.InvariantCulture)
                },
                cargarRedNeuronal = bool.Parse(configuration["cargarRedNeuronal"].ToString()),
                guardarRedNeuronal = bool.Parse(configuration["guardarRedNeuronal"].ToString()),
                direccionDatosEntrada = configuration["direccionDatosEntrada"],
                direccionDatosSalida = configuration["direccionDatosSalida"],
                direccionRedNeuronal = configuration["direccionRedNeuronal"],
                alpha = double.Parse(configuration["alpha"].ToString(), CultureInfo.InvariantCulture),
                errorMax = double.Parse(configuration["errorMax"].ToString(), CultureInfo.InvariantCulture),
                iteracionesDeEntrenamiento = int.Parse(configuration["iteracionesDeEntrenamiento"].ToString(), CultureInfo.InvariantCulture)
            };
            
            Red red = new Red
            (
                redConfig
            );
            red.Ejecutar();
        }
    }
}

