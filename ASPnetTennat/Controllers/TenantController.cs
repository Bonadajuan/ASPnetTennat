using ASPnetTennat.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPnetTennat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly TenantRepository _tenantRepository;

        public TenantController(CustomerRepository customerRepository, TenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return await _tenantRepository.GetAllTenantsAndCustomers();
        }
    }

}
