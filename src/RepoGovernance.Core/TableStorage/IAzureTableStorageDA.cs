﻿//namespace RepoGovernance.Core.TableStorage
//{
//    public interface IAzureTableStorageDA
//    {
//        //List<AzureDevOpsSettings> GetAzureDevOpsSettingsFromStorage(TableStorageConfiguration tableStorageConfig, string settingsTable, string rowKey);
//        //List<GitHubSettings> GetGitHubSettingsFromStorage(TableStorageConfiguration tableStorageConfig, string settingsTable, string rowKey);
//        //JArray GetTableStorageItemsFromStorage(TableStorageConfiguration tableStorageConfig, string tableName, string partitionKey, bool includePartitionAndRowKeys = false);
//        //Task<int> UpdateAzureDevOpsBuildsInStorage(string patToken, TableStorageConfiguration tableStorageConfig, string organization, string project, string branch, string buildName, string buildId, int numberOfDays, int maxNumberOfItems);
//        //Task<int> UpdateAzureDevOpsPullRequestCommitsInStorage(string patToken, TableStorageConfiguration tableStorageConfig, string organization, string project, string repository, string pullRequestId, int numberOfDays, int maxNumberOfItems);
//        //Task<int> UpdateAzureDevOpsPullRequestsInStorage(string patToken, TableStorageConfiguration tableStorageConfig, string organization, string project, string repository, int numberOfDays, int maxNumberOfItems);
//        //Task<bool> UpdateAzureDevOpsSettingInStorage(TableStorageConfiguration tableStorageConfig, string settingsTable, string organization, string project, string repository, string branch, string buildName, string buildId, string resourceGroupName, int itemOrder, bool showSetting);
//        //Task<int> UpdateChangeFailureRate(TableStorageCommonDA tableChangeFailureRateDA, ChangeFailureRateBuild newBuild, string partitionKey, bool forceUpdate = false);
//        //Task<bool> UpdateDevOpsMonitoringEventInStorage(TableStorageConfiguration tableStorageConfig, MonitoringEvent monitoringEvent);
//        //Task<int> UpdateGitHubActionPullRequestCommitsInStorage(string clientId, string clientSecret, TableStorageConfiguration tableStorageConfig, string owner, string repo, string pull_number);
//        //Task<int> UpdateGitHubActionPullRequestsInStorage(string clientId, string clientSecret, TableStorageConfiguration tableStorageConfig, string owner, string repo, string branch, int numberOfDays, int maxNumberOfItems);
//        //Task<int> UpdateGitHubActionRunsInStorage(string clientId, string clientSecret, TableStorageConfiguration tableStorageConfig, string owner, string repo, string branch, string workflowName, string workflowId, int numberOfDays, int maxNumberOfItems);
//        //Task<bool> UpdateGitHubSettingInStorage(TableStorageConfiguration tableStorageConfig, string settingsTable, string owner, string repo, string branch, string workflowName, string workflowId, string resourceGroupName, int itemOrder, bool showSetting);
//        //List<ProjectLog> GetProjectLogsFromStorage(TableStorageConfiguration tableStorageConfig, string partitionKey);
//        //Task<bool> UpdateProjectLogInStorage(TableStorageConfiguration tableStorageConfig, ProjectLog log);
//    }
//}