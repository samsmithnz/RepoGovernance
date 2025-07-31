using Newtonsoft.Json;

namespace RepoGovernance.Core.APIAccess
{
    public static class BaseApi
    {
        public static async Task<T?> GetResponse<T>(HttpClient client, string url, bool ignoreErrors = false)
        {
            T? obj = default;
            if (client != null && url != null)
            {
                // Validate URL to prevent path traversal and SSRF attacks
                if (!IsValidUrl(url))
                {
                    throw new ArgumentException("Invalid or unsafe URL provided", nameof(url));
                }
                
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    if (ignoreErrors == true || response.IsSuccessStatusCode == true)
                    {
                        if (response.StatusCode.ToString() != "NotFound")
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            if (string.IsNullOrEmpty(responseBody) == false)
                            {
                                obj = JsonConvert.DeserializeObject<T>(responseBody);
                            }
                        }
                    }
                    else
                    {
                        //Throw an exception
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// Validates that a URL is safe and prevents path traversal and SSRF attacks
        /// </summary>
        /// <param name="url">The URL to validate</param>
        /// <returns>True if the URL is safe, false otherwise</returns>
        private static bool IsValidUrl(string url)
        {
            // Check for null or empty
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            // Check for path traversal sequences
            if (url.Contains("../") || url.Contains("..\\") || url.Contains("%2e%2e%2f") || url.Contains("%2e%2e%5c"))
            {
                return false;
            }

            // For relative URLs (like "/api/..."), allow them as they're used internally
            if (url.StartsWith("/"))
            {
                return true;
            }

            // For absolute URLs, validate the URI
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
            {
                return false;
            }

            // Only allow HTTP and HTTPS schemes
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                return false;
            }

            // Prevent requests to private network ranges to avoid SSRF
            string host = uri.Host.ToLowerInvariant();
            
            // Block localhost and loopback
            if (host == "localhost" || host == "127.0.0.1" || host.StartsWith("127."))
            {
                return false;
            }

            // Block private IP ranges (10.x.x.x, 192.168.x.x, 172.16-31.x.x)
            if (System.Net.IPAddress.TryParse(host, out System.Net.IPAddress? ipAddress))
            {
                byte[] bytes = ipAddress.GetAddressBytes();
                if (bytes.Length == 4) // IPv4
                {
                    // 10.0.0.0/8
                    if (bytes[0] == 10)
                        return false;
                    
                    // 192.168.0.0/16
                    if (bytes[0] == 192 && bytes[1] == 168)
                        return false;
                    
                    // 172.16.0.0/12 (172.16.0.0 to 172.31.255.255)
                    if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
                        return false;
                }
            }

            return true;
        }
    }
}
