namespace BFFTDD
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public bool Update(Customer customer)
        {
            if (customer.Age < 18)
                throw new CustomerProcessException("Customer is under 18.");

            var lookup = _repo.GetCustomer(customer.Id);

            if(lookup.Count > 1)
                throw new CustomerProcessException("Duplicate record found.");

            var result = _repo.UpdateCustomer(customer);

            return result;
        }
    }
}