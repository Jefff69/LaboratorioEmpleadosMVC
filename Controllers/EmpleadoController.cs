using LaboratorioEmpleadosMVC.Data;
using LaboratorioEmpleadosMVC.Models;
using System;
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
                    return View(new EmpleadoInsertarViewModel());
                }

                var empleado = db.ObtenerEmpleadoPorId(id.Value);

                if (empleado == null)
                {
                    ViewBag.Mensaje = "No se encontró el empleado.";
                    return View(new EmpleadoInsertarViewModel());
                }

                return View(empleado);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al cargar el empleado: " + ex.Message;
                return View(new EmpleadoInsertarViewModel());
            }
        }



        // POST: Empleado/InsertarEmpleados
        [HttpPost]
        public ActionResult InsertarEmpleados(EmpleadoInsertarViewModel empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (empleado.Id > 0)
                    {
                        db.ActualizarEmpleado(empleado);

                        ViewBag.Mensaje = "Empleado actualizado correctamente.";
                    }
                    else
                    {
                        db.InsertarEmpleado(empleado);

                        ViewBag.Mensaje = "Empleado insertado correctamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al procesar el empleado: " + ex.Message;
            }

            return View(empleado);
        }




    }
}