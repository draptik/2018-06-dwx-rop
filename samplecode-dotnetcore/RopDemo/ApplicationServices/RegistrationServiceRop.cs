using CSharpFunctionalExtensions;
using RopDemo.Domain;
using RopDemo.DTOs;

namespace RopDemo.ApplicationServices
{
    public class RegistrationServiceRop
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMailer _mailConfirmer;

        public RegistrationServiceRop(ICustomerRepository customerRepository, IMailer mailer)
        {
            _customerRepository = customerRepository;
            _mailConfirmer = mailer;
        }

        public CustomerRegisteredVM RegisterCustomer(RegisterCustomerVM vm)
        {
            return RegisterCustomerRop(vm);    
        }

        public CustomerRegisteredVM RegisterCustomerRop(RegisterCustomerVM vm)
        {
            var customer = Map(vm);
            var result = IsValid(customer)
                .OnSuccess(c => _customerRepository.Save(c))
                .OnSuccess(c => _mailConfirmer.SendWelcome(c))
                .OnBoth(cEnd => cEnd.IsSuccess
                    ? new CustomerRegisteredVM {CustomerId = cEnd.Value.Id}
                    : new CustomerRegisteredVM {ErrorMessage = cEnd.Error});
            return result;
        }

        private Result<Customer> IsValid(Customer customer)
        {
            return string.IsNullOrWhiteSpace(customer.Name)
                ? Result.Fail<Customer, RopError>(
                    new RopError {Message = "Name is empty!"})
                : Result.Ok(customer);
        }

        private Customer Map(RegisterCustomerVM vm)
        {
            return new Customer {Name = vm.Name};
        }
    }
}