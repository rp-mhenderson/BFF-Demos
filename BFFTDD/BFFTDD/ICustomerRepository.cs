using System.Collections;
using System.Collections.Generic;

namespace BFFTDD
{
    public interface ICustomerRepository
    {
        bool UpdateCustomer(Customer isAny);
        ICollection<Customer> GetCustomer(int id);
    }
}