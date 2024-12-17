using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTO;
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

        //GET: Nuevo_Camion
        public ActionResult Nuevo_Camion()
        {
            ViewBag.Titulo = "Nuevo camion";
            cargarDDL();
            return View();
        }

        //POST:Nuevo_Camion
        [HttpPost]
        public ActionResult Nuevo_Camion(Camiones_DTO model, HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (TransportesEntities context = new TransportesEntities())
                    {
                        var camion = new Camiones();//creo una instancia de un objeto del modelo original (<PROYECTO>.Models)
                        //asignar todos los valores de modelo d entrada (DTO) al otro objetop que sera enviado a la BD
                        camion.Matricula = model.Matricula;
                        camion.Marca = model.Marca;
                        camion.Modelo = model.Modelo;
                        camion.Tipo_Camion = model.Tipo_Camion;
                        camion.Capacidad = model.Capacidad;
                        camion.Kilometraje = model.Kilometraje;
                        camion.Disponibilidad = model.Disponibilidad;
                        //validamos si existe una imagen en la peticion 
                        if (imagen != null && imagen.ContentLength>0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);//recupero el nombre la imagen que viene de la peticion
                            string pathdir = Server.MapPath("~/Assets/Imagenes/Camiones/");//mapeo la ruta donde guardare mi imagen en el servidor
                            if(!Directory.Exists(pathdir))//sino existe el directorio, lo creo{
                            {
                                Directory.CreateDirectory(pathdir);
                            }
                            imagen.SaveAs(pathdir + filename);//guardo la imagen en el servidor
                            camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;//guardo la ruta y el nombre del archivo para enviarla a la BD

                            //Impacto sobre la BD usando EF
                            context.Camiones.Add(camion);//agregar un nuevo camion al contexto 
                            context.SaveChanges();//inpacto la base de datos enviando las notificaciones sufridas en el contexto

                            return RedirectToAction("Index");//finalmente, regreso al listado de este mismo controlador (Camiones ) si es que todo salio bien.
                        }
                        else
                        {
                            cargarDDL();
                            return View(model);
                        }
                    } 
                }
                else
                {
                    cargarDDL();
                    return View(model);
                }
            }catch(Exception ex)
            {
                //en caso de que ocurra una exepcion, voy a mostrar un msj con el error(SweetAlert), voy a devolver la vista del modeo que causo elmconflicto (return View(model)) y vuelvona cargar el DDL que esten disponibles esas opciones 
                cargarDDL();
                return View();
            }
        }

        #region Auxilires
        private class Opciones
        {
            public string Numero { get; set; }
            public string Descripcion { get; set; }
        }

        public void cargarDDL()
        {
            List<Opciones> lista_opciones = new List<Opciones>()
            {
                new Opciones() { Numero = "0", Descripcion = "Selecciones una opcioón"},
                new Opciones() { Numero = "1", Descripcion = "Volteo" },
                new Opciones() { Numero = "2", Descripcion = "Redilas" },
                new Opciones() { Numero = "3", Descripcion = "Transporte" }
            };
            ViewBag.ListaTipos = lista_opciones;
        }
        #endregion
    }
}