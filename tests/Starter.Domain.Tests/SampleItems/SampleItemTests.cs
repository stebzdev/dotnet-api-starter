using System;
using System.Collections.Generic;
using System.Text;
using Starter.Domain.Common;
using Starter.Domain.SampleItems;

namespace Starter.Domain.Tests.SampleItems;

public sealed class SampleItemTests
{
    [Theory]
    [InlineData("Acme Bank", "ACME")]
    [InlineData("Global Finance", "GLOBAL")]
    public void Create_ShouldCreateSampleEntity(string name, string codeValue)
    {
        var code = new SampleItemCode(codeValue);

        var sampleEntity = SampleItem.Create(name, code);

        Assert.NotEqual(Guid.Empty, sampleEntity.Id.Value);
        Assert.Equal(name, sampleEntity.Name);
        Assert.Equal(code, sampleEntity.Code);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WhenNameIsEmpty_ShouldThrowDomainException(string name)
    {
        var code = new SampleItemCode("ACME");

        void Action() => SampleItem.Create(name, code);

        Assert.Throws<DomainException>(Action);
    }

    [Theory]
    [InlineData("  Acme Bank  ", "Acme Bank")]
    [InlineData(" Global Finance ", "Global Finance")]
    public void Create_ShouldTrimName(string name, string expected)
    {
        var code = new SampleItemCode("TEST");

        var sampleEntity = SampleItem.Create(name, code);

        Assert.Equal(expected, sampleEntity.Name);
    }
}
