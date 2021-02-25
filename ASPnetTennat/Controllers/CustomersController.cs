using ASPnetTennat.Models;
using ASPnetTennat.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPnetTennat.Controllers
{
    // CustomersController clase usa inyección de dependencia para crear instancias de las clases CustomerRepository y TenantRepository. 
    // Luego, estas instancias se utilizan en el método GetAll para recuperar todos los registros de Customers
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;
        private readonly TenantRepository _tenantRepository;
        public CustomersController(CustomerRepository customerRepository,

        TenantRepository tenantRepository)
        {
            _customerRepository = customerRepository;
            _tenantRepository = tenantRepository;

        }

        [HttpGet]
        public async Task<List<Customer>> GetAll()
        {
            string tenantId = await _tenantRepository.GetTenantId();
            return await _customerRepository.GetAllCustomers(tenantId);
        }
    }

}
