using Microsoft.Azure.Cosmos.Table;
using RepoGovernance.Core.TableStorage;

//TODO: Update: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/tables/Azure.Data.Tables/MigrationGuide.md
namespace RepoGovernance.Core.Models
{
    public class AzureStorageTableModel : TableEntity
    {
        public AzureStorageTableModel(string partitionKey, string rowKey, string data)
        {
            PartitionKey = TableStorageCommonDA.EncodePartitionKey(partitionKey);
            RowKey = TableStorageCommonDA.EncodePartitionKey(rowKey);
            Data = data;
        }

        //TableEntity requires an empty constructor
        public AzureStorageTableModel() { }

        public string? Data { get; set; }
    }
}
