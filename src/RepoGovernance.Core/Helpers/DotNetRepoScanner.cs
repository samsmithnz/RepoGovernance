namespace RepoGovernance.Core.Helpers
{
    public class DotNetRepoScanner
    {        
        public static string? GetColorFromStatus(string? framework)
        {
            if (framework == "unknown")
            {
                //Unknown/gray
                return "bg-secondary";
            }
            else if (framework == "deprecated")
            {
                //Unsupported/End of life/red  
                return "bg-danger";
            }
            else if (framework?.StartsWith("EOL") == true)
            {
                //Supported, but old/orange
                return "bg-warning";
            }
            else if (framework == "supported")
            {
                //Supported/Ok/blue
                return "bg-primary";
            }
            else if (framework == "in preview")
            {
                //Usable, but new - light blue
                return "bg-info";
            }
            else
            {
                //Unknown/gray
                return "bg-secondary";
            }
        }
    }
}