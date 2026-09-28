namespace LaboratorioEmpleadosMVC.Models
{
    public class EmpleadoSPResult
    {
        public int Id { get; set; }

        public string NombreEmpleado { get; set; }

        public string TipoTrabajador { get; set; }

        public decimal CostoHora { get; set; }

        public int IdDistritoEmpleado { get; set; }

        public string Provincia { get; set; }

        public int IdProvincia { get; set; }

        public int IdCanton { get; set; }

        public string Canton { get; set; }

        public int IdDistrito { get; set; }

        public string Distrito { get; set; }

        public string Estado { get; set; }
    }
}