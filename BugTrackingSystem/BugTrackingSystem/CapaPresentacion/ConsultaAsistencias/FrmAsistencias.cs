﻿using BugTrackingSystem.CapaLogicaNegocio;
using BugTrackingSystem.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTrackingSystem.CapaPresentacion
{
    public partial class FrmAsistencias : Form
    {
        private readonly AsistenciaUsuarioService asistenciaService;
        private readonly UsuarioService usuarioService;
        private readonly EstadoAsistenciaService estadoAsistenciaService;

        private readonly Dictionary<string, object> parametros = new Dictionary<string, object>();

        // Métodos de carga e inicialización:

        public FrmAsistencias()
        {
            InitializeComponent();
            InitializeDataGridView();

            asistenciaService = new AsistenciaUsuarioService();
            usuarioService = new UsuarioService();
            estadoAsistenciaService = new EstadoAsistenciaService();
        }

        private void InitializeDataGridView()
        {
            // Cree un DataGridView no vinculado declarando un recuento de columnas.
            dgvAsistencias.ColumnCount = 7;
            dgvAsistencias.ColumnHeadersVisible = true;

            // Configuramos la AutoGenerateColumns en false para que no se autogeneren las columnas
            dgvAsistencias.AutoGenerateColumns = false;

            // Cambia el estilo de la cabecera de la grilla.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle
            {
                BackColor = Color.Beige,
                Font = new Font("Verdana", 8, FontStyle.Bold)
            };
            dgvAsistencias.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            // Definimos el nombre de la columnas y el DataPropertyName que se asocia a DataSource

            CrearColumnas(dgvAsistencias, 0, "Nombre", "Usuario", 150);
            CrearColumnas(dgvAsistencias, 1, "Fecha", "Fecha", 70);
            CrearColumnas(dgvAsistencias, 2, "Hora Ingreso", "HoraIngreso", 100);
            CrearColumnas(dgvAsistencias, 3, "Hora Salida", "HoraSalida", 100);
            CrearColumnas(dgvAsistencias, 4, "Estado", "EstadoAsistencia", 110);
            CrearColumnas(dgvAsistencias, 5, "Comentario", "Comentario", 320);
            CrearColumnas(dgvAsistencias, 6, "Borrado", "Borrado", 80);

        }

        private void FrmAsignaciones_Load(object sender, EventArgs e)
        {
            LlenarCombo(cboUsuario, usuarioService.ObtenerUsuarios(), "Nombre", "Nombre");
            LlenarCombo(cboEstado, estadoAsistenciaService.ObtenerEstadosAsistencia(), "Nombre", "IdEstadoAsistencia");

            IList<AsistenciaUsuario> listadoAsistencias = asistenciaService.ObtenerAsistenciasUsuario();
            dgvAsistencias.DataSource = listadoAsistencias;
            lblTotal.Text = "Registros encontrados: " + listadoAsistencias.Count;
        }

        private void CrearColumnas(DataGridView tabla, int columna, string nombre, string propiedad, int tamaño)
        {
            tabla.Columns[columna].Name = nombre;
            tabla.Columns[columna].DataPropertyName = propiedad;
            tabla.Columns[columna].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            tabla.Columns[columna].Width = tamaño;
        }

        private void LlenarCombo(ComboBox cbx, Object source, string display, String value)
        {
            // Datasource: establece el origen de datos de este objeto.
            cbx.DataSource = source;
            // DisplayMember: establece la propiedad que se va a mostrar para este ListControl.
            cbx.DisplayMember = display;
            // ValueMember: establece la ruta de acceso de la propiedad que se utilizará como valor real para los elementos de ListControl.
            cbx.ValueMember = value;
            //SelectedIndex: establece el índice que especifica el elemento seleccionado actualmente.
            cbx.SelectedIndex = -1;
        }

        // Botones:

        private void BtnConsultar_Click(object sender, EventArgs e)
        {
            // Sección reviso de filtros:

            parametros.Clear();

            if (DateTime.TryParse(dateFechaDesde.Text, out DateTime fechaDesde) && dateFechaDesde.Checked)
            {
                parametros.Add("fechaDesde", fechaDesde);
            }

            if (DateTime.TryParse(dateFechaHasta.Text, out DateTime fechaHasta) && dateFechaHasta.Checked)
            {
                parametros.Add("fechaHasta", fechaHasta);
            }

            string usuario;
            if (cboUsuario.SelectedValue == null && !string.IsNullOrEmpty(cboUsuario.Text))
            {
                usuario = cboUsuario.Text;
                parametros.Add("usuario", usuario);
            }
            else if (cboUsuario.SelectedValue != null)
            {
                usuario = cboUsuario.SelectedValue.ToString();
                parametros.Add("usuarioExacto", usuario);
            }

            if (cboEstado.SelectedValue != null)
            {
                var idEstadoAsistencia = cboEstado.SelectedValue.ToString();
                parametros.Add("idEstadoAsistencia", idEstadoAsistencia);
            }

            if (ChkBaja.Checked)
            {
                parametros.Add("borrado", true);
            }

            // Solicitamos la lista de bugs que cumplan con los filtros:
            IList<AsistenciaUsuario> listadoAsistencias = asistenciaService.ObtenerAsistenciasUsuario(parametros);
            dgvAsistencias.DataSource = listadoAsistencias;
            lblTotal.Text = "Registros encontrados: " + listadoAsistencias.Count;

            if (listadoAsistencias.Count == 0)
            {
                MessageBox.Show("No se encontraron coincidencias para el/los filtros ingresados", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            cboEstado.SelectedIndex = -1;
            cboUsuario.SelectedIndex = -1;
            
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            FrmAsistenciasABM frmAgregar = new FrmAsistenciasABM(FrmAsistenciasABM.FormMode.nuevo);
            frmAgregar.ShowDialog();

            IList<AsistenciaUsuario> listadoAsistencias = asistenciaService.ObtenerAsistenciasUsuario(parametros);
            dgvAsistencias.DataSource = listadoAsistencias;
            lblTotal.Text = "Registros encontrados: " + listadoAsistencias.Count;
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvAsistencias.RowCount.Equals(0))
            {
                MessageBox.Show("Debe seleccionar un registro antes de eliminarlo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AsistenciaUsuario asistenciaUsuario = (AsistenciaUsuario)dgvAsistencias.CurrentRow.DataBoundItem;

            if (asistenciaUsuario.Borrado == true)
            {
                MessageBox.Show("¡No puede eliminar un registro ya borrado!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                
            DialogResult rta = MessageBox.Show("¿Seguro que desea borrar el registro seleccionado?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (rta == DialogResult.Yes)
            {
                asistenciaUsuario.Borrado = true;
                if (!asistenciaService.ActualizarAsistenciaUsuario(asistenciaUsuario))
                    MessageBox.Show("El registro no se pudo borrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                IList<AsistenciaUsuario> listadoAsistencias = asistenciaService.ObtenerAsistenciasUsuario(parametros);
                dgvAsistencias.DataSource = listadoAsistencias;
                lblTotal.Text = "Registros encontrados: " + listadoAsistencias.Count;
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            
            if (dgvAsistencias.RowCount.Equals(0))
            {
                MessageBox.Show("Debe seleccionar un registro antes de editarlo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
                
            AsistenciaUsuario asistenciaUsuario = (AsistenciaUsuario)dgvAsistencias.CurrentRow.DataBoundItem;
            FrmAsistenciasABM frmEditar = new FrmAsistenciasABM(FrmAsistenciasABM.FormMode.actualizar, asistenciaUsuario);
            frmEditar.ShowDialog();

            IList<AsistenciaUsuario> listadoAsistencias = asistenciaService.ObtenerAsistenciasUsuario(parametros);
            dgvAsistencias.DataSource = listadoAsistencias;
            lblTotal.Text = "Registros encontrados: " + listadoAsistencias.Count;
        }
    }
}
