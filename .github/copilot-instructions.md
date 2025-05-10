# Copilot Instructions
- Prefer brief, clear, and concise responses of a single paragraph

# Team Best Practices
- Our team uses Azure DevOps for tracking items of work.
- Our team uses build and deployment pipelines using Azure DevOps.
- Our team uses uses GitHub repositories for file history and storage.
- Prefer async and await over synchronous code.
- Private fields should start with an underscore.
- A file can only contain one class declaration.

## Testing Guidelines
- Use the AAA pattern (Arrange, Act, Assert)
- Avoid infrastructure dependencies
- Name tests clearly
- Write minimally passing tests
- Avoid magic strings
- Avoid logic in tests
- Prefer helper methods for setup and teardown
- Avoid multiple acts in a single test

- @azure Rule - Use Azure Best Practices: When generating code for Azure, running terminal commands for Azure, or performing operations related to Azure, invoke your `azure_development-get_best_practices` tool if available.