using CSharpFunctionalExtensions;
using FluentAssertions;
using NSubstitute;
using RopDemo.ApplicationServices;
using RopDemo.Domain;
using RopDemo.DTOs;
using Xunit;

namespace RopDemo.Tests
{
    public class RegistrationServiceRopTests
    {
        private static ICustomerRepository _customerRepository;
        private static IMailer _mailer;

        private static RegistrationServiceRop DefaultSetup(Result<Customer> customer)
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _customerRepository.Save(Arg.Any<Customer>()).Returns(customer);

            _mailer = Substitute.For<IMailer>();
            _mailer.SendWelcome(Arg.Any<Customer>()).Returns(customer);

            var sut = new RegistrationServiceRop(_customerRepository, _mailer);
            return sut;
        }

        [Fact]
        public void Registration_customer_db_failure_returns_error_message()
        {
            // Arrange
            var customer = Result.Ok(new Customer {Id = 42});
            var sut = DefaultSetup(customer);

            _customerRepository.Save(Arg.Any<Customer>())
                .Returns(Result.Fail<Customer, RopError>(
                    new RopError {Message = "db error"}));

            var validVm = new RegisterCustomerVM {Name = "valid"};

            // Act
            var result = sut.RegisterCustomer(validVm);

            // Assert
            result.ErrorMessage.Should().Be("db error");
        }

        [Fact]
        public void Registration_customer_mail_failure_returns_error_message()
        {
            // Arrange
            var customer = Result.Ok(new Customer {Id = 42});
            var sut = DefaultSetup(customer);

            _mailer.SendWelcome(Arg.Any<Customer>())
                .Returns(Result.Fail<Customer, RopError>(
                    new RopError {Message = "mail error"}));

            var validVm = new RegisterCustomerVM {Name = "valid"};

            // Act
            var result = sut.RegisterCustomer(validVm);

            // Assert
            result.ErrorMessage.Should().Be("mail error");
        }

        [Fact]
        public void Registration_invalid_customer_returns_error_message()
        {
            // Arrange
            var customer = Result.Ok(new Customer {Id = 42});

            var sut = DefaultSetup(customer);

            var invalidVm = new RegisterCustomerVM {Name = ""};

            // Act
            var result = sut.RegisterCustomer(invalidVm);

            // Assert
            result.ErrorMessage.Should().Be("Name is empty!");
        }

        [Fact]
        public void Registration_valid_customer_returns_correct_id()
        {
            // Arrange
            var customer = Result.Ok(new Customer {Id = 42});

            var sut = DefaultSetup(customer);

            var validVm = new RegisterCustomerVM {Name = "valid"};

            // Act
            var result = sut.RegisterCustomer(validVm);

            // Assert
            result.CustomerId.Should().Be(42);
        }
    }
}