using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.TableStorage
{
    public class AzureTableStorageDA
    {
        //Note that this can't be async due to performance issues with Azure Storage when you retrieve items
        public List<SummaryItem> GetSummaryItemsFromTable(string connectionString, string tableName,
            string partitionKey)
        {
            TableStorageCommonDA tableDA = new TableStorageCommonDA(connectionString, tableName);
            List<AzureStorageTableModel> items = tableDA.GetItems(partitionKey);
            List<SummaryItem> results = new();
            foreach (AzureStorageTableModel item in items)
            {
                string data = item.Data?.ToString();
                SummaryItem summaryItem = JsonConvert.DeserializeObject<SummaryItem>(data);
                results.Add(summaryItem);
            }
            return results;
        }

        //Update the storage with the data
        public async Task<int> UpdateSummaryItemsIntoTable(string connectionString,
                string owner, string repo, string json)
        {
            int itemsAdded = 0;
            TableStorageCommonDA tableBuildsDA = new TableStorageCommonDA(connectionString, "Summary");

            string partitionKey = owner;
            string rowKey = owner + "_" + repo;
            AzureStorageTableModel row = new(partitionKey, rowKey, json);
            if (await tableBuildsDA.AddItem(row) == true)
            {
                itemsAdded++;
            }
            else
            {
                await tableBuildsDA.SaveItem(row);
                itemsAdded++;
            }

            return itemsAdded;
        }

    }
}
