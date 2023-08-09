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
                if (ExpirationDate != null)
                {
                    return "Expiring on " + ExpirationDate?.ToString("R");
                }
                else
                {
                    return "No expiration date found";
                }
            }
        }
        public DateTimeOffset? ExpirationDate
        {
            get
            {
                DateTimeOffset? minDate = null;
                foreach (DateTimeOffset? item in ExpirationDates)
                {
                    if (item != null && (minDate == null || item < minDate))
                    {
                        minDate = item;
                    }
                }
                return minDate;
            }
        }

        public AzureAppRegistration()
        {
            ExpirationDates = new();
        }
    }
}
