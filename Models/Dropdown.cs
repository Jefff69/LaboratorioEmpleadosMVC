
using System.Collections.Generic;
using System.Web.Mvc;

namespace LaboratorioEmpleadosMVC.Models
{
    /// <summary>
    /// Modelo utilizado para administrar los datos de los
    /// dropdowns de provincia, cantón y distrito.
    /// </summary>
    public class Dropdown
    {
        // Identificador de la provincia seleccionada.
        public int IdProvincia { get; set; }

        // Identificador del cantón seleccionado.
        public int IdCanton { get; set; }

        // Identificador del distrito seleccionado.
        public int IdDistrito { get; set; }

        // Lista de provincias que se mostrarán en el formulario.
        public IEnumerable<SelectListItem> Provincias { get; set; }

        // Lista de cantones correspondientes a una provincia.
        public IEnumerable<SelectListItem> Cantones { get; set; }

        // Lista de distritos correspondientes a un cantón.
        public IEnumerable<SelectListItem> Distritos { get; set; }
    }
}
