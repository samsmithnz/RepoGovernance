using Newtonsoft.Json;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.TableStorage
{
    //Note that these calls to Azure Storage table can't be async due to performance issues with Azure Storage when you retrieve items
    public static class AzureTableStorageDA
    {
        public static List<SummaryItem> GetSummaryItemsFromTable(string connectionString, string tableName,
            string partitionKey)
        {
            TableStorageCommonDA tableDA = new(connectionString, tableName);
            List<AzureStorageTableModel> items = tableDA.GetItems(partitionKey);
            List<SummaryItem> results = new();
            foreach (AzureStorageTableModel item in items)
            {
                string? data = item.Data?.ToString();
                if (data != null)
                {
                    SummaryItem? summaryItem = JsonConvert.DeserializeObject<SummaryItem>(data);
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
                string user, string owner, string repo, string json)
        {
            int itemsAdded = 0;
            TableStorageCommonDA tableBuildsDA = new(connectionString, "Summary");

            string partitionKey = user;
            string rowKey = owner + "_" + repo;
            AzureStorageTableModel row = new(partitionKey, rowKey, json);
            await tableBuildsDA.SaveItem(row);
            itemsAdded++;
            //if (await tableBuildsDA.AddItem(row) == true)
            //{
            //    itemsAdded++;
            //}
            //else
            //{
            //    await tableBuildsDA.SaveItem(row);
            //    itemsAdded++;
            //}

            return itemsAdded;
        }

        //TODO: Move this into Azure Storage - currently this is a single static list of repos
        public static List<UserOwnerRepo> GetRepos(string user)
        {
            return DatabaseAccess.GetRepos(user);
        }

    }
}
