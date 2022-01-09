using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoGovernance.Core.Models
{
    public class SearchResults
    {
        public int total_count { get; set; }
        public List<Repo> items { get; set; }
    }
}
