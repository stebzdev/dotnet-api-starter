using Starter.Application.Common.CQRS;
using Starter.Architecture.Tests.Setup;
using NetArchTest.Rules;

namespace Starter.Architecture.Tests;

public sealed class NamingConventionTests
{
    [Fact]
    public void Interfaces_ShouldStartWithI()
    {
        var result = Types
            .InAssemblies(TestAssemblies.All)
            .That()
            .AreInterfaces()
            .Should()
            .HaveNameStartingWith("I")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Commands_ShouldHaveCommandSuffix()
    {
        var result = Types
            .InAssembly(TestAssemblies.Application)
            .That()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Queries_ShouldHaveQuerySuffix()
    {
        var result = Types
            .InAssembly(TestAssemblies.Application)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .HaveNameEndingWith("Query")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void CommandHandlers_ShouldHaveHandlerSuffix()
    {
        var result = Types
            .InAssembly(TestAssemblies.Application)
            .That()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void QueryHandlers_ShouldHaveHandlerSuffix()
    {
        var result = Types
            .InAssembly(TestAssemblies.Application)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}

