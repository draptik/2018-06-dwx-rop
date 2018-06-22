using FluentAssertions;
using Xunit;

namespace RopDemo.ResultSimple.Tests
{
    public class ResultSimpleTests
    {
        [Fact]
        public void Creating_a_result_success_has_no_error_message()
        {
            var result = ResultSimple.Ok(new MyClass1());
            result.Error.Should().BeNullOrEmpty();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Creating_a_result_failure_has_an_error_message()
        {
            var result = ResultSimple.Fail<MyClass1>("ups");
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("ups");
        }
    }

    class MyClass1
    {
    }
}