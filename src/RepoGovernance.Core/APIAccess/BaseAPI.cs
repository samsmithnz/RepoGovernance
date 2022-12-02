using DotNetCensus.Core.Models.GitHub;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoGovernance.Core.APIAccess
{
    public static class BaseAPI
    {
        public static async Task<T?> GetResponse<T>(HttpClient client, string url, bool ignoreErrors = false)
        {
            T obj = default;
            if (client != null && url != null)
            {
                //Debug.WriteLine("Running url: " + client.BaseAddress.ToString() + url);
                //Console.WriteLine("Running url: " + client.BaseAddress.ToString() + url);
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
