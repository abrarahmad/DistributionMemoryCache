using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleDemoDMC.Services;

namespace SampleDemoDMC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    [ApiExplorerSettings(GroupName = "DistributionCacheAPI")]
    public class DistributionCacheController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public DistributionCacheController(ICustomerService customerService) => _customerService = customerService;

        [HttpGet("get-customer", Name = "get-customer")]
        public async Task<IActionResult> GetCustomer(int customerId)
        {
            var result = await _customerService.GetCustomer(customerId).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("get-customers", Name = "get-customers")]
        public async Task<IActionResult> GetCustomers()
        {
            var result = await _customerService.GetCustomers().ConfigureAwait(false);
            return Ok(result);
        }
    }
}
