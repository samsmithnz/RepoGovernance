namespace RepoGovernance.Core.Models
{
    public class AzureAppRegistration
    {
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ExpirationDateString
        {
            get
            {
                return "";
            }
        }
    }
}
