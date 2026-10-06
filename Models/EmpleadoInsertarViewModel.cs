using System.ComponentModel.DataAnnotations;

namespace LaboratorioEmpleadosMVC.Models
{
    public class EmpleadoInsertarViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El tipo de trabajador es obligatorio.")]
        public int TipoTrabajador { get; set; }

        [Required(ErrorMessage = "El costo por hora es obligatorio.")]
        public int CostoHora { get; set; }

        [Required(ErrorMessage = "El distrito es obligatorio.")]
        public int IdDistrito { get; set; }
    }
}