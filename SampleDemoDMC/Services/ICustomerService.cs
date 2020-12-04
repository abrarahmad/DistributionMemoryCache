using System.Collections.Generic;
using System.Threading.Tasks;
using SampleDemoDMC.Model;

namespace SampleDemoDMC.Services
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetCustomer(int customerId);
        Task<IReadOnlyList<CustomerDto>> GetCustomers();
    }
}
