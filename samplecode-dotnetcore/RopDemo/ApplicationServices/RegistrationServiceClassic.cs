using RopDemo.Domain;
using RopDemo.DTOs;

namespace RopDemo.ApplicationServices
{
    public class RegistrationServiceClassic
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMailer _mailConfirmer;

        public RegistrationServiceClassic(ICustomerRepository customerRepository, IMailer mailer)
        {
            _customerRepository = customerRepository;
            _mailConfirmer = mailer;
        }

        public CustomerRegisteredVM RegisterCustomer(RegisterCustomerVM vm)
        {
            return RegisterCustomerOldSchoolHappyCase(vm);
            // return RegisterCustomerOldSchoolWithErrorHandling(vm);
        }

        public CustomerRegisteredVM RegisterCustomerOldSchoolHappyCase(RegisterCustomerVM vm)
        {
            var customer = Map(vm);
            var isValid = IsValid(customer);
            if (isValid)
            {
                customer = _customerRepository.SaveClassic(customer);
                _mailConfirmer.SendWelcome(customer);
                return new CustomerRegisteredVM{CustomerId = customer.Id};
            }
            return new CustomerRegisteredVM{ErrorMessage = "ups"};
        }

        public CustomerRegisteredVM RegisterCustomerOldSchoolWithErrorHandling(RegisterCustomerVM vm)
        {
            var customer = Map(vm);
            var isValid = IsValid(customer);
            if (isValid)
            {
                try
                {
                    customer = _customerRepository.SaveClassic(customer);
                }
                catch (System.Exception e)
                {
                    return new CustomerRegisteredVM {ErrorMessage = e.Message};
                }
                try
                {
                    _mailConfirmer.SendWelcomeClassic(customer);
                }
                catch (System.Exception e)
                {
                    return new CustomerRegisteredVM {ErrorMessage = e.Message};
                }
                
                // Happy path
                return new CustomerRegisteredVM{CustomerId = customer.Id};
            }

            return new CustomerRegisteredVM{ErrorMessage = "Name is empty!"};
        }

        private bool IsValid(Customer customer)
        {
            return !string.IsNullOrWhiteSpace(customer.Name);
        }

        private Customer Map(RegisterCustomerVM vm)
        {
            return new Customer {Name = vm.Name};
        }
    }
}