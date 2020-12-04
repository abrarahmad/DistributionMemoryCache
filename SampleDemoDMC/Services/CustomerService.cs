using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleDemoDMC.Model;

namespace SampleDemoDMC.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDopDistributionCache _dopDistributionCache;
        public CustomerService(IDopDistributionCache dopDistributionCache)
        {
            _dopDistributionCache = dopDistributionCache;
        }
        public async Task<CustomerDto> GetCustomer(int customerId)
        {
            return await _dopDistributionCache.GetOrCreate(customerId.ToString(), () => GetCustomerById(customerId)).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<CustomerDto>> GetCustomers()
        {
            return await _dopDistributionCache.GetOrCreate("AllCustomer", () => GetAllCustomer()).ConfigureAwait(false);
        }

        public async Task<CustomerDto> GetCustomerById(int id)
        {
            var customers = await GetAllCustomer().ConfigureAwait(false);
            return customers.FirstOrDefault(f => f.Id == id);
        }
        private async Task<IReadOnlyList<CustomerDto>> GetAllCustomer()
        {
            var customers = new List<CustomerDto>
            {
            new CustomerDto
            {
                Id=1,
                Name ="Abrar",
                Description ="Respository- data"
            },
           new CustomerDto
            {
                Id=2,
                Name ="Alisha",
                Description ="Respository- data"
            }
            };
            return await Task.FromResult(customers).ConfigureAwait(false);
        }

    }
}
