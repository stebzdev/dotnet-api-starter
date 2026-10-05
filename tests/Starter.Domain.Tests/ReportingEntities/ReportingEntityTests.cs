using System;
using System.Collections.Generic;
using System.Text;
using Starter.Domain.Common;
using Starter.Domain.ReportingEntities;

namespace Starter.Domain.Tests.ReportingEntities;

public sealed class ReportingEntityTests
{
    [Theory]
    [InlineData("Acme Bank", "ACME")]
    [InlineData("Global Finance", "GLOBAL")]
    public void Create_ShouldCreateReportingEntity(string name, string codeValue)
    {
        var code = new ReportingEntityCode(codeValue);

        var reportingEntity = ReportingEntity.Create(name, code);

        Assert.NotEqual(Guid.Empty, reportingEntity.Id.Value);
        Assert.Equal(name, reportingEntity.Name);
        Assert.Equal(code, reportingEntity.Code);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WhenNameIsEmpty_ShouldThrowDomainException(string name)
    {
        var code = new ReportingEntityCode("ACME");

        void Action() => ReportingEntity.Create(name, code);

        Assert.Throws<DomainException>(Action);
    }

    [Theory]
    [InlineData("  Acme Bank  ", "Acme Bank")]
    [InlineData(" Global Finance ", "Global Finance")]
    public void Create_ShouldTrimName(string name, string expected)
    {
        var code = new ReportingEntityCode("TEST");

        var reportingEntity = ReportingEntity.Create(name, code);

        Assert.Equal(expected, reportingEntity.Name);
    }
}
