namespace RepoGovernance.Core.Models
{
    public class AzureAppRegistration
    {
        public string? Name { get; set; }
        public List<DateTimeOffset?> ExpirationDates { get; set; }
        public string ExpirationDateString
        {
            get
            {
                return "";
            }
        }

        public AzureAppRegistration()
        {
            ExpirationDates = new();
        }
    }
}
