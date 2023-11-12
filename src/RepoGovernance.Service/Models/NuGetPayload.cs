using System.Text;

namespace RepoGovernance.Service.Models
{
    public class NuGetPayload
    {
        public NuGetPayload(string user, string owner, string repo, string[] jsonPayload, string payloadType)
        {
            User = user;
            Owner = owner;
            Repo = repo;
            JsonPayload = jsonPayload;
            PayloadType = payloadType;
        }
        public string? User { get; set; }
        public string? Owner { get; set; }
        public string? Repo { get; set; }
        //There is some weirdness when the json is embedded in this object and then the object is serialized a second time - it returns an array of strings.
        public string[]? JsonPayload { get; set; }
        public string JsonPayloadString
        {
            get
            {
                if (JsonPayload == null)
                {
                    return string.Empty;
                }
                return UsingLoopStringBuilder(JsonPayload);
            }
        }
        public string? PayloadType { get; set; }

        private string UsingLoopStringBuilder(string[] array)
        {
            StringBuilder result = new();
            foreach (string item in array)
            {
                result.Append(item);
            }
            return result.ToString();
        }
    }
}
