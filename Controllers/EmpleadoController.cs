using LaboratorioEmpleadosMVC.Data;
using LaboratorioEmpleadosMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LaboratorioEmpleadosMVC.Controllers
{
    public class EmpleadoController : Controller
    {

        private PviLabDataConnection db = new PviLabDataConnection();


        /// <summary>
        /// Devuelve la lista de provincias en formato JSON para AJAX.
        /// </summary>
        [HttpGet]
        public JsonResult ObtenerProvincias()
        {
            // Consulta las provincias mediante la conexión a datos.
            var provincias = db.ObtenerProvincias();

            // Permite que la respuesta JSON sea consumida por AJAX.
            return Json(provincias, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Devuelve los cantones correspondientes a una provincia.
        /// </summary>
        /// <param name="idProvincia">ID de la provincia seleccionada.</param>
        [HttpGet]
        public JsonResult ObtenerCantones(int idProvincia)
        {
            // Consulta únicamente los cantones de la provincia recibida.
            var cantones = db.ObtenerCantones(idProvincia);

            // Devuelve los cantones para actualizar el segundo dropdown.
            return Json(cantones, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Devuelve los distritos correspondientes a un cantón.
        /// </summary>
        /// <param name="idCanton">ID del cantón seleccionado.</param>
        [HttpGet]
        public JsonResult ObtenerDistritosPorCanton(int idCanton)
        {
            // Consulta únicamente los distritos del cantón recibido.
            var distritos = db.ObtenerDistritosPorCanton(idCanton);

            // Devuelve los distritos para actualizar el tercer dropdown.
            return Json(distritos, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Index()
        {
            using (var db = new PviLabDataConnection())
            {
                var datosSP = db.ObtenerEmpleados();

                var empleados = datosSP.Select(e => new Empleado
                {
                    Id = e.Id,
                    Nombre = e.NombreEmpleado,

                    Costo = e.TipoTrabajador == "Supervisor"
                        ? e.CostoHora * 1.5m
                        : e.CostoHora * 1m,

                    Provincia = e.Provincia,
                    Canton = e.Canton,
                    Distrito = e.Distrito

                }).ToList();

                return View(empleados);
            }
        }





        // GET: Empleado/InsertarEmpleados
        // Carga el formulario para insertar un empleado nuevo o editar uno existente.
        public ActionResult InsertarEmpleados(int? id)
        {
            try
            {
                // Valores iniciales para los dropdowns en cascada.
                ViewBag.IdProvinciaSeleccionada = 0;
                ViewBag.IdCantonSeleccionado = 0;
                ViewBag.IdDistritoSeleccionado = 0;

                // Si no se recibe un ID, se prepara un formulario nuevo.
                if (id == null)
                {
                    return View(new EmpleadoInsertarViewModel());
                }

                // Busca los datos del empleado que se desea editar.
                var empleado = db.ObtenerEmpleadoPorId(id.Value);

                // Si el empleado no existe, informa al usuario.
                if (empleado == null)
                {
                    ViewBag.Mensaje = "No se encontró el empleado.";
                    return View(new EmpleadoInsertarViewModel());
                }

                // Obtiene el cantón al que pertenece el distrito del empleado.
                int idCanton =
                    db.ObtenerCantonDelDistrito(empleado.IdDistrito);

                // Obtiene la provincia correspondiente a ese distrito.
                int idProvincia =
                    db.ObtenerProvinciaDelDistrito(empleado.IdDistrito);

                // Guarda las selecciones para que JavaScript las restaure.
                ViewBag.IdProvinciaSeleccionada = idProvincia;
                ViewBag.IdCantonSeleccionado = idCanton;
                ViewBag.IdDistritoSeleccionado = empleado.IdDistrito;

                // Devuelve el formulario con los datos del empleado.
                return View(empleado);
            }
            catch (Exception ex)
            {
                // Informa si ocurre un error al cargar el formulario.
                ViewBag.Mensaje =
                    "Error al cargar el empleado: " + ex.Message;

                // Devuelve un modelo vacío para evitar una vista sin modelo.
                return View(new EmpleadoInsertarViewModel());
            }
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertarEmpleados(EmpleadoInsertarViewModel empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (empleado.Id > 0)
                    {
                        db.ActualizarEmpleado(empleado);
                        TempData["Mensaje"] = "Empleado actualizado correctamente.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.InsertarEmpleado(empleado);
                        TempData["Mensaje"] = "Empleado insertado correctamente.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al procesar el empleado: " + ex.Message;
            }

            CargarDistritos();

            return View(empleado);
        }





        [HttpGet]
        public ActionResult ConfirmarEliminar(int id)
        {
            var empleado = db.ObtenerEmpleadoPorId(id);

            if (empleado == null)
            {
                TempData["Mensaje"] = "No se encontró el empleado.";
                return RedirectToAction("Index");
            }

            return View(empleado);
        }








        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            try
            {
                db.EliminarEmpleado(id);
                TempData["Mensaje"] = "Empleado eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar el empleado: " + ex.Message;
            }

            return RedirectToAction("Index");
        }







        private void CargarDistritos(int idCanton = 1, int? idDistritoSeleccionado = null)
        {
            var distritos = db.ObtenerDistritos(idCanton);

            foreach (var distrito in distritos)
            {
                distrito.Selected =
                    idDistritoSeleccionado.HasValue &&
                    distrito.Value == idDistritoSeleccionado.Value.ToString();
            }

            ViewBag.IdDistrito = distritos;
        }





    }
}