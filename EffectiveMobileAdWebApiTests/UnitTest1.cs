using System.Diagnostics;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using RestSharp;
using Xunit.Sdk;

namespace EffectiveMobileAdWebApiTests;

public class AdvertisementControllerTests
{
    readonly RestClient client = new("https://localhost:7193/api/Advertisement");

    [Fact]
    public async void TestLoadPlatformsFromFile()
    {
        RestRequest request = new("load-platforms-from-file", Method.Post);

        request.AddFile("formFile", Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..", "test.txt"));

        string? statusCode = (await client.ExecutePostAsync(request)).StatusCode.ToString();

        Assert.Equal("OK", statusCode);
    }

    [Fact]
    public async void TestSearchByAddress()
    {
        RestRequest request = new("load-platforms-from-file", Method.Post);

        request.AddFile("formFile", Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..", "test.txt"));

        request = new("search-by-address/{address}");
        request.AddParameter("address", "/ru/svrd", false);

        string? statusCode = (await client.ExecuteGetAsync(request)).StatusDescription;

        Assert.Equal("OK", statusCode);
    }
}