using System;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RopDemo.ApplicationServices;
using RopDemo.Domain;
using RopDemo.DTOs;
using Xunit;

namespace RopDemo.Tests
{
    public class RegistrationServiceClassicTests
    {
        private static ICustomerRepository _customerRepository;
        private static IMailer _mailer;

        private static RegistrationServiceClassic DefaultSetup(Customer customer)
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _customerRepository.SaveClassic(Arg.Any<Customer>()).Returns(customer);

            _mailer = Substitute.For<IMailer>();
            _mailer.SendWelcomeClassic(Arg.Any<Customer>()).Returns(customer);

            var sut = new RegistrationServiceClassic(_customerRepository, _mailer);
            return sut;
        }

        [Fact]
        public void Registration_customer_db_failure_returns_error_message()
        {
            // Arrange
            var customer = new Customer {Id = 42};
            var sut = DefaultSetup(customer);

            _customerRepository.SaveClassic(Arg.Any<Customer>())
                .Throws(new Exception("db error"));

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
            var customer = new Customer {Id = 42};
            var sut = DefaultSetup(customer);

            _mailer.SendWelcomeClassic(Arg.Any<Customer>())
                .Throws(new Exception("mail error"));

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
            var customer = new Customer {Id = 42};

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
            var customer = new Customer {Id = 42};

            var sut = DefaultSetup(customer);

            var validVm = new RegisterCustomerVM {Name = "valid"};

            // Act
            var result = sut.RegisterCustomer(validVm);

            // Assert
            result.CustomerId.Should().Be(42);
        }
    }
}