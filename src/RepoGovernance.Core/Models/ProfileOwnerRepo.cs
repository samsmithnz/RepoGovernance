namespace RepoGovernance.Core.Models
{
    public class ProfileOwnerRepo
    {
        public ProfileOwnerRepo(string profile, string owner, string repo)
        {
            Profile = profile;
            Owner = owner;
            Repo = repo;
        }

        public string Profile { get; set; }
        public string Owner { get; set; }
        public string Repo { get; set; }
    }
}
