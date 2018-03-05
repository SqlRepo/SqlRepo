[![Build status](https://ci.appveyor.com/api/projects/status/8idf8kwnvaiudnig?svg=true)](https://ci.appveyor.com/project/testpossessed/sqlrepo)

# SqlRepo
SqlRepo is an implementation of the Repository Pattern that allows you to build CRUD SQL statements using Lambda Expressions.

[Read the documentation on our Wiki](https://github.com/testpossessed/sqlrepo/wiki)

## Features
* Compatible with .NET Standard 2.0 and .NET Framework 4.7
* Intuitively build SQL statements using C# Lambda Expressions
* Map SQL query results to plain old CLR objects
* Low memory footprint and [high performance](https://github.com/SqlRepo/Benchmarks)
* Almost 100% unit test coverage

## Installation

**NuGet Package Manager**
```
Install-Package SqlRepo.SqlServer
```

**dotnet cli**
```
dotnet add package SqlRepo.SqlServer
```

## Example
```csharp

public class GettingStarted
{
    private IRepositoryFactory repositoryFactory;

    public GettingStarted(IRepositoryFactory repositoryFactory)
    {
        this.repositoryFactory = repositoryFactory;
    }

    public void DoIt()
    {
         var repository = this.repositoryFactory.Create<ToDo>();
         var results = repository.Query()
         .Select(e => e.Id)
         .Select(e => e.Task)
         .Select(e => e.CreatedDate)
         .Where(e => e.IsCompleted == false)
         .Go("MyConnectionString");
    }
}

```
Generates the following SQL statement and maps the results back to the list of ToDo objects.

```sql

SELECT [dbo].[ToDo].[Id], [dbo].[ToDo].[Task], [dbo].[ToDo].[CreatedDate]
FROM [dbo].[ToDo]
WHERE [dbo].[ToDo].[IsCompleted] = 0;

```
