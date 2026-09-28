using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using LaboratorioEmpleadosMVC.Models;

namespace LaboratorioEmpleadosMVC.Data
{
    public class PviLabDataConnection : DataConnection
    {
        public PviLabDataConnection()
            : base(
                new DataOptions()
                    .UseConnectionString(
                        ProviderName.SqlServer2012,
                        ConfigurationManager
                            .ConnectionStrings["PVI_LAB"]
                            .ConnectionString))
        {
        }

        public List<EmpleadoSPResult> ObtenerEmpleados()
        {
            var command = new CommandInfo(
                this,
                "dbo.sp_readEmpleados");

            var resultados = command.QueryProc(
                reader => new EmpleadoSPResult
                {
                    Id = Convert.ToInt32(reader[0]),
                    NombreEmpleado = reader[1]?.ToString(),
                    TipoTrabajador = reader[2]?.ToString(),
                    CostoHora = Convert.ToDecimal(reader[3]),
                    IdDistritoEmpleado = Convert.ToInt32(reader[4]),
                    Provincia = reader[5]?.ToString(),
                    IdProvincia = Convert.ToInt32(reader[6]),
                    IdCanton = Convert.ToInt32(reader[7]),
                    Canton = reader[8]?.ToString(),
                    IdDistrito = Convert.ToInt32(reader[9]),
                    Distrito = reader[10]?.ToString(),
                    Estado = reader[11]?.ToString()
                });

            return resultados.ToList();
        }
    }
}