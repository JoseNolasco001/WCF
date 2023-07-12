using Dapper;
using NuestroWCF.BLL;
using NuestroWCF.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NuestroWCF.DAL
{
    public class CustomerDO
    {
        public CustomerDTO Bind(Customer customer)
        {
            CustomerDTO customerResult = new CustomerDTO();
            customerResult.Id = customer.Id;
            customerResult.Nombre = customer.Nombre;
            customerResult.Edad = customer.Edad;
            customerResult.Email = customer.Email;
            return customerResult;
        }
        public List<Customer> BindList(List<CustomerDTO> customerList)
        {
            List<Customer> result = new List<Customer>();
            customerList.ForEach(res =>
            {
                Customer customer = new Customer();
                customer.Id = res.Id;
                customer.Nombre = res.Nombre;
                customer.Edad = res.Edad;
                customer.Email = res.Email;
                result.Add(customer);
            });
            
            return result;
        }

        public async Task<bool> Save(Customer customer)
        {
            bool result = false;
            try
            {
                CustomerDTO customerSave = Bind(customer);
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerSaleDB"].ConnectionString))
                {
                    connection.Open();
                    await connection.ExecuteAsync("PuntoDeVenta.spCustomerSave", customerSave, commandType: CommandType.StoredProcedure);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public async Task<bool> Update(Customer customer)
        {
            bool result = false;
            try
            {
                CustomerDTO customerSave = Bind(customer);
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerSaleDB"].ConnectionString))
                {
                    connection.Open();
                    await connection.ExecuteAsync("PuntoDeVenta.spCustomerEdit", customerSave, commandType: CommandType.StoredProcedure);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public async Task<bool> Delete(int Id)
        {
            bool result = false;
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerSaleDB"].ConnectionString))
                {
                    connection.Open();
                    await connection.ExecuteAsync("PuntoDeVenta.spCustomerDelete", new { Id = Id }, commandType: CommandType.StoredProcedure);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public async Task<List<Customer>> GetAll()
        {
            List<Customer> list = new List<Customer>();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerSaleDB"].ConnectionString))
                {
                    connection.Open();
                    var res = await connection.QueryAsync<CustomerDTO>("PuntoDeVenta.spCustomerGetList", commandType: CommandType.StoredProcedure);
                    list = BindList(res.ToList());
                }
            }catch (Exception ex){ }

            return list;
        }
    }
}