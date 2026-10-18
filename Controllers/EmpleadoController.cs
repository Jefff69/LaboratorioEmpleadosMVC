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
        public ActionResult InsertarEmpleados(int? id)
        {
            try
            {
                if (id == null)
                {
                    CargarDistritos();
                    return View(new EmpleadoInsertarViewModel());
                }

                var empleado = db.ObtenerEmpleadoPorId(id.Value);

                if (empleado == null)
                {
                    CargarDistritos();
                    ViewBag.Mensaje = "No se encontró el empleado.";
                    return View(new EmpleadoInsertarViewModel());
                }

                int idCanton = db.ObtenerCantonDelDistrito(empleado.IdDistrito);

                CargarDistritos(idCanton, empleado.IdDistrito);

                return View(empleado);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al cargar el empleado: " + ex.Message;

                try
                {
                    CargarDistritos();
                }
                catch
                {
                    ViewBag.IdDistrito = new List<System.Web.Mvc.SelectListItem>();
                }

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