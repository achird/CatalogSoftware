using AutoMapper;
using Xunit;

namespace catalog.Test.Core.Locating.Application;

public class MappingTest
{
    [Fact]
    public void MappingConfigTest()
    {
        var configuration = new MapperConfiguration(c =>
        {
            c.AddProfile(new catalog.Core.Locating.Application.Mapping.MapperProfile());
        });
        configuration.AssertConfigurationIsValid();
    }
}
