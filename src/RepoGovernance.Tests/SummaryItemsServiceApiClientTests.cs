using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RepoGovernance.Web.Services;

namespace RepoGovernance.Tests
{
    [TestClass]
    public class SummaryItemsServiceApiClientTests
    {
        [TestMethod]
        public async Task IgnoreRecommendation_ShouldUsePostRequest()
        {
            // Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            mockConfiguration["AppSettings:WebServiceURL"].Returns("https://test.com");
            
            HttpRequestMessage capturedRequest = null;
            MockHttpMessageHandler mockHandler = new MockHttpMessageHandler((request) =>
            {
                capturedRequest = request;
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("true")
                };
            });
            
            HttpClient mockClient = new HttpClient(mockHandler)
            {
                BaseAddress = new Uri("https://test.com")
            };
            
            SummaryItemsServiceApiClient client = new SummaryItemsServiceApiClient(mockConfiguration);
            // Use reflection to set the private _client field for testing
            System.Reflection.FieldInfo clientField = typeof(BaseServiceApiClient).GetField("_client", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            clientField.SetValue(client, mockClient);

            // Act
            bool result = await client.IgnoreRecommendation("testuser", "testowner", "testrepo", "Git Version", "Consider adding Git Versioning");

            // Assert
            Assert.IsNotNull(capturedRequest, "HTTP request should have been made");
            Assert.AreEqual(HttpMethod.Post, capturedRequest.Method, "Request should use POST method");
            Assert.IsTrue(capturedRequest.RequestUri.ToString().Contains("IgnoreRecommendation"), "Request URL should contain IgnoreRecommendation endpoint");
            Assert.IsTrue(result, "Method should return true for successful request");
        }

        [TestMethod]
        public async Task RestoreRecommendation_ShouldUsePostRequest()
        {
            // Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            mockConfiguration["AppSettings:WebServiceURL"].Returns("https://test.com");
            
            HttpRequestMessage capturedRequest = null;
            MockHttpMessageHandler mockHandler = new MockHttpMessageHandler((request) =>
            {
                capturedRequest = request;
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("true")
                };
            });
            
            HttpClient mockClient = new HttpClient(mockHandler)
            {
                BaseAddress = new Uri("https://test.com")
            };
            
            SummaryItemsServiceApiClient client = new SummaryItemsServiceApiClient(mockConfiguration);
            // Use reflection to set the private _client field for testing
            System.Reflection.FieldInfo clientField = typeof(BaseServiceApiClient).GetField("_client", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            clientField.SetValue(client, mockClient);

            // Act
            bool result = await client.RestoreRecommendation("testuser", "testowner", "testrepo", "Git Version", "Consider adding Git Versioning");

            // Assert
            Assert.IsNotNull(capturedRequest, "HTTP request should have been made");
            Assert.AreEqual(HttpMethod.Post, capturedRequest.Method, "Request should use POST method");
            Assert.IsTrue(capturedRequest.RequestUri.ToString().Contains("RestoreRecommendation"), "Request URL should contain RestoreRecommendation endpoint");
            Assert.IsTrue(result, "Method should return true for successful request");
        }

        [TestMethod]
        public async Task IgnoreRecommendation_ShouldEscapeUrlParameters()
        {
            // Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            mockConfiguration["AppSettings:WebServiceURL"].Returns("https://test.com");
            
            HttpRequestMessage capturedRequest = null;
            MockHttpMessageHandler mockHandler = new MockHttpMessageHandler((request) =>
            {
                capturedRequest = request;
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("true")
                };
            });
            
            HttpClient mockClient = new HttpClient(mockHandler)
            {
                BaseAddress = new Uri("https://test.com")
            };
            
            SummaryItemsServiceApiClient client = new SummaryItemsServiceApiClient(mockConfiguration);
            // Use reflection to set the private _client field for testing
            System.Reflection.FieldInfo clientField = typeof(BaseServiceApiClient).GetField("_client", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            clientField.SetValue(client, mockClient);

            string maliciousUser = "user&evil=true";
            string maliciousDetails = "Details with spaces & special chars";

            // Act
            await client.IgnoreRecommendation(maliciousUser, "testowner", "testrepo", "Git Version", maliciousDetails);

            // Assert
            string requestUrl = capturedRequest.RequestUri.ToString();
            Assert.IsFalse(requestUrl.Contains("&evil=true"), "Malicious parameter should be escaped");
            Assert.IsTrue(requestUrl.Contains("user%26evil%3Dtrue") || requestUrl.Contains("user&amp;evil=true"), "User parameter should be URL encoded");
            Assert.IsFalse(requestUrl.Contains("Details with spaces & special chars"), "Details should be URL encoded");
        }
    }

    // Mock HTTP message handler that captures requests
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responseFactory;

        public MockHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
        {
            _responseFactory = responseFactory;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = _responseFactory(request);
            return Task.FromResult(response);
        }
    }
}