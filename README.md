.NET SDK is required to launch tests

All bellow commands should be used from root directory (with .sln in it).

Here's multiple ways to launch tests:
1. `dotnet test` - this will take fallback base url
2. `dotnet test --environment 'url=<your_url>'` - replace `<your_url>` with desired base url
3. `dotnet test --filter "Category=Web Only Tests"` - will launch tests that are only doing webbrowser manipulation
4. `dotnet test --filter "Category=Download Tests"` - will launch tests that are only doing item download test

Both options that use filter can also use `--environment 'url=<your_url>'`
