using CSharpFunctionalExtensions;
using RopDemo.Domain;

namespace RopDemo.ApplicationServices
{
    public interface ICustomerRepository
    {
        Customer SaveClassic(Customer customer);
        Result<Customer> Save(Customer customer);
    }
}