﻿using BugTrackingSystem.CapaAccesoDatos;
using BugTrackingSystem.Entidades;
using System.Collections.Generic;

namespace BugTrackingSystem.CapaLogicaNegocio
{
    public class UsuarioService
    {
        //-------------------------------------------------------Atributos-------------------------------------------------------
        private readonly UsuarioDao usuarioDao;

        //--------------------------------------------------------Metodos--------------------------------------------------------
        //Constructor
        public UsuarioService()
        {
            usuarioDao = new UsuarioDao();
        }

        //Validar un usuario pasando como parametro el nombre y la contraseña
        public Usuario ValidarUsuario(string nom, string cont)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "nombreExacto", nom }
            };

            var usu = usuarioDao.ObtenerUsuarios(parametros);

            if (usu.Count != 0)
            {
                if (usu[0] != null && usu[0].Contrasena.Equals(cont) && usu[0].Nombre.Equals(nom))
                {
                    return usu[0];
                }
            }
            return null;
        }

        //Crear un usuario
        internal bool CrearUsuario(Usuario usuario)
        {
            return usuarioDao.CrearUsuario(usuario);
        }

        //Actualizar un usuario
        internal bool ActualizarUsuario(Usuario usuario)
        {
            // Borrado en cascada para las asistencias
            if (usuario.Borrado)
            {
                var parametros = new Dictionary<string, object>()
                {
                    { "idUsuario", usuario.IdUsuario }
                };

                AsistenciaUsuarioService asistenciaUsuarioService = new AsistenciaUsuarioService();
                IList<AsistenciaUsuario> asistencias = asistenciaUsuarioService.ObtenerAsistenciasUsuario(parametros);
                foreach (AsistenciaUsuario a in asistencias)
                {
                    a.Borrado = true;
                    parametros = new Dictionary<string, object>()
                    {
                        { "fechaBase", a.Fecha },
                        { "idUsuarioBase", a.Usuario.IdUsuario }
                    };
                    asistenciaUsuarioService.ActualizarAsistenciaUsuario(a, parametros);
                }
            }

            return usuarioDao.ActualizarUsuario(usuario);
        }

        //Consultar varios usuarios por filtros
        internal IList<Usuario> ObtenerUsuarios(Dictionary<string, object> parametros = null)
        {
            return usuarioDao.ObtenerUsuarios(parametros);
        }
    }
}
