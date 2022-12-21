using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Xunit;

namespace kittyshop.Tests;

public class ResponseTest
{
    public static string _jsonMediaType = "application/json";
    public static int _expectedMaxElapsedMilliseconds = 5000;
    public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public static async Task AssertResponseWithContentAsync<T>(Stopwatch stopwatch,
        HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode,
        T expectedContent)
    {
        AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        AssertEqualMediaType(response.Content.Headers.ContentType?.MediaType);
        await AssertEqualResponseContent(response, expectedContent);
    }
    
    public static void AssertCommonResponseParts(Stopwatch stopwatch,
        HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode)
    {
        AssertStatusCode(expectedStatusCode, response.StatusCode);
        AssertTimeElapsed(stopwatch);
    }

    private static void AssertEqualMediaType(string? mediaType)
    {
        Assert.Equal(_jsonMediaType, mediaType);
    }

    private static async Task AssertEqualResponseContent<T>(HttpResponseMessage response, T expectedContent)
    {
        Assert.Equal(expectedContent, await JsonSerializer.DeserializeAsync<T?>(
            await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions));
    }

    private static void AssertTimeElapsed(Stopwatch stopwatch) =>
        Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds,
            $"Method took longer than expected. Method took {stopwatch.ElapsedMilliseconds}, but expected {_expectedMaxElapsedMilliseconds}.");

    private static void AssertStatusCode(System.Net.HttpStatusCode expectedStatusCode,
        System.Net.HttpStatusCode actualStatusCode) =>
        Assert.True(expectedStatusCode.Equals(actualStatusCode),
            $"Wrong status code. Got: {actualStatusCode}, expected: {expectedStatusCode}.");

    public static StringContent GetJsonStringContent<T>(T model)
        => new(JsonSerializer.Serialize(model), Encoding.UTF8, _jsonMediaType);
}