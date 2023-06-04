using Azure;
using Azure.Data.Tables;
using RepoGovernance.Core.TableStorage;

namespace RepoGovernance.Core.Models
{
    public class AzureStorageTableModel : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string? Data { get; set; }

        //Public parameterless constructor needed for storage table access. If you remove this the tests will fail
        public AzureStorageTableModel() { }

        public AzureStorageTableModel(string partitionKey, string rowKey, string data)
        {
            PartitionKey = TableStorageCommonDA.EncodePartitionKey(partitionKey);
            RowKey = TableStorageCommonDA.EncodePartitionKey(rowKey);
            Data = data;
        }
    }
}
