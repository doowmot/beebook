# C# Acebook Template

A template application for Acebook in C#

The following features have already been implemented – you are inheriting a small legacy codebase!

- Signing up
- Signing in
- Making a post

Your task is to extend the application.

## Setup
- Installing dependencies
`dotnet tool install --global dotnet-ef`
- Creating the database 
- Running migrations
`dotnet ef migrations add <MigrationName>` -> how to avoid using --context?
`dotnet ef database update`

## Running Acebook
Use `dotnet watch run` so that the application automatically recompiles whenever you make changes to the codebase.

## Testing
- Run the app using `dotnet watch run`
- In a new terminal, execute the tests using `dotnet test`