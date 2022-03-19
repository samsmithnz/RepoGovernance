# RepoGovernance
[![CI/CD](https://github.com/samsmithnz/RepoGovernance/actions/workflows/workflow.yml/badge.svg)](https://github.com/samsmithnz/RepoGovernance/actions/workflows/workflow.yml)

**RepoGovernance is a project I'm building to monitor my side-projects and ensure they are following best practices**, currently including repo settings, branch policies, actions, dependabot, and auto-versioning. It uses a number of building blocks I've built in the past:
- [GitHubDotNet](https://github.com/samsmithnz/GitHubActionsDotNet): To codify and generate Actions and Dependabot files with .NET
- [RepoAutomation](https://github.com/samsmithnz/RepoAutomation): To automate the creation of repos and my preferred settings
- [SamsFeatureFlags](https://github.com/samsmithnz/SamsFeatureFlags): To use Feature Flags/Toggles on my projects
- [TechDebtIdentification](https://github.com/samsmithnz/TechDebtIdentification): To identify what version of .NET my projects are using

## Current solution... 
Currently viewable at https://repogovernance-prod-eu-web.azurewebsites.net/

## The future?
- Remediation: allowing users to not just identify an issue, but to resolve it with a few clicks. 
- a GitHubApp: so that it's easy for anyone to setup, configure, and use
- More configuration and settings
