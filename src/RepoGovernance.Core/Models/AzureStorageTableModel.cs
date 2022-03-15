﻿using Microsoft.Azure.Cosmos.Table;
using RepoGovernance.Core.TableStorage;

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
