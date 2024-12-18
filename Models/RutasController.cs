using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTO;
using System.Data.Entity;

namespace Trasportes_MVC.Models
{
    public class RutasController : Controller
    {
        //creo una instancia glogal de mi vontexto
        private TransportesEntities context = new TransportesEntities();
        // GET: Rutas
        public ActionResult Index()
        {
            //retorno el modelo exacto de view rutas  DIRECTAMENTE USANDO linq
            return View(context.View_Rutas.ToList());
        }
        public ActionResult View_LinQ()
        {
            //la idea es controlar es poder crear objeetos y datos a partir de datos ya existentes de la BD
            //creammos una lista de objetos DTO
            List<View_Rutas_DTO> lista_view = new List<View_Rutas_DTO>();

            //lleno mi lista apartir de los datos del contextos usando LinQ
            lista_view = (from r in context.Rutas//el orige del dato donde r representa cada renglo  de la tabla
                          join carg in context.Cargamentos on r.ID_Ruta equals carg.Ruta_ID//unir las tablas usando join de linq es similas r a los join en sql en este caso carg representa cada renglon de la tabla cargamentos on puedo traducirlo como donde r.ID_Ruta sea igual a carg.Ruta_Id
                          join cam in context.Camiones on r.Camion_ID equals cam.ID_Camion
                          join cho in context.Choferes on r.Chofer_ID equals cho.ID_Chofer
                          join dir_o in context.Direcciones on r.Direccionorigen_ID equals dir_o.ID_Direccion
                          join dir_d in context.Direcciones on r.Direcciondestino_ID equals dir_d.ID_Direccion

                          select new View_Rutas_DTO()
                          {
                              C_ = r.ID_Ruta, //en funcion del dato que deseo, selecciono conforme a la declracion de los  join 
                              ID_Cargamento = carg.ID_Cargamento,
                              cargamento = carg.Descripcion,
                              ID_Direccion_Origen = dir_o.ID_Direccion,
                              Origen = "Calle" + dir_o.Calle + "#" + dir_o.Numero + "CP" + dir_o.CP + "col:" + dir_o.Colonia,
                              Estado_Origen = dir_o.Estado,


                              Destino = "Calle: " + dir_d.Calle + "#" + dir_d.Numero + "col: " + dir_d.Colonia + "cp:" + dir_d.CP, 
                              Estado_Destino = dir_d.Estado,
                              ID_Chofer = cho.ID_Chofer,
                              Chofer = cho.Nombre +" "+ cho.Apellido_Paterno +" "+ cho.Apellido_Materno,
                              
                              ID_Camion = cam.ID_Camion,
                              Camión="Matricula: " +cam.Matricula +"Modelo: " + cam.Modelo + "Matricula " + cam.Matricula,
                             
                              Salida=(DateTime)r.Fecha_salida,
                              LLegada_Estimada=(DateTime)r.Fecha_llegadaestimada
                          }

               ) .ToList();
            ViewBag.Title = "Vista creada con Linq";
            return View(lista_view);
  
        }

        public ActionResult EF_Nav()
        {
            var rutas =context.Rutas.Include(r => r.Camiones)
                .Include(r => r.Choferes)
                .Include(r => r.Direcciones)
                .Include(r => r.Direcciones1)
                .Include(r => r.Cargamentos);
            ViewBag.Titulo = "Vista de rutas con navegacion EF";
            return View(rutas.ToList());
        }
        // GET: Rutas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rutas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rutas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rutas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rutas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rutas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Rutas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
