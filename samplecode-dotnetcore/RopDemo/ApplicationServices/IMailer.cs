using CSharpFunctionalExtensions;
using RopDemo.Domain;

namespace RopDemo.ApplicationServices
{
    public interface IMailer
    {
        Customer SendWelcomeClassic(Customer customer);
        Result<Customer> SendWelcome(Customer customer);
    }
}