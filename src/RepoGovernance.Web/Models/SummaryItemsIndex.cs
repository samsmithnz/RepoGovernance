﻿using RepoAutomation.Core.Models;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Web.Models
{
    public class SummaryItemsIndex
    {
        public List<SummaryItem> SummaryItems { get; set; }
        public List<RepoLanguage> SummaryRepoLanguages { get; set; }
        public bool IsContributor { get; set; }
    }
}
