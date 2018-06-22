using FluentAssertions;
using Xunit;

namespace RopDemo.ResultSimple.Tests
{
    public class ResultSimpleExtensionsTests
    {
        [Fact]
        public void OnSuccess_with_success_runs_function()
        {
            var myresult = ResultSimple.Ok(new MyClass {Property = "original"});
            var result = myresult
                .OnSuccess(x => ResultSimple.Ok(new MyClass {Property = "changed"}));
            result.Value.Property.Should().Be("changed");
        }

        [Fact]
        public void OnSuccess_with_failure_returns_error()
        {
            var myresult = ResultSimple.Fail<MyClass>("ups");
            var result = myresult
                .OnSuccess(x => ResultSimple.Ok(new MyClass {Property = "changed"}));
            result.Error.Should().Be("ups");
        }

        [Fact]
        public void OnFailure_with_failure_runs_action()
        {
            var myBool = false;
            var myresult = ResultSimple.Fail<MyClass>("ups");
            myresult.OnFailure(error => myBool = true);
            myBool.Should().BeTrue();
        }

        [Fact]
        public void OnFailure_with_success_does_not_run_action()
        {
            var myBool = false;
            var myresult = ResultSimple.Ok(new MyClass());
            myresult.OnFailure(error => myBool = true);
            myBool.Should().BeFalse();
        }

        [Fact]
        public void OnBoth_with_success_runs_function()
        {
            var myresult = ResultSimple.Ok(new MyClass {Property = "original"});
            var result = myresult
                .OnBoth(x => x.IsFailure
                    ? new Dummy {Name = x.Error}
                    : new Dummy {Name = x.Value.Property});
            result.Name.Should().Be("original");
        }

        [Fact]
        public void OnBoth_with_failure_runs_function()
        {
            var myresult = ResultSimple.Fail<MyClass>("ups");
            var result = myresult
                .OnBoth(x => x.IsFailure
                    ? new Dummy {Name = x.Error}
                    : new Dummy {Name = x.Value.Property});
            result.Name.Should().Be("ups");
        }
    }

    public class MyClass
    {
        public string Property { get; set; }
    }

    public class Dummy
    {
        public string Name { get; set; }
    }
}