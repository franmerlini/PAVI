﻿using BugTrackingSystem.CapaAccesoDatos;
using BugTrackingSystem.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTrackingSystem.CapaLogicaNegocio
{
    public class UsuarioService
    {
        //---Atributos---
        private UsuarioDao usuarioDao;

        //---Metodos---
        //Constructor
        public UsuarioService()
        {
            usuarioDao = new UsuarioDao();
        }
        //Validar un usuario pasando como parametro el nombre y la contraseña
        public Usuario ValidarUsuario(string nom, string cont)
        {
            Dictionary<string, object> parametro = new Dictionary<string, object>();
            parametro.Add("nombreExacto", nom);

            var usu = usuarioDao.ObtenerUsuarios(parametro);

            if (usu[0] != null && usu[0].Contrasena.Equals(cont))
            {
                return usu[0];
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
            return usuarioDao.ActualizarUsuario(usuario);
        }
        //Eliminar un usuario
        internal bool EliminarUsuario(Usuario usuario)
        {
            return usuarioDao.EliminarUsuario(usuario);
        }
        //Consultar varios usuarios por filtros
        internal IList<Usuario> ObtenerUsuarios(Dictionary<string, object> filtros = null)
        {
            return usuarioDao.ObtenerUsuarios(filtros);
        }
    }
}
