# CS360DatabaseProject

A .NET 10 ASP.NET Core Razor Pages application for the Life Auto Shop database project.

## Requirements

- .NET 10 SDK
- Visual Studio 2026 or another .NET-compatible IDE
- Access to the SQL Server database configured in `CS360DatabaseProject/appsettings.json`

## Run locally

From the repository root:

```powershell
dotnet restore .\CS360DatabaseProject\CS360DatabaseProject.csproj
dotnet run --project .\CS360DatabaseProject\CS360DatabaseProject.csproj
```

The solution can also be opened with `CS360DatabaseProject.slnx` in Visual Studio.

## Database configuration

The application reads the `CS360DatabaseProjectContext` connection string from `appsettings.json`. Do not publish database credentials in a public repository. If this repository is public, rotate the current database password and move the connection string to User Secrets, environment variables, or a managed secret store.
