using Newtonsoft.Json.Linq;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.TableStorage
{
    public class AzureTableStorageDA
    {
        //Note that this can't be async due to performance issues with Azure Storage when you retrieve items
        public JArray GetSummaryItemsFromTable(string connectionString, string tableName, 
            string partitionKey, bool includePartitionAndRowKeys = false)
        {
            TableStorageCommonDA tableDA = new TableStorageCommonDA(connectionString, tableName);
            List<AzureStorageTableModel> items = tableDA.GetItems(partitionKey);
            JArray list = new();
            foreach (AzureStorageTableModel item in items)
            {
                if (includePartitionAndRowKeys == true)
                {
                    string data = item.Data?.ToString();
                    list.Add(new JObject(
                                new JProperty("PartitionKey", item.PartitionKey),
                                new JProperty("RowKey", item.RowKey),
                                new JProperty("Data", data)
                            )
                        );
                }
                else
                {
                    list.Add(JToken.Parse(item.Data));
                }
            }
            return list;
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
