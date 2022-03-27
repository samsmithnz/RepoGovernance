namespace RepoGovernance.Core.Models
{
    public class UserOwnerRepo
    {
        public UserOwnerRepo(string user, string owner, string repo)
        {
            User = user;
            Owner = owner;
            Repo = repo;
        }

        public string User { get; set; }
        public string Owner { get; set; }
        public string Repo { get; set; }
    }
}
