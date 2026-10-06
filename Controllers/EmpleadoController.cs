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
        public ActionResult InsertarEmpleados()
        {
            return View();
        }




        // POST: Empleado/InsertarEmpleados
        [HttpPost]
        public ActionResult InsertarEmpleados(EmpleadoInsertarViewModel empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.InsertarEmpleado(empleado);

                    ViewBag.Mensaje = "Empleado insertado correctamente.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al insertar el empleado: " + ex.Message;
            }

            return View(empleado);
        }




    }
}