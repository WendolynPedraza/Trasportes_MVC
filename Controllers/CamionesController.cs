﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DTO;
using Microsoft.Ajax.Utilities;
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
            using (TransportesEntities context = new TransportesEntities())
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
                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);//recupero el nombre la imagen que viene de la peticion
                            string pathdir = Server.MapPath("~/Assets/Imagenes/Camiones/");//mapeo la ruta donde guardare mi imagen en el servidor
                            if (!Directory.Exists(pathdir))//sino existe el directorio, lo creo{
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
            }
            catch (Exception ex)
            {
                //en caso de que ocurra una exepcion, voy a mostrar un msj con el error(SweetAlert), voy a devolver la vista del modeo que causo elmconflicto (return View(model)) y vuelvona cargar el DDL que esten disponibles esas opciones 
                cargarDDL();
                return View();
            }
        }

        //GET: Editar_camion/{id}
        public ActionResult Editar_Camion(int id)
        {
            if (id > 0)
            {
                Camiones_DTO camion = new Camiones_DTO(); //creo una instancia del tipo DTO para pasar informacion desde el contexto a la vista en ayuda de EF y LinQ
                using (TransportesEntities context = new TransportesEntities())
                {
                    //busco a aquel elemento que coincida con el ID
                    //Bajo metodo
                    //No puedo 
                    var camion_aux = context.Camiones.Where(x => x.ID_Camion == id).FirstOrDefault();
                    var camion_aux2 = context.Camiones.FirstOrDefault(x => x.ID_Camion == id);

                    camion.Matricula = camion_aux.Matricula;
                    camion.Marca = camion_aux.Marca;
                    camion.Modelo = camion_aux.Modelo;
                    camion.Capacidad = camion_aux.Capacidad;
                    camion.Kilometraje = camion_aux.Kilometraje;
                    camion.Tipo_Camion = camion_aux.Tipo_Camion;
                    camion.Disponibilidad = camion_aux.Disponibilidad;
                    camion.UrlFoto = camion_aux.UrlFoto;
                    camion.ID_Camion = camion_aux.ID_Camion;

                    //bajo una consulta (usando LinQ)
                    //cuando hago una consulta directa , tengo la oportunodad de asignar valores a tipo de datos mas complejos o diferentes, incluso, pudiendo crear nuevos datos a partir de dtos existentes 
                    camion = (from c in context.Camiones
                              where c.ID_Camion == id
                              select new Camiones_DTO()
                              {
                                  ID_Camion = c.ID_Camion,
                                  Matricula = c.Matricula,
                                  Marca = c.Marca,
                                  Modelo = c.Modelo,
                                  Capacidad = c.Capacidad,
                                  Kilometraje = c.Kilometraje,
                                  Tipo_Camion = c.Tipo_Camion,
                                  Disponibilidad = c.Disponibilidad,
                                  UrlFoto = c.UrlFoto
                              }).FirstOrDefault();
                }//cierre el Using(context)
                if (camion == null)//valido si realmente recupere los datos de la base de dtos {
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Titulo = $"Editar Camion °{camion.ID_Camion}";
                cargarDDL();
                return View(camion);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        //POST: Editar_Camion
        //POST: Editar_Camion
        [HttpPost]
        public ActionResult Editar_Camion(Camiones_DTO model, HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (TransportesEntities context = new TransportesEntities())
                    {
                        var camion = new Camiones();

                        camion.ID_Camion = model.ID_Camion;
                        camion.Matricula = model.Matricula;
                        camion.Marca = model.Marca;
                        camion.Modelo = model.Modelo;
                        camion.Capacidad = model.Capacidad;
                        camion.Tipo_Camion = model.Tipo_Camion;
                        camion.Disponibilidad = model.Disponibilidad;
                        camion.Kilometraje = model.Kilometraje;

                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Assets/Imagenes/Camiones/");
                            if (model.UrlFoto.Length == 0)
                            {
                                //la imagen en la BD es null y hay que darle la imagen
                                if (!Directory.Exists(pathdir))
                                {
                                    Directory.CreateDirectory(pathdir);
                                }

                                imagen.SaveAs(pathdir + filename);
                                camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                            }
                            else
                            {
                                //validar si es la misma o es nueva
                                if (model.UrlFoto.Contains(filename))
                                {
                                    //es la misma
                                    camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                                }
                                else
                                {
                                    //es diferente
                                    if (!Directory.Exists(pathdir))
                                    {
                                        Directory.CreateDirectory(pathdir);
                                    }

                                    //Borro la imagen anterios
                                    //valido si existe

                                    try
                                    {
                                        string pathdir_old = Server.MapPath("~" + model.UrlFoto); //busco la imagen que catualmente tiene el camión
                                        if (System.IO.File.Exists(pathdir_old)) //valido si existe dicho archivo
                                        {
                                            //procedo a eliminarlo
                                            System.IO.File.Delete(pathdir_old);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Sweet Alert
                                    }

                                    imagen.SaveAs(pathdir + filename);
                                    camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                                }
                            }
                        }
                        else //si no hya una nueva imagen, paso la misma
                        {
                            camion.UrlFoto = model.UrlFoto;
                        }

                        //Guardar cambios, validar excepciones, redirigir
                        //actualizar el estado de nuestro elemento
                        //.Entry() registrar la entrada de nueva información al contexto y notificar un cambio de estado usando System.Data.Entity.EntityState.Modified
                        context.Entry(camion).State = System.Data.Entity.EntityState.Modified;
                        //impactamos la BD
                        try
                        {
                            context.SaveChanges();
                        }
                        //agregar using desde 'using System.Data.Entity.Validation;'
                        catch (DbEntityValidationException ex)
                        {
                            string resp = "";
                            //recorro todos los posibles errores de la Entidad Referencial
                            foreach (var error in ex.EntityValidationErrors)
                            {
                                //recorro los detalles de cada error
                                foreach (var validationError in error.ValidationErrors)
                                {
                                    resp += "Error en la Entidad: " + error.Entry.Entity.GetType().Name;
                                    resp += validationError.PropertyName;
                                    resp += validationError.ErrorMessage;
                                }
                            }
                            //Sweet Alert
                        }
                        //Sweet Alert
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    //Sweet Alert
                    cargarDDL();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                //Sweet Alert
                cargarDDL();
                return View(model);
            }
        }

        //GET: Eliminar_Camion /{id}
        public ActionResult Eliminar_Camion(int id)
        {
            try
            {
                using (TransportesEntities context = new TransportesEntities())
                {
                    //recuperar el camion que deseo eliminar 
                    var camion = context.Camiones.FirstOrDefault(x => x.ID_Camion == id);
                    //validamos sis existe dicho valor 
                    if (camion == null)
                    {
                        //me voy pa tras
                        //swet
                        SweetAlert("No encontrado", $"No hemos encntrado el camion con identificador: {id}", NotificationType.info);
                        return RedirectToAction("Index");
                    }

                    //procedo a eliminar 
                    context.Camiones.Remove(camion);
                    context.SaveChanges();
                    //sweetAlert

                    SweetAlert("Elominar", "camion eloiminado con exito", NotificationType.success);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //sweetAlert
                SweetAlert("Opsss..", $"Ha ocurrido un error: {ex.Message}", NotificationType.error);

                return RedirectToAction("Index");
            }
        }

        //Get: confimar eliminar 
        public ActionResult Confirmar_Eliminar(int id)
        {
            SweetAlert_Eliminar(id);
            return RedirectToAction("Index");
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

        #region SweetAlert
        //Declaraion de un HTMLHelperpersonalizado: Digase de aquel metodo auxiliar que me permite construir codigo HTML/JS en tiempo seal basado en las acciones del Razor/Controller
        private void SweetAlert(string title, string msg, NotificationType type)
        {
            var script = "<script languaje='javascript'> " +
                         "Swal.fire({" +
                         "title: '" + title + "'," +
                         "text: '" + msg + "'," +
                         "icon: '" + type + "'" +
                         "});" +
                         "</script>";
            //tempData funcion como ViewbAG, PASANDO INFORMACION DEL CONTROLADOR A CUALQUIER PARATE DE MI PROYECTO, SIENDO ESTE, MAS OBSERVABLE Y CON UN TI´PO DE VIDA MAYOR
            TempData["sweetalert"] = script;

        }

        private void SweetAlert_Eliminar(int id)
        {
            var script = "<script languaje='javascript'>" +
                "Swal.fire({" +
                "title: '¿Estás Seguro?'," +
                "text: 'Estás apunto de Eliminar el Camión: " + id.ToString() + "'," +
                "icon: 'info'," +
                "showDenyButton: true," +
                "showCancelButton: true," +
                "confirmButtonText: 'Eliminar'," +
                "denyButtonText: 'Cancelar'" +
                "}).then((result) => {" +
                "if (result.isConfirmed) {  " +
                "window.location.href = '/Camiones/Eliminar_Camion/" + id + "';" +
                "} else if (result.isDenied) {  " +
                "Swal.fire('Se ha cancelado la operación','','info');" +
                "}" +
                "});" +
                "</script>";

            TempData["sweetalert"] = script;
        }

        public enum NotificationType
        {
            error,
            success,
            warning,
            info,
            question
            #endregion
        }
    }
}