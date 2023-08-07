namespace RepoGovernance.Core.Models.Azure
{
    public class Application
    {
        //https://learn.microsoft.com/en-us/graph/api/application-list?view=graph-rest-1.0&tabs=http#http-request
        //response
        //        {
        //  "@odata.context": "https://graph.microsoft.com/v1.0/$metadata#applications",
        //  "value": [
        //    {
        //      "appId": "00000000-0000-0000-0000-000000000000",
        //      "identifierUris": [ "http://contoso/" ],
        //      "displayName": "My app",
        //      "publisherDomain": "contoso.com",
        //      "signInAudience": "AzureADMyOrg"
        //    }
        //  ]
        //}

        public string? appId { get; set; }
        public string? displayName { get; set; }
    }
}
