namespace RepoGovernance.Service.Models
{
    public class NuGetPayload
    {
        public NuGetPayload(string user, string owner, string repo, string jsonPayload, string payloadType)
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
        public string? JsonPayload { get; set; }
        public string? PayloadType { get; set; }
    }
}
