using System.Diagnostics;
using Xunit;

namespace kittyshop.Tests;

public class CatsTest
{
    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:5186") };

    [Fact]
    public async Task Get_ReturnsAnEmptyList_OnEmptyDatabase()
    {
        var expectedStatusCode = System.Net.HttpStatusCode.OK;
        var stopwatch = Stopwatch.StartNew();
        var expectedContent = new object[] { };

        var response = await _httpClient.GetAsync("/api/Cats");

        await ResponseTest.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }
}