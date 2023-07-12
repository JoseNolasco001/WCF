using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace Crud
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool doubleClic;
        private List<Person> peoples = new List<Person>();
        CustomerService.Service1Client service;

        public MainWindow()
        {
            InitializeComponent();
            service = new CustomerService.Service1Client();
            this.GetAll();
            doubleClic = false;
        }

        private void Button_Click_Guardar(object sender, RoutedEventArgs e)
        {
            bool ban = false;
            int i = 0;
            try
            {   
                if(!nombre_txt.Text.Equals("") && !edad_txt.Text.Equals("") && !email_txt.Text.Equals(""))
                {
                    CustomerService.Customer customer = new CustomerService.Customer();
                    customer.Nombre = nombre_txt.Text;
                    customer.Edad = Int16.Parse(edad_txt.Text);
                    customer.Email = email_txt.Text;

                    if (doubleClic)
                    {   
                        customer.Id = Int16.Parse(id_txt.Text);
                        service.UpdateCustomer(customer);
                    }
                    else
                    {
                        service.InsertCustomer(customer);
                    }
                    
                    this.GetAll();
                    doubleClic = false;
                    Limpiar();
                }
            }
            catch (Exception ex) { }
        }
        private void Button_Click_Nuevo(object sender, RoutedEventArgs e)
        {
            Limpiar();
            doubleClic = false;
        }
        private void Button_Click_Eliminar(object sender, RoutedEventArgs e)
        {
            int i;
            try
            {
                var result = MessageBox.Show("Seguro que deseas eliminar este registro!", "Message", MessageBoxButton.YesNo);
                
                if (result == MessageBoxResult.Yes)
                {
                    var row = (dataGrid.SelectedItem as CustomerService.Customer).Id;
                    service.DeleteCustomer(row);
                    this.GetAll();
                }
            }
            catch (Exception ex) { }
        }
        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            doubleClic = true;
            CustomerService.Customer person = (dataGrid.SelectedItem as CustomerService.Customer);
            Console.WriteLine("Id: " + person.Id);
            Cargar(person);
        }
        public void Limpiar()
        {
            id_txt.Clear();
            nombre_txt.Clear();
            edad_txt.Clear();
            email_txt.Clear();
        }
        private void Cargar(CustomerService.Customer person)
        {
            id_txt.Text = person.Id.ToString();
            edad_txt.Text = person.Edad.ToString();
            nombre_txt.Text = person.Nombre.ToString();
            email_txt.Text = person.Email.ToString();
        }

        private void GetAll()
        {
            try
            {
                CustomerService.Customer[] customers;
                customers = service.GetAllCustomer();

                dataGrid.ItemsSource = customers;
            }
            catch (Exception ex) { }
        }
    }
}
