﻿using BugTrackingSystem.CapaLogicaNegocio;
using BugTrackingSystem.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTrackingSystem.CapaPresentacion.Consultas.ConsultaSueldos
{
    public partial class FrmSueldosDetalles : Form
    {
        private readonly SueldoAsignacionService sueldoAsignacionesService;
        private readonly SueldoDescuentoService sueldoDescuentosService;
        private readonly SueldoService sueldoService;

        public FrmSueldosDetalles(Dictionary<string, object> parametros)
        {
            InitializeComponent();
            InitializeDataGridViewDescuentos();
            InitializeDataGridViewAsignaciones();
            sueldoAsignacionesService = new SueldoAsignacionService();
            sueldoDescuentosService = new SueldoDescuentoService();
            sueldoService = new SueldoService();

            Sueldo sueldo = (sueldoService.ObtenerSueldos(parametros)).First();
            IList<SueldoAsignacion> listadoAsignaciones = sueldoAsignacionesService.ObtenerSueldoAsignaciones(parametros);
            DgvAsignaciones.DataSource = listadoAsignaciones;
            IList<SueldoDescuento> listadoDescuentos = sueldoDescuentosService.ObtenerSueldoDescuentos(parametros);
            DgvDescuentos.DataSource = listadoDescuentos;

            TxtUsuario.Text = sueldo.Usuario.Nombre;
            TxtFecha.Text = sueldo.Fecha.ToString("dd/MM/yyyy");
            TxtSueldoBruto.Text = sueldo.SueldoBruto.ToString("C");
            decimal totalAsignaciones = listadoAsignaciones.Sum(a => a.Monto);
            TxtAsignaciones.Text = totalAsignaciones.ToString("C");
            decimal totalDescuentos = listadoDescuentos.Sum(d => d.Monto);
            TxtDescuentos.Text = totalDescuentos.ToString("C");
            var importeTotal = sueldo.SueldoBruto + totalAsignaciones - totalDescuentos;
            TxtTotal.Text = importeTotal.ToString("C");
        }

        private void InitializeDataGridViewDescuentos()
        {
            // Cree un DataGridView no vinculado declarando un recuento de columnas.
            DgvDescuentos.ColumnCount = 3;
            DgvDescuentos.ColumnHeadersVisible = true;

            // Configuramos la AutoGenerateColumns en false para que no se autogeneren las columnas
            DgvDescuentos.AutoGenerateColumns = false;

            // Cambia el estilo de la cabecera de la grilla.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle
            {
                BackColor = Color.Beige,
                Font = new Font("Verdana", 8, FontStyle.Bold)
            };
            DgvDescuentos.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            // Definimos el nombre de la columnas y el DataPropertyName que se asocia a DataSource

            CrearColumnas(DgvDescuentos, 0, "Descuento", "Descuento", 190);
            CrearColumnas(DgvDescuentos, 1, "Importe Total", "Monto", 170);
            DgvDescuentos.Columns[1].DefaultCellStyle.Format = "C";
            CrearColumnas(DgvDescuentos, 2, "Cantidad", "Cantidad", 160);
        }

        private void InitializeDataGridViewAsignaciones()
        {
            // Cree un DataGridView no vinculado declarando un recuento de columnas.
            DgvAsignaciones.ColumnCount = 3;
            DgvAsignaciones.ColumnHeadersVisible = true;

            // Configuramos la AutoGenerateColumns en false para que no se autogeneren las columnas
            DgvAsignaciones.AutoGenerateColumns = false;

            // Cambia el estilo de la cabecera de la grilla.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle
            {
                BackColor = Color.Beige,
                Font = new Font("Verdana", 8, FontStyle.Bold)
            };
            DgvAsignaciones.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            // Definimos el nombre de la columnas y el DataPropertyName que se asocia a DataSource

            CrearColumnas(DgvAsignaciones, 0, "Asignación", "Asignacion", 190);
            CrearColumnas(DgvAsignaciones, 1, "Importe Total", "Monto", 170);
            DgvAsignaciones.Columns[1].DefaultCellStyle.Format = "C";
            CrearColumnas(DgvAsignaciones, 2, "Cantidad", "Cantidad", 160);
        }

        private void CrearColumnas(DataGridView tabla, int columna, string nombre, string propiedad, int tamaño)
        {
            tabla.Columns[columna].Name = nombre;
            tabla.Columns[columna].DataPropertyName = propiedad;
            tabla.Columns[columna].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            tabla.Columns[columna].Width = tamaño;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // Código para poder mover la ventana desde el menu strip
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
