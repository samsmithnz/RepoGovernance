using Microsoft.Azure.Cosmos.Table;
using RepoGovernance.Core.Models;

//TODO: Update: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/tables/Azure.Data.Tables/MigrationGuide.md
namespace RepoGovernance.Core.TableStorage
{
    public class TableStorageCommonDA
    {
        private readonly string? ConfigurationString;
        private readonly string? TableName;

        public TableStorageCommonDA(string connectionString, string tableName)
        {
            ConfigurationString = connectionString;
            TableName = tableName;
        }

        //This is needed for Dependency Injection
        public TableStorageCommonDA() { }

        private CloudTable CreateConnection()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Get a reference to a table named "items"
            CloudTable table = tableClient.GetTableReference(TableName);

            // Create the table if it doesn't exist
            // DON"T use table.CreateIfNotExists, it throws an internal 409 in App insights: https://stackoverflow.com/questions/48893519/azure-table-storage-exception-409-conflict-unexpected
            if (!table.Exists())
            {
                table.Create();
            }

            return table;
        }      

        public async Task<AzureStorageTableModel> GetItem(string partitionKey, string rowKey)
        {
            //prepare the partition key
            partitionKey = EncodePartitionKey(partitionKey);

            CloudTable table = CreateConnection();

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<AzureStorageTableModel>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            return (AzureStorageTableModel)retrievedResult.Result;
        }

        //This can't be async, because of how it queries the underlying data
        public List<AzureStorageTableModel> GetItems(string partitionKey)
        {
            partitionKey = EncodePartitionKey(partitionKey);

            CloudTable table = CreateConnection();

            // execute the query on the table
            List<AzureStorageTableModel> list = table.CreateQuery<AzureStorageTableModel>()
                                     .Where(ent => ent.PartitionKey == partitionKey)
                                     .ToList();

            return list;
        }

        public async Task<bool> SaveItem(AzureStorageTableModel data)
        {
            CloudTable table = CreateConnection();

            // Create the TableOperation that inserts/merges the entity.
            TableOperation operation = TableOperation.InsertOrMerge(data);
            await table.ExecuteAsync(operation);
            return true;
        }

        public static string EncodePartitionKey(string text)
        {
            //The forward slash(/) character
            text = text.Replace("/", "_");

            return text;
        }

    }
}