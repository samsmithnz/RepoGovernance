using Azure;
using Azure.Data.Tables;
using RepoGovernance.Core.Models;

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

        private async Task<TableClient> CreateConnection()
        {
            //Create a connection to the Azure Table
            TableServiceClient serviceClient = new(ConfigurationString);

            // Get a reference to the TableClient from the service client instance.
            TableClient tableClient = serviceClient.GetTableClient(TableName);

            // Create the table if it doesn't exist.
            await tableClient.CreateIfNotExistsAsync();

            return tableClient;
        }

        public async Task<AzureStorageTableModel> GetItem(string partitionKey, string rowKey)
        {
            //prepare the partition key
            partitionKey = EncodePartitionKey(partitionKey);

            //Create a connection to the Azure Table
            TableClient tableClient = await CreateConnection();

            // Create a retrieve operation that takes a customer entity.
            AzureStorageTableModel result = tableClient.GetEntity<AzureStorageTableModel>(partitionKey, rowKey);

            return result;
        }

        //This can't be async, because of how it queries the underlying data
        public async Task<List<AzureStorageTableModel>> GetItems(string partitionKey)
        {
            partitionKey = EncodePartitionKey(partitionKey);

            //Create a connection to the Azure Table
            TableClient tableClient = await CreateConnection();

            // Create the query
            Pageable<AzureStorageTableModel>? query = tableClient.Query<AzureStorageTableModel>(e => e.PartitionKey == partitionKey);

            // Execute the query.
            List<AzureStorageTableModel> list = query.ToList();

            return list;
        }

        public async Task<bool> SaveItem(AzureStorageTableModel data)
        {
            //Create a connection to the Azure Table
            TableClient tableClient = await CreateConnection();

            // Create the TableOperation that inserts/merges the entity.
            Response response = await tableClient.UpsertEntityAsync(data);

            return !response.IsError;
        }

        public static string EncodePartitionKey(string text)
        {
            //The forward slash(/) character
            text = text.Replace("/", "_");

            return text;
        }

    }
}