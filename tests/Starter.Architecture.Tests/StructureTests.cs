using Starter.Application.Common.CQRS;
using Starter.Architecture.Tests.Setup;
using NetArchTest.Rules;

namespace Starter.Architecture.Tests;

public sealed class StructureTests
{
    [Fact]
    public void Handlers_ShouldBeSealed()
    {
        var result = Types
            .InAssembly(TestAssemblies.Application)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .BeSealed()
            .GetResult();

        ArchitectureAssert.IsSuccessful(result);
    }

    [Fact]
    public void QueryHandlers_ShouldBeSealed()
    {
        var result = Types
            .InAssembly(TestAssemblies.Application)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .BeSealed()
            .GetResult();

        ArchitectureAssert.IsSuccessful(result);
    }

    [Fact]
    public void Commands_ShouldBeSealed()
    {
        var result = Types
            .InAssembly(TestAssemblies.Application)
            .That()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .BeSealed()
            .GetResult();

        ArchitectureAssert.IsSuccessful(result);
    }

    [Fact]
    public void Queries_ShouldBeSealed()
    {
        var result = Types
            .InAssembly(TestAssemblies.Application)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .BeSealed()
            .GetResult();

        ArchitectureAssert.IsSuccessful(result);
    }

    [Fact]
    public void Types_Should_Have_Valid_Project_Namespace()
    {
        var invalidTypes = TestAssemblies.All
            .SelectMany(assembly =>
            {
                var projectName = assembly.GetName().Name!;

                return assembly
                    .GetTypes()
                    .Where(type => !TestAssemblies.IsGeneratedType(type))
                    .Where(type => !TestAssemblies.IsGeneratedFrameworkType(type))
                    .Where(type => !TestAssemblies.IsCoverageInstrumentationType(type))
                    .Where(type => type.Namespace is null || !type.Namespace.StartsWith(projectName, StringComparison.Ordinal));
            }).ToArray();

        Assert.True(
            invalidTypes.Length == 0,
            $"The following types have an invalid namespace:{Environment.NewLine}" + string.Join(Environment.NewLine, invalidTypes.Select(type => type.FullName)));
    }
}

internal static class ArchitectureAssert
{
    internal static void IsSuccessful(TestResult result)
    {
        if (result.IsSuccessful)
        {
            return;
        }

        Assert.Fail(
            $"The following types violate the architecture rule:{Environment.NewLine}" +
            string.Join(Environment.NewLine, result.FailingTypes.Select(x => x.FullName)));
    }
}
