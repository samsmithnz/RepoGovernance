namespace RepoGovernance.Core.Models
{
    public class AzureDeployment
    {
        public List<AzureAppRegistration> AppRegistrations { get; set; }
        public string? DeployedURL { get; set; }

        public AzureDeployment()
        {
            AppRegistrations = new();
        }
    }
}
