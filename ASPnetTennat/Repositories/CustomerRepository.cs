using ASPnetTennat.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASPnetTennat.Repositories
{
    // se utiliza para trabajar con la Customer tabla de la base de datos de Demo. En aras de la simplicidad, usaré ADO.NET aquí, sin mapeadores relacionale
    public class CustomerRepository
    {
        private readonly IConfiguration _configuration;

        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Customer>> GetAllCustomers(string tenantId)
        {
            try
            {
                List<Customer> customers = new List<Customer>();

                using (var connection = new SqlConnection(_configuration["ConnectionStrings:TenantDbConnection"]))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("SELECT * FROM dbo.Customer Where TenantId = @TenantId", connection))
                    {
                        SqlParameter param = new SqlParameter();
                        param.ParameterName = "@TenantId";
                        param.Value = tenantId;
                        command.Parameters.Add(param);

                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Customer customer = new Customer();
                            customer.Id = Guid.Parse(reader["Id"].ToString());
                            customer.TenantId = Guid.Parse(reader["TenantId"].ToString());
                            customer.CustomerName = reader["CustomerName"].ToString();
                            customer.IsActive = bool.Parse(reader["IsActive"].ToString());
                            customers.Add(customer);
                        }

                        if (!reader.IsClosed) await reader.CloseAsync();
                    }

                    if (connection.State != ConnectionState.Closed) await connection.CloseAsync();
                    return customers;
                }
            }
            catch
            {
                return null;
            }
        }
    }

}
