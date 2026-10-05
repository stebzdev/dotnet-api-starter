
using System.Reflection;
using System.Runtime.CompilerServices;


namespace Starter.Architecture.Tests.Setup;

internal static class TestAssemblies
{
    internal static readonly Assembly Domain = typeof(Domain.ReportingPeriods.ReportingPeriod).Assembly;
    internal static readonly Assembly Application = typeof(Application.DependencyInjection).Assembly;
    internal static readonly Assembly Api = typeof(Api.ReportingPeriods.Create.CreateReportingPeriodEndpoint).Assembly;
  //  internal static readonly Assembly Infrastructure = typeof(Infrastructure.DependencyInjection).Assembly;


    internal static readonly Assembly[] All =
    [
        TestAssemblies.Domain,
        TestAssemblies.Application,
        TestAssemblies.Api,
      //  TestAssemblies.Infrastructure
    ];

    internal static bool IsGeneratedFrameworkType(Type type)
    {
        return type.FullName?.Contains("OpenApiXmlCommentSupport_generated", StringComparison.Ordinal) == true;
    }

    internal static bool IsGeneratedType(Type type)
    {
        return type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false)
            || type.DeclaringType?.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false) == true
            || type == typeof(Program);
    }

    internal static bool IsCoverageInstrumentationType(Type type)
    {
        return type.Namespace?.StartsWith("Coverlet.Core.Instrumentation.Tracker", StringComparison.Ordinal) == true;
    }
}
