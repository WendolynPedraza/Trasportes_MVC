using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Trasportes_MVC.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "CalculadoraServer" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione CalculadoraServer.svc o CalculadoraServer.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class CalculadoraServer : ICalculadoraServer//nos posicionamos en ICalculadira y dar ctrl punto (generar interfaz)
    {
        public double dividir(double a, double b)
        {
            return a/b;
        }

        public double multiplicar(double a, double b)
        {
            return a*b;
        }

        public double restar(double a, double b)
        {
            return a-b;
        }

        public double sumar(double a, double b)
        {
            return a+b;
        }
    }
}
