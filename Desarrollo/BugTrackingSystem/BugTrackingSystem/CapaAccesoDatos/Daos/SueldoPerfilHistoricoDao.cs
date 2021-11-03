﻿using BugTrackingSystem.DAO;
using BugTrackingSystem.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTrackingSystem.CapaAccesoDatos.Daos
{
    class SueldoPerfilHistoricoDao
    {
        public IList<SueldoPerfilHistorico> ObtenerSueldosPerfilHistorico(Dictionary<string, object> parametros)
        {
            List<SueldoPerfilHistorico> listadoSueldos = new List<SueldoPerfilHistorico>();

            String consultaSQL = string.Concat(" SELECT SueldoPerfilHistorico.id_perfil, ",
                                               "        SueldoPerfilHistorico.fecha, ",
                                               "        SueldoPerfilHistorico.sueldo, ",
                                               "        Perfiles.id_perfil, ",
                                               "        Perfiles.nombre ",
                                               " FROM SueldoPerfilHistorico ",
                                               " INNER JOIN Perfiles ON Perfiles.id_perfil = SueldoPerfilHistorico.id_perfil ",
                                               " WHERE 1 = 1 ");

            if (parametros != null)
            {
                if (parametros.ContainsKey("idPerfil"))
                    consultaSQL += " AND (SueldoPerfilHistorico.id_perfil = @idPerfil) ";
                if (parametros.ContainsKey("fechaDesde"))
                    consultaSQL += " AND (SueldoPerfilHistorico.fecha >= @fechaDesde) ";
                if (parametros.ContainsKey("fechaHasta"))
                    consultaSQL += " AND (SueldoPerfilHistorico.fecha <= @fechaHasta) ";
                if (parametros.ContainsKey("fechaExacta"))
                    consultaSQL += " AND (SueldoPerfilHistorico.fecha = @fechaExacta) ";
            }
            
            consultaSQL += " ORDER BY SueldoPerfilHistorico.fecha DESC";

            var resultados = DataManager.ObtenerInstancia().ConsultaSQL(consultaSQL, parametros);
            foreach (DataRow resultado in resultados.Rows)
            {
                listadoSueldos.Add(MapeoObjeto(resultado));
            }
            return listadoSueldos;
        }

        private SueldoPerfilHistorico MapeoObjeto(DataRow row)
        {
            SueldoPerfilHistorico sueldoPerfilHistorico = new SueldoPerfilHistorico
            {
                Fecha = Convert.ToDateTime(row["fecha"].ToString()),
                Sueldo = Convert.ToDecimal(row["sueldo"].ToString()),
                Perfil = new Perfil()
                {
                    IdPerfil = Convert.ToInt32(row["id_perfil"].ToString()),
                    Nombre = row["nombre"].ToString(),
                }
            };

            return sueldoPerfilHistorico;
        }
    }
}
