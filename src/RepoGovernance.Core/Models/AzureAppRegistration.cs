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
                DateTimeOffset? minDate = null;
                foreach (DateTimeOffset? item in ExpirationDates)
                {
                    if (item != null && item < minDate)
                    {
                        minDate = item;
                    }
                }
                if (minDate != null)
                {
                    return "Expiring on " + minDate?.ToString("R");
                }
                else
                {
                    return "No Expiration Date Found";
                }

            }
        }

        public AzureAppRegistration()
        {
            ExpirationDates = new();
        }
    }
}
