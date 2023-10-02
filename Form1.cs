using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Practica_5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //obtener los datos de los textbox
            string nombre = tbNombre.Text;
            string apellido = tbApellido.Text;
            string edad = tbEdad.Text;
            string estatura = tbEstatura.Text;
            string telefono = tbTelefono.Text;

            //obtener el genero seleccionado
            string genero = "";
            if (rbHombre.Checked)
            {
                genero = "Hombre";
            }
            else if (rbMujer.Checked)
            {
                genero = "Mujer";
            }
            //crear una cadena con los datos
            string datos = $"Nombres: {nombre}\r\nApellidos: {apellido}\r\nTelefono: {telefono}\r\nEstatura: {estatura} cm\r\nEdad: {edad} años\r\nGenero: {genero}";
            //guardar los datos en un archivo de texto
            string rutaArchivo = "C:/Users/Vlees/Documents/datos.txt";
            bool archivoExiste = File.Exists(rutaArchivo);
            //File.WriteAllText(rutaArchivo, datos);
            //verificar si el archivo ya existe

            using (StreamWriter writer= new StreamWriter(rutaArchivo, true))
            {
                if (archivoExiste)
                {
                    //si el archivo existe, añadir un separador antes del nuevo registro
                    writer.WriteLine();
                }
                writer.WriteLine(datos);
                //mostrar un mensaje con los datos capturados
                MessageBox.Show("Datos guardados con exito:\n\n" + datos, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            tbNombre.Clear();
            tbApellido.Clear();
            tbEstatura.Clear();
            tbTelefono.Clear();
            tbEdad.Clear();
            rbHombre.Checked = false;
            rbMujer.Checked = false;
        }
    }
}
