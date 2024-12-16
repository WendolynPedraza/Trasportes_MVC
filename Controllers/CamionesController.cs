using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trasportes_MVC.Models;

namespace Trasportes_MVC.Controllers
{
    public class CamionesController : Controller
    {
        // GET: Camiones
        public ActionResult Index()
        {
            //Crear la lista de camiones del modelo original
            List<Camiones> lista_camiones = new List<Camiones>();
            //lleno la lista con elelemntos existentes dentro dle contexto (BD) ustoilizando EF y LinQ
            using (TransportesEntities context =new TransportesEntities())
            {
                //lleno mi loista directamente usando LinQ
                //consulta directa
                //lista_camiones = (from camion in context.Camiones select camion).ToList();
                //otra forma de usar LinQ=
                lista_camiones = context.Camiones.ToList();
                //otra forma de hacerlo
                //foreach(Camiones cam in context.Camiones)
                //{
                  //  lista_camiones.Add(cam);
                //}
            }
            //ViewBang (forma parte de razor) se caracteriza por hacer uso de una propiedad arbitraria que sirve para pasar informacion desde el controlador a la vista 
            ViewBag.Titulo = "Lista de camiones";
            ViewBag.Subtitulo = "Utilizando ASP.NET MVC";

            //ViewData se caracteriza por ahacer uso de un atributo arbitrario y tiene el mismo funcionamiento que el ViewBag
            ViewData["Titulo2"] = "Segundo Titulo";

            //TempData se caracteriza por permitir crear variables temporales que existen dutante la ejecucion del Runtime de ASP
            //ademas , los temdata me permiten compartir informacion no soslo del controlador a la vista, sino tambien entre otras vistas y otros controladores 
            //TempData.Add("Clave", "Valor");

            //retorno la vista con los datos del modelo 
            return View(lista_camiones);
        }
    }
}