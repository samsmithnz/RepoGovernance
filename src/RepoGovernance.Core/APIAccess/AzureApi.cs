using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess
{

    public class AzureApi
    {
        private readonly GraphServiceClient Client;

        public AzureApi(string tenantId, string clientId, string clientSecret)//string azureAPIURL = "https://graph.microsoft.com/v1.0/")
        { 
            // The client credentials flow requires that you request the
            // /.default scope, and pre-configure your permissions on the
            // app registration in Azure. An administrator must grant consent
            // to those permissions beforehand.
            string[] scopes = new[] { "https://graph.microsoft.com/.default" };

            // using Azure.Identity;
            ClientSecretCredentialOptions options = new()
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };

            // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            ClientSecretCredential clientSecretCredential = new(tenantId, clientId, clientSecret, options);

            Client = new(clientSecretCredential, scopes);
        }

        public async Task<List<AzureAppRegistration>> GetApplications()
        {
            List<AzureAppRegistration> results = new();
            //string url = "applications";
            //Application? applications = await BaseApi.GetResponse<Application>(Client, url);

            if (Client != null)
            {
                ApplicationCollectionResponse? application = await Client.Applications.GetAsync();
            }

            return results;
        }

    }
}
