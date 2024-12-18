using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DTO;
using Trasportes_MVC.Models;

namespace Trasportes_MVC.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "CamionesService" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione CamionesService.svc o CamionesService.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class CamionesService : ICamionesService
    {
        //creo una instacia de mi contexto que se visble para tidos los metodps 
        private readonly TransportesEntities _context;
        public CamionesService()
        {
            _context = new TransportesEntities();
        }
        public string create_Camion(string Matricula, string Tipo_Camion, string Marca, string Modelo, int Capacidad, double Kilometraje, string UrlFoto, bool Disponibilidad)
        {
            //preparo una respuesta
            string repuesta = "";
            try
            {
                Camiones _camione = new Camiones();
                _camione.Matricula = Matricula;
                _camione.Tipo_Camion = Tipo_Camion;
                _camione.Marca = Marca;
                _camione.Modelo = Modelo; 
                _camione.Capacidad = Capacidad;
                _camione.Kilometraje = Kilometraje;
                _camione.UrlFoto = UrlFoto;
                _camione.Disponibilidad = Disponibilidad;

                _context.Camiones.Add(_camione);
                _context.SaveChanges();
                return repuesta = "Camion regustrado con exito";
            }
            catch(Exception ex)
            {
                return repuesta ="Error: " + ex.Message;
            }
        }

        public string delete_camion(int ID_Camion)
        {
            string respuesta= "";
            try
            {
                Camiones _camion = _context.Camiones.Find(ID_Camion);
                _context.SaveChanges();
                return respuesta = $"Camion{ID_Camion} Eliminado con exito"; 

            }catch(Exception ex)
            {
                return respuesta= "Error: " + ex.Message;
            }
        }

        public List<Camiones_DTO> list_camiones(int id)
        {
            List<Camiones_DTO> list = new List<Camiones_DTO>();
            try
            {
                list = (from c in _context.Camiones
                        where (id == 0 || c.ID_Camion == id)
                        select new Camiones_DTO()
                        {
                            ID_Camion = c.ID_Camion,
                            Matricula = c.Matricula,
                            Marca = c.Marca,
                            Modelo=c.Modelo,
                            Capacidad=c.Capacidad,
                            Kilometraje=c.Kilometraje,
                            UrlFoto=c.UrlFoto,
                            Disponibilidad=c.Disponibilidad,
                            Tipo_Camion=c.Tipo_Camion
                        
                        }).ToList();
            }catch(Exception ex)
            {
                throw new Exception();
            }
            return list;
        }

        public string update_camion(int ID_Camion, string Matricula, string Tipo_Camion, string Marca, string Modelo, int Capacidad, double Kilometraje, string UrlFoto, bool Disponibilidad)
        {
            //preparo una respuesta
            string respuesta = "";
            try
            {
                Camiones _camione = new Camiones();
                _camione.Matricula = Matricula;
                _camione.Tipo_Camion = Tipo_Camion;
                _camione.Marca = Marca;
                _camione.Modelo = Modelo;
                _camione.Capacidad = Capacidad;
                _camione.Kilometraje = Kilometraje;
                _camione.UrlFoto = UrlFoto;
                _camione.Disponibilidad = Disponibilidad;

                _context.Entry(_camione).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return respuesta = "Camion actualizado con exito";
            }
            catch(Exception ex)
            {
                return respuesta = "Error" + ex.Message;
            }
        }
    }
}
