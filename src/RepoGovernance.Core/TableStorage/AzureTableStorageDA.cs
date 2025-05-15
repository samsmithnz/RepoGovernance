using System.Text.Json;
using RepoGovernance.Core.Models;

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
            string partitionKey)
        {
            TableStorageCommonDA tableDA = new(connectionString, tableName);
            List<AzureStorageTableModel> items = await tableDA.GetItems(partitionKey);
            List<SummaryItem> results = new();
            foreach (AzureStorageTableModel item in items)
            {
                string? data = item.Data?.ToString();
                if (data != null)
                {
                    SummaryItem? summaryItem = JsonSerializer.Deserialize<SummaryItem>(data);
                    if (summaryItem != null)
                    {
                        results.Add(summaryItem);
                    }
                }
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
