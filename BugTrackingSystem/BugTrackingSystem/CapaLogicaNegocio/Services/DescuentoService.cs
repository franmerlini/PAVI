﻿using BugTrackingSystem.CapaAccesoDatos;
using BugTrackingSystem.Entidades;
using System.Collections.Generic;

namespace BugTrackingSystem.CapaLogicaNegocio
{
    public class DescuentoService
    {
        //-------------------------------------------------------Atributos-------------------------------------------------------
        private readonly DescuentoDao descuentoDao;

        //--------------------------------------------------------Metodos--------------------------------------------------------
        //Constructor
        public DescuentoService()
        {
            descuentoDao = new DescuentoDao();
        }

        //Consultar varios descuentos por filtros
        public IList<Descuento> ObtenerDescuentos(Dictionary<string, object> parametros = null)
        {
            return descuentoDao.ObtenerDescuentos(parametros);
        }

        internal bool CrearDescuento(Descuento descuento)
        {
            return descuentoDao.CrearDescuento(descuento);
        }

        internal bool ActualizarDescuento(Descuento descuento)
        {
            return descuentoDao.ActualizarDescuento(descuento);
        }
    }
}
