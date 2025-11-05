using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RepoGovernance.Core.APIAccess;
using System;
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
            TestModel? result = await BaseApi.GetResponse<TestModel>(null!, "http://example.com");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_NullUrl_ReturnsDefault()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act
            TestModel? result = await BaseApi.GetResponse<TestModel>(client, null!);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_ValidJsonResponse_ReturnsDeserializedObject()
        {
            // Arrange
            string json = "{\"Name\":\"Test\",\"Value\":42}";
            MockHttpMessageHandler handler = new MockHttpMessageHandler(json, HttpStatusCode.OK);
            using HttpClient client = new HttpClient(handler);

            // Act
            TestModel? result = await BaseApi.GetResponse<TestModel>(client, "http://example.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Name);
            Assert.AreEqual(42, result.Value);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_EmptyResponse_ReturnsDefault()
        {
            // Arrange
            MockHttpMessageHandler handler = new MockHttpMessageHandler("", HttpStatusCode.OK);
            using HttpClient client = new HttpClient(handler);

            // Act
            TestModel? result = await BaseApi.GetResponse<TestModel>(client, "http://example.com");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_NotFoundStatus_ReturnsDefault()
        {
            // Arrange
            string json = "{\"Name\":\"Test\",\"Value\":42}";
            MockHttpMessageHandler handler = new MockHttpMessageHandler(json, HttpStatusCode.NotFound);
            using HttpClient client = new HttpClient(handler);

            // Act
            TestModel? result = await BaseApi.GetResponse<TestModel>(client, "http://example.com", true);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_BadRequestWithIgnoreErrors_ReturnsDeserializedObject()
        {
            // Arrange
            string json = "{\"Name\":\"Test\",\"Value\":42}";
            MockHttpMessageHandler handler = new MockHttpMessageHandler(json, HttpStatusCode.BadRequest);
            using HttpClient client = new HttpClient(handler);

            // Act
            TestModel? result = await BaseApi.GetResponse<TestModel>(client, "http://example.com", true);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Name);
            Assert.AreEqual(42, result.Value);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_BadRequestWithoutIgnoreErrors_ThrowsException()
        {
            // Arrange
            string json = "{\"Name\":\"Test\",\"Value\":42}";
            MockHttpMessageHandler handler = new MockHttpMessageHandler(json, HttpStatusCode.BadRequest);
            using HttpClient client = new HttpClient(handler);

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "http://example.com", false);
                Assert.Fail("Expected HttpRequestException was not thrown");
            }
            catch (HttpRequestException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_InvalidJson_HandledGracefully()
        {
            // Arrange
            string invalidJson = "{ invalid json }";
            MockHttpMessageHandler handler = new MockHttpMessageHandler(invalidJson, HttpStatusCode.OK);
            using HttpClient client = new HttpClient(handler);

            // Act & Assert
            // The BaseApi method doesn't handle JSON parsing errors gracefully
            // so this will throw an exception (which is the current behavior)
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "http://example.com");
                Assert.Fail("Expected JsonReaderException was not thrown");
            }
            catch (JsonReaderException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_PathTraversalUrl_ThrowsException()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "http://example.com/../../../etc/passwd");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_LocalhostUrl_ThrowsException()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "http://localhost:8080/api/data");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_PrivateIpUrl_ThrowsException()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "http://192.168.1.1/api/data");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_FtpScheme_ThrowsException()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "ftp://example.com/file.txt");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_RelativeUrl_ValidatesCorrectly()
        {
            // Arrange
            string json = "{\"Name\":\"Test\",\"Value\":42}";
            MockHttpMessageHandler handler = new MockHttpMessageHandler(json, HttpStatusCode.OK);
            using HttpClient client = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://example.com")
            };

            // Act
            TestModel? result = await BaseApi.GetResponse<TestModel>(client, "/api/test");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Name);
            Assert.AreEqual(42, result.Value);
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_UrlEncodedPathTraversal_ThrowsException()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "http://example.com/%2e%2e%2f%2e%2e%2fpasswd");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_EmptyUrl_ReturnsDefault()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_WhitespaceUrl_ThrowsException()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "   ");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_UppercaseUrlEncodedPathTraversal_ThrowsException()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "http://example.com/%2E%2E%2F%2E%2E%2Fpasswd");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_DoubleEncodedPathTraversal_ThrowsException()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "http://example.com/%252e%252e%252f%252e%252e%252fpasswd");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_NullByteInjection_ThrowsException()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "http://example.com/api%00/../passwd");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_ProtocolRelativeUrl_ThrowsException()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "//malicious.com/api");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public async Task BaseApi_GetResponse_MixedCasePathTraversal_ThrowsException()
        {
            // Arrange
            using HttpClient client = new HttpClient();

            // Act & Assert
            try
            {
                await BaseApi.GetResponse<TestModel>(client, "http://example.com/../..\\..\\passwd");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected exception
            }
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
            HttpResponseMessage response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_response, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }
}