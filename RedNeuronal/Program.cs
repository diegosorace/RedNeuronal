using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
                    cantidadEntradas = int.Parse(configuration["capaEntrada"]["cantidadEntradas"].ToString()),
                    valorMaximoEntrada = int.Parse(configuration["capaEntrada"]["cantidadEntradas"].ToString()),
                    valorMinimoEntrada = int.Parse(configuration["capaEntrada"]["cantidadEntradas"].ToString())
                },
                capaSalida = new CapaSalida
                {
                    salidas = new List<double[]>(),
                    cantidadSalidas = int.Parse(configuration["capaSalida"]["cantidadSalidas"].ToString()),
                    valorMaximoSalida = int.Parse(configuration["capaSalida"]["valorMaximoSalida"].ToString()),
                    valorMinimoSalida = int.Parse(configuration["capaSalida"]["valorMinimoSalida"].ToString())
                },
                capaOculta = new CapaOculta
                {
                    cantidadCapas = int.Parse(configuration["capaOculta"]["cantidadCapas"].ToString()),
                    cantidadNeuronasPorCapa = int.Parse(configuration["capaOculta"]["cantidadNeuronasPorCapa"].ToString())
                },
                cargarRedNeuronal = bool.Parse(configuration["cargarRedNeuronal"].ToString()),
                guardarRedNeuronal = bool.Parse(configuration["guardarRedNeuronal"].ToString()),
                direccionDatosEntrada = configuration["direccionDatosEntrada"].ToString(),
                direccionDatosSalida = configuration["direccionDatosSalida"].ToString(),
                direccionRedNeuronal = configuration["direccionRedNeuronal"].ToString(),
                alpha = double.Parse(configuration["alpha"].ToString()),
                errorMax = double.Parse(configuration["errorMax"].ToString()),
                iteracionesDeEntrenamiento = int.Parse(configuration["iteracionesDeEntrenamiento"].ToString())
            };

            Red red = new Red
            (
                redConfig
            );
            red.Ejecutar();
        }
    }
}

