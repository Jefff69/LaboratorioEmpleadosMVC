using System.Linq;
using System.Web.Mvc;
using LaboratorioEmpleadosMVC.Data;
using LaboratorioEmpleadosMVC.Models;

namespace LaboratorioEmpleadosMVC.Controllers
{
    public class EmpleadoController : Controller
    {
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
    }
}