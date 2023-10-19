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
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;  //referencias necesarias para trabajar con localhost y xampp

namespace Practica_5
{
    public partial class Form1 : Form
    {
        //datos de conexion a mysql (xampp)
        string conexionSQL="Server=localhost;Port=3306;Database=programacion;Uid=root;Pwd=;";
        //metodo para insertar registros

        public Form1()
        {
            InitializeComponent();

            //agregar controladores de eventos TextChanged a los campos
            tbEdad.TextChanged += ValidarEdad;
            tbEstatura.TextChanged += ValidarEstatura;
            tbTelefono.TextChanged += ValidarTelefono;
            tbNombre.TextChanged += ValidarNombre;
            tbApellido.TextChanged += ValidarApellidos;
        }
        private void InsertarRegistro(string nombre, string apellido, int edad, decimal estatura, string telefono, string genero) 
        {
            using(MySqlConnection conection = new MySqlConnection(conexionSQL))
            {
                conection.Open();
                string insertQuery = "INSERT INTO registros (nombre, apellido, edad, estatura, telefono, genero)" + "VALUES (@nombre, @apellidos, @edad, @estatura, @telefono, @genero)";

                using (MySqlCommand command=new MySqlCommand (insertQuery, conection))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@apellidos", apellido);
                    command.Parameters.AddWithValue("@edad", edad);
                    command.Parameters.AddWithValue("@estatura", estatura);
                    command.Parameters.AddWithValue("@telefono", telefono);
                    command.Parameters.AddWithValue("@genero", genero);

                    command.ExecuteNonQuery();
                }
                conection.Close();
            }
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

            //validar que los campos tengan el formato correcto
            if(EsEnteroValido(edad)&& EsDecimalValido(estatura)&& EsEnteroValidoDe10Digitos(telefono)&& EsTextoValido(nombre)&& EsTextoValido(apellido))
            {
                //crear una cadena con los datos
                string datos = $"Nombres: {nombre}\r\nApellidos: {apellido}\r\nTelefono: {telefono}\r\nEstatura: {estatura} cm\r\nEdad: {edad} años\r\nGenero: {genero}\r\n";
                //guardar los datos en un archivo de texto
                string rutaArchivo = "C:/Users/Vlees/Documents/datos.txt";
                bool archivoExiste = File.Exists(rutaArchivo);
                //File.WriteAllText(rutaArchivo, datos);
                if (archivoExiste == false)
                {
                    File.WriteAllText(rutaArchivo, datos);
                }
                else
                {
                    //verificar si el archivo ya existe
                    using (StreamWriter writer = new StreamWriter(rutaArchivo, true))
                    {
                        if (archivoExiste)
                        {
                            //si el archivo existe, añadir un separador antes del nuevo registro
                            writer.WriteLine();
                            //programacion de funcionalidad de insert SQL
                            InsertarRegistro(nombre, apellido, int.Parse(edad), decimal.Parse(estatura), telefono, genero);
                            MessageBox.Show("Datos ingresados correctamente.");
                        }
                        else
                        {
                            writer.WriteLine(datos);
                            //programacion de funcionalidad de insert SQL
                            InsertarRegistro(nombre, apellido, int.Parse(edad), decimal.Parse(estatura), telefono, genero);
                            MessageBox.Show("Datos ingresados correctamente.");
                        }
                    }
                }
                //mostrar un mensaje con los datos capturados
                MessageBox.Show("Datos guardados con exito:\n\n" + datos, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Por favor, ingrese datos validos en los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool EsEnteroValido(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }
        private bool EsDecimalValido(string valor)
        {
            decimal resultado;
            return decimal.TryParse(valor, out resultado);
        }
        private bool EsEnteroValidoDe10Digitos(string input)
        {
            if (input.Length != 10)
            {
                return false;
            }
            if(!input.All(char.IsDigit))
            {
                return false;
            }
            return true;
        }
        private bool EsTextoValido(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-Z\s]+$"); //solo letras y espacios
        }
        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if(!EsEnteroValido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese una edad valida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }
        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!EsDecimalValido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese una estatura valida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }
        private void ValidarTelefono(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string input = textBox.Text;
            //eliminar espacios en blanco y guiones si es necesario
            //input=input.Replace(" ","").Replace("-","");
            if (input.Length < 10)
            {
                return;
            }
            if (!EsEnteroValidoDe10Digitos(input))
            {
                MessageBox.Show("Por favor, ingrese un numero de telefono valido de 10 digitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }

        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!EsTextoValido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese un nombre valido (solo letras y espacios).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }
        private void ValidarApellidos(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!EsTextoValido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese apellidos validos (solo letras y espacios).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
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
