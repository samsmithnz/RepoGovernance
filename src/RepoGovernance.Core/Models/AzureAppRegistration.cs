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
                    if (ExpirationDate < DateTime.Now)
                    {
                        return "Expired on " + ExpirationDate?.ToString("R");
                    }
                    else
                    {
                        return "Expiring on " + ExpirationDate?.ToString("R");
                    }
                    
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
                DateTimeOffset? maxDate = null;
                foreach (DateTimeOffset? item in ExpirationDates)
                {
                    //Get the date furthest in the future
                    if (item != null && (maxDate == null || item > maxDate))
                    {
                        maxDate = item;
                    }
                }
                return maxDate;
            }
        }

        public AzureAppRegistration()
        {
            ExpirationDates = new();
        }
    }
}
