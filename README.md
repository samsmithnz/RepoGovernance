# RepoGovernance
[![CI/CD](https://github.com/samsmithnz/RepoGovernance/actions/workflows/workflow.yml/badge.svg)](https://github.com/samsmithnz/RepoGovernance/actions/workflows/workflow.yml)
[![Coverage Status](https://coveralls.io/repos/github/samsmithnz/RepoGovernance/badge.svg?branch=main)](https://coveralls.io/github/samsmithnz/RepoGovernance?branch=main)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=samsmithnz_RepoGovernance&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=samsmithnz_RepoGovernance)
[![Current Release](https://img.shields.io/github/release/samsmithnz/RepoGovernance/all.svg)](https://github.com/samsmithnz/RepoGovernance/releases)

**RepoGovernance is a project I'm building to monitor my side-projects and ensure they are following best practices**. I currently track a number of properties, including repo settings, branch policies, actions, dependabot, auto-versioning, technical debt identification, and DevOps metrics. It uses a number of building blocks I've built in the past to achieve this:

- [GitHubDotNet](https://github.com/samsmithnz/GitHubActionsDotNet): Models to generate Actions and Dependabot files with .NET
- [RepoAutomation](https://github.com/samsmithnz/RepoAutomation): To automate the creation of repos and my preferred repo settings
- [SamsFeatureFlags](https://github.com/samsmithnz/SamsFeatureFlags): To use Feature Flags/Toggles on my projects
- [TechDebtIdentification](https://github.com/samsmithnz/TechDebtIdentification): To identify what version of .NET my projects are using
- [DevOpsMetrics](https://github.com/samsmithnz/DevOpsMetrics): to calculate DORA/DevOps Metrics on my projects

```mermaid
  graph LR;
      GitHubDotNet-->RepoAutomation;
      RepoAutomation-->RepoGovernance;
      SamsFeatureFlags-->RepoGovernance;
      TechDebtIdentification-->RepoGovernance;
      DevOpsMetrics-->RepoGovernance;
```

## Current solution
Currently hosted at https://repogovernance-prod-eu-web.azurewebsites.net/. This shows current projects I have configured, with recommendations, pull requests, frameworks detected, and current DORA DevOps metrics

![image](https://user-images.githubusercontent.com/8389039/208060001-d842917c-a642-4065-8133-db2946062db0.png)

## The future?
- Remediation: allowing users to not just identify an issue, but to resolve it with a few clicks. 
- A GitHubApp: so that it's easy for anyone to setup, configure, and use - as well as adding authenication!
- More configuration and settings
