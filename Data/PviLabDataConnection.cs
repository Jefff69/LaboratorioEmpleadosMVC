using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using LaboratorioEmpleadosMVC.Models;
using System.Web.Mvc;

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




        public void InsertarEmpleado(EmpleadoInsertarViewModel empleado)
        {
            var command = new CommandInfo(
                this,
                "EXEC dbo.sp_insertEmpleado @nombre, @apellido, @tipoTrabajador, @costoHora, @id_distrito",
                new DataParameter("@nombre", empleado.Nombre),
                new DataParameter("@apellido", empleado.Apellido),
                new DataParameter("@tipoTrabajador", empleado.TipoTrabajador),
                new DataParameter("@costoHora", empleado.CostoHora),
                new DataParameter("@id_distrito", empleado.IdDistrito)
            );

            command.Execute();
        }



        public void ActualizarEmpleado(EmpleadoInsertarViewModel empleado)
        {
            var command = new CommandInfo(
                this,
                "EXEC dbo.sp_updateEmpleado @id, @nombre, @apellido, @tipoTrabajador, @costoHora, @id_distrito, @estado",
                new DataParameter("@id", empleado.Id),
                new DataParameter("@nombre", empleado.Nombre),
                new DataParameter("@apellido", empleado.Apellido),
                new DataParameter("@tipoTrabajador", empleado.TipoTrabajador),
                new DataParameter("@costoHora", empleado.CostoHora),
                new DataParameter("@id_distrito", empleado.IdDistrito),
                new DataParameter("@estado", empleado.Estado)
            );

            command.Execute();
        }





        public void EliminarEmpleado(int id)
        {
            var command = new CommandInfo(
                this,
                "EXEC dbo.sp_deleteEmpleado @id",
                new DataParameter("@id", id)
            );

            command.Execute();
        }





        public List<System.Web.Mvc.SelectListItem> ObtenerDistritos(int idCanton = 1)
        {
            var distritos = this.Query<
                System.Web.Mvc.SelectListItem>(
                @"SELECT
              CAST(id_Distrito AS varchar(10)) AS Value,
              nombre AS Text
          FROM Distrito
          WHERE id_Canton = @idCanton
          ORDER BY nombre",
                new DataParameter("@idCanton", idCanton)
            ).ToList();

            return distritos;
        }



        public int ObtenerCantonDelDistrito(int idDistrito)
        {
            var idCanton = this.Query<int>(
                @"SELECT id_Canton
          FROM Distrito
          WHERE id_Distrito = @idDistrito",
                new DataParameter("@idDistrito", idDistrito)
            ).FirstOrDefault();

            return idCanton;
        }







        public EmpleadoInsertarViewModel ObtenerEmpleadoPorId(int id)
        {
            var empleado = this.Query<EmpleadoInsertarViewModel>(
                @"SELECT 
            Id,
            nombre AS Nombre,
            apellido AS Apellido,
            tipoTrabajador AS TipoTrabajador,
            costoHora AS CostoHora,
            id_distrito AS IdDistrito,
            estado AS Estado
          FROM Empleados
          WHERE Id = @id",
                new DataParameter("@id", id)
            ).FirstOrDefault();

            return empleado;
        }





        /// <summary>
        /// Obtiene la lista de provincias mediante el procedimiento almacenado.
        /// </summary>
        public List<SelectListItem> ObtenerProvincias()
        {
            // Ejecuta el procedimiento almacenado que devuelve las provincias.
            var command = new CommandInfo(
                this,
                "dbo.RetornaProvincias"
            );

            // Convierte cada registro en una opción para el dropdown.
            var provincias = command.QueryProc(
                reader => new SelectListItem
                {
                    // El identificador se utiliza como valor de la opción.
                    Value = reader["id_Provincia"].ToString(),

                    // El nombre es el texto visible para el usuario.
                    Text = reader["nombre"].ToString()
                }
            );

            return provincias.ToList();
        }

        /// <summary>
        /// Obtiene los cantones que pertenecen a una provincia.
        /// </summary>
        /// <param name="idProvincia">Identificador de la provincia seleccionada.</param>
        public List<SelectListItem> ObtenerCantones(int idProvincia)
        {
            // Ejecuta el procedimiento almacenado con el ID de la provincia.
            var command = new CommandInfo(
                this,
                "dbo.RetornaCantones_ProvinciaID",
                new DataParameter("@id_Provincia", idProvincia)
            );

            // Convierte los cantones en opciones para el dropdown.
            var cantones = command.QueryProc(
                reader => new SelectListItem
                {
                    // El identificador del cantón será el valor de la opción.
                    Value = reader["id_Canton"].ToString(),

                    // El nombre del cantón será el texto visible.
                    Text = reader["nombre"].ToString()
                }
            );

            return cantones.ToList();
        }

        /// <summary>
        /// Obtiene los distritos que pertenecen a un cantón.
        /// </summary>
        /// <param name="idCanton">Identificador del cantón seleccionado.</param>
        public List<SelectListItem> ObtenerDistritosPorCanton(int idCanton)
        {
            // Ejecuta el procedimiento almacenado con el ID del cantón.
            var command = new CommandInfo(
                this,
                "dbo.RetornaDistritos_CantonID",
                new DataParameter("@id_Canton", idCanton)
            );

            // Convierte los distritos en opciones para el dropdown.
            var distritos = command.QueryProc(
                reader => new SelectListItem
                {
                    // El identificador del distrito será el valor de la opción.
                    Value = reader["id_Distrito"].ToString(),

                    // El nombre del distrito será el texto visible.
                    Text = reader["nombre"].ToString()
                }
            );

            return distritos.ToList();
        }





        /// <summary>
        /// Obtiene el identificador de la provincia a la que pertenece
        /// el distrito seleccionado, pasando por su cantón.
        /// </summary>
        /// <param name="idDistrito">Identificador del distrito.</param>
        /// <returns>Identificador de la provincia o 0 si no existe.</returns>
        public int ObtenerProvinciaDelDistrito(int idDistrito)
        {
            // Consulta la provincia relacionada con el distrito indicado.
            var idProvincia = this.Query<int>(
                @"SELECT C.id_Provincia
                  FROM Distrito D
                  INNER JOIN Canton C
                      ON D.id_Canton = C.id_Canton
                  WHERE D.id_Distrito = @idDistrito",
                new DataParameter("@idDistrito", idDistrito)
            ).FirstOrDefault();

            // Devuelve el identificador de la provincia encontrada.
            return idProvincia;
        }







    }
}