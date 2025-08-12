# Manual Testing Guide for isContributor Parameter

This guide describes how to manually test that the `isContributor` parameter is properly passed throughout the application navigation.

## Testing Scenarios

### 1. Navigation with isContributor=true

1. Start with URL: `http://localhost:5000/?isContributor=true`
2. Verify all navigation links maintain the parameter:
   - Navbar "Home" link should go to `/?isContributor=true` 
   - Navbar "Task List" link should go to `/TaskList?isContributor=true`
   - Navbar "Privacy" link should go to `/Privacy?isContributor=true`
   - Footer "Privacy" link should go to `/Privacy?isContributor=true`

### 2. Navigation without isContributor parameter

1. Start with URL: `http://localhost:5000/` 
2. Verify all navigation links do NOT include the parameter:
   - Navbar "Home" link should go to `/`
   - Navbar "Task List" link should go to `/TaskList`
   - Navbar "Privacy" link should go to `/Privacy`
   - Footer "Privacy" link should go to `/Privacy`

### 3. Controller Action Redirects

1. Test UpdateAll action with isContributor=true:
   - Navigate to `/UpdateAll?isContributor=true`
   - Should redirect to `/?isContributor=true`

2. Test UpdateAll action without isContributor:
   - Navigate to `/UpdateAll`
   - Should redirect to `/` (no parameter)

### 4. View-Specific Navigation

#### TaskList View
1. Access `/TaskList?isContributor=true`
2. Verify "Back to repository overview" link goes to `/?isContributor=true`
3. Verify repository detail links include `isContributor=true`

#### Details View  
1. Access a details URL with `isContributor=true`
2. Verify "Go back to index" link includes the parameter

#### RepoDetails View
1. Access a repo details URL with `isContributor=true` 
2. Verify "Back to task list" link includes the parameter

### 5. Contributor-Only Features

With `isContributor=true`, verify these features are visible:
- "Update all" button on Index page
- "Approve all open PRs" button on Index page  
- "Update metrics" links for individual repositories
- "Update configuration" links for individual repositories

Without the parameter, these features should be hidden.

## Expected Behavior

- When `isContributor=true`, the parameter should be included in ALL navigation URLs
- When `isContributor=false` or not present, the parameter should NEVER appear in URLs
- The URL should never show `isContributor=false` explicitly
- All redirects should preserve the isContributor state appropriately
- Contributor-only UI elements should only appear when `isContributor=true`

## Test Results

The automated tests cover the controller logic and ViewBag/model behavior. Manual testing should focus on verifying that the UI actually generates the expected URLs and that navigation flows work correctly in a real browser environment.