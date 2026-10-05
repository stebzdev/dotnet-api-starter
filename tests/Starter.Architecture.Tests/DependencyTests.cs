using Starter.Architecture.Tests.Setup;
using NetArchTest.Rules;

namespace Starter.Architecture.Tests;

public sealed class DependencyTests
{
    [Fact]
    public void Domain_ShouldNotDependOnOtherLayers()
    {
        var result = Types
            .InAssembly(TestAssemblies.Domain)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Starter.Application",
                "Starter.Infrastructure",
                "Starter.Api")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_ShouldNotDependOnInfrastructureOrApi()
    {
        var result = Types
            .InAssembly(TestAssemblies.Application)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Starter.Infrastructure",
                "Starter.Api")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    /*  [Fact]
       public void Infrastructure_ShouldNotDependOnApi()
       {
           var result = Types
               .InAssembly(TestAssemblies.InfrastructureAssembly)
               .ShouldNot()
               .HaveDependencyOn("Starter.Api")
               .GetResult();

           Assert.True(result.IsSuccessful);
       }*/
}
