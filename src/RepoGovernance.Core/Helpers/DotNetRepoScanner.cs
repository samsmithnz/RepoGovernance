namespace RepoGovernance.Core.Helpers
{
    public class DotNetRepoScanner
    {
        //private static string? GetColor(string? framework)
        //{
        //    if (framework == null)
        //    {
        //        //Unknown/gray
        //        return "bg-secondary";
        //    }
        //    else if (framework.Contains(".NET Framework v1") ||
        //        framework.Contains(".NET Framework v2") ||
        //        framework.Contains(".NET Framework v3") ||
        //        framework.Contains(".NET Framework v4.0") ||
        //        framework.Contains(".NET Framework v4.1") ||
        //        framework.Contains(".NET Framework v4.2") ||
        //        framework.Contains(".NET Framework v4.3") ||
        //        framework.Contains(".NET Framework v4.4") ||
        //        framework.Contains(".NET Framework v4.5") ||
        //        framework.Contains(".NET Framework v4.6.1") ||
        //        framework.Contains("net5.0"))
        //    {
        //        //Unsupported/End of life/red
        //        return "bg-danger";
        //    }
        //    else if (framework.Contains(".NET Framework") ||
        //        framework.Contains("netcoreapp"))
        //    {
        //        //Supported, but old/orange
        //        return "bg-info";
        //    }
        //    else if (framework.Contains("net6.0") ||
        //        framework.Contains("net7.0") ||
        //        framework.Contains("netstandard") ||
        //        framework.Contains("Unity3d v2020") ||
        //        framework.Contains(".NET Framework v4.6.2") ||
        //        framework.Contains(".NET Framework v4.7") ||
        //        framework.Contains(".NET Framework v4.8"))
        //    {
        //        //Supported/Ok/blue
        //        return "bg-primary";
        //    }
        //    else
        //    {
        //        //Unknown/gray
        //        return "bg-secondary";
        //    }
        //}

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