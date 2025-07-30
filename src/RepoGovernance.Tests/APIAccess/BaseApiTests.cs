using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RepoGovernance.Core.APIAccess;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RepoGovernance.Tests.APIAccess
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BaseApiTests
    {
        private class TestModel
        {
            public string? Name { get; set; }
            public int Value { get; set; }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_NullClient_ReturnsDefault()
        {
            // Act
            var result = await BaseApi.GetResponse<TestModel>(null!, "http://example.com");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_NullUrl_ReturnsDefault()
        {
            // Arrange
            using var client = new HttpClient();

            // Act
            var result = await BaseApi.GetResponse<TestModel>(client, null!);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_ValidJsonResponse_ReturnsDeserializedObject()
        {
            // Arrange
            var json = "{\"Name\":\"Test\",\"Value\":42}";
            var handler = new MockHttpMessageHandler(json, HttpStatusCode.OK);
            using var client = new HttpClient(handler);

            // Act
            var result = await BaseApi.GetResponse<TestModel>(client, "http://example.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Name);
            Assert.AreEqual(42, result.Value);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_EmptyResponse_ReturnsDefault()
        {
            // Arrange
            var handler = new MockHttpMessageHandler("", HttpStatusCode.OK);
            using var client = new HttpClient(handler);

            // Act
            var result = await BaseApi.GetResponse<TestModel>(client, "http://example.com");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_NotFoundStatus_ReturnsDefault()
        {
            // Arrange
            var json = "{\"Name\":\"Test\",\"Value\":42}";
            var handler = new MockHttpMessageHandler(json, HttpStatusCode.NotFound);
            using var client = new HttpClient(handler);

            // Act
            var result = await BaseApi.GetResponse<TestModel>(client, "http://example.com", true);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_BadRequestWithIgnoreErrors_ReturnsDeserializedObject()
        {
            // Arrange
            var json = "{\"Name\":\"Test\",\"Value\":42}";
            var handler = new MockHttpMessageHandler(json, HttpStatusCode.BadRequest);
            using var client = new HttpClient(handler);

            // Act
            var result = await BaseApi.GetResponse<TestModel>(client, "http://example.com", true);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Name);
            Assert.AreEqual(42, result.Value);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_BadRequestWithoutIgnoreErrors_ThrowsException()
        {
            // Arrange
            var json = "{\"Name\":\"Test\",\"Value\":42}";
            var handler = new MockHttpMessageHandler(json, HttpStatusCode.BadRequest);
            using var client = new HttpClient(handler);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<HttpRequestException>(async () =>
            {
                await BaseApi.GetResponse<TestModel>(client, "http://example.com", false);
            });
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_InvalidJson_HandledGracefully()
        {
            // Arrange
            var invalidJson = "{ invalid json }";
            var handler = new MockHttpMessageHandler(invalidJson, HttpStatusCode.OK);
            using var client = new HttpClient(handler);

            // Act & Assert
            // The BaseApi method doesn't handle JSON parsing errors gracefully
            // so this will throw an exception (which is the current behavior)
            await Assert.ThrowsExceptionAsync<JsonReaderException>(async () =>
            {
                await BaseApi.GetResponse<TestModel>(client, "http://example.com");
            });
        }
    }

    // Mock HTTP message handler for testing
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;
        private readonly HttpStatusCode _statusCode;

        public MockHttpMessageHandler(string response, HttpStatusCode statusCode)
        {
            _response = response;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_response, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }
}