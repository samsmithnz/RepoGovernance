using RepoAutomation.Core.APIAccess;
using RepoGovernance.Core.Models;
using System.Text.Json;

namespace RepoGovernance.Core.TableStorage
{
    //Note that these calls to Azure Storage table can't be async due to performance issues with Azure Storage when you retrieve items
    public static class AzureTableStorageDA
    {
        public static async Task<List<UserOwnerRepo>> GetUserOwnerRepoItemsFromTable(string connectionString, string tableName,
            string partitionKey)
        {
            TableStorageCommonDA tableDA = new(connectionString, tableName);
            List<AzureStorageTableModel> items = await tableDA.GetItems(partitionKey);
            List<UserOwnerRepo> results = new();
            foreach (AzureStorageTableModel item in items)
            {
                string? data = item.RowKey.ToString();
                if (data != null)
                {
                    results.Add(new UserOwnerRepo(data.Split('_')[0], data.Split('_')[1], data.Split('_')[2]));
                }
            }
            return results;
        }

        public static async Task<List<SummaryItem>> GetSummaryItemsFromTable(string connectionString, string tableName,
            string partitionKey, string? gitHubId, string? gitHubSecret)
        {
            TableStorageCommonDA tableDA = new(connectionString, tableName);
            List<AzureStorageTableModel> items = await tableDA.GetItems(partitionKey);
            List<SummaryItem> results = new();
            foreach (AzureStorageTableModel item in items)
            {
                string? data = item.Data?.ToString();
                if (string.IsNullOrEmpty(data) == false)
                {
                    SummaryItem? summaryItem = JsonSerializer.Deserialize<SummaryItem>(data);
                    if (summaryItem != null)
                    {
                        results.Add(summaryItem);
                    }
                }
                else
                {
                    if (item.RowKey != null)
                    {
                        string[] keys = item.RowKey.Split('_');
                        if (keys.Length == 2)
                        {
                            SummaryItem newSummaryItem = new(partitionKey, keys[0], keys[1]);
                            RepoAutomation.Core.Models.Repo? repoSettings = await GitHubApiAccess.GetRepo(gitHubId, gitHubSecret, keys[0], keys[1]);
                            if (repoSettings != null)
                            {
                                newSummaryItem.RepoSettings = repoSettings;
                            }
                            await UpdateSummaryItemsIntoTable(connectionString, partitionKey, keys[0], keys[1], newSummaryItem);
                            results.Add(newSummaryItem);
                        }
                    }
                }
                            // Run the API/database update in parallel
                            tasks.Add(Task.Run(async () =>
                            {
                                SummaryItem newSummaryItem = new(partitionKey, keys[0], keys[1]);
                                RepoAutomation.Core.Models.Repo? repoSettings = await GitHubApiAccess.GetRepo(gitHubId, gitHubSecret, keys[0], keys[1]);
                                if (repoSettings != null)
                                {
                                    newSummaryItem.RepoSettings = repoSettings;
                                }
                                await UpdateSummaryItemsIntoTable(connectionString, partitionKey, keys[0], keys[1], newSummaryItem);
                                concurrentResults.Add(newSummaryItem);
                            }));
                        }
                    }
                }
            }
            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
                results.AddRange(concurrentResults);
            }
            return results;
        }

        //Update the storage with the data
        public static async Task<int> UpdateSummaryItemsIntoTable(string connectionString,
                string user, string owner, string repo, SummaryItem summaryItem)
        {
            int itemsAdded = 0;
            TableStorageCommonDA tableBuildsDA = new(connectionString, "Summary");
            string json = JsonSerializer.Serialize(summaryItem);
            string partitionKey = user;
            string rowKey = owner + "_" + repo;

            AzureStorageTableModel row = new(partitionKey, rowKey, json);
            await tableBuildsDA.SaveItem(row);
            itemsAdded++;

            return itemsAdded;
        }

    }
}
