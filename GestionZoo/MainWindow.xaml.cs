using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Data;
using System.Windows.Media.Animation;
using GestionZoo.MiPrimeraBaseDeDatosDataSetTableAdapters;
using Microsoft.Win32;

namespace GestionZoo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            String connectionString = ConfigurationManager.
 ConnectionStrings["GestionZoo.Properties.Settings."
 + "MiPrimeraBaseDeDatosConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);
            MuestraZoos();
            MuestraAnimales();
        }

        private void MuestraZoos()
        {
            try
            {
                //Creamos la consulta como String
                string consulta = "select * from Zoo";
                //Creamos el adaptador para que se pueda ejecutar la consulta desde aquí
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, sqlConnection);
                //Inicializamos el adaptador
                using (sqlDataAdapter)
                {
                    //Guardamos el resultado de la consulta en un objeto
                    DataTable zooTable = new DataTable();
                    //Ejecutamos el adaptador
                    sqlDataAdapter.Fill(zooTable);
                    //Expresamos cómo queremos que se muestre en el ListBox
                    ListaZoos.DisplayMemberPath = "Ubicacion";
                    //Mostrará el campo Ubicación
                    ListaZoos.SelectedValuePath = "Id";
                    //Decimos que use el campo ID para identificar cada registro
                    ListaZoos.ItemsSource = zooTable.DefaultView;
                    //Ponemos como fuente de datos una vista de la tabla con valores por defecto
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        private void MuestraAnimalesAsociados()
        {
            try
            {
                //Creamos la consulta como String
                string consulta = "select * from Animales a Inner Join AnimalZoo az on a.Id=az.AnimalId where az.ZooId=@ZooId";
                SqlCommand sqlCommand = new
                SqlCommand(consulta, sqlConnection);
                //Creamos el adaptador para que se pueda ejecutar la consulta desde aquí
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                //Inicializamos el adaptador
                using (sqlDataAdapter)
                {
                    //Añadimos valores a sqlCommand; aquí especificamos @ZooId
                    sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                    //Guardamos el resultado de la consulta en un objeto DataTable
                     DataTable AnimalTabla = new DataTable();
                    //Ejecutamos el adaptador
                    sqlDataAdapter.Fill(AnimalTabla);
                    //Expresamos cómo queremos que se muestre en el ListBox
                    ListaAnimalesAsociados.DisplayMemberPath = "Nombre";//Mostrará el campo Ubicación
                    ListaAnimalesAsociados.SelectedValuePath = "Id";//Decimos que use el campo ID para identificar cada registro
                    ListaAnimalesAsociados.ItemsSource = AnimalTabla.DefaultView;//Ponemos como fuente de datos una vista de la tabla con valores por defecto
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void MuestraAnimales()
        {
            try
            {
                string consulta = "Select * from Animales";
                SqlDataAdapter sqlDataAdapter = new
               SqlDataAdapter(consulta, sqlConnection);
                //Especificamos como vamos a usar el SqlAdapter
                using (sqlDataAdapter)
                {
                    DataTable animalTabla = new DataTable();
                    sqlDataAdapter.Fill(animalTabla);
                    //Definimos como fuente del tercer listbox el resultado de la tabla
                    ListaAnimales.DisplayMemberPath = "Nombre";//Campos que mostraremos
                    ListaAnimales.SelectedValuePath = "Id";//Valor para la búsqueda
                    ListaAnimales.ItemsSource = animalTabla.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void ListaZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraAnimalesAsociados();
        }

        private void EliminarZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Creamos una consulta de eliminaciónç
                string consulta = "Delete from Zoo where id=@ZooId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                //Creamos la conexión, esta vez sin el adaptador
                sqlConnection.Open();//Abrimos la conexión a la BD
                sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);//Se le asigna a la variable el valor del id del Zoo seleccionado
                sqlCommand.ExecuteScalar();//Ejecuta la consulta y devuelve el primer valor de la 1ªcolumna del 1ºregistro
                MuestraZoos();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();//Cerramos la conexión
            }

        }

        private void AgregarAnimalZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Creamos una consulta de eliminaciónç
                string consulta = "Insert into AnimalZoo values (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(consulta,sqlConnection);
                //Creamos la conexión, esta vez sin el adaptador
                sqlConnection.Open();//Abrimos la conexión a la BD

                sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);

                sqlCommand.Parameters.AddWithValue("@AnimalId", ListaAnimales.SelectedValue);//Se le asigna a la variable el valor del id del Zoo seleccionado
                sqlCommand.ExecuteScalar();//Ejecuta la consulta y devuelve el primer valor de la 1ªcolumna del 1ºregistro
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();//Cerramos la conexión
                MuestraAnimalesAsociados();
            }
        }

        private void AgregarZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Creamos una consulta de eliminaciónç
                string consulta = "Insert into Zoo values (@Ubicación)";
                SqlCommand sqlCommand = new SqlCommand(consulta,
               sqlConnection);
                //Creamos la conexión, esta vez sin el adaptador
                sqlConnection.Open();//Abrimos la conexión a la BD
                sqlCommand.Parameters.AddWithValue("@Ubicación", miTextBox.Text);//Se le asigna a la variable el valor del id del Zoo seleccionado
                sqlCommand.ExecuteScalar();//Ejecuta la consulta y devuelve el primer valor de la 1ªcolumna del 1ºregistro
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();//Cerramos la conexión
                MuestraZoos();
            }
        }


        private void EliminarAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Creamos una consulta de eliminaciónç
                string consulta = "Delete from Animal where id=@AnimalId";
                SqlCommand sqlCommand = new SqlCommand(consulta,
               sqlConnection);
                //Creamos la conexión, esta vez sin el adaptador
                sqlConnection.Open();//Abrimos la conexión a la BD
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListaAnimales.SelectedValue);//Se le asigna a la variable el valor del id del Zoo seleccionado
                sqlCommand.ExecuteScalar();//Ejecuta la consulta y devuelve el primer valor de la 1ªcolumna del 1ºregistro
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();//Cerramos la conexión
                MuestraAnimales();
            }
        }


    }
}
