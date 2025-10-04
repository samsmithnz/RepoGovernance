using RepoGovernance.Core.Models;
using RepoGovernance.Core.TableStorage;
using System.Text.Json;

namespace RepoGovernance.Core
{
    public class IgnoredRecommendationsDA : IIgnoredRecommendationsDA
    {
        private readonly TableStorageCommonDA _tableStorageDA;

        public IgnoredRecommendationsDA(string connectionString)
        {
            _tableStorageDA = new(connectionString, "IgnoredRecommendations");
        }

        public async Task<List<IgnoredRecommendation>> GetIgnoredRecommendations(string user)
        {
            List<IgnoredRecommendation> ignoredRecommendations = new();
            List<AzureStorageTableModel> items = await _tableStorageDA.GetItems(user);
            
            foreach (AzureStorageTableModel item in items)
            {
                if (!string.IsNullOrEmpty(item.Data))
                {
                    IgnoredRecommendation? ignoredRecommendation = JsonSerializer.Deserialize<IgnoredRecommendation>(item.Data);
                    if (ignoredRecommendation != null)
                    {
                        ignoredRecommendations.Add(ignoredRecommendation);
                    }
                }
            }
            
            return ignoredRecommendations;
        }

        public async Task<List<IgnoredRecommendation>> GetIgnoredRecommendations(string user, string owner, string repository)
        {
            List<IgnoredRecommendation> allIgnoredRecommendations = await GetIgnoredRecommendations(user);
            
            // Filter by owner and repository
            List<IgnoredRecommendation> filteredRecommendations = allIgnoredRecommendations
                .Where(ir => ir.Owner == owner && ir.Repository == repository)
                .ToList();
            
            return filteredRecommendations;
        }

        public async Task<bool> IgnoreRecommendation(string user, string owner, string repository, string recommendationType, string recommendationDetails)
        {
            try
            {
                Console.WriteLine($"IgnoreRecommendation called with user={user}, owner={owner}, repository={repository}, type={recommendationType}, details={recommendationDetails}");
                IgnoredRecommendation ignoredRecommendation = new(user, owner, repository, recommendationType, recommendationDetails);
                string data = JsonSerializer.Serialize(ignoredRecommendation);
                string uniqueId = ignoredRecommendation.GetUniqueId();
                Console.WriteLine($"Generated uniqueId: {uniqueId}");
                AzureStorageTableModel storageItem = new(user, uniqueId, data);
                Console.WriteLine($"Saving item to table storage...");
                bool result = await _tableStorageDA.SaveItem(storageItem);
                Console.WriteLine($"SaveItem result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in IgnoreRecommendation: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UnignoreRecommendation(string user, string owner, string repository, string recommendationType, string recommendationDetails)
        {
            IgnoredRecommendation temp = new(user, owner, repository, recommendationType, recommendationDetails);
            string uniqueId = temp.GetUniqueId();
            
            // Get the item first to delete it
            AzureStorageTableModel? existingItem = await _tableStorageDA.GetItem(user, uniqueId);
            if (existingItem == null)
            {
                return false; // Item doesn't exist
            }

            // For now, we'll mark it as "deleted" by updating with empty data
            // This is a simple approach that works with the existing SaveItem method
            AzureStorageTableModel updatedItem = new(user, uniqueId, "");
            return await _tableStorageDA.SaveItem(updatedItem);
        }

        public async Task<bool> IsRecommendationIgnored(string user, string owner, string repository, string recommendationType, string recommendationDetails)
        {
            IgnoredRecommendation temp = new(user, owner, repository, recommendationType, recommendationDetails);
            string uniqueId = temp.GetUniqueId();
            
            AzureStorageTableModel? item = await _tableStorageDA.GetItem(user, uniqueId);
            return item != null && !string.IsNullOrEmpty(item.Data);
        }
    }
}