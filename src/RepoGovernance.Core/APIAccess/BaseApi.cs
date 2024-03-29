﻿using Newtonsoft.Json;

namespace RepoGovernance.Core.APIAccess
{
    public static class BaseApi
    {
        public static async Task<T?> GetResponse<T>(HttpClient client, string url, bool ignoreErrors = false)
        {
            T? obj = default;
            if (client != null && url != null)
            {
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
    }
}
